using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Models.Momo;
using FinalProject.Services.Momo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Database
            builder.Services.AddDbContext<WebDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("FinalProject"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null
                        );
                    }
                )
            );

            // Identity
            builder.Services.AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                options.Password.RequireDigit = true;              // B?t bu?c có s?
                options.Password.RequiredLength = 8;               // Ít nh?t 8 ký t?
                options.Password.RequireNonAlphanumeric = true;    // B?t bu?c ký t? ??c bi?t
                options.Password.RequireUppercase = true;          // B?t bu?c ch? hoa
                options.Password.RequireLowercase = true;          // B?t bu?c ch? th??ng
                options.Password.RequiredUniqueChars = 1;           // s? ký t? khác nhau t?i thi?u
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<WebDbContext>();

            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

                //always displays name claim from google, not email
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");

                //chon tai khoan khi dang nhap bang google
                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
                    return Task.CompletedTask;
                };
            });

            // Authentication (Google)
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "FinalProjectAuthCookie";

            });

            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // HttpClient
            builder.Services.AddHttpClient();

            // MoMo
            builder.Services.Configure<MomoOptionModel>(
            builder.Configuration.GetSection("MomoAPI"));
            builder.Services.AddScoped<IMomoService, MomoService>();

            var app = builder.Build();

            // Configure middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

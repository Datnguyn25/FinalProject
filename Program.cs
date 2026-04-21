using FinalProject.Data;
using FinalProject.Hubs;
using FinalProject.Models;
using FinalProject.Models.Momo;
using FinalProject.Services.Email;
using FinalProject.Services.Momo;
using FinalProject.Services.Zalo;
using FinalProject.Services.ZaloPay;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"./keys/"))
                .SetApplicationName("FinalProject_Unique_Name");

            // Add services to the container
            builder.Services.AddControllersWithViews(options =>
            {
                
            });

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
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa trong 15 phút
                options.Lockout.MaxFailedAccessAttempts = 5;                       // Sai 5 lần thì khóa
                options.Lockout.AllowedForNewUsers = true;

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
                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");

                options.SaveTokens = true;

                //chon tai khoan khi dang nhap bang google
                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
                    return Task.CompletedTask;
                };
            });

            // Authentication (Google)
            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "FinalProjectAuthCookie";

                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Giữ đăng nhập 30 ngày
                options.SlidingExpiration = true;              // Reset thời gian 30 ngày mỗi khi bạn có hoạt động
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

                options.Cookie.MaxAge = options.ExpireTimeSpan;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            

            // HttpClient
            builder.Services.AddHttpClient();

            // MoMo
            builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
            builder.Services.AddScoped<IMomoService, MomoService>();

            builder.Services.AddTransient<IEmailService, EmailService>();
            
            
            // zalopay
            builder.Services.AddScoped<IZaloPayService, ZaloPayService>();


            builder.Services.AddSignalR();
          



            var app = builder.Build();
            app.MapHub<OrderHub>("/orderHub");




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

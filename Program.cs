using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Models.Momo;
using FinalProject.Services.Momo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("FinalProject");

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<WebDbContext>(options =>
<<<<<<< HEAD
                options.UseSqlServer(builder.Configuration.GetConnectionString("FinalProject")));
=======
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("FinalProject"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null
                        );
                    }
                )
            );
>>>>>>> 342cecc507d78faff00b79ec29079f2828c0259e

            // Add Identity
            builder.Services.AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<WebDbContext>();
                

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            // Google Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = "391898321966-7es0tec74nvjfm60aoev79o780epfqev.apps.googleusercontent.com";
                options.ClientSecret = "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";

                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
                    return Task.CompletedTask;
                };
            });
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            builder.Services.AddHttpClient();

            //Connect MomoAPI
            builder.Services.Configure<MomoOptionModel> (builder.Configuration.GetSection("MomoAPI"));
            builder.Services.AddScoped<IMomoService, MomoService>();

            var app = builder.Build();

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
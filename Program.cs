using FinalProject.Data;
using FinalProject.Models.Momo;
using FinalProject.Services.Momo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

namespace FinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // DbContext
            builder.Services.AddDbContext<WebDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Authentication
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
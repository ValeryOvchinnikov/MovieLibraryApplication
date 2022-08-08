using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpLogging;
using System.IdentityModel.Tokens.Jwt;

namespace MovieLibrary.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("MyResponseHeader");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;

            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", c =>
            {
                c.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5001";

                options.ClientId = "WebApplication";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.ClaimActions.MapJsonKey("role", "role", "role");
                options.ClaimActions.MapJsonKey("sub", "sub", "sub");
                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";
                options.SaveTokens = true;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("movieLibraryApi");

                options.GetClaimsFromUserInfoEndpoint = true;
            });



            var app = builder.Build();
            app.Use(async (context, next) =>
            {
                context.Response.Headers["MyResponseHeader"] =
                    new string[] { "My Response Header Value" };

                await next();
            });
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpLogging();
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });

            app.Run();
        }
    }
}
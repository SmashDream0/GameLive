using System;
using System.Linq;
using GameLive.Web.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameLive.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<Config.ConfigSettings>(Configuration.GetSection("appSettings"));
            services.PostConfigure<Config.ConfigSettings>(configSettings => 
            {
                var errors = configSettings.Validate();

                if (errors.Any())
                {
                    var aggrErrors = string.Join(Environment.NewLine, errors);
                    var configType = configSettings.GetType().Name;
                    throw new ApplicationException(
                        $"Found few configuration error(s) in {configType}:{Environment.NewLine}{aggrErrors}");
                }
            });
            services.AddSingleton<Logic.AccountLogic>();
            services.AddSingleton<Logic.GameWorldRepository>();
            services.AddSingleton<Logic.ConnectionMapping>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSession();

            services.AddSignalR();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => //CookieAuthenticationOptions
                    {
                        options.LoginPath = new PathString("/Account/Login");
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors();
            app.UseStaticFiles();
            app.UseSession();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<WorldUpdateHub>("/worldUpdateHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Game}/{action=Index}/{id?}");
            });
        }
    }
}

using Cover.Domain.Data;
using Cover.Domain.Services;
using Cover.Neo4J.Domain.Core.Repository;
using Cover.Neo4J.Domain.Core.Settings;
using Cover.Neo4J.Domain.Core.Settings.Factory;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Principal;

namespace Cover
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("/{0}.cshtml");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);


            services.AddControllersWithViews();
            services.AddHttpContextAccessor();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                });
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);


            /// My
            services.Configure<CoverDatabaseSettings>(
                Configuration.GetSection(nameof(CoverDatabaseSettings)));

            services.AddTransient<ICoverDatabaseSettings, CoverDatabaseSettings>(
                sp => sp.GetRequiredService<IOptions<CoverDatabaseSettings>>().Value);

            services.AddTransient<PostService>();
            services.AddTransient<UserService>();

            //Neo4j
            services.Configure<CoverGraphDatabaseSettings>(
                Configuration.GetSection(nameof(CoverGraphDatabaseSettings)));


            services.AddTransient<ICoverGraphDatabaseSettings, CoverGraphDatabaseSettings>(
                sp => sp.GetRequiredService<IOptions<CoverGraphDatabaseSettings>>().Value);

            services.AddTransient<CoverGraphDatabaseClientFactory>();
            services.AddTransient<UserRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=News}/{id?}");
            });
        }
    }
}

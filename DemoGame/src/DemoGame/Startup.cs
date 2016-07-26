using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoGame
{
    /// <summary>
    /// This is the middleware pipeline startup for a .NET Core application
    /// </summary>
    public class Startup
    {
        private IHostingEnvironment _env;

        /// <summary>
        /// The constructor is where you want to build your configuration
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add injectable services to the DI container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Register MVC services
            services.AddMvc();

            // Register SignalR services
            services.AddSignalR(options =>
            {

                // only enable debug errors in Development
                options.Hubs.EnableDetailedErrors = _env.IsDevelopment();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline and middleware.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //
            // Logging configuration
            //
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //
            // Error handling configuration
            //
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //
            // Static file serving
            app.UseStaticFiles();

            //
            // SignalR
            app.UseWebSockets();
            app.UseSignalR();

            //
            // MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

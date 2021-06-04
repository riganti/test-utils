using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Riganti.Selenium.Coordinator.Service.Services;
using Riganti.Selenium.Coordinator.Service.Data;
using Riganti.Selenium.Coordinator.Service.Hubs;

namespace Riganti.Selenium.Coordinator.Service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (Directory.Exists("/mnt/config"))
            {
                builder.AddJsonFile("/mnt/config/appsettings.json", optional: true, reloadOnChange: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IServiceProvider ServiceProvider { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddAuthorization();
            services.AddWebEncoders();

            services.Configure<AppConfiguration>(Configuration);

            services.AddDotVVM<DotvvmStartup>();

            services.AddMvc();
            services.AddLogging(s =>
            {
                s.AddConsole();
                // add SignalR logger
                s.AddProvider(new LogHubProvider(() => ServiceProvider)
                {
                    Level = LogLevel.Debug
                });
            });

            services.AddSignalR();

            services.AddSingleton<ContainerLeaseRepository>();
            services.AddSingleton<DockerProvisioningService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            this.ServiceProvider = app.ApplicationServices;


            // use SignalR
            app.UseEndpoints(options =>
            {
                options.MapHub<LogHub>("/log");
            });

            // use DotVVM
            var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(env.ContentRootPath);

            // use ASP.NET Core
            app.UseMvc();


            // use static files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(env.WebRootPath)
            });
        }
    }
}

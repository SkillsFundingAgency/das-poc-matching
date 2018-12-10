using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sfa.poc.matching.configuration.Configuration;
using sfa.poc.matching.configuration.Services;

namespace sfa.poc.matching.configuration
{
    public class Startup
    {
        public IWebConfiguration Configuration { get; set; }

        public Startup(IConfiguration config)
        {
            Configuration = ConfigurationService.GetConfig(config["EnvironmentName"], 
                config["ConfigurationStorageConnectionString"],
                config["Version"], 
                config["ServiceName"]).Result;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Should be able to set up SQL connection like this
            //services.AddTransient<DbConnection>(provider =>
            //    new SqlConnection(Configuration.SqlConnectionString));

            services.AddMvc();
            services.AddTransient<IWebConfiguration>(provider => Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            app.UseStaticFiles()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sfa.poc.matching.search.azure.application.Configuration;
using sfa.poc.matching.search.azure.application.Interfaces;
using sfa.poc.matching.search.azure.application.Services;
using sfa.poc.matching.search.azure.data;

namespace sfa.poc.matching.search.azure.web
{
    public class Startup
    {
        public ISearchConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = ConfigurationService.GetConfig(configuration["EnvironmentName"],
                configuration["ConfigurationStorageConnectionString"],
                configuration["Version"],
                configuration["ServiceName"]).Result;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<ISearchConfiguration>(provider => Configuration);

            services.AddTransient<IPostcodeLoader, ExternalPostcodeLoaderService>();

            services.AddScoped<ISqlDataRepository>(provider => new SqlDataRepository(Configuration.SqlConnectionString));

            services.AddTransient<ISearchService, SearchService>();

            if (Configuration.AzureSearchConfiguration.Name.StartsWith("UseSqlServer=true", StringComparison.OrdinalIgnoreCase))
            {
                services.AddTransient<ISearchProvider>(provider =>
                    new SqlSearchProvider(Configuration.SqlConnectionString));
            }
            else
            {
                services.AddTransient<ISearchProvider, AzureSearchProvider>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

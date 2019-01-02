using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sfa.poc.matching.search.azure.application.Configuration;
using sfa.poc.matching.search.azure.application.Interfaces;
using sfa.poc.matching.search.azure.application.Services;
using sfa.poc.matching.search.azure.data;

namespace sfa.poc.matching.search.azure
{
    public class Program
    {
        internal static ISearchConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                var localConfiguration = builder.Build();

                //IWebConfiguration Configuration { get; }
                Configuration = ConfigurationService.GetConfig(
                        localConfiguration["EnvironmentName"],
                        localConfiguration["ConfigurationStorageConnectionString"],
                        localConfiguration["Version"],
                        localConfiguration["ServiceName"])
                    .Result;

                Console.WriteLine($"Retrieved configuration. ");
                Console.WriteLine($" - Search Service Name:   '{Configuration.AzureSearchConfiguration.SearchServiceName}'");
                Console.WriteLine($" - Sql Connection String: '{Configuration.SqlConnectionString}'");
                
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                var serviceProvider = serviceCollection.BuildServiceProvider();

                await serviceProvider.GetService<App>().Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Make configuration settings available
            services.AddSingleton<ISearchConfiguration>(Configuration);

            //services.AddScoped<DbConnection>(provider => new SqlConnection(Configuration.SqlConnectionString));
            services.AddScoped<ISqlDataRepository>(provider => new SqlDataRepository(Configuration.SqlConnectionString));

            services.AddTransient<ISearchService, SearchService>();

            if (Configuration.AzureSearchConfiguration.SearchServiceName.StartsWith("UseSqlServer=true", StringComparison.OrdinalIgnoreCase))
            {
                services.AddTransient<ISearchProvider>(provider =>
                    new SqlSearchProvider(Configuration.SqlConnectionString));
            }
            else
            {
                services.AddTransient<ISearchProvider, AzureSearchProvider>();
            }

            services.AddTransient<App>();
        }
    }
}

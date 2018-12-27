using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.POC.Matching.Application.Importer;
using SFA.POC.Matching.Application.Interfaces;
using SFA.POC.Matching.Data;
using SFA.POC.Matching.Proximity.Infrastructure.Configuration;
using SFA.POC.Matching.Proximity.Infrastructure.Services;

namespace SFA.POC.Matching.Proximity.Console
{
    public class Program
    {
        internal static IMatchingConfiguration Configuration { get; set; }

        public static async Task Main(string[] args)
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

                System.Console.WriteLine($"Retrieved configuration. ");
                System.Console.WriteLine($" - Sql Connection String is '{Configuration.SqlConnectionString}'");
                System.Console.WriteLine($" - Cosmos Connection String is '{Configuration.CosmosConnectionString}'");
                System.Console.WriteLine($" - Post code baseurl is '{Configuration.PostcodeRetrieverBaseUrl}'");

                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                var serviceProvider = serviceCollection.BuildServiceProvider();

                await serviceProvider.GetService<App>().Run();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Make configuration settings available
            services.AddSingleton<IMatchingConfiguration>(Configuration);

            //services.AddScoped<DbConnection>(provider => new SqlConnection(Configuration.SqlConnectionString));

            services.AddTransient<IPostcodeImporter, ExternalPostcodeImporter>();
            services.AddTransient<ILocationWriter, SqlLocationWriter>(p => new SqlLocationWriter(Configuration.SqlConnectionString));

            //var appConnStr = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "WindowsConnStr" : "LinuxConnStr";
            //services.AddDbContext<MyDbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString(appConnStr));
            //}, ServiceLifetime.Scoped);

            // Repositories
            //services.AddScoped(typeof(IRepository<>), typeof(DomainRepository<>));

            // Add caching
            //services.AddMemoryCache();

            // Add logging            
            //services.AddLogging(builder =>
            //{
            //    builder.AddConfiguration(Configuration.GetSection("Logging"))
            //        .AddConsole()
            //        .AddDebug();
            //});

            // Add Application 
            services.AddTransient<App>();
        }
    }
}

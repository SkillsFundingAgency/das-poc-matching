using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SFA.POC.Matching.Proximity.Infrastructure.Configuration;
using SFA.POC.Matching.Proximity.Infrastructure.Services;

namespace SFA.POC.Matching.Proximity.Infrastructure.Console
{
    class Program
    {
        internal static IMatchingConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            BuildConfiguration();

            System.Console.WriteLine("Done.");
            System.Console.ReadKey();
        }

        private static void BuildConfiguration()
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
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SFA.POC.Matching.Proximity.Infrastructure.Configuration;

namespace SFA.POC.Matching.Proximity.Infrastructure.Services
{
    public class ConfigurationService
    {
        public static async Task<IMatchingConfiguration> GetConfig(string environment, string storageConnectionString,
            string version, string serviceName)
        {
            Console.WriteLine($"Getting configuration for {environment} {serviceName} {version}");

            var conn = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = conn.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Configuration");

            var operation = TableOperation.Retrieve(environment, $"{serviceName}_{version}");
            var result = await table.ExecuteAsync(operation);

            var dynResult = result.Result as DynamicTableEntity;
            var data = dynResult.Properties["Data"].StringValue;

            return JsonConvert.DeserializeObject<MatchingConfiguration>(data);
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace sfa.poc.matching.search.azure.application.Configuration
{
    public class ConfigurationService
    {
        public static async Task<ISearchConfiguration> GetConfig(string environment, string storageConnectionString,
            string version, string serviceName)
        {
            var conn = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = conn.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Configuration");

            var operation = TableOperation.Retrieve(environment, $"{serviceName}_{version}");
            var result = await table.ExecuteAsync(operation);

            var dynResult = result.Result as DynamicTableEntity;
            var data = dynResult.Properties["Data"].StringValue;

            return JsonConvert.DeserializeObject<SearchConfiguration>(data);
        }
    }
}

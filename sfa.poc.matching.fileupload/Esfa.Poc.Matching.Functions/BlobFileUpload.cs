using System.IO;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application;
using Esfa.Poc.Matching.Application.Commands;
using Esfa.Poc.Matching.Common;
using Esfa.Poc.Matching.Database;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Esfa.Poc.Matching.Functions
{
    public static class BlobFileUpload
    {
        [FunctionName("BlobFileUpload")]
        public static async Task Run(
            [BlobTrigger("employer/{name}", Connection = "AzureWebJobsStorage")] Stream stream,
            string name,
            ILogger log,
            ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var sqlConnectionString = config.GetConnectionString("Sql");

            var fileUploadCreate = new CreateFileUpload(new DateTimeProvider(),
                new CreateFileUploadCommand(new FileUploadContext(sqlConnectionString)));

            var createdBy = "";

            var fileUploadResult = await fileUploadCreate.Create(name, createdBy);

            if (!fileUploadResult.IsSuccess)
            {
                // TODO AU
            }

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {stream.Length} Bytes");
        }
    }
}
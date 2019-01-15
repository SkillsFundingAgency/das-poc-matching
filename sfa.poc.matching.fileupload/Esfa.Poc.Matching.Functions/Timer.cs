using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Enums;
using Esfa.Poc.Matching.Application.Factories;
using Esfa.Poc.Matching.Application.Queries;
using Esfa.Poc.Matching.Database;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Esfa.Poc.Matching.Functions
{
    public static class Timer
    {
        //[Disable]
        [FunctionName("Timer")]
        public static async Task Run(
            [TimerTrigger("%TimerInterval%")] TimerInfo timer,
            ILogger log,
            [Queue("employers", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> employerOutput,
            [Queue("contacts", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> contactOutput,
            [Queue("queries", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> queryOutput,
            ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var sqlConnectionString = config.GetConnectionString("Sql");
            var storageAccountConnection = config.GetConnectionString("StorageAccount");

            var dbContextService = new FileUploadContext(sqlConnectionString);

            var getFileUploadQuery = new GetFileUploadQuery(dbContextService);
            var filesToProcess = await getFileUploadQuery.Execute();

            var fileUploadContext = new FileUploadContext(sqlConnectionString);

            var filesToProcessGroupedByType = filesToProcess.GroupBy(f => f.Type);
            foreach (var files in filesToProcessGroupedByType)
            {
                var fileUploadType = (FileUploadType)files.Key;
                //var containerName = GetContainerName(fileUploadType);
                var fileUploads = files.ToList();
                
                var queueOutput = GetQueueOutput(fileUploadType,
                    employerOutput,
                    contactOutput,
                    queryOutput);

                var blobImport = BlobImportFactory.Create(fileUploadContext, fileUploadType, storageAccountConnection);
                var import = blobImport.Import(fileUploads, queueOutput);

            }
        }

        private static IAsyncCollector<string> GetQueueOutput(FileUploadType fileUploadType,
            IAsyncCollector<string> employerOutput,
            IAsyncCollector<string> contactOutput,
            IAsyncCollector<string> queryOutput)
        {
            switch (fileUploadType)
            {
                case FileUploadType.Employer:
                    return employerOutput;
                case FileUploadType.Contact:
                    return contactOutput;
                case FileUploadType.Query:
                    return queryOutput;
            }

            throw new InvalidOperationException();
        }

        //private static string GetContainerName(FileUploadType fileUploadType)
        //{
        //    switch (fileUploadType)
        //    {
        //        case FileUploadType.Employer:
        //        case FileUploadType.Contact:
        //        case FileUploadType.Query:
        //            return ContainerConstants.Employer;
        //    }

        //    throw new InvalidOperationException();
        //}
    }
}
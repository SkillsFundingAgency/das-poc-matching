using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using sfa.poc.matching.functions.application.Services;
using sfa.poc.matching.functions.extensions;

namespace sfa.poc.matching.functions
{
    public static class BlobTestFunction
    {
        //TODO: Look at imperative bindings so we can avoid configuring blob connection/path in local config
        //https://xebia.com/blog/azure-functions-imperative-bindings/
        //https://stackoverflow.com/questions/47625498/azure-functions-blob-stream-dynamic-input-bindings

        [FunctionName("BlobTestFunction")]
        public static void Run(
            [BlobTrigger("tl-matching-files/{name}", Connection = "BlobStorageConnectionString")]Stream myBlob,
            string name,
            [Inject]ITestService testService,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var response = testService.GetMessage();
            log.LogInformation($"C# Blob trigger is running in environment {response.EnvironmentName}");
        }
    }
}

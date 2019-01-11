using System.Linq;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Database;
using Esfa.Poc.Matching.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Esfa.Poc.Matching.Functions
{
    public static class EmployerContact
    {
        [FunctionName("EmployerContact")]
        public static async Task Run(
            [QueueTrigger("contacts", Connection = "AzureWebJobsStorage")] string contactJson,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation($"C# Queue trigger function processed: {contactJson}");

            var contact = JsonConvert.DeserializeObject<FileUploadContact>(contactJson);

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            var sqlConnectionString = config.GetConnectionString("Sql");
            var dbContextService = new FileUploadContext(sqlConnectionString);

            var contactInSql = await dbContextService.Contact.Where(c => c.Employer.CompanyName == contact.CompanyName
                                                         && c.Employer.CreatedOn == contact.CreatedOnCompany).ToListAsync();
        }
    }
}
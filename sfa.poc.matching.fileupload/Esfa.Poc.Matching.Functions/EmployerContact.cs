using System.Threading;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Commands;
using Esfa.Poc.Matching.Application.Mappers;
using Esfa.Poc.Matching.Application.Queries;
using Esfa.Poc.Matching.Database;
using Esfa.Poc.Matching.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

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
            var logPrefix = GetLogPrefix();

            log.LogInformation($"{logPrefix}: {contactJson}");

            var contact = JsonConvert.DeserializeObject<FileUploadContact>(contactJson);

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            var sqlConnectionString = config.GetConnectionString("Sql");
            var fileUploadContext = new FileUploadContext(sqlConnectionString);

            var getContactQuery = new GetContactQuery(fileUploadContext);
            var contactInSql = await getContactQuery.Execute(contact.CompanyName, contact.CreatedOnCompany);

            if (contactInSql == null)
            {
                log.LogInformation($"{logPrefix} Creating Contact: {contact.Contact}");

                var newContact = ContactMapper.Map(contact, contactInSql);
                var createEmployerCommand = new CreateContactCommand(fileUploadContext);
                await createEmployerCommand.Execute(newContact);

                log.LogInformation($"{logPrefix} Created Contact: {contact.Contact}");
            }
            else
            {
                var areEqual = contactInSql.Equals(contact);
                if (!areEqual)
                {
                    log.LogInformation($"{logPrefix} Updating Contact: {contact.Contact}");

                    var updatedEmployer = ContactMapper.Map(contact, contactInSql);
                    var updateContactCommand = new UpdateContactCommand(fileUploadContext);
                    await updateContactCommand.Execute(updatedEmployer);

                    log.LogInformation($"{logPrefix} Updated Contact: {contact.Contact}");
                }
            }

            log.LogInformation($"{logPrefix} Processed Contact: {contact.Contact}");
        }

        private static string GetLogPrefix()
        {
            var logPrefix = $"{nameof(EmployerContact)} - ThreadId={Thread.CurrentThread.ManagedThreadId}";

            return logPrefix;
        }
    }
}
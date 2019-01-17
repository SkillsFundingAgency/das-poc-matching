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

            var contactFromFile = JsonConvert.DeserializeObject<FileUploadContact>(contactJson);

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            var sqlConnectionString = config.GetConnectionString("Sql");
            var fileUploadContext = new FileUploadContext(sqlConnectionString);

            var getEmployerQuery = new GetEmployerQuery(fileUploadContext);
            var employerInSql = await getEmployerQuery.Execute(contactFromFile.CompanyName, contactFromFile.CreatedOnCompany);
            if (employerInSql == null)
            {
                // TODO Employer has to exist
            }

            var getContactQuery = new GetContactQuery(fileUploadContext);
            var contactInSql = await getContactQuery.Execute(contactFromFile.CompanyName, contactFromFile.CreatedOnCompany);

            var contact = ContactMapper.Map(contactFromFile, contactInSql, employerInSql.Id);
            if (contactInSql == null)
            {
                log.LogInformation($"{logPrefix} Creating Contact: {contactFromFile.Contact}");

                var createEmployerCommand = new CreateContactCommand(fileUploadContext);
                await createEmployerCommand.Execute(contact);

                log.LogInformation($"{logPrefix} Created Contact: {contactFromFile.Contact}");
            }
            else
            {
                var areEqual = contactInSql.Equals(contact);
                if (!areEqual)
                {
                    log.LogInformation($"{logPrefix} Updating Contact: {contactFromFile.Contact}");

                    var updateContactCommand = new UpdateContactCommand(fileUploadContext);
                    await updateContactCommand.Execute(contact);

                    log.LogInformation($"{logPrefix} Updated Contact: {contactFromFile.Contact}");
                }
            }

            log.LogInformation($"{logPrefix} Processed Contact: {contactFromFile.Contact}");
        }

        private static string GetLogPrefix()
        {
            var logPrefix = $"{nameof(EmployerContact)} - ThreadId={Thread.CurrentThread.ManagedThreadId}";

            return logPrefix;
        }
    }
}
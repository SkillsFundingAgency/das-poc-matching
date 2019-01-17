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
    public static class Employer
    {
        [FunctionName("Employer")]
        public static async Task Run(
            [QueueTrigger("employers", Connection = "AzureWebJobsStorage")] string employersJson,
            ILogger log,
            ExecutionContext context)
        {
            var logPrefix = GetLogPrefix();

            log.LogInformation($"{logPrefix}: {employersJson}");

            var employerFromFile = JsonConvert.DeserializeObject<FileUploadEmployer>(employersJson);

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            var sqlConnectionString = config.GetConnectionString("Sql");
            var fileUploadContext = new FileUploadContext(sqlConnectionString);

            var getEmployerQuery = new GetEmployerQuery(fileUploadContext);
            var employerInSql = await getEmployerQuery.Execute(employerFromFile.Account);

            var employer = EmployerMapper.Map(employerFromFile, employerInSql);
            if (employerInSql == null)
            {
                log.LogInformation($"{logPrefix} Creating Employer: {employerFromFile.Account}");
                var createEmployerCommand = new CreateEmployerCommand(fileUploadContext);
                await createEmployerCommand.Execute(employer);

                log.LogInformation($"{logPrefix} Created Employer: {employerFromFile.Account}");
            }
            else
            {
                var areEqual = employerInSql.Equals(employer);
                if (!areEqual)
                {
                    log.LogInformation($"{logPrefix} Updating Employer: {employerFromFile.Account}");

                    var updateEmployerCommand = new UpdateEmployerCommand(fileUploadContext);
                    await updateEmployerCommand.Execute(employer);

                    log.LogInformation($"{logPrefix} Updated Employer: {employerFromFile.Account}");
                }
            }

            log.LogInformation($"{logPrefix} Processed Employer: {employerFromFile.Account}");
        }

        private static string GetLogPrefix()
        {
            var logPrefix = $"{nameof(Employer)} - ThreadId={Thread.CurrentThread.ManagedThreadId}";

            return logPrefix;
        }
    }
}
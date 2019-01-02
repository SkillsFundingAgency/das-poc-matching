using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using sfa.poc.matching.functions.application.Services;
using sfa.poc.matching.functions.extensions;

namespace sfa.poc.matching.functions
{
    public static class TimerTestFunction
    {
        [FunctionName("TimerTestFunction")]
        public static void Run(
            [TimerTrigger("%CronSchedule%")]TimerInfo myTimer, 
            [Inject]ITestService testService,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var response = testService.GetMessage();
            log.LogInformation($"C# Timer trigger is running in environment {response.EnvironmentName}");
        }
    }
}

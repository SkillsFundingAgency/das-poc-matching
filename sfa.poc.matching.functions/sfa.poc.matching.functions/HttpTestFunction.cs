using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sfa.poc.matching.functions.application.Services;
using sfa.poc.matching.functions.extensions;

namespace sfa.poc.matching.functions
{
    public static class HttpTestFunction
    {
        [FunctionName("HttpTestFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Inject]ITestService testService,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            var response = testService.GetMessage();

            return (ActionResult) new JsonResult(response);
        }
    }
}

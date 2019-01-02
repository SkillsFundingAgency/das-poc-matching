using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Hosting;
using sfa.poc.matching.functions.extensions;

//There is a bug in VS where it won't automatically create the extensions.json which tells it where the Startup is
//https://github.com/Azure/Azure-Functions/issues/972
// To work around this manually creating and copying extensions.json
//https://stackoverflow.com/questions/52123538/iextensionconfigprovider-not-initializing-or-binding-with-microsoft-azure-webjob

[assembly: WebJobsStartup(typeof(sfa.poc.matching.functions.Startup))]
namespace sfa.poc.matching.functions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder
                .AddAzureStorage()
                .AddAzureStorageCoreServices()
                //.AddTimers()
                .AddInject();
        }
    }
}

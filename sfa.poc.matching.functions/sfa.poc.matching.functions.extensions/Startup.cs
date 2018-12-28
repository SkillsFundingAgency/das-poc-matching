using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using sfa.poc.matching.functions;
using sfa.poc.matching.functions.extensions;

[assembly: WebJobsStartup(typeof(Startup))]

namespace sfa.poc.matching.functions.extensions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("config.json")
            //    .Build();

            /*
var config = new ConfigurationBuilder()
    .SetBasePath(context.FunctionAppDirectory)
    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();             */

            //Use this to get values:
            //var value = Environment.GetEnvironmentVariable("your_key_here")
            var serviceName = Environment.GetEnvironmentVariable("ServiceName");

            //Registering a filter
            //builder.Services.AddSingleton<IFunctionFilter, ScopeCleanupFilter>();

            //builder.Services.AddTransient<IZipFileProcessor, ZipFileProcessor>();

            //builder.AddExtension<InjectConfiguration>();

            //Registering an extension
            //builder.AddExtension<InjectConfiguration>(); //AddExtension returns a builder that allows extending the configuration model

        }
    }
}

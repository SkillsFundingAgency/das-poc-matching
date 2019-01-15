using System;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using sfa.poc.matching.functions.application.Services;
using sfa.poc.matching.functions.core.Interfaces;

namespace sfa.poc.matching.functions.extensions
{
    public class InjectConfiguration : IExtensionConfigProvider
    {
        private IConfigurationService _configurationService;

        //Default constructor creates a configuration service.
        public InjectConfiguration()
            : this(new ConfigurationService())
        {
        }

        //This constructor is available for tests to inject a configuration service
        public InjectConfiguration(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            //TODO: Any way to inject the configuration service?
            //      Maybe a ctor that can be called from tests, 
            //      and create it here if it doesn't exist?
            var configurationService = new ConfigurationService();

            var services = new ServiceCollection();
            RegisterServices(services, configurationService);
            var serviceProvider = services.BuildServiceProvider(true);

            context
                .AddBindingRule<InjectAttribute>()
                .Bind(new InjectBindingProvider(serviceProvider));

            //See https://github.com/Azure/azure-functions-core-tools/issues/684
            //Or better: https://github.com/Azure/azure-webjobs-sdk/issues/1865#issuecomment-417958408
            // 
            // https://blog.wille-zone.de/post/azure-functions-proper-dependency-injection/

            //var registry = context.Config.GetService<IExtensionRegistry>();
            //var filter = new ScopeCleanupFilter();
            //registry.RegisterExtension(typeof(IFunctionExceptionFilter), filter);
            //var registry = context...Config.GetService<IExtensionRegistry>();
            //var filter = new ScopeCleanupFilter();
            //registry.RegisterExtension(typeof(IFunctionInvocationFilter), filter);
            //registry.RegisterExtension(typeof(IFunctionExceptionFilter), filter);

            //https://blog.jongallant.com/2018/01/azure-function-config/
        }

        private void RegisterServices(IServiceCollection services, IConfigurationService configurationService)
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory());
            //    config.AddJsonFile("local.settings.json");
            //    config.Build();

            //https://blog.jongallant.com/2018/01/azure-function-config/
            //var config = new ConfigurationBuilder()
            //    //.SetBasePath(context.FunctionAppDirectory)
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            //    .AddEnvironmentVariables()
            //    .Build();

            var configuration = configurationService.GetConfig(
                    Environment.GetEnvironmentVariable("EnvironmentName"),
                    Environment.GetEnvironmentVariable("ConfigurationStorageConnectionString"),
                    Environment.GetEnvironmentVariable("Version"),
                    Environment.GetEnvironmentVariable("ServiceName"))
                .Result;

            services.AddSingleton<IMatchingConfiguration>(configuration);

            //var connectionString = config.GetConnectionString("SqlConnectionString");

            //Read env variables and call configuration servie

            services.AddScoped<ITestRepository, TestRepository>();
            services.AddTransient<ITestService, TestService>();
        }
    }
}

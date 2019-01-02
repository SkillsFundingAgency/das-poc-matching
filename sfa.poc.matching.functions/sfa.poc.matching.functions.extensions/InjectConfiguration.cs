using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Host.Config;

namespace sfa.poc.matching.functions.extensions
{
    public class InjectConfiguration : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            //https://github.com/stevedenman/azure-functions-v2-di/tree/master/AzureFunctionV2DI
        }
    }
}

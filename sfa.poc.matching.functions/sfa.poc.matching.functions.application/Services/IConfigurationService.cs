using System.Threading.Tasks;
using sfa.poc.matching.functions.core.Interfaces;

namespace sfa.poc.matching.functions.application.Services
{
    public interface IConfigurationService
    {
        Task<IMatchingConfiguration> GetConfig(string environment, string storageConnectionString,
            string version, string serviceName);
    }
}

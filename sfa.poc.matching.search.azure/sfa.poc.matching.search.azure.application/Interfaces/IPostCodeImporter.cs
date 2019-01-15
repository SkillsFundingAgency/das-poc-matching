using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;

namespace sfa.poc.matching.search.azure.application.Interfaces
{
    public interface IPostcodeLoader
    {
        Task<PostcodeModel> RetrievePostcodeAsync(string postcode);
    }
}

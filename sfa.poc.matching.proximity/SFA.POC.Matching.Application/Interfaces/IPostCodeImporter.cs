using System.Threading.Tasks;
using SFA.POC.Matching.Application.Models;

namespace SFA.POC.Matching.Application.Interfaces
{
    public interface IPostcodeImporter
    {
        Task<PostcodeModel> RetrievePostcodeAsync(string postcode);

        Task<PostcodeModel> RetrieveRandomPostcodeAsync(); //int count);
    }
}

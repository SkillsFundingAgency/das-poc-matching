using System.IO;
using System.Threading.Tasks;

namespace Esfa.Poc.Matching.Application.Interfaces
{
    public interface IEmployerBlobStorage
    {
        Task<BlobResult> Download(Stream stream, string blobName);
    }
}
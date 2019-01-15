using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Common;
using Esfa.Poc.Matching.Entities;
using Microsoft.Azure.WebJobs;

namespace Esfa.Poc.Matching.Application.Interfaces
{
    public interface IBlobImport
    {
        Task<Result> Import(List<FileUpload> fileUploads, IAsyncCollector<string> output);
    }
}
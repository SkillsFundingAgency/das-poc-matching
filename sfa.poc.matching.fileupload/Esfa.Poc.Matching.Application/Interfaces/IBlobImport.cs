using System.IO;
using Esfa.Poc.Matching.Common;
using Esfa.Poc.Matching.Entities;
using Microsoft.Azure.WebJobs;

namespace Esfa.Poc.Matching.Application.Interfaces
{
    public interface IBlobImport
    {
        Result Import(Stream stream, FileUpload fileUpload, IAsyncCollector<string> output);
    }
}
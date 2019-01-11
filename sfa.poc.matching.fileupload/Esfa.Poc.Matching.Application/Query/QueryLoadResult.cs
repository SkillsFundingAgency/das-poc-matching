using System.Collections.Generic;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Query
{
    public class QueryLoadResult
    {
        public List<FileUploadQuery> Data { get; set; }
        public string Error { get; set; }
    }
}
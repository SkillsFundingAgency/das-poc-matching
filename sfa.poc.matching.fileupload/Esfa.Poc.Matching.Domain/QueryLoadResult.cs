using System.Collections.Generic;

namespace Esfa.Poc.Matching.Domain
{
    public class QueryLoadResult
    {
        public List<FileUploadQuery> Data { get; set; }
        public string Error { get; set; }
    }
}
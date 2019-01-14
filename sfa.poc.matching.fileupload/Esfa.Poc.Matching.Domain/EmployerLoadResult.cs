using System.Collections.Generic;

namespace Esfa.Poc.Matching.Domain
{
    public class EmployerLoadResult
    {
        public List<FileUploadEmployer> Data { get; set; }
        public string Error { get; set; }
    }
}
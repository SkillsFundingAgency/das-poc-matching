using System.Collections.Generic;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Employer
{
    public class EmployerLoadResult
    {
        public List<FileUploadEmployer> Data { get; set; }
        public string Error { get; set; }
    }
}
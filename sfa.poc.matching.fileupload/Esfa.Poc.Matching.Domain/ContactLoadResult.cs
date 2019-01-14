using System.Collections.Generic;

namespace Esfa.Poc.Matching.Domain
{
    public class ContactLoadResult
    {
        public List<FileUploadContact> Data { get; set; }
        public string Error { get; set; }
    }
}
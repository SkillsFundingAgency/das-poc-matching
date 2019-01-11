using System.Collections.Generic;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Contact
{
    public class ContactLoadResult
    {
        public List<FileUploadContact> Data { get; set; }
        public string Error { get; set; }
    }
}
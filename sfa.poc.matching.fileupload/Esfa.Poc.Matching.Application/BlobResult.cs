using System.IO;

namespace Esfa.Poc.Matching.Application
{
    public class BlobResult
    {
        public Stream Blob { get; set; }
        public bool Success { get; set; }
    }
}
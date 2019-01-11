using System.IO;

namespace Esfa.Poc.Matching.Application.Query.Readers
{
    public interface IQueryFileReader
    {
        QueryLoadResult Load(Stream stream);
    }
}
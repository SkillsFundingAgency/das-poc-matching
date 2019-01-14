using System.IO;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Common.Interfaces
{
    public interface IQueryFileReader
    {
        QueryLoadResult Load(Stream stream);
    }
}
using System.IO;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Common.Interfaces
{
    public interface IContactFileReader
    {
        ContactLoadResult Load(Stream stream);
    }
}
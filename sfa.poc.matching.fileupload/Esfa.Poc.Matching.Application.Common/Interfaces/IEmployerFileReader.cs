using System.IO;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Common.Interfaces
{
    public interface IEmployerFileReader
    {
        EmployerLoadResult Load(Stream stream);
    }
}
using System.IO;

namespace Esfa.Poc.Matching.Application.Employer.Readers
{
    public interface IEmployerFileReader
    {
        EmployerLoadResult Load(Stream stream);
    }
}
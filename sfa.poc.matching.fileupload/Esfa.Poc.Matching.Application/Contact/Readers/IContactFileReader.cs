using System.IO;

namespace Esfa.Poc.Matching.Application.Contact.Readers
{
    public interface IContactFileReader
    {
        ContactLoadResult Load(Stream stream);
    }
}
using CsvHelper;
using System.IO;
using System.Linq;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.Application.Contact.Readers
{
    public class CsvContactFileReader : IContactFileReader
    {
        private const string FailedToImportMessage = "Failed to load Contacts file. Please check the format.";

        public ContactLoadResult Load(Stream stream)
        {
            var textReader = new StreamReader(stream);
            var csv = new CsvReader(textReader);
            csv.Configuration.RegisterClassMap<ContactCsvMap>();

            var fileLoadResult = new ContactLoadResult();
            try
            {
                var fileData = csv.GetRecords<FileUploadContact>().ToList();
                fileLoadResult.Data = fileData;
            }
            catch (ReaderException re)
            {
                fileLoadResult.Error = $"{FailedToImportMessage} {re.Message} {re.InnerException?.Message}";
            }
            catch (ValidationException ve)
            {
                fileLoadResult.Error = $"{FailedToImportMessage} {ve.Message} {ve.InnerException?.Message}";
            }
            catch (BadDataException bde)
            {
                fileLoadResult.Error = $"{FailedToImportMessage} {bde.Message} {bde.InnerException?.Message}";
            }

            return fileLoadResult;
        }
    }
}
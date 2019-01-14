using System.IO;
using System.Linq;
using CsvHelper;
using Esfa.Poc.Matching.Application.Common.Interfaces;
using Esfa.Poc.Matching.Domain;

namespace Esfa.Poc.Matching.FileReader.Csv.Query
{
    public class CsvQueryFileReader : IQueryFileReader
    {
        private const string FailedToImportMessage = "Failed to load Contacts file. Please check the format.";

        public QueryLoadResult Load(Stream stream)
        {
            var textReader = new StreamReader(stream);
            var csv = new CsvReader(textReader);
            csv.Configuration.RegisterClassMap<QueryCsvMap>();

            var fileLoadResult = new QueryLoadResult();
            try
            {
                var fileData = csv.GetRecords<FileUploadQuery>().ToList();
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
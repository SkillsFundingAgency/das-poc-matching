using System;
using Esfa.Poc.Matching.Application.Query.Readers;

namespace Esfa.Poc.Matching.Application.Query
{
    public class QueryReaderFactory
    {
        public static IQueryFileReader Create(string fileType)
        {
            switch (fileType)
            {
                case ".csv":
                    return new CsvQueryFileReader();
                case ".xlsx":
                    return new ExcelQueryFileReader();
            }

            throw new InvalidOperationException();
        }
    }
}
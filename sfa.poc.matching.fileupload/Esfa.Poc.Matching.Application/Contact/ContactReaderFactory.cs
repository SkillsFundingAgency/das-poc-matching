using System;
using Esfa.Poc.Matching.Application.Common.Interfaces;
using Esfa.Poc.Matching.FileReader.Csv.Contact;
using Esfa.Poc.Matching.FileReader.Excel.Contact;

namespace Esfa.Poc.Matching.Application.Contact
{
    public class ContactReaderFactory
    {
        public static IContactFileReader Create(string fileType)
        {
            switch (fileType)
            {
                case ".csv":
                    return new CsvContactFileReader();
                case ".xlsx":
                    return new ExcelContactFileReader();
            }

            throw new InvalidOperationException();
        }
    }
}
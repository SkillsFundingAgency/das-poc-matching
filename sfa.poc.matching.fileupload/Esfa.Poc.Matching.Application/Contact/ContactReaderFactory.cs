using Esfa.Poc.Matching.Application.Contact.Readers;
using System;

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
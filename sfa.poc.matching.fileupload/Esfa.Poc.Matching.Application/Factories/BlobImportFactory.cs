using System;
using Esfa.Poc.Matching.Application.Contact;
using Esfa.Poc.Matching.Application.Employer;
using Esfa.Poc.Matching.Application.Enums;
using Esfa.Poc.Matching.Application.Interfaces;
using Esfa.Poc.Matching.Application.Query;

namespace Esfa.Poc.Matching.Application.Factories
{
    public class BlobImportFactory
    {
        public static IBlobImport Create(IFileUploadContext fileUploadContext, FileUploadType fileType)
        {
            switch (fileType)
            {
                case FileUploadType.Employer:
                    return new EmployerBlobImport();
                case FileUploadType.Contact:
                    return new ContactBlobImport();
                case FileUploadType.Query:
                    return new QueryBlobImport();
            }

            throw new InvalidOperationException();
        }
    }
}
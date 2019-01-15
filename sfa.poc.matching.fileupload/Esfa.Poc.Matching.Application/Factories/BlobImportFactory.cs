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
        public static IBlobImport Create(IFileUploadContext fileUploadContext, FileUploadType fileType, string storageAccountConnection)
        {
            var employerBlobStorage = new EmployerBlobStorage(storageAccountConnection);

            switch (fileType)
            {
                case FileUploadType.Employer:
                    return new EmployerBlobImport(new EmployerDataLoader(employerBlobStorage), new EmployerDataValidator());
                case FileUploadType.Contact:
                    return new ContactBlobImport(new ContactDataLoader(employerBlobStorage), new ContactDataValidator());
                case FileUploadType.Query:
                    return new QueryBlobImport(new QueryDataLoader(employerBlobStorage), new QueryDataValidator());
            }

            throw new InvalidOperationException();
        }
    }
}
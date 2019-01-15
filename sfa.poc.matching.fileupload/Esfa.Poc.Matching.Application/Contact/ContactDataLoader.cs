using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Common;
using Esfa.Poc.Matching.Domain;
using Esfa.Poc.Matching.Entities;

namespace Esfa.Poc.Matching.Application.Contact
{
    public class ContactDataLoader
    {
        private readonly EmployerBlobStorage _employerBlobStorage;

        public ContactDataLoader(EmployerBlobStorage employerBlobStorage)
        {
            _employerBlobStorage = employerBlobStorage;
        }

        public async Task<ReturnResult<List<FileUploadContact>>> Load(List<FileUpload> fileUploads)
        {
            var returnResult = new ReturnResult<List<FileUploadContact>>();
            foreach (var fileUpload in fileUploads)
                returnResult = await GetFileData(fileUpload);

            return returnResult;
        }

        #region Private Methods
        private async Task<ReturnResult<List<FileUploadContact>>> GetFileData(FileUpload fileUpload)
        {
            var fileData = new List<FileUploadContact>();

            using (var stream = new MemoryStream())
            {
                var blobStream = await _employerBlobStorage.Download(stream, fileUpload.Path);
                if (!blobStream.Success)
                {
                    // TODO AU Failed to download stream
                }

                var fileExtension = Path.GetExtension(fileUpload.Path);
                var reader = ContactReaderFactory.Create(fileExtension);
                var loadResult = reader.Load(blobStream.Blob);

                if (!string.IsNullOrEmpty(loadResult.Error))
                    return new ReturnResult<List<FileUploadContact>>(Result.Fail(loadResult.Error));

                fileData.AddRange(loadResult.Data);

                var returnResult = new ReturnResult<List<FileUploadContact>>(fileData, Result.Ok());

                return returnResult;
            }
        }
        #endregion
    }
}
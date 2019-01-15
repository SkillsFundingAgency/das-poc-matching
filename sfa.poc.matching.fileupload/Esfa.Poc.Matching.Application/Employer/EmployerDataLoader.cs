using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Esfa.Poc.Matching.Common;
using Esfa.Poc.Matching.Domain;
using Esfa.Poc.Matching.Entities;

namespace Esfa.Poc.Matching.Application.Employer
{
    public class EmployerDataLoader
    {
        private readonly IEmployerBlobStorage _employerBlobStorage;

        public EmployerDataLoader(IEmployerBlobStorage employerBlobStorage)
        {
            _employerBlobStorage = employerBlobStorage;
        }

        public async Task<ReturnResult<List<FileUploadEmployer>>> Load(List<FileUpload> fileUploads)
        {
            var returnResult = new ReturnResult<List<FileUploadEmployer>>();
            foreach (var fileUpload in fileUploads)
                returnResult = await GetFileData(fileUpload);

            return returnResult;
        }

        #region Private Methods
        private async Task<ReturnResult<List<FileUploadEmployer>>> GetFileData(FileUpload fileUpload)
        {
            var fileData = new List<FileUploadEmployer>();

            using (var stream = new MemoryStream())
            {
                var blobStream = await _employerBlobStorage.Download(stream, fileUpload.Path);
                if (!blobStream.Success)
                {
                    // TODO AU Failed to download stream
                }

                var fileExtension = Path.GetExtension(fileUpload.Path);
                var reader = EmployerReaderFactory.Create(fileExtension);
                var loadResult = reader.Load(blobStream.Blob);

                if (!string.IsNullOrEmpty(loadResult.Error))
                    return new ReturnResult<List<FileUploadEmployer>>(Result.Fail(loadResult.Error));

                fileData.AddRange(loadResult.Data);

                var returnResult = new ReturnResult<List<FileUploadEmployer>>(fileData, Result.Ok());
                return returnResult;
            }
        }
        #endregion
    }
}
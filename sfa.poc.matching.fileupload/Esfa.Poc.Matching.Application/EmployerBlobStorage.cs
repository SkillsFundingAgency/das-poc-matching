using System.IO;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Common;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Esfa.Poc.Matching.Application
{
    public class EmployerBlobStorage : IEmployerBlobStorage
    {
        private readonly string _storageAccountConnection;

        public EmployerBlobStorage(string storageAccountConnection)
        {
            _storageAccountConnection = storageAccountConnection;
        }

        public async Task<BlobResult> Download(Stream stream, string blobName)
        {
            var blobResult = new BlobResult();
            var cloudBlobContainer = await GetContainer(ContainerConstants.Employer);
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
            var blobExists = await cloudBlockBlob.ExistsAsync();
            if (!blobExists)
                return blobResult;

            await cloudBlockBlob.DownloadToStreamAsync(stream);
            stream.Position = 0;
            blobResult.Blob = stream;
            blobResult.Success = true;

            return blobResult;
        }

        #region Private Methods
        private async Task<CloudBlobContainer> GetContainer(string containerName)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(_storageAccountConnection);

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            return cloudBlobContainer;
        }
        #endregion
    }
}
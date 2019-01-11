using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Esfa.Poc.Matching.Application.Queries
{
    public class EmployerBlobStorage
    {
        private readonly string _storageAccountConnection;

        public EmployerBlobStorage(string storageAccountConnection)
        {
            _storageAccountConnection = storageAccountConnection;
        }

        public async Task<BlobResult> Download(string containerName, string blobName)
        {
            var blobResult = new BlobResult();
            var cloudBlobContainer = await GetContainer(containerName);
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
            var blobExists = await cloudBlockBlob.ExistsAsync();
            if (!blobExists)
                return blobResult;

            var stream = new MemoryStream();
            //using (var stream = new MemoryStream())
            //{
            await cloudBlockBlob.DownloadToStreamAsync(stream);
            stream.Position = 0;
            blobResult.Blob = stream;
            blobResult.Success = true;
            //}

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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Esfa.Poc.Matching.Web.Pages
{
    public class Upload : PageModel
    {
        private readonly IConfiguration _configuration;

        public string Message { get; set; }

        public Upload(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            // TODO Show missing files -- Go to Sql database and see whats available?
            var sqlConnectionString = _configuration.GetConnectionString("Sql");

        }

        public async Task<IActionResult> OnPostUploadFilesAsync(List<IFormFile> files)
        {
            var storageAccountConnection = _configuration.GetConnectionString("StorageAccount");
            var storageAccount = CloudStorageAccount.Parse(storageAccountConnection);
            var client = storageAccount.CreateCloudBlobClient();
            var blobContainer = client.GetContainerReference("employer");

            int uploadFileCount = 0;

            // TODO Validate file. Show errors
            foreach (var file in files)
                await UploadToBlob(file, blobContainer);

            Message = $"{uploadFileCount} files uploaded.";

            return Page();
        }

        private static async Task UploadToBlob(IFormFile file, CloudBlobContainer blobContainer)
        {
            if (file.Length > 0)
            {
                var blockBlob = blobContainer.GetBlockBlobReference(Path.GetFileName(file.FileName));
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    await blockBlob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);
                }
            }
        }
    }
}
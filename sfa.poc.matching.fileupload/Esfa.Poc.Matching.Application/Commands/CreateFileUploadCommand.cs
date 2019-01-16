using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Commands
{
    public class CreateFileUploadCommand
    {
        private readonly IFileUploadContext _fileUploadContext;

        public CreateFileUploadCommand(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task Execute(Entities.FileUpload fileUpload)
        {
            _fileUploadContext.FileUpload.Add(fileUpload);

            int createdRecordCount;
            try
            {
                createdRecordCount = await _fileUploadContext.SaveAsync();
            }
            catch (DbUpdateException due)
            {
                // Log
                throw;
            }
        }
    }
}
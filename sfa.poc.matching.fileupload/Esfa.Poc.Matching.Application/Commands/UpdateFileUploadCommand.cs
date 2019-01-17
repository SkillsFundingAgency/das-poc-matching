using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Commands
{
    public class UpdateFileUploadCommand
    {
        private readonly IFileUploadContext _fileUploadContext;

        public UpdateFileUploadCommand(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task Execute(List<Entities.FileUpload> fileUploads)
        {
            foreach (var fu in fileUploads)
                _fileUploadContext.FileUpload.Update(fu).State = EntityState.Modified;

            int updatedRecordCount;
            try
            {
                updatedRecordCount = await _fileUploadContext.SaveAsync();
            }
            catch (DbUpdateException due)
            {
                // Log
                throw;
            }
        }
    }
}
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Commands
{
    public class CreateFileUploadCommand
    {
        private readonly IFileUploadContext _dbContextService;

        public CreateFileUploadCommand(IFileUploadContext dbContextService)
        {
            _dbContextService = dbContextService;
        }

        public async Task Execute(Entities.FileUpload fileUpload)
        {
            _dbContextService.FileUpload.Add(fileUpload);

            int createdRecordCount;
            try
            {
                createdRecordCount = await _dbContextService.SaveAsync();
            }
            catch (DbUpdateException due)
            {
                // Log
                throw;
            }
        }
    }
}
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Commands
{
    public class UpdateEmployerCommand
    {
        private readonly IFileUploadContext _fileUploadContext;

        public UpdateEmployerCommand(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task Execute(Entities.Employer employer)
        {
            _fileUploadContext.Employer.Update(employer).State = EntityState.Modified;

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
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Commands
{
    public class UpdateEmployerCommand
    {
        private readonly IFileUploadContext _dbContextService;

        public UpdateEmployerCommand(IFileUploadContext dbContextService)
        {
            _dbContextService = dbContextService;
        }

        public async Task Execute(Entities.Employer employer)
        {
            _dbContextService.Employer.Update(employer).State = EntityState.Modified;

            int updatedRecordCount;
            try
            {
                updatedRecordCount = await _dbContextService.SaveAsync();
            }
            catch (DbUpdateException due)
            {
                // Log
                throw;
            }
        }
    }
}
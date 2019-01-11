using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Commands
{
    public class CreateEmployerCommand
    {
        private readonly IFileUploadContext _dbContextService;

        public CreateEmployerCommand(IFileUploadContext dbContextService)
        {
            _dbContextService = dbContextService;
        }

        public async Task Execute(Entities.Employer employer)
        {
            _dbContextService.Employer.Add(employer);

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
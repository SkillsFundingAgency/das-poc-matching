using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Queries
{
    public class GetEmployerQuery
    {
        private readonly IFileUploadContext _dbContextService;

        public GetEmployerQuery(IFileUploadContext dbContextService)
        {
            _dbContextService = dbContextService;
        }

        public async Task<Entities.Employer> Execute(Guid id)
        {
            var employer = await _dbContextService.Employer.Where(e => e.Id == id)
                .FirstAsync();

            return employer;
        }
    }
}
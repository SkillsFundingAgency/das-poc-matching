using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Queries
{
    public class GetIndustryPlacementQuery
    {
        private readonly IFileUploadContext _dbContextService;

        public GetIndustryPlacementQuery(IFileUploadContext dbContextService)
        {
            _dbContextService = dbContextService;
        }

        public async Task<Entities.IndustryPlacement> Execute(Guid id)
        {
            var industryPlacement = await _dbContextService.IndustryPlacement.Where(i => i.Id == id)
                .FirstAsync();

            return industryPlacement;
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Queries
{
    public class GetIndustryPlacementQuery
    {
        private readonly IFileUploadContext _fileUploadContext;

        public GetIndustryPlacementQuery(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task<Entities.IndustryPlacement> Execute(Guid id)
        {
            var industryPlacement = await _fileUploadContext.IndustryPlacement.Where(i => i.Id == id)
                .FirstAsync();

            return industryPlacement;
        }
    }
}
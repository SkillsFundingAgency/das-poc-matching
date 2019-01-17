using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Queries
{
    public class GetEmployerQuery
    {
        private readonly IFileUploadContext _fileUploadContext;

        public GetEmployerQuery(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task<Entities.Employer> Execute(Guid id)
        {
            var employer = await _fileUploadContext.Employer.Where(e => e.Id == id)
                    .FirstOrDefaultAsync();

            return employer;
        }

        public async Task<Entities.Employer> Execute(string companyName, DateTime createdOn)
        {
            var employer = await _fileUploadContext.Employer.Where(e => e.CompanyName == companyName
                                                                      && e.CreatedOn == createdOn)
                .FirstOrDefaultAsync();

            return employer;
        }
    }
}
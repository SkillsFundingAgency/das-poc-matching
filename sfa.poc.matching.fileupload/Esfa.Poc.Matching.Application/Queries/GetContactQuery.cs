using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Queries
{
    public class GetContactQuery
    {
        private readonly IFileUploadContext _dbContextService;

        public GetContactQuery(IFileUploadContext dbContextService)
        {
            _dbContextService = dbContextService;
        }

        public async Task<Entities.Contact> Execute(string companyName, DateTime createdOn)
        {
            var contact = await _dbContextService.Contact.Where(c => c.Employer.CompanyName == companyName
                                                && c.Employer.CreatedOn == createdOn).FirstAsync();

            return contact;
        }
    }
}
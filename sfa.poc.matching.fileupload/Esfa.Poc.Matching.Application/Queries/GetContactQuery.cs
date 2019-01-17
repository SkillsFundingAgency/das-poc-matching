using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Queries
{
    public class GetContactQuery
    {
        private readonly IFileUploadContext _fileUploadContext;

        public GetContactQuery(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task<Entities.Contact> Execute(string companyName, DateTime createdOn)
        {
            var contact = await _fileUploadContext.Contact.Where(c => c.Employer.CompanyName == companyName
                                                && c.Employer.CreatedOn == createdOn)
                .FirstOrDefaultAsync();

            return contact;
        }
    }
}
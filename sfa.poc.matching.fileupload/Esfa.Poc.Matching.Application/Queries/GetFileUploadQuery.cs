using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Esfa.Poc.Matching.Entities;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Queries
{
    public class GetFileUploadQuery
    {
        private readonly IFileUploadContext _fileUploadContext;

        public GetFileUploadQuery(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task<List<FileUpload>> Execute()
        {
            var fileUploads = await _fileUploadContext.FileUpload.Where(fu => fu.ProcessedOn == null)
                .OrderBy(fu => fu.Type)
                .ToListAsync();

            return fileUploads;
        }
    }
}
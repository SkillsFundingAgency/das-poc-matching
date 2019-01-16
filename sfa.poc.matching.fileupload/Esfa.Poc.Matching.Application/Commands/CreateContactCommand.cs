using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Commands
{
    public class CreateContactCommand
    {
        private readonly IFileUploadContext _fileUploadContext;

        public CreateContactCommand(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task Execute(Entities.Contact contact)
        {
            _fileUploadContext.Contact.Add(contact);

            int createdRecordCount;
            try
            {
                createdRecordCount = await _fileUploadContext.SaveAsync();
            }
            catch (DbUpdateException due)
            {
                // Log
                throw;
            }
        }
    }
}
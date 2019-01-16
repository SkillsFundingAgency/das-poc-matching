using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Commands
{
    public class UpdateContactCommand
    {
        private readonly IFileUploadContext _fileUploadContext;

        public UpdateContactCommand(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task Execute(Entities.Contact contact)
        {
            _fileUploadContext.Contact.Update(contact).State = EntityState.Modified;

            int updatedRecordCount;
            try
            {
                updatedRecordCount = await _fileUploadContext.SaveAsync();
            }
            catch (DbUpdateException due)
            {
                // Log
                throw;
            }
        }
    }
}
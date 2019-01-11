using Esfa.Poc.Matching.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Esfa.Poc.Matching.Application.Interfaces
{
    public interface IFileUploadContext
    {
        DbSet<Entities.Employer> Employer{ get; set; }

        DbSet<FileUpload> FileUpload { get; set; }

        void Save();
        Task<int> SaveAsync();
    }
}
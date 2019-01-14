using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Esfa.Poc.Matching.Entities;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Database
{
    public class FileUploadContext : DbContext, IFileUploadContext
    {
        public DbSet<Employer> Employer { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<IndustryPlacement> IndustryPlacement { get; set; }

        public DbSet<FileUpload> FileUpload { get; set; }

        public FileUploadContext(string connectionString) : base(GetOptions(connectionString)) { }

        private static DbContextOptions GetOptions(string connectionString) =>
            new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Employer)
                .WithMany()
                .HasForeignKey(e => new { e.EntityRefId })
                .IsRequired();
            
            // TODO Add configuration
        }

        public void Save() =>
            SaveChanges();

        public async Task<int> SaveAsync() =>
            await SaveChangesAsync();
    }
}
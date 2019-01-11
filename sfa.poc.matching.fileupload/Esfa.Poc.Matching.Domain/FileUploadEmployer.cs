using System;

namespace Esfa.Poc.Matching.Domain
{
    public class FileUploadEmployer
    {
        public Guid Account { get; set; }
        public string Checksum { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAka { get; set; }
        public string Aupa { get; set; }
        public string CompanyType { get; set; }
        public string PrimaryContact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string CountryRegion { get; set; }
        public string PostCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string Owner { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}
using System;

namespace Esfa.Poc.Matching.Domain
{
    public class FileUploadContact
    {
        public Guid Contact { get; set; }
        public string Checksum { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedOnCompany { get; set; }
        public string ContactType { get; set; }
        public string PhoneHome { get; set; }
        public string JobTitle { get; set; }
        public string PhoneMobile { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string PreferredContact { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneBusiness { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}
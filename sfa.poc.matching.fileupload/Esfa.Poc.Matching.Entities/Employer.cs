using System;
using System.ComponentModel.DataAnnotations;

namespace Esfa.Poc.Matching.Entities
{
    public class Employer
    {
        [Key]
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string AlsoKnownAs { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CompanyType { get; set; }
        public int AupaStatus { get; set; }
    }
}
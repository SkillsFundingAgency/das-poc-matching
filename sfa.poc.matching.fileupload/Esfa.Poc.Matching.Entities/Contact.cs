using System;
using System.ComponentModel.DataAnnotations;

namespace Esfa.Poc.Matching.Entities
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }
        public int ContactTypeId { get; set; }
        public int PreferredContactMethodType { get; set; }
        public bool IsPrimary { get; set; }
        public Guid EntityRefId { get; set; }
        public Employer Employer { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Esfa.Poc.Matching.Entities
{
    public class IndustryPlacement
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
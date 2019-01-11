using System;
using System.ComponentModel.DataAnnotations;

namespace Esfa.Poc.Matching.Entities
{
    public class FileUpload
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public int Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ProcessedOn { get; set; }
    }
}
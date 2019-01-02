using System;
using sfa.poc.matching.notifications.Application.Entities;

namespace sfa.poc.matching.notifications.Application.Entities
{
    public class EmailTemplate : EntityBase
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateId { get; set; }
        public string Recipients { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using sfa.poc.matching.notifications.Application.Entities;

namespace sfa.poc.matching.notifications.Application.Interfaces
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate> GetEmailTemplate(string templateName);
    }
}

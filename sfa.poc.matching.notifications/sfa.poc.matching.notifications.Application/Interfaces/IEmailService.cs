using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sfa.poc.matching.notifications.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string templateName, string toAddress, dynamic tokens, string replyToAddress);
    }
}

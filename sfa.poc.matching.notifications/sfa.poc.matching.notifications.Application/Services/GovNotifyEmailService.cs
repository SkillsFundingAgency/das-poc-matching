using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Notify.Client;
using Notify.Interfaces;
using sfa.poc.matching.notifications.Application.Configuration;
using sfa.poc.matching.notifications.Application.Interfaces;

namespace sfa.poc.matching.notifications.Application.Services
{
    public class GovNotifyEmailService : IEmailService
    {
        private readonly ILogger<GovNotifyEmailService> _logger;
        private readonly IMatchingConfiguration _configuration;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly INotificationClient _notificationClient;

        public GovNotifyEmailService(ILogger<GovNotifyEmailService> logger, IMatchingConfiguration configuration,
            IEmailTemplateRepository emailTemplateRepository, INotificationClient notificationClient)
        {
            _configuration = configuration;
            _logger = logger;
            _emailTemplateRepository = emailTemplateRepository;
            _notificationClient = notificationClient;
        }

        public async Task SendEmail(string templateName, string toAddress, dynamic tokens, string replyToAddress)
        {
            //TODO: Pass in INotificationClient to ctor, create transient with ApiKey


            //var client = new NotificationClient(_configuration.GovNotifyApiKey);


            var emailTemplate = await _emailTemplateRepository.GetEmailTemplate(templateName);
            //TODO: Make sure the id is the one we expectGet template from db
            //var templateId = "192e704d-a5db-4f77-9b20-4eb9cdff5501";
            if (emailTemplate != null)
            {
                var personalisationTokens = new Dictionary<string, dynamic>();
                foreach (var property in tokens.GetType().GetProperties())
                {
                    personalisationTokens[property.Name] = property.GetValue(tokens);
                }

                //TODO: Set up replytoid - see on https://docs.notifications.service.gov.uk/net.html#send-an-email-arguments-personalisation-optional
                _notificationClient.SendEmail(
                    emailAddress: toAddress,
                    templateId: emailTemplate.TemplateId,
                    personalisation: personalisationTokens);
            }
        }
    }
}

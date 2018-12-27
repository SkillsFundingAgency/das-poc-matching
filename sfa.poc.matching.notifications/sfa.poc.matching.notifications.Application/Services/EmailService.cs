using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using sfa.poc.matching.notifications.Application.Interfaces;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.Notifications;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.Notifications.Api.Client.Configuration;
using NotificationsApiClientConfiguration = SFA.DAS.Notifications.Api.Client.Configuration.NotificationsApiClientConfiguration;

namespace sfa.poc.matching.notifications.Application.Services
{
    public class EmailService : IEmailService
    {
        //TODO: Pass some of these in configuration or as parameters to the send method
        private const string SYSTEM_ID = "TLevelsIndustryPlacement";
        //private const string REPLY_TO_ADDRESS = "digital.apprenticeship.service @notifications.service.gov.uk";
        private const string REPLY_TO_ADDRESS = "";
        private const string SUBJECT = "Update on your application";

        private readonly ILogger<EmailService> _logger;
        //private readonly IConfigurationService _configurationService;
        private IMatchingConfiguration _configuration;
        private readonly INotificationsApi _notificationsApi;
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public EmailService(ILogger<EmailService> logger, IMatchingConfiguration configuration, IEmailTemplateRepository emailTemplateRepository)
        {
            _logger = logger;
            _configuration = configuration;

            _emailTemplateRepository = emailTemplateRepository;
            _notificationsApi = SetupNotificationApi();
        }

        public async Task SendEmail(string templateName, string toAddress, dynamic tokens, string replyToAddress)
        {
            var emailTemplate = await _emailTemplateRepository.GetEmailTemplate(templateName);

            if (emailTemplate != null)
            {
                var recipients = new List<string>();

                if (!string.IsNullOrWhiteSpace(toAddress))
                {
                    recipients.Add(toAddress.Trim());
                }

                if (!string.IsNullOrWhiteSpace(emailTemplate.Recipients))
                {
                    recipients.AddRange(emailTemplate.Recipients.Split(';').Select(x => x.Trim()));
                }

                var personalisationTokens = new Dictionary<string, string>();

                foreach (var property in tokens.GetType().GetProperties())
                {
                    personalisationTokens[property.Name] = property.GetValue(tokens);
                }

                if (string.IsNullOrWhiteSpace(replyToAddress))
                {
                    replyToAddress = REPLY_TO_ADDRESS;
                }

                foreach (var recipient in recipients)
                {
                    _logger.LogInformation($"Sending {templateName} email to {recipient}");
                    await SendEmailViaNotificationsApi(recipient, emailTemplate.TemplateId, personalisationTokens, replyToAddress);
                }
            }
        }

        private async Task SendEmailViaNotificationsApi(string toAddress, string templateId, Dictionary<string, string> personalisationTokens, string replyToAddress)
        {
            // Note: It appears that if anything is hard copied in the template it'll ignore any values below
            var email = new SFA.DAS.Notifications.Api.Types.Email
            {
                RecipientsAddress = toAddress,
                TemplateId = templateId,
                ReplyToAddress = replyToAddress,
                Subject = SUBJECT,
                SystemId = SYSTEM_ID,
                Tokens = personalisationTokens
            };

            try
            {
                await _notificationsApi.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email template {templateId} to {toAddress}");
            }
        }

        private INotificationsApi SetupNotificationApi()
        {
            // Note: This is placed here as the types used pollute the DI setup.
            // Also there wasn't any nice way to create this as an StructureMap Registry
            //var config = _configurationService.GetConfig().GetAwaiter().GetResult();

            //var apiConfiguration = new NotificationsApiClientConfiguration
            //{
            //    ApiBaseUrl = config.NotificationsApiClientConfiguration.ApiBaseUrl,
            //    ClientToken = config.NotificationsApiClientConfiguration.ClientToken,
            //    ClientId = config.NotificationsApiClientConfiguration.ClientId,
            //    ClientSecret = config.NotificationsApiClientConfiguration.ClientSecret,
            //    IdentifierUri = config.NotificationsApiClientConfiguration.IdentifierUri,
            //    Tenant = config.NotificationsApiClientConfiguration.Tenant
            //};
            var apiConfiguration = _configuration.NotificationsApiClientConfiguration;

            var httpClient = string.IsNullOrWhiteSpace(apiConfiguration.ClientId)
                ? new HttpClientBuilder().WithBearerAuthorisationHeader(new JwtBearerTokenGenerator(apiConfiguration)).Build()
                : new HttpClientBuilder().WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(apiConfiguration)).Build();

            return new NotificationsApi(httpClient, apiConfiguration);
        }
    }
}

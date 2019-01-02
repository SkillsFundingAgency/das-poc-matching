using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using sfa.poc.matching.notifications.Application.Interfaces;
using sfa.poc.matching.notifications.Services;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.Notifications.Api.Client.Configuration;
using SFA.DAS.Notifications.Api.Types;

namespace sfa.poc.matching.notifications.simpleconsole
{
    class Program
    {
        internal static IMatchingConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Configure();

            SendEmail();

            Console.WriteLine("done ...");
        }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var localConfiguration = builder.Build();

            Configuration = ConfigurationService.GetConfig(
                    localConfiguration["EnvironmentName"],
                    localConfiguration["ConfigurationStorageConnectionString"],
                    localConfiguration["Version"],
                    localConfiguration["ServiceName"])
                .Result;
        }

        private static void SendEmail()
        {
            //var apiBaseUrl = "https://at-notifications.apprenticeships.sfa.bis.gov.uk/";
            var apiBaseUrl = Configuration.NotificationsApiClientConfiguration.ApiBaseUrl;

            //var token = "<your token>";
            var token = Configuration.NotificationsApiClientConfiguration.ClientToken;

            var config = new NotificationsApiClientConfiguration
            {
                ApiBaseUrl = apiBaseUrl,
                ClientToken = token
            };

            IGenerateBearerToken jwtToken = new JwtBearerTokenGenerator(config);

            var httpClient = new HttpClientBuilder()
                .WithBearerAuthorisationHeader(jwtToken)
                .WithDefaultHeaders()
                .Build();

            var apiClient = new NotificationsApi(httpClient, config);

            var recipients = Configuration.DefaultEmailAddress;
            var sender = Configuration.DefaultEmailSenderAddress;
            var replyTo = Configuration.DefaultEmailReplyToAddress;

            var customFields = new Dictionary<string, string>
            {
                { "UserEmailAddress", sender },
                { "UserFullName", "Test User" },
                { "UserEnquiry", "I have a question" },
                { "UserEnquiryDetails", "Wanted to have different appSettings for debug and release when building your app ? " }
            };

            var c = new Email
            {
                Tokens = customFields,
                RecipientsAddress = recipients,
                ReplyToAddress = replyTo,
                SystemId = "xyz",
                TemplateId = "VacancyService_CandidateContactUsMessage",
                Subject = "hello"
            };

            apiClient.SendEmail(c).Wait();
        }
    }
}

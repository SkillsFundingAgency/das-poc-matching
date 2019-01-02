using sfa.poc.matching.notifications.Application.Interfaces;
using SFA.DAS.Notifications.Api.Client.Configuration;

namespace sfa.poc.matching.notifications.Application.Configuration
{
    public class MatchingConfiguration : IMatchingConfiguration
    {
        public NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        public string SqlConnectionString { get; set; }

        public string DefaultEmailAddress { get; set; }

        public string DefaultEmailSenderAddress { get; set; }

        public string DefaultEmailReplyToAddress { get; set; }
    }
}

using sfa.poc.matching.notifications.Application.Interfaces;
using SFA.DAS.Notifications.Api.Client.Configuration;

namespace sfa.poc.matching.notifications.Application.Configuration
{
    public class MatchingConfiguration : IMatchingConfiguration
    {
        public NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        public string SqlConnectionString { get; set; }
    }
}

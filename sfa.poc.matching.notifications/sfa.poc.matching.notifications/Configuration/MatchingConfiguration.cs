
using SFA.DAS.Notifications.Api.Client.Configuration;

namespace sfa.poc.matching.notifications.Configuration
{
    public class MatchingConfiguration : IMatchingConfiguration
    {
        public NotificationsApiClientConfiguration NotificationsApiClient { get; set; }
    }
}

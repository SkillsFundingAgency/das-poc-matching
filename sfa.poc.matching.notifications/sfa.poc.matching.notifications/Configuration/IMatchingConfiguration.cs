
using SFA.DAS.Notifications.Api.Client.Configuration;

namespace sfa.poc.matching.notifications.Configuration
{
    public interface IMatchingConfiguration
    {
        NotificationsApiClientConfiguration NotificationsApiClient { get; set; }
    }
}

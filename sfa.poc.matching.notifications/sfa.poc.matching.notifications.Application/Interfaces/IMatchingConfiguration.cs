using SFA.DAS.Notifications.Api.Client.Configuration;

namespace sfa.poc.matching.notifications.Application.Interfaces
{
    public interface IMatchingConfiguration
    {
        NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        string SqlConnectionString { get; set; }

        string DefaultEmailAddress { get; set; }

        string DefaultEmailSenderAddress { get; set; }

        string DefaultEmailReplyToAddress { get; set; }
    }
}

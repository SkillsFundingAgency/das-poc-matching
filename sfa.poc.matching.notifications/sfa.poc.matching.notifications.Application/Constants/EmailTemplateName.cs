
namespace sfa.poc.matching.notifications.Application.Constants
{
    public static class EmailTemplateName
    {
        /// <summary>
        /// Requires tokens: { contactname }
        /// </summary>
        public const string APPLY_SIGNUP_ERROR = "ApplySignupError";

        public const string CANDIDATE_CONTACT_US = "VacancyService_CandidateContactUsMessage";

        /// <summary>
        /// Requires tokens: { first_name }
        /// </summary>
        public const string GOV_NOTIFY_TEST = "Test_Template";
    }
}

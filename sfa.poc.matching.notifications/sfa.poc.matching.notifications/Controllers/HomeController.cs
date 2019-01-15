using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sfa.poc.matching.notifications.Application.Constants;
using sfa.poc.matching.notifications.Application.Interfaces;
using sfa.poc.matching.notifications.ViewModels;

namespace sfa.poc.matching.notifications.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;

        public HomeController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View(new EmailViewModel
            {
                Template = EmailTemplateName.CANDIDATE_CONTACT_US,
                Tokens = "Contact Name",
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmailViewModel email)
        {
            if(ModelState.IsValid)
            {
                var template = !string.IsNullOrWhiteSpace(email.Template)
                    ? email.Template
                    : EmailTemplateName.GOV_NOTIFY_TEST;

                //var tokens = new { contactname = email.Tokens };
                //var customFields = new 
                //{
                //    UserEmailAddress = email.ReplyTo,
                //    UserFullName = "Test User",
                //    UserEnquiry = "I have a question",
                //    UserEnquiryDetails = "Wanted to have different appSettings for debug and release when building your app ? "
                //};

                var tokens = template == EmailTemplateName.CANDIDATE_CONTACT_US
                    ? (dynamic)new
                        {
                            UserEmailAddress = email.ReplyTo,
                            UserFullName = "Test User",
                            UserEnquiry = "I have a question",
                            UserEnquiryDetails = "Wanted to have different appSettings for debug and release when building your app ? "
                        }
                    : new { first_name = email.Tokens };

                await _emailService.SendEmail(
                    template, 
                    email.EmailTo,
                    //template == EmailTemplateName.CANDIDATE_CONTACT_US ? (dynamic)customFields : tokens,
                    tokens,
                    email.ReplyTo);

                return View(email);
            }
            else {
                // there is something wrong with the data values
                return View(email);
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

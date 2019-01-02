using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sfa.poc.matching.notifications.Application.Constants;
using sfa.poc.matching.notifications.Application.Interfaces;
using sfa.poc.matching.notifications.ViewModels;
using SFA.DAS.Notifications.Api.Client.Configuration;

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
                Template = EmailTemplateName.APPLY_SIGNUP_ERROR,
                Tokens = "Contact Name",
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmailViewModel email)
        {
            if(ModelState.IsValid) {

                await _emailService.SendEmail(
                    !string.IsNullOrWhiteSpace(email.Template)
                        ? email.Template
                        : EmailTemplateName.APPLY_SIGNUP_ERROR, 
                    email.EmailTo,
                    //new { contactname = $"{existingContact.GivenNames} {existingContact.FamilyName}" },
                    new { contactname = email.Tokens },
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

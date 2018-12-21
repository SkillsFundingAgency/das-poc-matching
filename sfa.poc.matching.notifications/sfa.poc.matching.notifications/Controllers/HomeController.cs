using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sfa.poc.matching.notifications.Models;
using sfa.poc.matching.notifications.ViewModels;
using SFA.DAS.Notifications.Api.Client.Configuration;

namespace sfa.poc.matching.notifications.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(NotificationsApiClientConfiguration notificationsApiClientConfig)
        {
            return View(new EmailViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmailViewModel email)
        {
            if(ModelState.IsValid) {
                //TODO: Use API to send the message

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

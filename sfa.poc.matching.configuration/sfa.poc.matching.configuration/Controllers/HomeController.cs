using Microsoft.AspNetCore.Mvc;
using sfa.poc.matching.configuration.Configuration;

namespace sfa.poc.matching.configuration.Controllers
{
    public class HomeController : Controller
    {
        private IWebConfiguration Configuration { get; }

        public HomeController(IWebConfiguration config)
        {
            Configuration = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(Configuration);
        }
    }
}
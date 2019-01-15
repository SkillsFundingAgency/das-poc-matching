using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sfa.poc.matching.search.azure.application.Interfaces;
using sfa.poc.matching.search.azure.web.ViewModels;

namespace sfa.poc.matching.search.azure.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostcodeLoader _postcodeLoader;
        private readonly ISearchProvider _searchProvider;

        public HomeController(IPostcodeLoader postcodeLoader, ISearchProvider searchProvider)
        {
            _postcodeLoader = postcodeLoader;
            _searchProvider = searchProvider;
        }

        public IActionResult Index()
        {
            return View(new SearchViewModel
            {
                SearchRadius = 5
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(SearchViewModel model)
        {
            var postcode = await _postcodeLoader.RetrievePostcodeAsync(model.Postcode);
            model.SearchResults = (await _searchProvider.SearchCombinedIndex(
                model.SearchText,
                postcode?.Latitude ?? 0,
                postcode?.Longitude ?? 0,
                model.SearchRadius)).ToList();
            
            return View(model);
        }
    }
}
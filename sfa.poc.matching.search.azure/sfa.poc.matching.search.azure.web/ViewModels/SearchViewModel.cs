using System.Collections.Generic;
using sfa.poc.matching.search.azure.application.Entities;

namespace sfa.poc.matching.search.azure.web.ViewModels
{
    public class SearchViewModel
    {
        public string Postcode { get; set; }

        public string SearchText { get; set; }

        public decimal SearchRadius { get; set; }

        public IList<CombinedIndexedItem> SearchResults { get; set; }
        
        public SearchViewModel()
        {
            //SearchResults = new List<CombinedIndexedItem>();
        }
    }
}

using System.Collections.Generic;
using SFA.POC.Matching.Application.Models;

namespace SFA.POC.Matching.Proximity.Web.ViewModels
{
    public class LocationSearchViewModel
    {
        public string Postcode { get; set; }

       // private readonly List<Location> _searchResults = new List<Location>();

        public IList<LocationModel> SearchResults { get; set; }

        public decimal SearchRadius { get; set; }

        public LocationSearchViewModel()
        {
            SearchResults = new List<LocationModel>();
        }
    }
}

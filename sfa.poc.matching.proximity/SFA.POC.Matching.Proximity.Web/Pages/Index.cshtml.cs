using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SFA.POC.Matching.Application.Interfaces;
using SFA.POC.Matching.Application.Models;
using SFA.POC.Matching.Proximity.Infrastructure.Configuration;
using SFA.POC.Matching.Proximity.Web.ViewModels;

namespace SFA.POC.Matching.Proximity.Web.Pages
{
    public class IndexModel : PageModel
    {
        //https://forums.asp.net/t/2135142.aspx?How+to+Post+data+in+ASP+NET+Core+2+Razor+Pages

        private readonly IMatchingConfiguration _configuration;
        private readonly ILocationReader _locationReader;
        private readonly IPostcodeImporter _postcodeImporter;

        [BindProperty]
        public LocationSearchViewModel LocationSearch { get; set; }

        public bool ShowSearchResults { get; set; }

        public string MapMarkers { get; set; }

        //[BindProperty]
        public string GoogleMapsApiKey { get; set; }

        public IndexModel(IMatchingConfiguration configuration, ILocationReader locationReader, IPostcodeImporter postcodeImporter)
        {
            _configuration = configuration;
            _locationReader = locationReader;
            _postcodeImporter = postcodeImporter;

            GoogleMapsApiKey = configuration.GoogleMapsApiKey;
        }

        public void OnGet()
        {
            if (LocationSearch == null)
            {
                LocationSearch = new LocationSearchViewModel()
                {
                    Postcode = "CV1 2HJ", //Default post code
                    SearchRadius = 25m //Default search radius
                };
            }

            //return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                //Process error
            }

            //Lookup postcode to get lat long
            var postCodeResult = await _postcodeImporter.RetrievePostcodeAsync(LocationSearch.Postcode);

            //A bit clunky - the importer will save to DB as well as returning the postcode.
            //TODO: Refactor so the randomisation is done in a loop in App.cs, then the save is done from there
            //TODO: Remove ILocationWriter from ctor of post code importer
            
            var searchRadiusInMeters = LocationSearch.SearchRadius * Application.Constants.MilesToMeters;

            //TODO: Return a message if the post code search fails or has no location
            if (postCodeResult != null && postCodeResult.Latitude.HasValue && postCodeResult.Longitude.HasValue)
            {
                //LocationSearch.SearchResults = await _locationReader.SearchLocationsWithByDistanceAsync(postcode, searchRadiusInMeters);
                LocationSearch.SearchResults = await _locationReader.SearchLocationsWithByDistanceAsync(
                    postCodeResult.Latitude.Value,
                    postCodeResult.Longitude.Value,
                    searchRadiusInMeters);

                MapMarkers = LoadMap(LocationSearch.SearchResults);

                ShowSearchResults = true;
            }
            
            return Page();
        }

        private string LoadMap(IList<LocationModel> searchResults)
        {
            var markers = new StringBuilder("[");

            foreach (var searchResult in searchResults)
            {
                markers.Append("{");
                markers.Append($"'title': '{searchResult.Postcode}',");
                markers.Append($"'lat': '{searchResult.Latitude}',");
                markers.Append($"'lng': '{searchResult.Longitude}',");

                markers.Append($"'description': '{searchResult.Postcode}");
                if (!String.IsNullOrEmpty(searchResult.Region))
                {
                    markers.Append($" {WebUtility.HtmlEncode(searchResult.Region)}");
                }
                markers.Append("'");

                markers.Append("},");
            }
            markers.Append("];");

            return markers.ToString();
        }
    }
}

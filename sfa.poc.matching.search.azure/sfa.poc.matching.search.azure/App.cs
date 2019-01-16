using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Interfaces;
using sfa.poc.matching.search.azure.application.Search;

namespace sfa.poc.matching.search.azure
{
    public class App
    {
        //private readonly ISearchConfiguration _configuration;
        private readonly ISearchService _searchService;
        //private readonly ISqlDataRepository _sqlDataRepository;

        private const decimal DefaultLatitude = 52.400997M;
        private const decimal DefaultLongitude = -1.508122M;
        private const decimal DefaultRadius = 25M;

        public App(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task Run()
        {
            ShowOptions();

            do
            {
                try
                {
                    Console.WriteLine("Enter something to search for, or q to exit.");
                    var input = Console.ReadLine();

                    if (input?.ToUpper() == "Q")
                    {
                        break;
                    }

                    var tokens = input?.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (input == "?")
                    {
                        ShowOptions();
                    }
                    //else if (string.Compare(input, "/index", StringComparison.CurrentCultureIgnoreCase) == 0
                    //    || string.Compare(input, "/i", StringComparison.CurrentCultureIgnoreCase) == 0)
                    else if (tokens?.Length > 0 && 
                             (tokens[0].ToLower() == "/index" 
                              || tokens[0].ToLower() =="/i"))
                    {
                        var options = IndexingOptions.Default;
                        if (tokens.Any(t => t == "/syn"))
                        {
                            options |= IndexingOptions.UseSynonyms;
                        }
                        //Recreate the search index
                        await _searchService.Index(options);
                    }
                    else if (string.Compare(input, "/loc", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        await PerformLocationSearch(DefaultLatitude, DefaultLongitude, DefaultRadius);
                    }
                    else if (string.Compare(input, "/c ", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        await PerformCourseSearch(input);
                    }
                    else if (!string.IsNullOrEmpty(input))
                    {
                        await PerformFullSearch(input);
                    }
                    else
                    {
                        Console.WriteLine($"{DefaultLatitude} {DefaultLongitude} {DefaultRadius} plumber");
                        await PerformFullSearch(new[]
                        {
                            DefaultLatitude.ToString(CultureInfo.InvariantCulture),
                            DefaultLongitude.ToString(CultureInfo.InvariantCulture),
                            DefaultRadius.ToString(CultureInfo.InvariantCulture),
                            "plumber"
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            } while (true);

            Console.WriteLine("Done ...");
        }

        private void ShowOptions()
        {
            Console.WriteLine("Please enter one of the following options:");
            Console.WriteLine("    a location followed by keywords to run a combined search");
            Console.WriteLine("    /c plus some keywords to run a course search");
            Console.WriteLine("    /loc to run a default location search");
            Console.WriteLine("    Return with no input to run a sample combined search");
            Console.WriteLine("    q to exit.");
            Console.WriteLine("    ? to see these options again");
        }

        private async Task PerformCourseSearch(string input)
        {
            var results = (await _searchService.SearchCourses(input)).ToList();
            Console.WriteLine($"Found {results.Count} results.");
            foreach (var course in results)
            {
                Console.WriteLine($"{course.Id}, {course.Name}");
            }
        }

        private async Task PerformLocationSearch(decimal latitude, decimal longitude, decimal radius)
        {
            var results = (await _searchService.SearchLocations(latitude, longitude, radius)).ToList();
            Console.WriteLine($"Found {results.Count} results.");
            foreach (var location in results)
            {
                Console.WriteLine($"{location.Postcode}, ({location.Latitude}, {location.Latitude})");
            }
        }

        private async Task PerformFullSearch(string input)
        {
            //Crude way of getting lat/long/distance
            var tokens = input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            await PerformFullSearch(tokens);
        }

        private async Task PerformFullSearch(IReadOnlyList<string> tokens)
        {
            decimal latitude = 0;
            decimal longitude = 0;
            decimal radius = 0;

            ////Crude way of getting lat/long/distance
            //if (tokens.Count > 1) decimal.TryParse(tokens[0], out latitude);
            //if (tokens.Count > 2) decimal.TryParse(tokens[1], out longitude);
            //if (tokens.Count > 3) decimal.TryParse(tokens[2], out radius);

            var searchText = "";
            //Need to remove the numeric tokens
            for (var i = 0; i < tokens.Count; i++)
            {
                var handled = false;
                switch (i)
                {
                    case 0:
                        handled = decimal.TryParse(tokens[i], out latitude);
                        break;
                    case 1:
                        handled = decimal.TryParse(tokens[i], out longitude);
                        break;
                    case 2:
                        handled = decimal.TryParse(tokens[i], out radius);
                        break;
                }

                if (!handled)
                {
                    if (searchText.Length > 0)
                    {
                        searchText += " ";
                    }

                    searchText += tokens[i];
                }
            }

            var results = (await _searchService.SearchCombinedIndex(searchText, latitude, longitude, radius)).ToList();
            Console.WriteLine($"Found {results.Count} results.");
            foreach (var searchResult in results)
            {
                if (latitude != 0 && longitude != 0 && radius != 0)
                {
                    Console.WriteLine($"{searchResult.Postcode}, {searchResult.Distance} miles, ({searchResult.Location.Latitude}, {searchResult.Location.Latitude}) - {searchResult.ProviderName} has course {searchResult.LarsId} '{searchResult.CourseName}'");
                }
                else
                {
                    Console.WriteLine($"{searchResult.Postcode}, ({searchResult.Location.Latitude}, {searchResult.Location.Latitude}) - {searchResult.ProviderName} has course {searchResult.LarsId} '{searchResult.CourseName}'");
                }
            }
        }
    }
}

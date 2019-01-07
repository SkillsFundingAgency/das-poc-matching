using System;
using System.Linq;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Configuration;
using sfa.poc.matching.search.azure.application.Interfaces;

namespace sfa.poc.matching.search.azure
{
    public class App
    {
        private readonly ISearchConfiguration _configuration;

        private readonly ISearchService _searchService;
        private readonly ISqlDataRepository _sqlDataRepository;

        public App(ISearchConfiguration configuration, ISearchService searchService, ISqlDataRepository sqlDataRepository)
        {
            _configuration = configuration;
            _searchService = searchService;
            _sqlDataRepository = sqlDataRepository;
        }

        public async Task Run()
        {
            Console.WriteLine("App ready");

            do
            {
                try
                {
                    Console.WriteLine("Enter something to search for, or q to exit.");
                    var input = Console.ReadLine();
                    if (input != null && input.ToUpper().StartsWith('Q'))
                    {
                        break;
                    }

                    if (string.Compare(input, "/index", StringComparison.CurrentCultureIgnoreCase) == 0
                     || string.Compare(input, "/i", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        //Recreate the search index
                        await _searchService.Index();
                    }
                    if (string.Compare(input, "/loc", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        //Location search within 5 miles of CV1 2WT
                        var results = (await _searchService.SearchLocations(52.400997M, -1.508122M, 5M)).ToList();
                        Console.WriteLine($"Found {results.Count} results.");
                        foreach (var location in results)
                        {
                            Console.WriteLine($"{location.Postcode}, ({location.Latitude}, {location.Latitude})");
                        }
                    }
                    else if (input.Length > 0)
                    {
                        //Crude way of getting lat/long/distance
                        var tokens = input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tokens.Length == 3
                            && decimal.TryParse(tokens[0], out decimal latitude)
                            && decimal.TryParse(tokens[1], out decimal longitude)
                            && decimal.TryParse(tokens[2], out decimal distance))
                        {
                            var results = (await _searchService.SearchLocations(latitude, longitude, distance)).ToList();
                            Console.WriteLine($"Found {results.Count} results.");
                            foreach (var location in results)
                            {
                                Console.WriteLine($"{location.Postcode}, ({location.Latitude}, {location.Latitude})");
                            }
                        }
                        else
                        {
                            var results = (await _searchService.SearchCourses(input)).ToList();
                            Console.WriteLine($"Found {results.Count} results.");
                            foreach (var course in results)
                            {
                                Console.WriteLine($"{course.Id}, {course.Name}");
                            }
                        }
                    }
                    else
                    {
                        await RunDefaultAction();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            } while (true);

            Console.WriteLine("Done ...");
        }

        private async Task RunDefaultAction()
        {
        }
    }
}

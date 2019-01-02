using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Configuration;
using sfa.poc.matching.search.azure.application.Entities;
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
            System.Console.WriteLine("App ready");

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
                    else if (input.Length > 0)
                    {
                        var results = await _searchService.Search(input);
                        foreach (var course in results)
                        {
                            Console.WriteLine($"{course.Id}, {course.Name}");
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
            await TestPaging();
        }

        private async Task TestPaging()
        {
            Console.WriteLine("Testing Course paging:");
            var page = 1;
            var pageSize = 10;
            IList<Course> courses = null;
            do
            {
                courses = (await _sqlDataRepository.GetPageOfCourses(page, pageSize)).ToList();
                foreach (var course in courses)
                {
                    Console.WriteLine($"{course.Id}\t{course.Name}");
                }

                page++;
            } while (courses.Any() && page <= 3);

            Console.WriteLine("");
            Console.WriteLine("Testing Location paging:");
            page = 1;
            IList<Location> locations = null;
            do
            {
                locations = (await _sqlDataRepository.GetPageOfLocations(page, pageSize)).ToList();
                //TODO: Convert to list
                foreach (var location in locations)
                {
                    Console.WriteLine($"{location.Id}\t{location.Postcode} ({location.Latitude},{location.Longitude})");
                }

                page++;
            } while (locations.Any() && page <= 3);
        }
    }
}

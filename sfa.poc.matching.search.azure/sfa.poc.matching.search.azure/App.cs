using System;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Configuration;

namespace sfa.poc.matching.search.azure
{
    public class App
    {
        private ISearchConfiguration _configuration;

        public App(ISearchConfiguration configuration)
        {
            _configuration = configuration;
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
                    if (input.ToUpper().StartsWith('Q'))
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            } while (true);

            Console.WriteLine("Done ...");
        }
    }
}

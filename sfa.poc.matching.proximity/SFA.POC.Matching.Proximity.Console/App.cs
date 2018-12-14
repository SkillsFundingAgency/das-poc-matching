using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.POC.Matching.Application.Interfaces;
using SFA.POC.Matching.Application.Models;

namespace SFA.POC.Matching.Proximity.Console
{
    public class App
    {
        private readonly IPostcodeImporter _postcodeImporter;
        private readonly ILocationWriter _locationWriter;

        public App(IPostcodeImporter postcodeImporter, ILocationWriter locationWriter)
        {
            _postcodeImporter = postcodeImporter;
            _locationWriter = locationWriter;
        }

        public async Task Run()
        {
            System.Console.WriteLine("App ready");

            do
            {
                try
                {
                    IEnumerable<PostcodeModel> postcodes = null;

                    System.Console.WriteLine("Enter a post code, \"r\" followed by a number of random post codes to generate, or q to exit.");
                    var input = System.Console.ReadLine();
                    if (input.StartsWith('q') || input.StartsWith('Q'))
                    {
                        break;
                    }
                    else if (input.StartsWith("/r", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //Assume string starts.with "/r" so remove first 2 chars - the rest should be the number of items to generate
                        if (!int.TryParse(input.Remove(0, 2), out var numberOfCodesToGenerate))
                        {
                            numberOfCodesToGenerate = 1;
                        }
                        postcodes = await GetRandomPostcodes(numberOfCodesToGenerate);
                    }
                    else
                    {
                        postcodes = await GetPostcode(input);
                    }

                    if (postcodes != null)
                    {
                        /*
                        foreach (var postcode in postcodes.Where(p => !String.IsNullOrEmpty(p.Postcode)))
                        {
                            System.Console.WriteLine($"Found {postcode.Postcode} at {postcode.Latitude}, {postcode.Longitude}");

                            if (!postcode.Latitude.HasValue || !postcode.Longitude.HasValue)
                            {
                                System.Console.Write($"Skipping postcode '{postcode.Postcode}' because it has a null location");
                                //TODO: Do another lookup on first part of postcode
                            }
                            else
                            {
                                var location = new Location
                                {
                                    Postcode = postcode.Postcode,
                                    Longitude = postcode.Longitude.Value,
                                    Latitude = postcode.Latitude.Value,
                                    Country = postcode.Country,
                                    Region = postcode.Region,
                                    AdminCounty = postcode.Admin_County,
                                    AdminDistrict = postcode.Admin_District
                                };
                                await _locationWriter.SaveAsync(location);
                            }
                        }
                        */
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                }

            } while (true);

            System.Console.WriteLine("Done ...");
            //System.Console.ReadKey();
        }

        private async Task<IEnumerable<PostcodeModel>> GetPostcode(string postCode)
        {
            var postCodeResult = await _postcodeImporter.RetrievePostcodeAsync(postCode);
            return new List<PostcodeModel> { postCodeResult };
        }

        private async Task<IEnumerable<PostcodeModel>> GetRandomPostcodes(int numberOfCodesToGenerate)
        {
            var list = new List<PostcodeModel>();

            for (var i = 0; i < numberOfCodesToGenerate; i++)
            {
                list.Add(await _postcodeImporter.RetrieveRandomPostcodeAsync());
            }

            return list;
        }
    }
}

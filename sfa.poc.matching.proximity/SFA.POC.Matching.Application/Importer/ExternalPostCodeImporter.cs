using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFA.POC.Matching.Application.Interfaces;
using SFA.POC.Matching.Application.Models;
using SFA.POC.Matching.Proximity.Infrastructure.Configuration;

namespace SFA.POC.Matching.Application.Importer
{
    public class ExternalPostcodeImporter : IPostcodeImporter
    {
        private readonly ILocationWriter _locationWriter;
        private readonly IMatchingConfiguration _configuration;

        public ExternalPostcodeImporter(ILocationWriter locationWriter, IMatchingConfiguration configuration)
        {
            _configuration = configuration;
            _locationWriter = locationWriter;
        }

        public async Task<PostcodeModel> RetrievePostcodeAsync(string postcode)
        {
            var postcodeModel = await GetPostcodeAsync(postcode);

            //Don't put this retry inside GetPostcodeAsync as it might cause an infinite recursive loop
            postcodeModel = await CheckLocationAndRetryUsingOutcodeAsync(postcodeModel);

            await SavePostCode(postcodeModel);

            return postcodeModel;
        }

        public async Task<PostcodeModel> RetrieveRandomPostcodeAsync() //int count)
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync($"/random/postcodes");

            if (response.IsSuccessStatusCode)
            {
                var postcodeModel = await ConvertToPostcodeModel(response);
                postcodeModel = await CheckLocationAndRetryUsingOutcodeAsync(postcodeModel);

                await SavePostCode(postcodeModel);

                return postcodeModel;
            }

            return null;
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_configuration.PostcodeRetrieverBaseUrl)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private async Task<PostcodeModel> GetPostcodeAsync(string postcode)
        {
            var client = CreateHttpClient();

            var encodedPostcode = WebUtility.UrlEncode(postcode);
            var response = await client.GetAsync($"?q={encodedPostcode}");
            if (response.IsSuccessStatusCode)
            {
                var postCodeModel = await ConvertToPostcodeModel(response);
                return postCodeModel;
            }

            //TODO: Handle not found 
            return null;
        }

        private async Task<PostcodeModel> CheckLocationAndRetryUsingOutcodeAsync(PostcodeModel postcodeModel)
        {
            if (!postcodeModel.Latitude.HasValue || !postcodeModel.Longitude.HasValue)
            {
                postcodeModel = await GetPostcodeAsync(postcodeModel.OutCode);
            }

            return postcodeModel;
        }

        private async Task<PostcodeModel> ConvertToPostcodeModel(HttpResponseMessage response)
        {
#if DEBUG
            //String for debugging use
            //var stringResult = await response.Content.ReadAsStringAsync();
            //Debug.Print(stringResult);
#endif

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var token = JToken.Load(jsonReader);
                var resultToken = token.SelectToken("$.result");

                if (resultToken is JArray)
                {
                    var targetObjectList = resultToken.ToObject<List<PostcodeModel>>();
                    //TODO: Extend to work with multiple items - just taking the first one for now
                    var targetObject = targetObjectList.First();
                    return targetObject;
                }
                else
                {
                    try
                    {
                        var targetObject = resultToken.ToObject<PostcodeModel>();

                        //Properties in the model are named the same as the ones in json.
                        //This could be improved in a future version of this code.
                        //targetObject.Admin_County = resultToken["admin_county"].Value<string>();
                        //targetObject.Admin_District = resultToken["Admin_district"].Value<string>();

                        return targetObject;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }

        private async Task SavePostCode(PostcodeModel postcode)
        {
            if (!postcode.Latitude.HasValue || !postcode.Longitude.HasValue)
            {
                Console.WriteLine($"Skipping postcode '{postcode.Postcode}' because it has a null location");
                //TODO: Do another lookup on first part of postcode
            }
            else
            {
                Console.WriteLine($"Saving {postcode.Postcode} at {postcode.Latitude}, {postcode.Longitude}");

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

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sfa.poc.matching.search.azure.application.Configuration;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Interfaces;

namespace sfa.poc.matching.search.azure.application.Services
{
    public class ExternalPostcodeLoaderService : IPostcodeLoader
    {
        private readonly ISearchConfiguration _configuration;

        public ExternalPostcodeLoaderService(ISearchConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<PostcodeModel> RetrievePostcodeAsync(string postcode)
        {
            var postcodeModel = await GetPostcodeAsync(postcode);

            //Don't put this retry inside GetPostcodeAsync as it might cause an infinite recursive loop
            postcodeModel = await CheckLocationAndRetryUsingOutcodeAsync(postcodeModel);

            return postcodeModel;
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
            if (postcodeModel != null 
                && (!postcodeModel.Latitude.HasValue || !postcodeModel.Longitude.HasValue))
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
    }
}
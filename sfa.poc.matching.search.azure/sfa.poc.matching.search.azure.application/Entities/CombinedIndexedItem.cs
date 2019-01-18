using System.Diagnostics;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using Newtonsoft.Json;
using sfa.poc.matching.search.azure.application.Search;

namespace sfa.poc.matching.search.azure.application.Entities
{
    [SerializePropertyNamesAsCamelCase]
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ", nq}")]
    public class CombinedIndexedItem
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }

        public int CourseId { get; set; }

        public string LarsId { get; set; }
        
        //TODO: Define a custom search analyzer, use "standard" for 
        //       - see https://azure.microsoft.com/en-gb/blog/custom-analyzers-in-azure-search/
        [IsSearchable, IsFilterable, IsSortable]
        //[IndexAnalyzer("standardasciifolding.lucene")]
        [IndexAnalyzer(SearchConstants.AdvancedAnalyzerName)]
        //[IndexAnalyzer(AnalyzerName.AsString.EnMicrosoft)]
        //[SearchAnalyzer(AnalyzerName.AsString.EnMicrosoft)]
        //[SearchAnalyzer("standard")]
        //[SearchAnalyzer("standard.lucene")]
        [SearchAnalyzer(SearchConstants.AdvancedAnalyzerName)]
        //[Analyzer(SearchConstants.AdvancedAnalyzerName)]
        //[SynonymMap(Name = "course - synonymmap")]
        public string CourseName { get; set; }

        [IsSearchable, IsFilterable]
        //[Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        //[Analyzer(SearchConstants.AdvancedAnalyzerName)]
        public string CourseDescription { get; set; }

        public int ProviderId { get; set; }

        [IsSearchable, IsFilterable]
        [Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        //[Analyzer(SearchConstants.AdvancedAnalyzer_2_Name)]
        public string ProviderName { get; set; }

        //Fields from location
        public int LocationId { get; set; }

        [IsSearchable, IsFilterable]
        public string Postcode { get; set; }

        [JsonIgnore]
        public decimal Latitude { get; set; }

        [JsonIgnore]
        public decimal Longitude { get; set; }

        [IsFilterable, IsSortable]
        public GeographyPoint Location { get; set; }

        [JsonIgnore]
        public double? Distance { get; set; }

        [IsFilterable, IsFacetable, IsSortable]
        public string Country { get; set; }

        [IsFilterable, IsFacetable, IsSortable]
        public string Region { get; set; }

        [IsFilterable, IsFacetable, IsSortable]
        public string AdminDistrict { get; set; }

        [IsFilterable, IsFacetable, IsSortable]
        public string AdminDistrictCode { get; set; }

        [IsFilterable, IsFacetable, IsSortable]
        public string AdminCounty { get; set; }

        //This is used to display search score after results are returned.
        [JsonIgnore]
        public double SearchScore { get; set; }

        private string DebuggerDisplay => $"CoCombinedIndexedItem: {Id} {CourseName} {Postcode} {ProviderName}";
    }
}

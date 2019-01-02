using System.Diagnostics;

namespace sfa.poc.matching.search.azure.application.Entities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ", nq}")]
    public class Location
    {
        public long Id { get; set; }

        ///// <summary>
        ///// Key for Azure search
        ///// </summary>
        //public string Key
        //{
        //    get => Id.ToString();
        //    set => Id = long.Parse(value);
        //}

        public string Postcode { get; set; }
        
        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public decimal Distance { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string AdminDistrict { get; set; }

        public string AdminDistrictCode { get; set; }

        public string AdminCounty { get; set; }

        private string DebuggerDisplay => $"Location: {Postcode} ({Longitude}, {Latitude})";
    }
}

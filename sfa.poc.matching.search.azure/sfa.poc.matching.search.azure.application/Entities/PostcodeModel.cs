using System.Diagnostics;

namespace sfa.poc.matching.search.azure.application.Entities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ", nq}")]
    public class PostcodeModel
    {
        public string Postcode { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string OutCode { get; set; }

        public string InCode { get; set; }

        public string Admin_District { get; set; }

        public string Admin_County { get; set; }

        public Codes Codes { get; set; }

        private string DebuggerDisplay => $"PostcodeModel: { Postcode} ({ Longitude}, { Latitude})";
    }

    public class Codes
    {
        public string Admin_District { get; set; }

        public string Admin_County { get; set; }

        public string Admin_Ward { get; set; }

        public string Parish { get; set; }

        public string Parliamentary_Constituency { get; set; }

        public string Ccg { get; set; }

        public string Ced { get; set; }

        public string Nuts { get; set; }
    }

}

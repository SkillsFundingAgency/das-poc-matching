using System.Diagnostics;

namespace SFA.POC.Matching.Application.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ", nq}")]
    public class Location
    {
        public string Postcode { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string AdminDistrict { get; set; }

        public string AdminCounty { get; set; }

        private string DebuggerDisplay => $"Location: { Postcode} ({ Longitude}, { Latitude})";

    }
}


using System.Diagnostics;

namespace SFA.POC.Matching.Application.Models
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

        private string DebuggerDisplay => $"PostcodeModel: { Postcode} ({ Longitude}, { Latitude})";
    }
}

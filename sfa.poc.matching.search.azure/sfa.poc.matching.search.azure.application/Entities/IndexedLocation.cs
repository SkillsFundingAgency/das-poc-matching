using System;
using System.Diagnostics;
using Microsoft.Spatial;
using Microsoft.WindowsAzure.Storage.Table;

namespace sfa.poc.matching.search.azure.application.Entities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ", nq}")]
    public class IndexedLocation
    {
        public string Id { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string AdminDistrict { get; set; }

        public string AdminDistrictCode { get; set; }

        public string AdminCounty { get; set; }

        public GeographyPoint Location { get; set; }

        private string DebuggerDisplay => $"Location: {Postcode} ({Location.Latitude}, {Location.Longitude})";

      public static IndexedLocation FromLocation(Location location)
        {
            return new IndexedLocation
            {
                Id = location.Id.ToString(),
                Postcode = location.Postcode,
                // https://stackoverflow.com/questions/48566493/spatial-search-in-azure-search-net-client-api
                Location = GeographyPoint.Create(Convert.ToDouble(location.Latitude), Convert.ToDouble(location.Longitude)),
                Country = location.Country,
                Region = location.Region,
                AdminDistrict = location.AdminDistrict,
                AdminDistrictCode = location.AdminDistrictCode,
                AdminCounty = location.AdminCounty
            };
        }

        public Location ToLocation()
        {
            return new Location
            {
                Id = Convert.ToInt64(Id),
                Postcode = Postcode,
                //Location = Location,
                Latitude = Convert.ToDecimal(Location.Latitude),
                Longitude = Convert.ToDecimal(Location.Longitude),
                Country = Country,
                Region = Region,
                AdminDistrict = AdminDistrict,
                AdminDistrictCode = AdminDistrictCode,
                AdminCounty = AdminCounty
            };
        }
    }
}

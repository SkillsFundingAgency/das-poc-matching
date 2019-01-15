using System;
using sfa.poc.matching.search.azure.application.Interfaces;

namespace sfa.poc.matching.search.azure.application.Helpers
{
    public class DistanceCalculator : IDistanceCalculator
    {
        public const double MilesToMeters = 1609.34;

        public double DistanceFromLatLong(double lat1, double lon1, double lat2, double lon2)
        {
            bool inKilometres = false;
            var R = inKilometres
                ? 6372.8 // In kilometers
                : 3959.87433; // this is in miles.  For Earth radius in kilometers use 6372.8 km


            var lat = ToRadians(lat2 - lat1);
            var lng = ToRadians(lon2 - lon1);
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                     Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                     Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            var d = R * h2;

            return d;
        }

        public double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}

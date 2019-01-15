
namespace sfa.poc.matching.search.azure.application.Interfaces
{
    public interface IDistanceCalculator
    {
        double DistanceFromLatLong(double lat1, double lon1, double lat2, double lon2);
    }
}

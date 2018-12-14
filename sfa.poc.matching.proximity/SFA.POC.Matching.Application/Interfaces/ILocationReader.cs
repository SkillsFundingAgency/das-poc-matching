using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.POC.Matching.Application.Models;

namespace SFA.POC.Matching.Application.Interfaces
{
    public interface ILocationReader
    {
        Task<IList<LocationModel>> SearchLocationsWithByDistanceAsync(string postcode, decimal searchRadiusInMeters);

        Task<IList<LocationModel>> SearchLocationsWithByDistanceAsync(decimal latitude, decimal longitude, decimal searchRadiusInMeters);
    }
}

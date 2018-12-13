using System.Threading.Tasks;
using SFA.POC.Matching.Application.Models;

namespace SFA.POC.Matching.Application.Interfaces
{
    public interface ILocationWriter
    {
        Task SaveAsync(Location location);
    }
}

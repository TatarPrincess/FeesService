
using FeesService_BLL.Models;

namespace FeesService_BLL.Services.Interfaces
{
    public interface IDestinationService
    {
        IEnumerable<Destination> GetDestinations();
    }
}

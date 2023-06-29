using FeesService_BLL.Models;


namespace FeesService_BLL.IRepositories;

public interface IDestinationRepository
{
    IEnumerable<Destination> FindAllBySenderAndReceiver(int sender, int receiver);
}

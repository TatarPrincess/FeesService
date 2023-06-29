using FeesService_BLL.Models.Fees;

namespace FeesService_BLL.IRepositories
{
    public interface IFeesSettingsRepository
    {
        IEnumerable<FeesSettings> FindAllByDestination(int destinationId);
    }
}

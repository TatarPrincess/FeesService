using FeesService_BLL.Models.Partner;

namespace FeesService_BLL.IRepositories
{
    public interface IPartnerRepository
    {
        Partner? FindPartnerByCode(string? code);
    }
}

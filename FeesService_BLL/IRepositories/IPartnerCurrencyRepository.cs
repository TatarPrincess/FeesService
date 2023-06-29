using FeesService_BLL.Models.Partner;

namespace FeesService_BLL.IRepositories
{
    public interface IPartnerCurrencyRepository
    {
        public IEnumerable<PartnerCurrency>? GetPartnerCurrency(int partner);
    }
}

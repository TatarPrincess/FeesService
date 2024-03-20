using FeesService_BLL.Models.Partner;

namespace FeesService_BLL.Services.Interfaces
{
    public interface IPartnerService
    {
        public Partner GetPartnerData(string partnerIdentifier);
    }
}

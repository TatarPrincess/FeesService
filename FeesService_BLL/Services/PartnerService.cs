using FeesService_BLL.IRepositories;
using FeesService_BLL.Models.Partner;
using FeesService_BLL.Services.Interfaces;

namespace FeesService_BLL.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        public PartnerService(IPartnerRepository partnerRepository)
        {
            if (partnerRepository is null) throw new NullReferenceException("No partnerRepository is passed");
              
            _partnerRepository = partnerRepository; 
        }

        public Partner GetPartnerData(string code)
        {
            if (code is not null)
            {
                Partner? partner = _partnerRepository.FindPartnerByCode(code);
                if (partner is not null) return partner;
                throw new Exception("No patner is found");
            }
            throw new Exception("No partner code is given");
        }        
    }
}

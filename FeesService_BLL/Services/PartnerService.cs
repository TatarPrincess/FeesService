using FeesService_BLL.IRepositories;
using FeesService_BLL.Models.Partner;

namespace FeesService_BLL.Services
{
    public class PartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IPartnerCurrencyRepository _partnerCurrencyRepository;
        public PartnerService(IPartnerRepository partnerRepository, 
                              IPartnerCurrencyRepository partnerCurrencyRepository)
        {
            _partnerRepository = partnerRepository; 
            _partnerCurrencyRepository = partnerCurrencyRepository; 
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
        public List<int>? GetPartnerCurrency(int partner, PartnerType type)
        {
            if (partner > 0)
            {
                List<PartnerCurrency>? partnercurrencies = (List<PartnerCurrency>?)
                                                           _partnerCurrencyRepository.GetPartnerCurrency(partner);
                if (partnercurrencies is not null) 
                {
                    switch (type)
                    {
                        case PartnerType.Sending:
                            {
                                return (partnercurrencies.FindAll(p => p.IsSending == true)
                                                         .Select(p => p.Currency)).ToList<int>();
                            }
                        case PartnerType.Receiving:
                            {
                                return (partnercurrencies.FindAll(p => p.IsReceiving == true)
                                                         .Select(p => p.Currency)).ToList<int>();     
                            }
                        default: throw new Exception("Incorrect type parameter");
                    }                    
                }
                else throw new Exception("No partner currency is found");
            }
            throw new Exception("No partner code is given");
        }
        
    }
}

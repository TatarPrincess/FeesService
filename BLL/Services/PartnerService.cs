using FeesService.BLL.Models;
using FeesService.DAL.Entities;
using FeesService.DAL.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FeesService.BLL.Services
{
    public class PartnerService
    {
        private readonly IPartner _partnerRepository;
        private readonly IPartnerCurrency _partnerCurrencyRepository;
        public PartnerService()
        {
            DbProviderFactory provider = SqlClientFactory.Instance;
            _partnerRepository = new PartnerRepository(provider);  //переделай на DI
            _partnerCurrencyRepository = new PartnerCurrencyRepository(provider); //переделай на DI
        }

        public PartnerEntity GetPartnerData(string code)
        {
            if (code is not null)
            {
                PartnerEntity? partner = _partnerRepository.FindPartnerByCode(code);
                if (partner is not null) return partner;
                throw new Exception("No patner is found");
            }
            throw new Exception("No partner code is given");
        }
        public List<int>? GetPartnerCurrency(int partner, PartnerType type)
        {
            if (partner > 0)
            {
                List<PartnerCurrencyEntity>? partnercurrencies = (List<PartnerCurrencyEntity>?)
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

using Dapper;
using FeesService_BLL.Models.Partner;
using FeesService_BLL.IRepositories;
using System.Data.Common;
using System.Linq;


namespace FeesService_DAL.Repositories
{
    public class PartnerRepository : BaseRepository, IPartnerRepository
    {
        public PartnerRepository(DbProviderFactory provider, String cn) : base(provider, cn) { }
        public Partner? FindPartnerByCode(string? code)
        {
            if (code is null) return null;

            string commandText =
            $@"select p.id as {nameof(Partner.Id)},
                      p.code as {nameof(Partner.Code)},
                      p.name as {nameof(Partner.Name)},
                      p.country_id as {nameof(Partner.CountryId)},
                      p.state as {nameof(Partner.State)},
                      p.category as {nameof(Partner.Category)},
                      pc.id as {nameof(PartnerCurrency.Id)},
                      c.code as {nameof(PartnerCurrency.Currency)},
                      pc.is_sending as {nameof(PartnerCurrency.IsSending)},
                      pc.is_receiving as {nameof(PartnerCurrency.IsReceiving)} 
              from [dbo].[Partner] p (nolock)
              join [dbo].[Partner_currency]  pc  (nolock) on p.id = pc.partner
              join [dbo].[Currency] c on pc.CURRENCY = c.ID
              where p.code = @code";

            using (Connection)
            {
                CreateConnection();
                var lookup = new Dictionary<int, Partner>();
                Partner? partner = Connection.Query<Partner, PartnerCurrency, Partner>(commandText,  
                    (p, pc) =>
                    {
                        if (!lookup.TryGetValue(p.Id, out Partner? partner))
                        {
                            lookup.Add(p.Id, partner = p);
                        }
                        partner.PartnerCurrency ??= new List<PartnerCurrency>();
                        partner.PartnerCurrency.Add(pc);
                        return partner;
                    }, 
                    param: new { code }).FirstOrDefault();
                return partner; 
            }           
        }
    }
}

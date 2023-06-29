using Dapper;
using FeesService_BLL.IRepositories;
using FeesService_BLL.Models.Partner;
using System.Data.Common;


namespace FeesService_DAL.Repositories
{
    public class PartnerCurrencyRepository : BaseRepository, IPartnerCurrencyRepository
    {
        public PartnerCurrencyRepository(DbProviderFactory provider) : base(provider) { }
        public IEnumerable<PartnerCurrency>? GetPartnerCurrency(int partner)
        {
            if (partner <= 0) return null;

            string commandText =
            $@"select id as {nameof(PartnerCurrency.Id)}, 
                     partner as {nameof(PartnerCurrency.Partner)},
                     currency as {nameof(PartnerCurrency.Currency)},
                     is_sending as {nameof(PartnerCurrency.IsSending)},
                     is_receiving as {nameof(PartnerCurrency.IsReceiving)}
              from[dbo].[partner_currency] (nolock)
              where partner = @partner";

            using (cn = CreateConnection())
            {
                cn.Open();
                List<PartnerCurrency> partnercurrencies = cn.Query<PartnerCurrency>(commandText, new { partner }).ToList();
                return partnercurrencies;
            }
        }
    }
}

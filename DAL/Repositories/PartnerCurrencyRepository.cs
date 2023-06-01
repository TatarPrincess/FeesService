using Dapper;
using FeesService.DAL.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.DAL.Repositories
{
    public class PartnerCurrencyRepository : BaseRepository, IPartnerCurrency
    {
        public PartnerCurrencyRepository(DbProviderFactory provider) : base(provider) { }
        public IEnumerable<PartnerCurrencyEntity>? GetPartnerCurrency(int partner)
        {
            if (partner <= 0) return null;

            string commandText =
            $@"select id as {nameof(PartnerCurrencyEntity.Id)}, 
                     partner as {nameof(PartnerCurrencyEntity.Partner)},
                     currency as {nameof(PartnerCurrencyEntity.Currency)},
                     is_sending as {nameof(PartnerCurrencyEntity.IsSending)},
                     is_receiving as {nameof(PartnerCurrencyEntity.IsReceiving)}
              from[dbo].[partner_currency] (nolock)
              where partner = @partner";

            using (cn = CreateConnection())
            {
                cn.Open();
                List<PartnerCurrencyEntity> partnercurrencies = cn.Query<PartnerCurrencyEntity>(commandText, new { partner }).ToList();
                return partnercurrencies;
            }
        }
    }
}

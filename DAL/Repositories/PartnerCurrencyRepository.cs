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
            @"select * 
              from[dbo].[partner_currency] (nolock)
              where partner = @partner";


            using (cn = CreateConnection())
            {
                IDbCommand command = CreateCommand(commandText, cn);

                SqlParameter codeParam = new()
                {
                    ParameterName = "@partner",
                    DbType = DbType.String,
                    Value = partner
                };

                command.Parameters.Add(codeParam);

                cn.Open();
                using (IDataReader dataReader = ExecuteCommand(command, true))
                {
                    List<PartnerCurrencyEntity> partnercurrencies = new List<PartnerCurrencyEntity>();
                    while (dataReader.Read())
                    {
                        PartnerCurrencyEntity partnerCurrency = new PartnerCurrencyEntity(
                            (int)dataReader["id"],
                            (int)dataReader["partner"],
                            (int)dataReader["currency"],
                            (bool)dataReader["is_sending"],
                            (bool)dataReader["is_receiving"]);
                        
                        partnercurrencies.Add(partnerCurrency);
                    }
                    return partnercurrencies;
                }
            }
        }
    }
}

using FeesService.DAL.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.DAL.Repositories
{
    public class PartnerRepository : BaseRepository, IPartner
    {
        public PartnerRepository(DbProviderFactory provider) : base(provider) { }
        public PartnerEntity? FindPartnerByCode(string? code)
        {
            if (code is null) return null;

            string commandText =
            @"select * 
              from[dbo].[Partner] (nolock)
              where code = @code";

            
            using (cn = CreateConnection())
            {
                IDbCommand command = CreateCommand(commandText, cn);

                SqlParameter codeParam = new ()
                {
                    ParameterName = "@code",
                    DbType = DbType.String,
                    Value = code
                };              

                command.Parameters.Add(codeParam);                

                cn.Open();
                using (IDataReader dataReader = ExecuteCommand(command, true))
                {
                    while (dataReader.Read())
                    {
                        PartnerEntity partner = new PartnerEntity(
                            (int)dataReader["id"],
                            (string)dataReader["code"],
                            (string)dataReader["name"],
                            (int)dataReader["country_id"],
                            (int)dataReader["state"]);
                        return partner;
                    }                    
                }                
            }
            return null;
        }
    }
}

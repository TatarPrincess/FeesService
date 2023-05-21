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

                //Console.WriteLine("Параметры запроса: {0}", getParameters(command));
                

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
                            (int)dataReader["state"],
                            (int)dataReader["category"]);
                        return partner;
                    }                    
                }                
            }
            return null;
        }
        //private string getParameters(IDbCommand command)
        //{
        //    var builder = new StringBuilder();
        //    foreach (IDbDataParameter parametr in command.Parameters)
        //    { 
        //       builder.Append(parametr.ParameterName);
        //       builder.Append('=');
        //       builder.Append(parametr.Value);
        //    }
        //    return builder.ToString();
        //}
    }
}

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

namespace FeesService.DAL.Repositories;

public class FeesSettingsRepository : BaseRepository, IFeesSettings
{
    public FeesSettingsRepository(DbProviderFactory provider) : base(provider) { }
    public IEnumerable<FeesSettingsEntity> FindAllByDestination(int destinationId)
    {
        string commandText =
          @"select f.*
                from[dbo].[FEES_SETTINGS] f (nolock)
                where f.dest_id = @destinationId";


        using (cn = CreateConnection())
        {
            IDbCommand command = CreateCommand(commandText, cn);

            SqlParameter destIdParam = new SqlParameter();
            destIdParam.ParameterName = "@destinationId";
            destIdParam.DbType = DbType.Int32;
            destIdParam.Value = destinationId;

            command.Parameters.Add(destIdParam);


            cn.Open();
            using (IDataReader dataReader = ExecuteCommand(command))
            {
                List<FeesSettingsEntity> feesSettings = new List<FeesSettingsEntity>();
                while (dataReader.Read())
                {
                    FeesSettingsEntity entity = new FeesSettingsEntity(
                        (int)dataReader["id"],
                        (int)dataReader["dest_id"],
                        (int)dataReader["currency"],
                        (int)dataReader["calc_type"],
                        (decimal)dataReader["min_amount"],
                        (decimal)dataReader["max_amount"],
                        (int)dataReader["fees_type"],
                        (decimal)dataReader["prcnt"],
                        (decimal)dataReader["fix_fees"],
                        (int)dataReader["ext_id"],
                        (Guid)dataReader["rowguid"],
                        (decimal)dataReader["prcnt0"]);

                    feesSettings.Add(entity);
                }
                return feesSettings;
            }
        }
    }
}

using Dapper;
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
          $@"select f.id as {nameof(FeesSettingsEntity.Id)}, 
                    f.dest_id as {nameof(FeesSettingsEntity.DestId)},
                    f.currency as {nameof(FeesSettingsEntity.Currency)},
                    f.calc_type as {nameof(FeesSettingsEntity.CalcType)},
                    f.min_amount as {nameof(FeesSettingsEntity.MinAmount)},
                    f.max_amount as {nameof(FeesSettingsEntity.MaxAmount)},
                    f.fees_type as {nameof(FeesSettingsEntity.FeesType)},
                    f.prcnt as [{nameof(FeesSettingsEntity.Percent)}],
                    f.fix_fees as {nameof(FeesSettingsEntity.FixFees)},
                    f.ext_id as {nameof(FeesSettingsEntity.ExtId)},
                    f.rowguid as {nameof(FeesSettingsEntity.RowGuid)},
                    f.prcnt0 as {nameof(FeesSettingsEntity.Percent0)}
            from [dbo].[FEES_SETTINGS] f (nolock)
            where f.dest_id = @destinationId";
       
            using (cn = CreateConnection())
            {
                cn.Open();
                List<FeesSettingsEntity> feesSettings = cn.Query<FeesSettingsEntity>(commandText, new { destinationId }).ToList();
                return feesSettings;
            }         
    }
}

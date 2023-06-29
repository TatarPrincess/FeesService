using Dapper;
using FeesService_BLL.IRepositories;
using FeesService_BLL.Models.Fees;
using System.Data.Common;


namespace FeesService_DAL.Repositories;

public class FeesSettingsRepository : BaseRepository, IFeesSettingsRepository
{
    public FeesSettingsRepository(DbProviderFactory provider) : base(provider) { }
    public IEnumerable<FeesSettings> FindAllByDestination(int destinationId)
    {
        string commandText =
          $@"select f.id as {nameof(FeesSettings.Id)}, 
                    f.dest_id as {nameof(FeesSettings.DestId)},
                    f.currency as {nameof(FeesSettings.Currency)},
                    f.calc_type as {nameof(FeesSettings.CalcType)},
                    f.min_amount as {nameof(FeesSettings.MinAmount)},
                    f.max_amount as {nameof(FeesSettings.MaxAmount)},
                    f.fees_type as {nameof(FeesSettings.FeesType)},
                    f.prcnt as [{nameof(FeesSettings.Percent)}],
                    f.fix_fees as {nameof(FeesSettings.FixFees)},
                    f.ext_id as {nameof(FeesSettings.ExtId)},
                    f.rowguid as {nameof(FeesSettings.RowGuid)},
                    f.prcnt0 as {nameof(FeesSettings.Percent0)}
            from [dbo].[FEES_SETTINGS] f (nolock)
            where f.dest_id = @destinationId";
       
            using (cn = CreateConnection())
            {
                cn.Open();
                List<FeesSettings> feesSettings = cn.Query<FeesSettings>(commandText, new { destinationId }).ToList();
                return feesSettings;
            }         
    }
}

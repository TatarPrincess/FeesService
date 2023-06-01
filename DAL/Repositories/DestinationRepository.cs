using FeesService.DAL.Entities;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Data.Common;


namespace FeesService.DAL.Repositories;

public class DestinationRepository : BaseRepository, IDestination
{
    public DestinationRepository(DbProviderFactory provider) : base(provider) { }
    public IEnumerable<DestinationEntity> FindAllBySenderAndReceiver(int sender, int receiver)
    {
        string commandText =
            $@"select d.id as {nameof(DestinationEntity.Id)}, 
                      d.section_id as {nameof(DestinationEntity.SectionId)}, 
                      d.curr as {nameof(DestinationEntity.Currency)}, 
                      d.priority as {nameof(DestinationEntity.Priority)}, 
                      d.from_type as {nameof(DestinationEntity.FromType)}, 
                      d.from_id as {nameof(DestinationEntity.FromId)}, 
                      d.to_type as {nameof(DestinationEntity.ToType)}, 
                      d.to_id as {nameof(DestinationEntity.ToId)}, 
                      d.blocked as {nameof(DestinationEntity.Blocked)}, 
                      ds.priority as {nameof(DestinationEntity.SectionPriority)}
               from [dbo].[DESTINATION] d (nolock)
               join [dbo].[DESTINATION_SECTION] ds (nolock) on  ds.priority = d.section_id
               join [dbo].[PARTNER_ATTRIBUTE_VALUES] fv (nolock) 
                           on d.from_type = fv.attribute_type and d.from_id = fv.[value]
               join [dbo].[PARTNER_ATTRIBUTE_VALUES] tv(nolock) 
                           on d.to_type = tv.attribute_type and d.to_id = tv.[value]
               where fv.partner = @sender and tv.partner = @receiver
               order by d.priority desc";


        using (cn = CreateConnection())
        {
           cn.Open();
           List<DestinationEntity>  destinations = cn.Query<DestinationEntity>(commandText, new { sender, receiver }).ToList();
           return destinations;
        }
    }
}

using Dapper;
using System.Data.Common;
using FeesService_BLL.Models;
using FeesService_BLL.IRepositories;


namespace FeesService_DAL.Repositories;

public class DestinationRepository : BaseRepository, IDestinationRepository
{
    public DestinationRepository(DbProviderFactory provider, String cn) : base(provider, cn) { }
    public IEnumerable<Destination> FindAllBySenderAndReceiver(int sender, int receiver)
    {
        string commandText =
            $@"select d.id as {nameof(Destination.Id)}, 
                      d.section_id as {nameof(Destination.SectionId)}, 
                      d.curr as {nameof(Destination.Currency)}, 
                      d.priority as {nameof(Destination.Priority)}, 
                      d.from_type as {nameof(Destination.FromType)}, 
                      d.from_id as {nameof(Destination.FromId)}, 
                      d.to_type as {nameof(Destination.ToType)}, 
                      d.to_id as {nameof(Destination.ToId)}, 
                      d.blocked as {nameof(Destination.Blocked)}, 
                      ds.priority as {nameof(Destination.SectionPriority)}
               from [dbo].[DESTINATION] d (nolock)
               join [dbo].[DESTINATION_SECTION] ds (nolock) on  ds.priority = d.section_id
               join [dbo].[PARTNER_ATTRIBUTE_VALUES] fv (nolock) 
                           on d.from_type = fv.attribute_type and d.from_id = fv.[value]
               join [dbo].[PARTNER_ATTRIBUTE_VALUES] tv(nolock) 
                           on d.to_type = tv.attribute_type and d.to_id = tv.[value]
               where fv.partner = @sender and tv.partner = @receiver
               order by d.priority desc";


        using (Connection)
        {
           CreateConnection();           
           List<Destination>  destinations = Connection.Query<Destination>(commandText, new { sender, receiver }).ToList();
           return destinations;
        }
    }
}

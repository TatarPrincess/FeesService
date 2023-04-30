using FeesService.DAL.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.DAL.Repositories;

public class DestinationRepository : BaseRepository, IDestination
{
    public DestinationRepository(DbProviderFactory provider) : base(provider) { }
    public IEnumerable<DestinationEntity> FindAllBySenderAndReceiver(int sender, int receiver)
    {
        string commandText =
            @"select d.*, ds.priority as 'section_priority'
                  from [dbo].[DESTINATION] d (nolock)
                  join [dbo].[DESTINATION_SECTION] ds (nolock) 
                      on  ds.priority = d.section_id
                  join [dbo].[PARTNER_ATTRIBUTE_VALUES] fv (nolock) 
                      on d.from_type = fv.attribute_type and d.from_id = fv.[value]
                  join [dbo].[PARTNER_ATTRIBUTE_VALUES] tv(nolock) 
                      on d.to_type = tv.attribute_type and d.to_id = tv.[value]
                  where fv.partner = @sender and tv.partner = @receiver
                  order by d.priority desc";


        using (cn = CreateConnection())
        {
            IDbCommand command = CreateCommand(commandText, cn);

            SqlParameter senderParam = new SqlParameter();
            senderParam.ParameterName = "@sender";
            senderParam.DbType = DbType.Int32;
            senderParam.Value = sender;
            SqlParameter receiverParam = new SqlParameter();
            receiverParam.ParameterName = "@receiver";
            receiverParam.DbType = DbType.Int32;
            receiverParam.Value = receiver;

            command.Parameters.Add(senderParam);
            command.Parameters.Add(receiverParam);

            cn.Open();
            using (IDataReader dataReader = ExecuteCommand(command))
            {
                List<DestinationEntity> destinations = new List<DestinationEntity>();
                while (dataReader.Read())
                {
                    DestinationEntity entity = new DestinationEntity(
                        (int)dataReader["id"],
                        (int)dataReader["section_id"],
                        (long)dataReader["curr"],
                        (int)dataReader["priority"],
                        (int)dataReader["from_type"],
                        (int)dataReader["from_id"],
                        (int)dataReader["to_type"],
                        (int)dataReader["to_id"],
                        Convert.ToBoolean(dataReader["blocked"]),
                        (int)dataReader["section_priority"]);

                    destinations.Add(entity);
                }
                return destinations;
            }
        }
    }
}

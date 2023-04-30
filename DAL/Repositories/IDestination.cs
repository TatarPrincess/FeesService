using FeesService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.DAL.Repositories;

public interface IDestination
{
    IEnumerable<DestinationEntity> FindAllBySenderAndReceiver(int sender, int receiver);
}

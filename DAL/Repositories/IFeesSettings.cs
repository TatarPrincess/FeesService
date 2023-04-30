using FeesService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.DAL.Repositories
{
    public interface IFeesSettings
    {
        IEnumerable<FeesSettingsEntity> FindAllByDestination(int destinationId);
    }
}

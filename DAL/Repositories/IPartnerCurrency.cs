using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeesService.DAL.Entities;

namespace FeesService.DAL.Repositories
{
    public interface IPartnerCurrency
    {
        public IEnumerable <PartnerCurrencyEntity>? GetPartnerCurrency(int partner);
    }
}

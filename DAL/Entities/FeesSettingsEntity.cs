using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.DAL.Entities
{
    public class FeesSettingsEntity
    {
        public int Id { get; init; }
        public int DestId { get; init; }
        public int Currency { get; init; }
        public int CalcType { get; init; }
        public decimal MinAmount { get; init; }
        public decimal MaxAmount { get; init; }
        public int FeesType { get; init; }
        public decimal Percent { get; init; }
        public decimal FixFees { get; init; }
        public int ExtId { get; init; }
        public Guid RowGuid { get; init; }
        public decimal Percent0 { get; init; }

        public FeesSettingsEntity(int id, int destId, int currency, int calcType, decimal minAmount, decimal maxAmount,
            int feesType, decimal percent, decimal fixFees, int extId, Guid rowGuid, decimal percent0)
        {
            Id = id;
            DestId = destId;    
            Currency = currency;
            CalcType  = calcType;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            FeesType = feesType;
            Percent = percent;
            FixFees = fixFees;
            ExtId = extId;
            RowGuid = rowGuid;
            Percent0 = percent0;
        }

    }
}

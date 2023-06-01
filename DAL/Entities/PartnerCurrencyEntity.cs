using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.DAL.Entities
{
    public class PartnerCurrencyEntity
    {
        public int Id { get; init; }
        public int Partner { get; init; } 
        public int Currency { get; init; } 
        public bool IsSending { get; init; } 
        public bool IsReceiving { get; init; }
        public PartnerCurrencyEntity() { }
    }
}

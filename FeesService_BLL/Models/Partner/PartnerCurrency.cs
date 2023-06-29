using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService_BLL.Models.Partner
{
    public class PartnerCurrency
    {
        public int Id { get; init; }
        public int Partner { get; init; } 
        public int Currency { get; init; } 
        public bool IsSending { get; init; } 
        public bool IsReceiving { get; init; }
        public PartnerCurrency() { }
    }
}

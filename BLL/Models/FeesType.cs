using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.BLL.Models;

public enum FeesType
{
    ClientFee = 0,
    SendingPartnerFee = 2,
    BrokerFee = 3,
    FeeToDebitSendingPartner = 4,
    ReceivingPartnerFee = 5,
    ReimbursedBrokerFee = 6,
    ReimbursedSendingPartnerFee = 7,
    Undefined = -1
}

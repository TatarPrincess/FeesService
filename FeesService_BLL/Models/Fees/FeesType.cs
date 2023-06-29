namespace FeesService_BLL.Models.Fees;

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

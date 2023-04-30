using FeesService.DAL.Entities;
using FeesService.BLL.Models;


namespace FeesService.BLL.Services;

public class CalculationAlgorithm
{
    protected List<FeesSettingsEntity> schemas;
    protected PartnerService partnerService = new PartnerService(); //DI??
    public CalculationAlgorithm(List<FeesSettingsEntity> schemas)
    { 
      this.schemas = schemas;
    }
    public virtual List<FeeEntity> Execute()
    {
        return new List<FeeEntity>();
    }
    private List<FeesSettingsEntity> ProcessFeesSettings()
        {
            int idxBrokerFee = schemas.FindIndex(f => f.FeesType == (int)FeesType.BrokerFee);
            int idxSendingPartnerFee = schemas.FindIndex(m => m.FeesType == (int)FeesType.SendingPartnerFee);
            int idxClientFee = schemas.FindIndex(m => m.FeesType == (int)FeesType.ClientFee);
            int idxFeesToDebitSenderPartner = schemas.FindIndex(f => f.FeesType == (int)FeesType.FeeToDebitSendingPartner);

            if (schemas.FindIndex(s => (s.FeesType == (int)FeesType.FeeToDebitSendingPartner 
                                         || s.FeesType == (int)FeesType.ReimbursedBrokerFee)) > 0)
            {
                if (idxBrokerFee > 0) schemas.RemoveAt(idxBrokerFee);
            }
            
            if (idxSendingPartnerFee > 0 & schemas[idxSendingPartnerFee].CalcType == (int)CalcType.PercentageOfTransactionAmount)
            {
                if (idxClientFee > 0) schemas.RemoveAt(idxClientFee);
            }

            if (idxClientFee > 0 && schemas[idxSendingPartnerFee].CalcType == (int)CalcType.Combined)
            {
                if (idxFeesToDebitSenderPartner > 0) schemas.RemoveAt(idxFeesToDebitSenderPartner);
            } 

            return schemas;
        }
}

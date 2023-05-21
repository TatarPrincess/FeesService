using FeesService.BLL.Models;
using FeesService.DAL.Entities;
using FeesService.DAL.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.BLL.Services.FeeCalculator;

public class FeeCalculator
{
    FeeSettingsService feeSettingsService;
    CalcInputData calcInputData;    
    public FeeCalculator(IEnumerable<DestinationEntity> destinations, CalcInputData calcInputData)
    {
        feeSettingsService = new FeeSettingsService(destinations, calcInputData);
        this.calcInputData = calcInputData;
    }
    public Dictionary<int, FeeData> GetFees()
    {
        List<FeesSettingsEntity> feesSettings = feeSettingsService.GetFeesSettingsForDestinations();

        Dictionary<int, FeeData> schemas = new Dictionary<int, FeeData>();
        if (feesSettings != null)
        {            
            foreach (FeesSettingsEntity schema in feesSettings)
            {
               schemas.Add(schema.FeesType, new FeeData(schema.Id, schema.DestId, schema.Currency, schema.CalcType,
                                        schema.MinAmount, schema.MaxAmount, schema.FeesType, schema.Percent,
                                        schema.FixFees, schema.ExtId, schema.RowGuid, schema.Percent0, 0));
            }
        }

        Dictionary<int, FeeData> fees = GetCalculationAlgorithm(schemas).Execute(); 
        return fees;
    }
    public CalculationAlgorithm GetCalculationAlgorithm(Dictionary<int, FeeData> schemas)
    {
        PartnerService partnerService = new PartnerService();
        PartnerEntity sendingPartner = partnerService.GetPartnerData(calcInputData.SendingPartner!.partnerCode);
        PartnerEntity receivingPartner = partnerService.GetPartnerData(calcInputData.ReceivingPartner!.partnerCode);
        
        return (sendingPartner.Category == (int)PartnerCategory.LargeEquity ||
                receivingPartner.Category == (int)PartnerCategory.LargeEquity) 
                ? new TransactionAmountBasedAlgorythm(schemas, calcInputData)
                : new ClientFeeBasedAlgorythm(schemas, calcInputData);       
    }
}




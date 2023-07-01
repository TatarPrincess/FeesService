﻿using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;
using FeesService_BLL.Models.Partner;


namespace FeesService_BLL.Services.FeeCalculator;

public class FeeCalculator
{
    private readonly FeeSettingsService _feeSettingsService;
    private readonly CalcInputData _calcInputData;
    private readonly PartnerService _partnerService;
    private readonly FeeAlgorithmFactory _feeAlgorithmFactory;
    public FeeCalculator(CalcInputData calcInputData,
                         FeeSettingsService feeSettingsService,  
                         PartnerService partnerService,
                         FeeAlgorithmFactory feeAlgorithmFactory)
    {
        _feeSettingsService = feeSettingsService;
        _calcInputData = calcInputData;
        _partnerService = partnerService;
        _feeAlgorithmFactory = feeAlgorithmFactory;
    }
    public Dictionary<int, FeeData> GetFees()
    {
        List<FeesSettings> feesSettings = _feeSettingsService.GetFeesSettingsForDestinations();

        Dictionary<int, FeeData> schemas = new Dictionary<int, FeeData>();
        if (feesSettings != null)
        {            
            foreach (FeesSettings schema in feesSettings)
            {
               schemas.Add(schema.FeesType, 
                           new FeeData(schema.Id, 
                                       schema.DestId, 
                                       schema.Currency, 
                                       schema.CalcType,
                                       schema.MinAmount, 
                                       schema.MaxAmount, 
                                       schema.FeesType, 
                                       schema.Percent,
                                       schema.FixFees, 
                                       schema.ExtId, 
                                       schema.RowGuid, 
                                       schema.Percent0, 
                                       0));
            }
        }

        Dictionary<int, FeeData> fees = GetCalculationAlgorithm(schemas).Execute(); 
        return fees;
    }
    public CalculationAlgorithm GetCalculationAlgorithm(Dictionary<int, FeeData> schemas)
    {
        Partner sendingPartner = _partnerService.GetPartnerData(_calcInputData.SendingPartner!.partnerCode);
        Partner receivingPartner = _partnerService.GetPartnerData(_calcInputData.ReceivingPartner!.partnerCode);
        
        return (sendingPartner.Category == (int)PartnerCategory.LargeEquity ||
                receivingPartner.Category == (int)PartnerCategory.LargeEquity) 
                ? _feeAlgorithmFactory.GetTransactionAmountBasedAlgorythm(schemas, _calcInputData)
                : _feeAlgorithmFactory.GetClientFeeBasedAlgorythm(schemas, _calcInputData);       
    }
}




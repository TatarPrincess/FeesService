using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;
using FeesService_BLL.Models.Partner;
using FeesService_BLL.Services.Interfaces;


namespace FeesService_BLL.Services.FeeCalculator;

public class FeeCalculator : IFeeCalculator 
{
    private readonly FeeSettingsService _feeSettingsService;
    private readonly IPartnerService _partnerService;
    private readonly Dictionary<PartnerCategory, ICalculationAlgorithm> _algorithms;
    public FeeCalculator(FeeSettingsService feeSettingsService,  
                         IPartnerService partnerService,
                         Dictionary<PartnerCategory, ICalculationAlgorithm> algorithms)
    {
        if (feeSettingsService is null) throw new NullReferenceException("No feeSettingsService is passed");
        if (partnerService is null) throw new NullReferenceException("No partnerService is passed");
        if (algorithms is null) throw new NullReferenceException("No fee calculation algorithm is passed");

        _feeSettingsService = feeSettingsService;
        _partnerService = partnerService;
        _algorithms = algorithms;
    }
    public IDictionary<int, FeeData> Calculate(CalcInputData calcInputData)
    {
        //поискали релевантные направления
        List<FeesSettings> feesSettings = _feeSettingsService.GetFeesSettingsForDestinations();
        //заполнили словарь уникальным набором тип комиссий и настроек из расчета (еще не рассчитанных)
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
        return GetCalculationAlgorithm(calcInputData).Execute(schemas, calcInputData);
    }
    public ICalculationAlgorithm GetCalculationAlgorithm(CalcInputData calcInputData)
    {
        Partner sendingPartner = _partnerService.GetPartnerData(calcInputData.SendingPartner!.partnerCode);
        Partner receivingPartner = _partnerService.GetPartnerData(calcInputData.ReceivingPartner!.partnerCode);
        
        return (sendingPartner.Category == (int)PartnerCategory.LargeEquity ||
                receivingPartner.Category == (int)PartnerCategory.LargeEquity) 
                ? _algorithms[PartnerCategory.LargeEquity]
                : _algorithms[PartnerCategory.SmallEquity];   
    }
}




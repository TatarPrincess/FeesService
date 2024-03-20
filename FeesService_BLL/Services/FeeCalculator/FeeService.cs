using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;
using FeesService_BLL.Services.Interfaces;

namespace FeesService_BLL.Services.FeeCalculator;

public class FeeService
{
    private readonly IDestinationService _destinationService;
    private readonly IValidationService _validationService;
    private readonly IFeeCalculator _feeCalculator;
    private readonly CalcInputData _calcInputData;
    public FeeService(IDestinationService destinationService,
                      IValidationService validationService,
                      IFeeCalculator feeCalculator,
                      CalcInputData calcInputData)
    {
        if (destinationService is null) throw new NullReferenceException("No destinationService is passed");
        if (validationService is null) throw new NullReferenceException("No validationService is passed");
        if (feeCalculator is null) throw new NullReferenceException("No feeCalculator is passed");
        if (calcInputData is null) throw new NullReferenceException("No calcInputData is passed");

        _destinationService = destinationService;
         _validationService = validationService;
        _feeCalculator = feeCalculator;
        _calcInputData = calcInputData;
    }
    public void GetFees()
    {
        //1. Getting relevant to input data destinations         
        var dests = _destinationService.GetDestinations();
        if (dests != null)
        {
            // 2. Checking destinations
            bool isValidated = _validationService.Validate();
            // 3. Getting fees
            if (isValidated)
            {
                Dictionary<int, FeeData> fees = (Dictionary<int, FeeData>)_feeCalculator.Calculate(_calcInputData);
                if (fees is not null)
                {
                    foreach (KeyValuePair<int, FeeData> fee in fees)
                        Console.WriteLine("Fee type: {0} - {1} {2} ",
                                         (FeesType)fee.Value.FeesType,
                                         fee.Value.Amount,
                                         (Currency)fee.Value.Currency);
                }
            }
        }
        else Console.WriteLine("No relevant fee settings is found. Try to change input parameters");
    }
}

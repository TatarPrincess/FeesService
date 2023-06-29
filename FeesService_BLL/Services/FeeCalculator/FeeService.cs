using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;

namespace FeesService_BLL.Services.FeeCalculator;

public class FeeService
{
    private readonly DestinationService _destinationService;
    private readonly ValidationService _validationService;
    private readonly FeeCalculator _feeCalculator;
    public FeeService(DestinationService destinationService,
                      ValidationService validationService,
                      FeeCalculator feeCalculator)
    {
        _destinationService = destinationService;
         _validationService = validationService;
        _feeCalculator = feeCalculator;
    }
    public void GetFees()
    {
        //1. Getting relevant to input data destinations         
        var dests = _destinationService.Dests;
        if (dests != null)
        {
            // 2. Checking destinations
            _validationService.Check();
            // 3. Getting fees
            Dictionary<int, FeeData> fees = _feeCalculator.GetFees();
            if (fees is not null)
            {
                foreach (KeyValuePair <int, FeeData> fee in fees)
                    Console.WriteLine("Fee type: {0} - {1} {2} ", 
                                     (FeesType)fee.Value.FeesType, 
                                     fee.Value.Amount, 
                                     (Currency)fee.Value.Currency);
            }
        }
        else Console.WriteLine("No relevant fee settings is found. Try to change input parameters");
    }
}

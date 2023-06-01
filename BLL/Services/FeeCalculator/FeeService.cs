using FeesService.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.BLL.Services.FeeCalculator;

public class FeeService
{
    private CalcInputData _calcInputData;
    public FeeService(CalcInputData calcInputData)
    {
        _calcInputData = calcInputData;
    }
    public void GetFees()
    {
        //1. Getting relevant to input data destinations 
        DestinationService destinationService = new DestinationService(_calcInputData);
        var dests = destinationService.GetRelevantDestinations();
        if (dests != null)
        {
            // 2. Validation of relevant destinations
            List<IValidator> validators = new List<IValidator>(); //переделай это на DI
            validators.Add(new PartnerValidationService(_calcInputData));
            validators.Add(new DestinationValidationService(dests, _calcInputData));
            ValidationService validationService = new ValidationService(validators);
            validationService.Check();
            // 3. Getting fees
            Dictionary<int, FeeData> fees = new FeeCalculator(dests, _calcInputData).GetFees();
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

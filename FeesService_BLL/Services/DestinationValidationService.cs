using FeesService_BLL.Models;
using FeesService_BLL.Services.Interfaces;

namespace FeesService_BLL.Services;

public class DestinationValidationService 
{
    private readonly CalcInputData _calcInputData;
    private readonly IEnumerable<IGrouping<int, Destination>>? _groupedDests = null;
    private readonly IValidationService _validationService;
    public DestinationValidationService(CalcInputData calcInputData, 
                                        IDestinationService destinationService, 
                                        IValidationService validationService)
    {
        if (calcInputData is null) throw new NullReferenceException("No calcInputData is passed");
        if (destinationService is null) throw new NullReferenceException("No DestinationService is passed");

        _calcInputData = calcInputData;
        if (destinationService.GetDestinations() != null) _groupedDests = destinationService.GetDestinations()?.GroupBy(e => e.SectionId)!;
        else Console.WriteLine("No relevant fee settings is found. Try to change input parameters"); 
        _validationService = validationService;

        ((ValidationService)_validationService).FeeDestinationsRetrieved += CheckIfTheMaxPriorityDestIsBlocked;
        ((ValidationService)_validationService).FeeDestinationsRetrieved += CheckTheMaxPriorityDestCurrency;
    }
    
    private void CheckIfTheMaxPriorityDestIsBlocked(object? sender, EventArgs e)
    {
        try
        {
            if (_groupedDests != null)
            {
                foreach (var group in _groupedDests)
                {
                    if (group.ToList()
                             .OrderByDescending(g => g.Priority)
                             .First()
                             .Blocked == true)
                        throw new Exception("The max priority destination should not be blocked");
                }
            }
        }
        finally
        {
            ((ValidationService)_validationService).FeeDestinationsRetrieved -= CheckIfTheMaxPriorityDestIsBlocked;
        }
        
    }
    private void CheckTheMaxPriorityDestCurrency(object? sender, EventArgs e)
    {
        try
        {
            if (_groupedDests != null)
            {
                foreach (var group in _groupedDests)
                {
                    if (group.ToList()
                             .OrderByDescending(g => g.Priority)
                             .First()
                             .Currency != _calcInputData.TransactionCurrency)
                        throw new Exception("The max priority destination's currency should match the transaction currency");
                }
            }
        }
        finally
        {
            ((ValidationService)_validationService).FeeDestinationsRetrieved -= CheckTheMaxPriorityDestCurrency;
        }

        
    }
}

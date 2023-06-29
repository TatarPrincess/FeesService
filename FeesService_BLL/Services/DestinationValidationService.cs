using FeesService_BLL.Models;

namespace FeesService_BLL.Services;

public class DestinationValidationService : IValidator
{
    private readonly CalcInputData _calcInputData;
    private readonly IEnumerable<IGrouping<int, Destination>> _groupedDests;
    public DestinationValidationService(CalcInputData calcInputData, DestinationService destinationService)
    {
        _calcInputData = calcInputData;
        if (destinationService.Dests != null) _groupedDests = destinationService.Dests?.GroupBy(e => e.SectionId)!;
        else Console.WriteLine("No relevant fee settings is found. Try to change input parameters"); 
    }
    private bool CheckIfTheMaxPriorityDestIsBlocked()
    {
        foreach (var group in _groupedDests)
        {
                if (group.ToList()
                         .OrderByDescending(g => g.Priority)
                         .First()
                         .Blocked == true)
                    throw new Exception("The max priority destination should not be blocked");
        }        
        return false;
    }
    private bool CheckTheMaxPriorityDestCurrency()
    {
        foreach (var group in _groupedDests)
        {
            if (group.ToList()
                     .OrderByDescending(g => g.Priority)
                     .First()
                     .Currency != _calcInputData.TransactionCurrency)
            throw new Exception("The max priority destination's currency should match the transaction currency");
        }
        return true;
    }
    public bool Check()
    {
        return CheckIfTheMaxPriorityDestIsBlocked() &&
               CheckTheMaxPriorityDestCurrency();
    }


}

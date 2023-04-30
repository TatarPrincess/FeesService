using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeesService.BLL.Models;
using FeesService.DAL.Entities;
using FeesService.DAL.Repositories;

namespace FeesService.BLL.Services;

public class DestinationValidationService : IValidator
{
    private readonly IEnumerable<IGrouping<int, DestinationEntity>> groupedDests;
    private readonly CalcInputData calcInputData;
    public DestinationValidationService(IEnumerable<DestinationEntity> dests, CalcInputData calcInputData)
    {
        this.calcInputData = calcInputData;
        this.groupedDests = dests.GroupBy(e => e.SectionId);

    }
    private bool CheckIfTheMaxPriorityDestIsBlocked()
    {
        foreach (var group in groupedDests) 
        {
            if (group.ToList()
                     .OrderByDescending(g => g.Priority)
                     .First()
                     .Blocked == true)
            throw new Exception("The max priority destination should not be blocked");
        }
        return true;
    }
    private bool CheckTheMaxPriorityDestCurrency()
    {
        foreach (var group in groupedDests)
        {
            if (group.ToList()
                     .OrderByDescending(g => g.Priority)
                     .First()
                     .Currency != calcInputData.TransactionCurrency)
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

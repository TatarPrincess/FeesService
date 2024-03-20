using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;
using FeesService_BLL.Services.Interfaces;

namespace FeesService_BLL.Services.FeeCalculator;

public class CalculationAlgorithm : ICalculationAlgorithm
{
    public CalculationAlgorithm() {}
    public virtual IDictionary<int, FeeData> Execute(Dictionary<int, FeeData> schemas, CalcInputData inputData)
    {
        return new Dictionary<int, FeeData>();
    }
    protected virtual Dictionary<int, FeeData> ProcessFeesSettings(Dictionary<int, FeeData> schemas, CalcInputData inputData)
    {
        if (schemas.ContainsKey((int)FeesType.FeeToDebitSendingPartner) ||
            schemas.ContainsKey((int)FeesType.ReimbursedBrokerFee))
        {
           schemas.Remove((int)FeesType.BrokerFee);
        }
        return schemas;
    }
    protected virtual Dictionary<int, FeeData> ProcessFeesSet(Dictionary<int, FeeData> feesSet, CalcInputData inputData)
    {
        foreach (KeyValuePair <int, FeeData> fee in feesSet.Where(f => f.Value.Amount == 0 && f.Value.Percent > 0))
        {
            fee.Value.Amount = (decimal) 0.01; 
        } 
        
        return feesSet;
    }
}

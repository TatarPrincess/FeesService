using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;

namespace FeesService_BLL.Services.FeeCalculator;

public class CalculationAlgorithm
{
    protected Dictionary<int, FeeData> schemas;
    protected CalcInputData inputData;  
    public CalculationAlgorithm(Dictionary<int, FeeData> schemas, CalcInputData inputData)
    {
        this.schemas = schemas;
        this.inputData = inputData;
    }
    public virtual Dictionary<int, FeeData> Execute()
    {
        return new Dictionary<int, FeeData>();
    }
    protected virtual Dictionary<int, FeeData> ProcessFeesSettings(Dictionary<int, FeeData> schemas)
    {
        if (schemas.ContainsKey((int)FeesType.FeeToDebitSendingPartner) ||
            schemas.ContainsKey((int)FeesType.ReimbursedBrokerFee))
        {
           schemas.Remove((int)FeesType.BrokerFee);
        }
        return schemas;
    }
    protected virtual Dictionary<int, FeeData> ProcessFeesSet(Dictionary<int, FeeData> feesSet)
    {
        foreach (KeyValuePair <int, FeeData> fee in feesSet.Where(f => f.Value.Amount == 0 && f.Value.Percent > 0))
        {
            fee.Value.Amount = (decimal) 0.01; 
        } 
        
        return feesSet;
    }
}

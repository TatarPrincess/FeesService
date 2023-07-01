using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;

namespace FeesService_BLL.Services.FeeCalculator
{
    public class FeeAlgorithmFactory
    {
        public FeeAlgorithmFactory() { }
        public CalculationAlgorithm GetTransactionAmountBasedAlgorythm(Dictionary<int, FeeData> schemas, CalcInputData inputData)
        { 
           return new TransactionAmountBasedAlgorythm(schemas, inputData);
        }
        public CalculationAlgorithm GetClientFeeBasedAlgorythm(Dictionary<int, FeeData> schemas, CalcInputData inputData)
        {
            return new ClientFeeBasedAlgorythm(schemas, inputData);
        }
    }
}

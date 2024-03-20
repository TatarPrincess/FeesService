using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;

namespace FeesService_BLL.Services.Interfaces
{
    public interface ICalculationAlgorithm
    {
        public IDictionary<int, FeeData> Execute(Dictionary<int, FeeData> schemas, CalcInputData inputData);
    }
}

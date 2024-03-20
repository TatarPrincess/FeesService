using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;

namespace FeesService_BLL.Services.Interfaces
{
    public interface IFeeCalculator
    {
        IDictionary<int, FeeData> Calculate(CalcInputData calcInputData);
    }
}

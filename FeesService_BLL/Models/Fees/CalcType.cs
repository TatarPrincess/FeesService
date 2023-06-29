namespace FeesService_BLL.Models.Fees;

public enum CalcType
{
    PercentageOfTransactionAmount = 0,
    PercentageOfClientFee = 2,
    Combined = 4,
    Undefined = -1
}

namespace FeesService_BLL.Models.Fees;

public class FeeEntity
{
    public int Type { get; init; }
    public int Currency { get; init; }
    public decimal Amount { get; set; }
}

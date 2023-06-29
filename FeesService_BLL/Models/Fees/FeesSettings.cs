
namespace FeesService_BLL.Models.Fees
{
    public class FeesSettings
    {
        public int Id { get; init; }
        public int DestId { get; init; }
        public int Currency { get; init; }
        public int CalcType { get; init; }
        public decimal MinAmount { get; init; }
        public decimal MaxAmount { get; init; }
        public int FeesType { get; init; }
        public decimal Percent { get; init; }
        public decimal FixFees { get; init; }
        public int ExtId { get; init; }
        public Guid RowGuid { get; init; }
        public decimal Percent0 { get; init; }
        public FeesSettings() { }

    }
}

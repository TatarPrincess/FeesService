namespace FeesService_BLL.Models.Partner;

public class Partner
{
    public int Id { get; set; } = -1;
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public int CountryId { get; set; } = -1;
    public int State { get; set; } = -1;
    public int Category { get; set; } = -1;

    public Partner() { }
}


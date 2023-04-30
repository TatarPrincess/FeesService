using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.DAL.Entities;

public class PartnerEntity
{
    public int Id { get; set; } = -1;
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public int State { get; set; } = -1;
    public int CountryId { get; set; } = -1;

    public PartnerEntity(int id, string code, string name, int state, int countryId)
    {
        Id = id;
        Code = code;
        Name = name;
        State = state;
        CountryId = countryId;
    }
    public PartnerEntity() { }
}


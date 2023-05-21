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
    public int CountryId { get; set; } = -1;
    public int State { get; set; } = -1;
    public int Category { get; set; } = -1;

    public PartnerEntity(int id, string code, string name, int countryId, int state,  int category)
    {
        Id = id;
        Code = code;
        Name = name;
        CountryId = countryId;
        State = state;        
        Category = category;
    }
    public PartnerEntity() { }
}


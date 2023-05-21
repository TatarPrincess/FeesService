using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.BLL.Models;

public class FeeEntity
{
    public int Type { get; init; }
    public int Currency { get; init; }
    public decimal Amount { get; set; }
}

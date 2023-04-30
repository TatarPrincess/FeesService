using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.BLL.Models;

public enum CalcType
{
    PercentageOfTransactionAmount = 0,
    PercentageOfClientFee = 2,
    Combined = 4,
    Undefined = -1

}

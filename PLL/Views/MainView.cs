using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeesService.BLL.Services;
using FeesService.BLL.Models;

namespace FeesService.PLL.Views;

public class MainView
{
    private FeeCalculator _feeCalculator;
    public MainView(FeeCalculator feeCalculator)
    {
        _feeCalculator = feeCalculator;
    }

    public void Show()
    {   //добавить зацикливвание при неправильном вводе!
        Console.WriteLine("Welcome to the Fee Service. To calculate fees answer the following questions.");
        
        Console.WriteLine("Enter sending partner code");
        string? sendingPartner = Console.ReadLine();
        
        Console.WriteLine("Enter receiving partner code");
        string? receivingPartner = Console.ReadLine();
        
        Console.WriteLine("Enter transaction currency (choose the one from the list): \"RUR\", \"USD\", \"EUR\"");
        string input = Console.ReadLine()?.ToUpper() ?? Currency.Undefined.ToString();
        ushort transactionCurrency = 
        (Enum.TryParse(input, out Currency inputCurr)) ? (ushort) inputCurr : (ushort) Currency.Undefined;

        Console.WriteLine("Enter transaction amount");
        decimal transactionAmount = Convert.ToDecimal(Console.ReadLine());

        new CalcInputData(sendingPartner, receivingPartner, transactionCurrency, transactionAmount);
    }    
}

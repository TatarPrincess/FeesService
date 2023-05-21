using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeesService.BLL.Models;
using FeesService.BLL.Services.FeeCalculator;
using static FeesService.BLL.Models.CalcInputData;

namespace FeesService.PLL.Views;

public class MainView
{
    private CalcInputData _inputData = new();
    public void Show()
    {   
        Console.WriteLine("Welcome to the Fee Service. To calculate fees please answer the following questions.");

        GetSenderPartner();
        GetReceiverPartner();
        GetTransactionCurrency();
        GetTransactionAmount();

        new FeeService(_inputData).GetFees();
    }
    private void GetSenderPartner()
    {
        Console.WriteLine("Enter sending partner code");
        try
        {
            _inputData.SendingPartner = new PartnerInputData
            {
                type = PartnerType.Sending,
                partnerCode = Console.ReadLine()!
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            GetSenderPartner();
        }
    }
    private void GetReceiverPartner()
    {
        Console.WriteLine("Enter receiving partner code");
        try
        {
            _inputData.ReceivingPartner = new PartnerInputData
            {
                type = PartnerType.Receiving,
                partnerCode = Console.ReadLine()!
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            GetReceiverPartner();
        }
    }
    private void GetTransactionCurrency()
    {
        Console.WriteLine("Enter transaction currency (choose the one from the list): \"RUR\", \"USD\", \"EUR\"");
        try
        {
            string input = Console.ReadLine()?.ToUpper() ?? Currency.Undefined.ToString();
            _inputData.TransactionCurrency =
            (Enum.TryParse(input, out Currency inputCurr)) ? (ushort)inputCurr : (ushort)Currency.Undefined;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            GetTransactionCurrency();
        }
    }
    private void GetTransactionAmount()
    {
        Console.WriteLine("Enter transaction amount");
        try
        {
            _inputData.TransactionAmount = Convert.ToDecimal(Console.ReadLine());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            GetTransactionAmount();
        }
    }
    
}

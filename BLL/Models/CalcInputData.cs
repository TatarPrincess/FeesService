using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FeesService.BLL.Models;

public record CalcInputData
{
    private ushort _transactionCurrency;
    private decimal _transactionAmount;
    private PartnerInputData _sendingPartner = new();
    private PartnerInputData _receivingPartner = new();

    public class PartnerInputData
    {
        public PartnerType type = PartnerType.Undefined;
        public string partnerCode = "";
    }
    public ushort TransactionCurrency
    {
        get => _transactionCurrency;
        set
        {
            if (value == (ushort) Currency.Undefined) 
                throw new Exception("Transaction currency should be the one from the list: \"RUR\", \"USD\", \"EUR\"");
           _transactionCurrency = value;            
        }
    }
    public decimal TransactionAmount
    {
        get => _transactionAmount;
        set
        {
            if (value <= 0) throw new Exception("Transaction amount should be a positive integer");
            _transactionAmount = value;           
        }
    }
    public PartnerInputData? SendingPartner
    {
        get => _sendingPartner;
        set
        {
            ArgumentNullException.ThrowIfNull(value, "Sending partner is null");  
            if (value.partnerCode.Length != 4) throw new Exception("Partner code should consist of 4 letters"); //добавь проверку, что это именно буквы
            _sendingPartner = value;
        }
    }
    public PartnerInputData? ReceivingPartner
    {
        get => _receivingPartner;
        set
        {
            ArgumentNullException.ThrowIfNull(value, "Receiving partner is null");
            if (value.partnerCode.Length != 4) throw new Exception("Partner code should consist of 4 letters"); //добавь проверку, что это именно буквы
            _receivingPartner = value;
        }
    }     
    public CalcInputData() { }

}

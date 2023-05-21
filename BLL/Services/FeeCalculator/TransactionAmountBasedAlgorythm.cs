using FeesService.BLL.Models;
using FeesService.DAL.Entities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.BLL.Services.FeeCalculator;

public class TransactionAmountBasedAlgorythm : CalculationAlgorithm
{
    public TransactionAmountBasedAlgorythm(Dictionary<int, FeeData> schemas, CalcInputData inputData)
           : base(schemas, inputData) { }
    public override Dictionary<int, FeeData> Execute()
    {
        decimal feeAmount;

        foreach (KeyValuePair <int, FeeData> fe in ProcessFeesSettings(schemas))
        {
            feeAmount = Math.Round(inputData.TransactionAmount / 100 * fe.Value.Percent + fe.Value.FixFees, 2, MidpointRounding.AwayFromZero);
            fe.Value.Amount = feeAmount;
        }
        return ProcessFeesSet(schemas);
    }
    protected override Dictionary<int, FeeData> ProcessFeesSet(Dictionary<int, FeeData> feesSet)
    {
        decimal feeAmount = 0;
        int clientFeeKey = (int)FeesType.ClientFee;
        FeeData sendingPartnerFee, brokerFee, receivingPartnerFee;
        if (feesSet.TryGetValue((int)FeesType.SendingPartnerFee, out sendingPartnerFee!)) feeAmount = sendingPartnerFee.Amount;
        if (feesSet.TryGetValue((int)FeesType.BrokerFee, out brokerFee!)) feeAmount += brokerFee.Amount;
        if (feesSet.TryGetValue((int)FeesType.ReceivingPartnerFee, out receivingPartnerFee!)) feeAmount += receivingPartnerFee.Amount;
        if (!feesSet.TryGetValue(clientFeeKey, out _))      
            feesSet.Add(clientFeeKey, new FeeData(-1, -1, inputData.TransactionCurrency, (int)CalcType.Undefined,
                        0, 1000000, clientFeeKey, 0, 0, -1, new Guid(), 0, feeAmount));      

        return base.ProcessFeesSet(feesSet);
    }
}

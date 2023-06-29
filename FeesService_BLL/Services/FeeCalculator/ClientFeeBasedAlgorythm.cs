using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;

namespace FeesService_BLL.Services.FeeCalculator
{
    public class ClientFeeBasedAlgorythm : CalculationAlgorithm
    {
        public ClientFeeBasedAlgorythm(Dictionary<int, FeeData> schemas, CalcInputData inputData)
               : base(schemas, inputData) { }
        public override Dictionary<int, FeeData> Execute()
        {
            decimal feeAmount;
            Dictionary<int, FeeData> feeData = ProcessFeesSettings(schemas);
            int clientFeeKey = (int)FeesType.ClientFee;
            decimal clientFeeAmount = feeData[clientFeeKey].Amount;

            if (clientFeeAmount > 0)
            {
                clientFeeAmount = Math.Round(inputData.TransactionAmount / 100 * feeData[clientFeeKey].Percent,
                                       2, 
                                       MidpointRounding.AwayFromZero);
                feeData[clientFeeKey].Amount = clientFeeAmount;

                foreach (KeyValuePair <int, FeeData> fe in feeData.Where(f => f.Value.FeesType != clientFeeKey))
                {
                    switch (fe.Value.CalcType)
                    {
                        case (int)CalcType.PercentageOfTransactionAmount:
                            {
                                fe.Value.Amount = Math.Round(inputData.TransactionAmount / 100 * fe.Value.Percent, 
                                                             2, 
                                                             MidpointRounding.AwayFromZero);                           
                                break;
                            }
                        case (int)CalcType.PercentageOfClientFee:
                            {
                                fe.Value.Amount = Math.Round(clientFeeAmount / 100 * fe.Value.Percent0, 
                                                             2, 
                                                             MidpointRounding.AwayFromZero);
                                break;
                            }
                        case (int)CalcType.Combined:
                            {
                                feeAmount = Math.Round(inputData.TransactionAmount / 100 * fe.Value.Percent, 
                                                       2, 
                                                       MidpointRounding.AwayFromZero);
                                fe.Value.Amount = Math.Min(feeAmount, clientFeeAmount);
                                break;
                            }
                    }
                }
                return this.ProcessFeesSet(feeData);
            }
            else throw new Exception("No client fee setting is found");
        }

        protected override Dictionary<int, FeeData> ProcessFeesSettings(Dictionary<int, FeeData> schemas)
        {
            base.ProcessFeesSettings(schemas);
            if (schemas.TryGetValue((int)FeesType.ClientFee, out _) &&
                schemas[(int)FeesType.SendingPartnerFee].CalcType == (int)CalcType.Combined)
                schemas.Remove((int)FeesType.FeeToDebitSendingPartner);

            return schemas;
        }
        protected override Dictionary<int, FeeData> ProcessFeesSet(Dictionary<int, FeeData> feesSet)
        {
            FeeData SendingPartnerFee;
            FeeData FeeToDebitSendingPartner;
            if (feesSet.TryGetValue((int)FeesType.SendingPartnerFee, out SendingPartnerFee!) && 
                SendingPartnerFee!.CalcType == (int)CalcType.Combined)
            {
                if (feesSet.TryGetValue((int)FeesType.FeeToDebitSendingPartner, out FeeToDebitSendingPartner!))
                {
                    FeeToDebitSendingPartner.Amount = feesSet[(int)FeesType.ClientFee].Amount - SendingPartnerFee!.Amount;
                }
            }
            return base.ProcessFeesSet(feesSet);
        }
    }
}

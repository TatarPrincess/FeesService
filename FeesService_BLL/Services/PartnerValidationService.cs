using FeesService_BLL.Models;
using FeesService_BLL.Models.Partner;
using FeesService_BLL.Services.Interfaces;

namespace FeesService_BLL.Services
{
    public class PartnerValidationService
    {
        private readonly CalcInputData _calcInputData;
        private readonly IPartnerService _partnerService;
        private readonly IValidationService _validationService;

        public PartnerValidationService(CalcInputData calcInputData,
                                        IPartnerService partnerService,
                                        IValidationService validationService)
        {
            if (calcInputData is null) throw new NullReferenceException("No calcInputData is passed");
            if (partnerService is null) throw new NullReferenceException("No partnerService is passed");

            _partnerService = partnerService;
            _calcInputData = calcInputData;
            _validationService = validationService;

            ((ValidationService)_validationService).FeeDestinationsRetrieved += CheckSendingPartnerState;
            ((ValidationService)_validationService).FeeDestinationsRetrieved += CheckReceivingPartnerState;
            ((ValidationService)_validationService).FeeDestinationsRetrieved += CheckSendingPartnerCurrency;
            ((ValidationService)_validationService).FeeDestinationsRetrieved += CheckReceivingPartnerCurrency;
        }

        private void CheckState(int partnerState, PartnerType type)
        {
            if (partnerState != 1) throw new Exception($"{type} partner's state should be \"Working\"");
        }

        private void CheckSendingPartnerState(object? sender, EventArgs e)
        {
            try
            {
                CheckState(_partnerService.GetPartnerData(_calcInputData.SendingPartner!.partnerCode).State,
                            PartnerType.Sending);
            }
            finally
            {
                ((ValidationService)_validationService).FeeDestinationsRetrieved -= CheckSendingPartnerState;
            }                      
        }

        private void CheckReceivingPartnerState(object? sender, EventArgs e)
        {
            try 
            {
                CheckState(_partnerService.GetPartnerData(_calcInputData.ReceivingPartner!.partnerCode).State,
                                PartnerType.Receiving);
            }
            finally 
            {
                ((ValidationService)_validationService).FeeDestinationsRetrieved -= CheckReceivingPartnerState;
            }
        }

        private void CheckCurrency(List<PartnerCurrency>? currency, PartnerType type)
        {
                if (currency != null && !(currency.Select(p => p.Currency).Contains(_calcInputData.TransactionCurrency)))
                    throw new Exception($"The list of {type} partner's currencies should contain transfer currency");
        }

        private void CheckSendingPartnerCurrency(object? sender, EventArgs e)
        {
            try 
            {
                CheckCurrency(_partnerService.GetPartnerData(_calcInputData.SendingPartner!.partnerCode).PartnerCurrency,
                            PartnerType.Sending);
            }
            finally
            {
                ((ValidationService)_validationService).FeeDestinationsRetrieved -= CheckSendingPartnerCurrency;
            }
            
        }
            
        private void CheckReceivingPartnerCurrency(object? sender, EventArgs e)
        {
            try 
            {
                CheckCurrency(_partnerService.GetPartnerData(_calcInputData.ReceivingPartner!.partnerCode).PartnerCurrency,
                            PartnerType.Receiving);
            }
            finally 
            {
                ((ValidationService)_validationService).FeeDestinationsRetrieved -= CheckReceivingPartnerCurrency;
            }       
        }
    }
}

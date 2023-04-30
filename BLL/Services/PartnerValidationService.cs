﻿using FeesService.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeesService.BLL.Services;

namespace FeesService.BLL.Services
{
    public class PartnerValidationService : IValidator
    {
        private readonly CalcInputData _calcInputData;
        private readonly PartnerService _partnerService;
        
        public PartnerValidationService(CalcInputData calcInputData)
        {
            if (calcInputData != null)
            {
                _calcInputData = calcInputData;
            }
            else throw new Exception();
            _partnerService = new PartnerService(); //вот эту связь надо переделать на DI!
        }

        private bool CheckPartnerState(PartnerType type)
        {
            int partnerState;
            switch (type)
            {
                case PartnerType.Sending:
                {
                  partnerState = _partnerService.GetPartnerData(_calcInputData.SendingPartner!.partnerCode).State;
                  break;
                }
                case PartnerType.Receiving:
                {
                  partnerState = _partnerService.GetPartnerData(_calcInputData.ReceivingPartner!.partnerCode).State;
                  break;
                }
                default: partnerState = 0; break;
            }
            return (partnerState == 1) ? true : throw new Exception($"{type} partner's state should be \"Working\""); 
        }
        private bool CheckPartnerCurrency(PartnerType type)
        {
            int partnerId = 0;
            switch (type)
            {
                case PartnerType.Sending: 
                    partnerId = _partnerService.GetPartnerData(_calcInputData.SendingPartner!.partnerCode).Id;
                    break;
                case PartnerType.Receiving:
                    partnerId = _partnerService.GetPartnerData(_calcInputData.ReceivingPartner!.partnerCode).Id;
                    break;
            }
            List<int>? currencies = _partnerService.GetPartnerCurrency(partnerId, type);
            return (currencies != null) 
                   ? currencies.Contains(_calcInputData.TransactionCurrency) 
                   : throw new Exception($"The list of {type} partner's currencies should contain transfer currecny");            
        }
        public bool Check()
        {
            return
            CheckPartnerState(PartnerType.Sending) &&
            CheckPartnerState(PartnerType.Receiving) &&
            CheckPartnerCurrency(PartnerType.Sending) &&
            CheckPartnerCurrency(PartnerType.Receiving);
        }
    }
}

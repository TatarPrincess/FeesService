using FeesService_BLL.Models;
using FeesService_BLL.Services.FeeCalculator;
using FeesService_BLL.Services;
using FeesService_BLL.IRepositories;
using FeesService_DAL.Repositories;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace FeesService_Root
{
    public static class Controller
    {
        public static FeeService Configure(CalcInputData calcInputData)
        {
            DbProviderFactory provider = SqlClientFactory.Instance;
            IDestinationRepository destinationRepository = new DestinationRepository(provider);
            IPartnerRepository partnerRepository = new PartnerRepository(provider);
            IPartnerCurrencyRepository partnerCurrencyRepository = new PartnerCurrencyRepository(provider);
            PartnerService partnerService = new (partnerRepository, partnerCurrencyRepository);
            DestinationService destinationService = new (calcInputData, destinationRepository, partnerService);

            PartnerValidationService partnerValidationService = new (calcInputData, partnerService);
            DestinationValidationService destinationValidationService = new (calcInputData, destinationService);
            List<IValidator> validators = new (); 
            validators.Add(partnerValidationService);
            validators.Add(destinationValidationService);
            ValidationService validationService = new (validators);

            IFeesSettingsRepository feesSettingsRepository = new FeesSettingsRepository(provider);
            FeeSettingsService feeSettingsService = new (destinationService, calcInputData, feesSettingsRepository);
            FeeCalculator feeCalculator = new (calcInputData, feeSettingsService, partnerService);

            return new FeeService(destinationService, validationService, feeCalculator);
        }       
    }
}

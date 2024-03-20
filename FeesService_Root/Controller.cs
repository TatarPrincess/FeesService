using FeesService_BLL.Models;
using FeesService_BLL.Services.FeeCalculator;
using FeesService_BLL.Services;
using FeesService_BLL.IRepositories;
using FeesService_DAL.Repositories;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using FeesService_BLL.Models.Partner;
using FeesService_BLL.Services.Interfaces;
using System.Net.NetworkInformation;

namespace FeesService_Root
{
    public static class Controller
    {
        private static List<BaseRepository> _disposables = new List<BaseRepository>();
        public static string GetProviderDataFromConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(@"C:\Store\C#\FeesService\FeesService")
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            var providerName = config["ProviderName"];

            if (providerName == "SqlServer")
            {
                var cn = config[$"{providerName}:ConnectionString"];
                return cn ?? "";
            }
            else throw new Exception("Invalid data provider value supplied.");

        }
        public static FeeService Configure(CalcInputData calcInputData)
        {
            string cn = GetProviderDataFromConfiguration();
            DbProviderFactory provider = SqlClientFactory.Instance; //добавь это в конфигурцию и читай оттуда, сейчас прибито гвоздями
            IDestinationRepository destinationRepository = new DestinationRepository(provider, cn);
            TrackDisposable((BaseRepository) destinationRepository);
            IPartnerRepository partnerRepository = new PartnerRepository(provider, cn);
            IPartnerService partnerService = new PartnerService(partnerRepository);
            IDestinationService destinationService = new DestinationService(calcInputData, destinationRepository, partnerService);

            IValidationService validationService = new ValidationService();
            PartnerValidationService partnerValidationService = new PartnerValidationService(calcInputData, partnerService, validationService);
            DestinationValidationService destinationValidationService = new DestinationValidationService(calcInputData, destinationService, validationService);
          
            IFeesSettingsRepository feesSettingsRepository = new FeesSettingsRepository(provider, cn);
            FeeSettingsService feeSettingsService = new(destinationService, calcInputData, feesSettingsRepository);
            Dictionary<PartnerCategory, ICalculationAlgorithm> algorithms = new ()
            {
              { PartnerCategory.SmallEquity, new ClientFeeBasedAlgorythm()},
              { PartnerCategory.LargeEquity, new TransactionAmountBasedAlgorythm()}
            };

            IFeeCalculator feeCalculator = new FeeCalculator (feeSettingsService, partnerService, algorithms);

            return new FeeService(destinationService, validationService, feeCalculator, calcInputData);
        }
        private static void TrackDisposable(BaseRepository disposable)
        {
            if (!_disposables.Contains(disposable)) _disposables.Add(disposable);
        }
    }
}

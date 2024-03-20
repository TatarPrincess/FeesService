using FeesService_BLL.Models;
using FeesService_BLL.Models.Fees;
using FeesService_BLL.IRepositories;
using FeesService_BLL.Services.Interfaces;


namespace FeesService_BLL.Services.FeeCalculator
{
    public class FeeSettingsService
    {
        private readonly IEnumerable<Destination>? _dests;
        private readonly CalcInputData _calcInputData;
        private readonly IFeesSettingsRepository _feesSettingsRepository;
        public FeeSettingsService(IDestinationService destinationService,
                                  CalcInputData calcInputData,
                                  IFeesSettingsRepository feesSettingsRepository)
        {
            if (destinationService is null) throw new NullReferenceException("No destinationService is passed");
            if (calcInputData is null) throw new NullReferenceException("No calcInputData is passed");
            if (feesSettingsRepository is null) throw new NullReferenceException("No feesSettingsRepository is passed");
            
            _calcInputData = calcInputData;
            _feesSettingsRepository = feesSettingsRepository;
            if (destinationService.GetDestinations() != null) _dests = destinationService.GetDestinations()!;
            else Console.WriteLine("No relevant fee settings is found. Try to change input parameters");
        }
        public List<FeesSettings> GetFeesSettingsForDestinations()
        {
            List<FeesSettings> schemas = new List<FeesSettings>();

            var groupedDests = _dests!.Where(d => d.Blocked == false)
                                     .GroupBy(e => new { e.SectionId, e.SectionPriority })
                                     .OrderByDescending(f => f.Key.SectionPriority);
            IEnumerable<FeesSettings> fsets;

            foreach (var group in groupedDests)
            {
                foreach (var record in group.OrderByDescending(g => g.Priority))
                {
                    fsets = _feesSettingsRepository.FindAllByDestination(record.Id);
                    schemas.AddRange(fsets
                                      .Where(s => s.Currency == _calcInputData.TransactionCurrency &&
                                                  s.MinAmount <= _calcInputData.TransactionAmount &&
                                                  s.MaxAmount >= _calcInputData.TransactionAmount )); //&& schemas.FindIndex(m => m.CalcType == s.CalcType) < 0
                }
            }
            return schemas;
        }
    }
}

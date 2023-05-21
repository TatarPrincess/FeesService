using FeesService.BLL.Models;
using FeesService.DAL.Entities;
using FeesService.DAL.Repositories;
using Microsoft.Data.SqlClient;
using System.Data.Common;


namespace FeesService.BLL.Services.FeeCalculator
{
    public class FeeSettingsService
    {
        private readonly IEnumerable<DestinationEntity> _dests;
        private readonly CalcInputData _calcInputData;
        private readonly FeesSettingsRepository _feesSettingsRepository;
        public FeeSettingsService(IEnumerable<DestinationEntity> dests, CalcInputData calcInputData)
        {
            _calcInputData = calcInputData;
            _dests = dests;
            DbProviderFactory provider = SqlClientFactory.Instance;
            _feesSettingsRepository = new FeesSettingsRepository(provider);
        }
        public List<FeesSettingsEntity> GetFeesSettingsForDestinations()
        {
            List<FeesSettingsEntity> schemas = new List<FeesSettingsEntity>();

            var groupedDests = _dests.Where(d => d.Blocked == false)
                                     .GroupBy(e => new { e.SectionId, e.SectionPriority })
                                     .OrderByDescending(f => f.Key.SectionPriority);
            IEnumerable<FeesSettingsEntity> fsets;

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

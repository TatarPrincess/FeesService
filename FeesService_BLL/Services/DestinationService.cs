using FeesService_BLL.Models;
using FeesService_BLL.IRepositories;

namespace FeesService_BLL.Services
{
    public class DestinationService
    {
        private readonly IDestinationRepository _destinationRepository;
        public  readonly CalcInputData _calcInputData;
        public readonly PartnerService _partnerService;
        private readonly IEnumerable<Destination>? _dests;

        public IEnumerable<Destination> Dests => _dests;
        public DestinationService(CalcInputData calcInputData,
                                  IDestinationRepository destinationRepository,
                                  PartnerService partnerService)
        {
            _destinationRepository = destinationRepository;
            _calcInputData = calcInputData;
            _partnerService = partnerService;
            _dests = GetRelevantDestinations();

        }

        private IEnumerable<Destination>? GetRelevantDestinations()
        {
            int senderId = _partnerService.GetPartnerData(_calcInputData.SendingPartner!.partnerCode).Id;
            int receiverId = _partnerService.GetPartnerData(_calcInputData.ReceivingPartner!.partnerCode).Id;

            List<Destination>? dests = (List<Destination>?)_destinationRepository
                                            .FindAllBySenderAndReceiver(senderId, receiverId);
            return dests;         
        }
    }
}

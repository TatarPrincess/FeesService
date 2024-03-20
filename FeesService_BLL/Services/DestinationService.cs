using FeesService_BLL.Models;
using FeesService_BLL.IRepositories;
using FeesService_BLL.Services.Interfaces;

namespace FeesService_BLL.Services
{
    public class DestinationService : IDestinationService
    {
        public readonly CalcInputData _calcInputData;
        public readonly IPartnerService _partnerService;
        private readonly IDestinationRepository _destinationRepository;
        private IEnumerable<Destination>? _dests = null;

        public DestinationService(CalcInputData calcInputData,
                                  IDestinationRepository destinationRepository,
                                  IPartnerService partnerService)
        {
            if (calcInputData is null) throw new NullReferenceException("No calcInputData is passed");
            if (destinationRepository is null) throw new NullReferenceException("No destinationRepository is passed");
            if (partnerService is null) throw new NullReferenceException("No partnerService is passed");

            _destinationRepository = destinationRepository;
            _calcInputData = calcInputData;
            _partnerService = partnerService;
        }

        public IEnumerable<Destination> GetDestinations()
        {
            if (_dests is null)
            {
                int senderId = _partnerService.GetPartnerData(_calcInputData.SendingPartner!.partnerCode).Id;
                int receiverId = _partnerService.GetPartnerData(_calcInputData.ReceivingPartner!.partnerCode).Id;

                List<Destination>? dests = (List<Destination>?)_destinationRepository
                                                .FindAllBySenderAndReceiver(senderId, receiverId);
                _dests = dests;
            }
            return _dests;
        }
    }
}

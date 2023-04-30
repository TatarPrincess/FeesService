using FeesService.BLL.Models;
using FeesService.DAL.Entities;
using FeesService.DAL.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.BLL.Services
{
    public class DestinationService
    {
        private readonly IDestination _destinationRepository;
        public  readonly CalcInputData _calcInputData; 


        public DestinationService(CalcInputData calcInputData)
        {
            DbProviderFactory provider = SqlClientFactory.Instance;
            _destinationRepository = new DestinationRepository(provider);
            _calcInputData = calcInputData;
        }

        public IEnumerable<DestinationEntity>? GetRelevantDestinations()
        {
            PartnerService partnerService = new PartnerService(); //вот эту связь надо переделать на DI!
            int senderId = partnerService.GetPartnerData(_calcInputData.SendingPartner!.partnerCode).Id;
            int receiverId = partnerService.GetPartnerData(_calcInputData.ReceivingPartner!.partnerCode).Id;

            List<DestinationEntity>? dests = (List<DestinationEntity>?)_destinationRepository
                                            .FindAllBySenderAndReceiver(senderId, receiverId);
            return dests;         
        }
    }
}

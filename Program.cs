using System.Data;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using FeesService.BLL.Models;
using FeesService.DAL.Repositories;
using FeesService.DAL.Entities;
using FeesService.BLL.Services;


CalcInputData calcInputData = new CalcInputData("AAAA", "CCCC", 0, 100);
//1. Getting relevant to input data destinations 
DestinationService destinationService = new DestinationService(calcInputData);
var dests = destinationService.GetRelevantDestinations();
if (dests != null)
{
    // 2. Validation of relevant destinations
    List<IValidator> validators = new List<IValidator>(); //переделай это на DI
    validators.Add(new PartnerValidationService(calcInputData)); 
    validators.Add(new DestinationValidationService(dests, calcInputData)); 
    ValidationService validationService = new ValidationService(validators);
    validationService.Check();
    // 3. Getting fees schemas
    FeeSettingsService feeSettingsService = new FeeSettingsService(dests, calcInputData);
    List<FeesSettingsEntity>  feesSettings = feeSettingsService.GetFeesSettingsForDestinations();

    // 4. Calculation of fees by fees schemas
    PartnerEntity sendingPartnerData = new PartnerService().GetPartnerData(calcInputData.SendingPartner.partnerCode);
    List<FeeEntity> fees = new CalculationAlgorithm(feesSettings).Execute();
}


if (dests is not null)
{
    foreach (DestinationEntity dest in dests)
        Console.WriteLine("Selected destinations ID: {0}", dest.Id);
}








using FeesService_BLL.Models.Partner;
using FeesService_BLL.Services.Interfaces;

namespace FeesService_BLL.Services
{
    public class ValidationService : IValidationService
    {      
        public event EventHandler? FeeDestinationsRetrieved;
      
        public bool Validate()
        {
            try 
            {
                FeeDestinationsRetrieved?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
               Console.WriteLine($"Exception is caught - {ex}");
                return false;
            }            
        }
    }
}

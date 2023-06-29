
namespace FeesService_BLL.Services
{
    public class ValidationService
    {
        private readonly List<IValidator> validators;

        public ValidationService(List<IValidator> validators)
        {
            this.validators = validators;
        }

        public bool Check()
        {
            try 
            {
                foreach (IValidator validator in validators)
                {
                    if (validator.Check() == false) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
        }
    }
}

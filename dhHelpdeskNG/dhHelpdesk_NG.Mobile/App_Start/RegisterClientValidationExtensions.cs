using DH.Helpdesk.Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(RegisterClientValidationExtensions), "Start")]

namespace DH.Helpdesk.Web
{
    using DataAnnotationsExtensions.ClientValidation;

    public static class RegisterClientValidationExtensions
    {
        #region Public Methods and Operators

        public static void Start()
        {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();
        }

        #endregion
    }
}
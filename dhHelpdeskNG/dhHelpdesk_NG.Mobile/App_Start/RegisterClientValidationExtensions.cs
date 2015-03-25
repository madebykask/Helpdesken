using DH.Helpdesk.Mobile;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(RegisterClientValidationExtensions), "Start")]

namespace DH.Helpdesk.Mobile
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
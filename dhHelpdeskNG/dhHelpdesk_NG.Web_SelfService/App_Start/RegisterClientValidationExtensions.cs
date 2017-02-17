using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(DH.Helpdesk.SelfService.App_Start.RegisterClientValidationExtensions), "Start")]
 
namespace DH.Helpdesk.SelfService.App_Start {
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}
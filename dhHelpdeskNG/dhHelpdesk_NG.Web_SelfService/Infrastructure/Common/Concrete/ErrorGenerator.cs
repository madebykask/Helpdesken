namespace DH.Helpdesk.SelfService.Infrastructure.Common.Concrete
{            
    using DH.Helpdesk.BusinessData.Models.Error;

    public static class ErrorGenerator
    {
        public static ErrorModel MakeError(string message, int? code = null)
        {
            var err = new ErrorModel((code!=null? code.Value:0), message);
            SessionFacade.LastError = err;
            return err;            
        }

        public static void MakeError(ErrorModel errModel)
        {            
            SessionFacade.LastError = errModel;            
        }
    }
}
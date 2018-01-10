namespace DH.Helpdesk.SelfService.Models.Error
{
    public class Error
    {
        #region ctor()

        public Error()
        {            
        }

        public Error(string message, string code = null, string backUrl = null)
        {
            ErrorCode = code;
            ErrorMessage = message;
            BackURL = backUrl;
        }

        #endregion

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string BackURL { get; set; }        
    }
}
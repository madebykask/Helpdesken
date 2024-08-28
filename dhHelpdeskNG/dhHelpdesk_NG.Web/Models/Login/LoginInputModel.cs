namespace DH.Helpdesk.Web.Models.Login
{
    public class LoginInputModel
    {
        public string txtUid { get; set; }
        public string txtPwd { get; set; }
        public string timeZoneOffsetInJan1 { get; set; }
        public string timeZoneOffsetInJul1 { get; set; }
        public string returnUrl { get; set; }
        public string returnUrlMS { get; set; }
        public string btnLogin { get; set; }
        public string reCaptchaToken { get; set; }
        public bool useRecaptcha { get; set; }
    }
}
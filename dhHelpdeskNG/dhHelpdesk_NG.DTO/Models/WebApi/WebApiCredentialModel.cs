namespace DH.Helpdesk.BusinessData.Models.WebApi
{
    public class WebApiCredentialModel
    {
        public WebApiCredentialModel(string uriPath, string userName, string password)
        {
            UriPath = uriPath;
            UserName = userName;
            Password = password;
        }

        public string UriPath { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

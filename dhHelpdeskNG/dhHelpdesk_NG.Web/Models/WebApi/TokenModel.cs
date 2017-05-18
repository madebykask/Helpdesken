namespace DH.Helpdesk.Web.Models.WebApi
{
    public class ApplicationToken
    {

        public ApplicationToken()
        {

        }

        public string access_token { get; set; }        

        public string refresh_token { get; set; }

        public int expires_in { get; set; }

        public string token_type { get; set; }
        
    }    
}

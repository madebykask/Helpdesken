using Newtonsoft.Json;

namespace upKeeper2Helpdesk.entities
{
    public class Token
	{

        public string Access_token { get; set; }

        [JsonProperty("token")]
        public string Access_token_v5 { get; set; }  
		
	}
}

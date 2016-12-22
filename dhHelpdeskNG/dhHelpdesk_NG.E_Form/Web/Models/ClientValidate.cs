using System.Collections.Generic;

namespace ECT.Web.Models
{
    public class ClientValidate
    {
        public ClientValidate()
        {
            Rules = new Dictionary<string, string>();
            //Messages = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public IDictionary<string, string> Rules { get; set; }
        //public IDictionary<string, string> Messages { get; set; } 
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECT.FormLib.Models
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
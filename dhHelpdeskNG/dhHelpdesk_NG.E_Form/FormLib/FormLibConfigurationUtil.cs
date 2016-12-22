using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DH.Helpdesk.EForm.FormLib
{
    // Work in progress....
    public class FormLibConfigurationUtil
    {
        public static Configuration Configuration { get; set; }

        static FormLibConfigurationUtil()
        {
            Configuration = new Configuration();
            string json = File.ReadAllText("config.json"); // to be path to json
            Configuration = JsonConvert.DeserializeObject<Configuration>(json);
        }
    }

    public class Configuration
    {
        public Dictionary<string, string> Settings { get; set; }
    }

    /*
     * 
     * JSON = {
            "Configuration": { 
     *          "Settings" : { 
     *              
     *          }
     *      }
     * }
     * 
     * */
}
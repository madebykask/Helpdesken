using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DH.Helpdesk.BusinessData.Models.ExtendedCase;
namespace DH.Helpdesk.TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			string metaData = "";
            var hejAnnaKarin = JsonConvert.DeserializeObject<ExtendedCaseFormJsonModel>(metaData);

            var alltingfungerarkanske = "blabla";

            //var mailer = new Mail();

            //mailer.Send("license@standby.eu", "johan.weinitz@dhsolutions.se", "test subject " + DateTime.Now.ToString(), "test body", "", "smtp.datahalland.se", Guid.NewGuid().ToString());
            //mailer.Send("license@standby.eu", "johan.weinitz@dhsolutions.se", "test subject", "test body", "", "smtp.office365.com;license@standby.eu;Bjare741!;587;true", Guid.NewGuid().ToString());
        }
	}
}

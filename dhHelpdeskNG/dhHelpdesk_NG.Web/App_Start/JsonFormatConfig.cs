using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DH.Helpdesk.Web
{
	public class JsonFormatConfig
	{
		public static void ConfigWebApi()
		{
			var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
			//By default it is ISO 8601 local time 2012-07-27T11:51:45.53403-07:00

			// Convert all dates to UTC 2012-07-27T18:51:45.53403Z
			//json.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			//json.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
			//json.SerializerSettings.DateFormatString = "MM/dd/yyyy-MM-dd";

			// Convert to Microsoft JSON date format ("\/Date(ticks)\/") 
			// json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
		}

		public static void ConfigMVC()
		{
			//// remove default implementation    
			//ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories
			//	.OfType<JsonValueProviderFactory>()
			//	.FirstOrDefault());
			//// add our custom one
			//ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());
		}
	}
}
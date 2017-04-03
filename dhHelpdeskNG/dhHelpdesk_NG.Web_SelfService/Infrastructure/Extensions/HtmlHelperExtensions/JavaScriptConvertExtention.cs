using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace DH.Helpdesk.SelfService.Infrastructure.Extensions.HtmlHelperExtensions
{
	public static class JavaScriptConvertExtention
	{
		//public static IHtmlString SerializeObject(this HtmlHelper helper, object value)
		//{
		//	return SerializeObject(helper, value, JsonSerializerSettings);
		//}

		//public static IHtmlString SerializeObject(this HtmlHelper helper, object value, string dateFormatString)
		//{
		//	var settings = new JsonSerializerSettings { DateFormatString = dateFormatString };
		//	return SerializeObject(helper, value, settings);
		//}

		public static IHtmlString SerializeObject(this HtmlHelper helper, object value, JsonSerializerSettings settings)
		{
			var stringWriter = new StringWriter();
			using (var jsonWriter = new JsonTextWriter(stringWriter))
			{
				var serializer = new JsonSerializer
				{
					//Settings here                   
					DateFormatHandling = settings.DateFormatHandling,
					DateFormatString = settings.DateFormatString,
					DateParseHandling = settings.DateParseHandling,
					DateTimeZoneHandling = settings.DateTimeZoneHandling
				};
				// We don't want quotes around object names
				jsonWriter.QuoteName = false;
				serializer.Serialize(jsonWriter, value);
			}
			return new HtmlString(stringWriter.ToString());
		}

		public static IHtmlString SerializeObjectToJson(this HtmlHelper helper, object value)
		{
			var stringWriter = new StringWriter();
			using (var jsonWriter = new JsonTextWriter(stringWriter))
			{
				var serializer = new JsonSerializer
				{
					////Settings here                   
					//DateFormatHandling = JsonSerializerSettings.DateFormatHandling,
					//DateFormatString = JsonSerializerSettings.DateFormatString,
					//DateParseHandling = JsonSerializerSettings.DateParseHandling,
					//DateTimeZoneHandling = JsonSerializerSettings.DateTimeZoneHandling
				};
				jsonWriter.QuoteName = true;
				serializer.Serialize(jsonWriter, value);
			}
			return new HtmlString(stringWriter.ToString());

		}

		//public static JsonSerializerSettings JsonSerializerSettings
		//{
		//	get
		//	{
		//		return GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
		//	}
		//}
	}
}
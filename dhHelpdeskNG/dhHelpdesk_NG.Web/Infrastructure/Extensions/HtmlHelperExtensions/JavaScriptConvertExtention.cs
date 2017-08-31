using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
	public static class JavaScriptConvertExtention
	{
        private const string _jSViewDataName = "RenderJavaScript";
        private const string _styleViewDataName = "RenderStyle";

        public static void AddJavaScript(this HtmlHelper htmlHelper,
                                   string scriptURL)
        {
            List<string> scriptList = htmlHelper.ViewContext.HttpContext
              .Items[JavaScriptConvertExtention._jSViewDataName] as List<string>;
            if (scriptList != null)
            {
                if (!scriptList.Contains(scriptURL))
                {
                    scriptList.Add(scriptURL);
                }
            }
            else
            {
                scriptList = new List<string>();
                scriptList.Add(scriptURL);
                htmlHelper.ViewContext.HttpContext
                  .Items.Add(JavaScriptConvertExtention._jSViewDataName, scriptList);
            }
        }
        public static MvcHtmlString RenderJavaScripts(this HtmlHelper HtmlHelper)
        {
            StringBuilder result = new StringBuilder();

            List<string> scriptList = HtmlHelper.ViewContext.HttpContext
              .Items[JavaScriptConvertExtention._jSViewDataName] as List<string>;
            if (scriptList != null)
            {
                foreach (string script in scriptList)
                {
                    result.AppendLine(string.Format(
                      "<script type=\"text/javascript\" src=\"{0}\"></script>",
                      script));
                }
            }

            return MvcHtmlString.Create(result.ToString());
        }

        public static void AddStyle(this HtmlHelper htmlHelper, string styleURL)
        {
            List<string> styleList = htmlHelper.ViewContext.HttpContext
              .Items[JavaScriptConvertExtention._styleViewDataName] as List<string>;

            if (styleList != null)
            {
                if (!styleList.Contains(styleURL))
                {
                    styleList.Add(styleURL);
                }
            }
            else
            {
                styleList = new List<string>();
                styleList.Add(styleURL);
                htmlHelper.ViewContext.HttpContext
                  .Items.Add(JavaScriptConvertExtention._styleViewDataName, styleList);
            }
        }

        public static MvcHtmlString RenderStyles(this HtmlHelper htmlHelper)
        {
            StringBuilder result = new StringBuilder();

            List<string> styleList = htmlHelper.ViewContext.HttpContext
              .Items[JavaScriptConvertExtention._styleViewDataName] as List<string>;

            if (styleList != null)
            {
                foreach (string script in styleList)
                {
                    result.AppendLine(string.Format(
                      "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />",
                      script));
                }
            }

            return MvcHtmlString.Create(result.ToString());
        }
        /// <summary>
        /// ///////////////////
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        public static IHtmlString SerializeObject(this HtmlHelper helper, object value)
		{
			return SerializeObject(helper, value, JsonSerializerSettings);
		}

		public static IHtmlString SerializeObject(this HtmlHelper helper, object value, string dateFormatString)
		{
			var settings = new JsonSerializerSettings { DateFormatString = dateFormatString };
			return SerializeObject(helper, value, settings);
		}

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
					//Settings here                   
					DateFormatHandling = JsonSerializerSettings.DateFormatHandling,
					DateFormatString = JsonSerializerSettings.DateFormatString,
					DateParseHandling = JsonSerializerSettings.DateParseHandling,
					DateTimeZoneHandling = JsonSerializerSettings.DateTimeZoneHandling
				};
				jsonWriter.QuoteName = true;
				serializer.Serialize(jsonWriter, value);
			}
			return new HtmlString(stringWriter.ToString());

		}

		public static JsonSerializerSettings JsonSerializerSettings
		{
			get
			{
				return GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
			}
		}
	}
}
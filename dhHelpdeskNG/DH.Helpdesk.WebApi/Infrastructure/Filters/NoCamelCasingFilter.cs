using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using Newtonsoft.Json.Serialization;

namespace DH.Helpdesk.WebApi.Infrastructure.Filters
{
    public class NoCamelCasingFilter : ActionFilterAttribute
    {
        private readonly JsonMediaTypeFormatter _mediaTypeFormatter = new JsonMediaTypeFormatter();

        public NoCamelCasingFilter()
        {
            _mediaTypeFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var content = actionExecutedContext.Response.Content as ObjectContent;
            if (content?.Formatter is JsonMediaTypeFormatter)
            {
                actionExecutedContext.Response.Content = new ObjectContent(content.ObjectType, content.Value, _mediaTypeFormatter);
            }
        }
    }
}
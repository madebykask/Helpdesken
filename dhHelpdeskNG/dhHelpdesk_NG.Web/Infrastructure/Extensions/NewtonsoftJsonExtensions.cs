using System.Web.Mvc;
using Newtonsoft.Json;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    public static class NewtonsoftJsonExtensions
    {
        public static ActionResult ToJsonResult(this object obj, JsonSerializerSettings settings)
        {
            var content = new ContentResult
            {
                Content = JsonConvert.SerializeObject(obj, settings),
                ContentType = "application/json"
            };
            return content;
        }       
    }
}
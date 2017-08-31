using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    public static class NewtonsoftJsonExtensions
    {
        public static ActionResult ToJsonResult(this object obj, JsonSerializerSettings settings)
        {
            var content = new ContentResult();
            content.Content = JsonConvert.SerializeObject(obj, settings);
            content.ContentType = "application/json";
            return content;
        }
    }
}
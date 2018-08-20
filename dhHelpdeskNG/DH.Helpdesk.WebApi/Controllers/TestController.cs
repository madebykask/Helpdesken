using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;

namespace DH.Helpdesk.WebApi.Controllers
{
    
    public class TestController : BaseApiController
    {
        [HttpGet]
        [AuthorizeApi(Roles = "4")]
        public async Task<IHttpActionResult> Admin()
        {
            return await Task.FromResult(Json("admin"));
        }

        [HttpGet]
        public async Task<IHttpActionResult> AnyRole()
        {
            //var prev = HttpContext.Current.Session["name"] ?? Guid.NewGuid().ToString();
            //HttpContext.Current.Session["name"] = prev;
            return await Task.FromResult(Json("test"));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Error()
        {
            throw new Exception("Exception message");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("~/api/Test/Language/{lang}")]
        public async Task<HttpResponseMessage> Language(string lang)
        {
            var filePath = HttpContext.Current.Server.MapPath($"~/App_Data/i18n/{lang}.json");

            if (!File.Exists(filePath)) 
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File not found");

            string content;
            using (var reader = File.OpenText(filePath))
            {
                content = await reader.ReadToEndAsync();
            }

            var msg = new HttpResponseMessage()
            {
                Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json")
            };

            return msg;

        }
    }
}

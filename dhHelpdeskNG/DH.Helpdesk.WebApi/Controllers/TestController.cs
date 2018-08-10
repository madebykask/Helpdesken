using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;

namespace DH.Helpdesk.WebApi.Controllers
{
    
    public class TestController : BaseApiController
    {
        [HttpGet]
        [AuthorizeApi(Roles = "Test")]
        public async Task<IHttpActionResult> Admin()
        {
            return await Task.FromResult(Json("test"));
        }

        [HttpGet]
        public async Task<IHttpActionResult> Anyone()
        {
            var prev = HttpContext.Current.Session["name"] ?? Guid.NewGuid().ToString();
            HttpContext.Current.Session["name"] = prev;
            return await Task.FromResult(Json(prev));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Error()
        {
            throw new Exception("Exception message");
        }
    }
}

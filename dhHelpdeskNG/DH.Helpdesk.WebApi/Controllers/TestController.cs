using System;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;

namespace DH.Helpdesk.WebApi.Controllers
{
    /// <summary>
    /// Ignore - Test controller.
    /// </summary>
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
        public IHttpActionResult Test()
        {
            return Ok("success");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Error()
        {
            throw new Exception("Exception message");
        }
    }
}

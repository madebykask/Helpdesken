using DH.Helpdesk.Web.Infrastructure;
using System.Web.Http;

namespace DH.Helpdesk.Web.Areas.WebApi
{    
    public class TestApiController : BaseApiController
    {

        [HttpGet]
        [Authorize]
        public string GetMyId()
        {
            return "My ID: 2525";
        }
    }    
}

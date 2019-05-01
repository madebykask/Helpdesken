using System.Web.Http;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/Users")]
    public class UsersController : BaseApiController
    {
        private readonly IUserEmailsSearchService _userEmailsSearchService;

        public UsersController(IUserEmailsSearchService userEmailsSearchService)
        {
            _userEmailsSearchService = userEmailsSearchService;
        }

        [HttpGet]
        [Route("searchEmails")]
        public IHttpActionResult SearchEmails(string query, int cid)
        {
            var searchScope = BusinessData.Models.Email.EmailSearchScope.All();
            var models = _userEmailsSearchService.GetUserEmailsForCaseSend(cid, query, searchScope);
            return Ok(models);
        }
    }
}
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController : BaseApiController
    {
        private readonly ICaseService _caseService;

        public HomeController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        [Route("casesstatus")]
        [HttpGet]
        public async Task<CustomerCasesStatus> GetCasesStatus(int cid)
        {
            var status = await _caseService.GetCustomerCasesStatusAsync(cid, UserId);
            return status;
        }
    }
}
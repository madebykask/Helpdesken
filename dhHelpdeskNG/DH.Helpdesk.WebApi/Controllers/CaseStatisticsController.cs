using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/casestatistics")]
    public class CaseStatisticsController : BaseApiController
    {
        private readonly ICaseService _caseService;

        public CaseStatisticsController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        [Route("")]
        [HttpGet]
        public async Task<CustomerCasesStatus> Get(int cid)
        {
            var status = await _caseService.GetCustomerCasesStatusAsync(cid, UserId);
            return status;
        }
    }
}
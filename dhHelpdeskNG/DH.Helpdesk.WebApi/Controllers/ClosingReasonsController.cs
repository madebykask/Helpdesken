using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.FinishingCause;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/closingreasons")]
    public class ClosingReasonsController : BaseApiController
    {
        private readonly IFinishingCauseService _finishingCauseService;

        public ClosingReasonsController(IFinishingCauseService finishingCauseService)
        {
            _finishingCauseService = finishingCauseService;
        }

        /// <summary>
        /// List of Finishing causes
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("options")]
        public async Task<IEnumerable<FinishingCauseOverview>> Get(int cid)
        {
            var closingReasons = await _finishingCauseService.GetFinishingCausesWithChildsAsync(cid);

            return closingReasons;
        }
    }
}
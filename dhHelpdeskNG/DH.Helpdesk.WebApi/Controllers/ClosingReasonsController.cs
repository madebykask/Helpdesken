using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        /// <param name="excludeMergedCause"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("options")]
        public async Task<IEnumerable<FinishingCauseOverview>> Get(int cid, bool excludeMergedCause = false)
        {
            var closingReasons = await _finishingCauseService.GetFinishingCausesWithChildsAsync(cid);
            
            if(excludeMergedCause)
            {
                closingReasons.Remove(closingReasons.SingleOrDefault(x=> x.Merged));
            }
            return closingReasons;
        }
    }
}
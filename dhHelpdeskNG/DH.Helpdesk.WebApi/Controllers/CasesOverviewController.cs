using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class CasesOverviewController : BaseApiController
    {
        private readonly ICaseSearchService _caseSearchService;

        public CasesOverviewController(ICaseSearchService caseSearchService)
        {
            _caseSearchService = caseSearchService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Overview()
        {
            return await Task.FromResult(Json("Overview"));
        }
    }
}

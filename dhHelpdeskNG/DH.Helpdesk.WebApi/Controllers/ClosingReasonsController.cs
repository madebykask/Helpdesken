using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.FinishingCause;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class ClosingReasonsController : BaseApiController
    {
        private readonly IFinishingCauseService _finishingCauseService;

        public ClosingReasonsController(IFinishingCauseService finishingCauseService)
        {
            _finishingCauseService = finishingCauseService;
        }

        [HttpGet]
        public async Task<IEnumerable<FinishingCauseOverview>> Get(int cid)
        {
            var closingReasons = await _finishingCauseService.GetFinishingCausesWithChildsAsync(cid);

            return closingReasons;
        }

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

    }
}
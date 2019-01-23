using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.WorkingGroup;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.ActionResults;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/workinggroups")]
    public class WorkingGroupsController : BaseApiController
    {
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IMapper _mapper;

        public WorkingGroupsController(IWorkingGroupService workingGroupService, IMapper mapper)
        {
            _workingGroupService = workingGroupService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("options")]
        public async Task<IList<ItemOverview>> Get([FromUri]int cid)
        {
            var items = await _workingGroupService.GetAllWorkingGroupsForCustomerAsync(cid, true)// TODO: filter active
                .ConfigureAwait(false);
            return items
                    .Select(d => new ItemOverview(d.WorkingGroupName, d.Id.ToString()))
                    .ToList();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id, int cid)
        {
            var wg = await _workingGroupService.GetWorkingGroupAsync(id).ConfigureAwait(false);
            if (wg == null) return BadRequest();
            if (wg.Customer_Id != cid) return StatusCode(HttpStatusCode.Forbidden);

            var model = _mapper.Map<WorkingGroupOutputModel>(wg);
            return Ok(model);
        }

    }
}
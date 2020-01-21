using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Models.Statuses;
using DH.Helpdesk.Services.Enums;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/statuses")]
    public class StatusesController : BaseApiController
    {
        private readonly IStatusService _statusService;
        private readonly ITranslateCacheService _translateCacheService;
        private readonly IMapper _mapper;

        public StatusesController(IStatusService statusService, ITranslateCacheService translateCacheService, IMapper mapper)
        {
            _statusService = statusService;
            _translateCacheService = translateCacheService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("options")]
        public async Task<List<ItemOverview>> Get(int cid, int langId)
        {
            return _statusService.GetActiveStatuses(cid)
                .Select(d => new ItemOverview(Translate(d.Name, langId, TranslationTextTypes.MasterData), d.Id.ToString()))
                .ToList();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get([FromUri]int id, [FromUri]int cid, [FromUri]int langId)
        {
            var status = _statusService.GetStatus(id);
            if (status == null) return BadRequest();
            if (status.Customer_Id != cid) return StatusCode(HttpStatusCode.Forbidden);

            status.Name = Translate(status.Name, langId, TranslationTextTypes.MasterData);
            var model = _mapper.Map<StatusOutputModel>(status);
            return Ok(model);
        }

        private string Translate(string translate, int languageId, int? tt = null)
        {
            return _translateCacheService.GetTextTranslation(translate, languageId, tt);
        }
    }
}

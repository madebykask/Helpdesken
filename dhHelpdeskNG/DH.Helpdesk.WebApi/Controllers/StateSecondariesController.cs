using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Models.StateSecondaries;
using DH.Helpdesk.Services.Enums;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/stateSecondaries")]
    public class StateSecondariesController : BaseApiController
    {
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ITranslateCacheService _translateCacheService;
        private readonly IMapper _mapper;

        public StateSecondariesController(IStateSecondaryService stateSecondaryService, ITranslateCacheService translateCacheService, IMapper mapper)
        {
            _stateSecondaryService = stateSecondaryService;
            _translateCacheService = translateCacheService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get([FromUri]int id, [FromUri]int cid, [FromUri]int langId)
        {
            var ss = await _stateSecondaryService.GetStateSecondaryAsync(id).ConfigureAwait(false);
            if (ss == null) return BadRequest();
            if (ss.Customer_Id != cid) return StatusCode(HttpStatusCode.Forbidden);

            ss.Name = Translate(ss.Name, langId, TranslationTextTypes.MasterData);
            var model = _mapper.Map<StateSecondaryOutputModel>(ss);
            return Ok(model);
        }

        [HttpGet]
        [Route("options")]
        public async Task<IList<ItemOverview>> Get([FromUri]int cid, [FromUri]int langId)
        {
            var items = await _stateSecondaryService.GetStateSecondariesAsync(cid).ConfigureAwait(false);
            return items
                .Select(d => new ItemOverview(Translate(d.Name, langId, TranslationTextTypes.MasterData), d.Id.ToString()))
                .ToList();
        }

        private string Translate(string translate, int languageId, int? tt = null)
        {
            return _translateCacheService.GetTextTranslation(translate, languageId, tt);
        }
    }
}
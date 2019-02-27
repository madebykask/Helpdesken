using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/templates")]
    public class CaseTemplatesController : BaseApiController
    {
        private readonly ITranslateCacheService _translateCacheService;
        private readonly IBaseCaseSolutionService _caseSolutionService;

        public CaseTemplatesController(IBaseCaseSolutionService caseSolutionService, ITranslateCacheService translateCacheService)
        {
            _caseSolutionService = caseSolutionService;
            _translateCacheService = translateCacheService;
        }

        [HttpGet]
        [Route("")]
        [SkipCustomerAuthorization()]
        public async Task<IList<CaseSolutionOverview>> Get([FromUri]int cid, [FromUri]int langId, [FromUri] bool mobileOnly = false)
        {
            var caseSolutions = await (mobileOnly
                ? _caseSolutionService.GetCustomerMobileCaseSolutionsAsync(cid)
                : _caseSolutionService.GetCustomerCaseSolutionsAsync(cid));

            var translatedItems = caseSolutions.Apply(item =>
            {
                item.Name = _translateCacheService.GetMasterDataTextTranslation(item.Name, langId);
                item.CategoryName = _translateCacheService.GetMasterDataTextTranslation(item.CategoryName, langId);
            });

            return translatedItems;
        }
    }
}
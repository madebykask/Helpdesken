using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/casetypes")]
    public class CaseTypesController : BaseApiController
    {
        private readonly ICaseTypeService _caseTypeService;
        private readonly ITranslateCacheService _translateCacheService;

        public CaseTypesController(ICaseTypeService caseTypeService, ITranslateCacheService translateCacheService)
        {
            _caseTypeService = caseTypeService;
            _translateCacheService = translateCacheService;
        }

        /// <summary>
        /// List of case types.
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="langId"></param>
        /// <returns></returns>
        [Route("options")]
        public async Task<IEnumerable<CaseTypeOverview>> Get(int cid, int langId)
        {
            const bool takeOnlyActive = true;//TODO: move to filter?
            var caseTypes = _caseTypeService.GetCaseTypesOverviewWithChildren(cid, takeOnlyActive);

            Translate(caseTypes.ToList(), langId, 0);

            return await Task.FromResult(caseTypes.OrderBy(p => p.Name));
        }

        private void Translate(List<CaseTypeOverview> caseTypes, int langId, int depth)
        {
            if (depth >= 20)
                throw new Exception("Iteration depth exceeded. Suspicion of infinte loop.");

            depth++;

            caseTypes.ForEach(p => 
            {
                p.Name = _translateCacheService.GetMasterDataTextTranslation(p.Name, langId);

                if (p.SubCaseTypes != null && p.SubCaseTypes.Any())
                {
                    Translate(p.SubCaseTypes, langId, depth);
                }
            });
        }
    }
}
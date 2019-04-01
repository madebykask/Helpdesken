using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CaseTypesController(ICaseTypeService caseTypeService, ITranslateCacheService translateCacheService, 
            IMapper mapper)
        {
            _caseTypeService = caseTypeService;
            _translateCacheService = translateCacheService;
            _mapper = mapper;
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

        /// <summary>
        /// Case type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cid"></param>
        /// <param name="langId"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        public async Task<CaseTypeOverview> GetCaseType(int id, int cid, int langId)
        {
            var caseType = _caseTypeService.GetCaseType(id);
            var caseTypeOverview = _mapper.Map<CaseTypeOverview>(caseType);
            Translate(new List<CaseTypeOverview> { caseTypeOverview }, langId, 0);

            return await Task.FromResult(caseTypeOverview);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Translate;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class CaseTypesController : BaseApiController
    {
        private readonly ICaseTypeService _caseTypeService;
        private readonly ITranslateCacheService _translateCacheService;

        public CaseTypesController(ICaseTypeService caseTypeService, ITranslateCacheService translateCacheService)
        {
            _caseTypeService = caseTypeService;
            _translateCacheService = translateCacheService;
        }

        // GET api/<controller>
        public async Task<IEnumerable<CaseTypeOverview>> Get(int cid, int langId)
        {
            const bool takeOnlyActive = true;//TODO: move to filter?
            var caseTypes = _caseTypeService.GetCaseTypesOverviewWithChildren(cid, takeOnlyActive);

            const int maxDepth = 20;
            var depth = 0;
            Translate(caseTypes.ToList());

            return await Task.FromResult(caseTypes.OrderBy(p => p.Name));

            void Translate(List<CaseTypeOverview> cts)
            {
                if (depth >= maxDepth)
                    throw new Exception("Iteration depth exceeded. Suspicion of infinte loop.");
                depth++;
                cts
                    .ForEach(p =>
                    {
                        p.Name = _translateCacheService.GetTextTranslation(p.Name, langId, 1);
                        if (p.SubCaseTypes != null && p.SubCaseTypes.Any())
                        {
                            Translate(p.SubCaseTypes);
                        }
                    });
            };
        }

        // GET api/<controller>/5
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
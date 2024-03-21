using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Enums;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/workflows")]
    public class CaseWorkflowsController : BaseApiController
    {
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly ICaseService _caseService;
        private readonly IUserService _userSerivice;
        private readonly ITranslateCacheService _translateCacheService;

        public CaseWorkflowsController(ICaseSolutionService caseSolutionService,
            ICaseService caseService, IUserService userSerivice, 
            ITranslateCacheService translateCacheService)
        {
            _caseSolutionService = caseSolutionService;
            _caseService = caseService;
            _userSerivice = userSerivice;
            _translateCacheService = translateCacheService;
        }

        [HttpGet]
        [Route("options")]
        public async Task<IEnumerable<ItemOverview>> Get([FromUri]int cid, [FromUri]int langId, [FromUri]int? caseId = null)
        {
            //Todo - soon
            var workflowCaseSolutionIds = _caseSolutionService.GetWorkflowCaseSolutionIds(cid, UserId);
            var isRelatedCase = caseId.HasValue && caseId.Value > 0 && _caseService.IsRelated(caseId.Value);
            var userSettings = await _userSerivice.GetUserOverviewAsync(UserId);
            var caseEntity =  caseId.HasValue ? _caseService.GetCaseById(caseId.Value) : (Case)null;
            var customerCaseSolutions = _caseSolutionService.GetCustomerCaseSolutionsOverview(cid);

            //New from BusinessRules 
            bool dontShowClosingWorksteps = false;
            List<string> disableCaseFields = new List<string>();
            (disableCaseFields, dontShowClosingWorksteps) = _caseService.ExecuteBusinessActionsDisable(caseEntity);

            if (dontShowClosingWorksteps)
            {
                workflowCaseSolutionIds = customerCaseSolutions
                     .Where(x => x.ConnectedButton == 0
                              && x.Status > 0
                              && x.HasFinishingCauseId == false)
                     .Select(x => x.CaseSolutionId)
                     .ToList();
            }
            var workflowSteps =
                _caseSolutionService.GetWorkflowSteps(cid, 
                    caseEntity,
                    workflowCaseSolutionIds,
                    isRelatedCase,
                    userSettings,
                    ApplicationType.Helpdesk,
                    null,
                    langId);

            return workflowSteps.Select(w => new ItemOverview(Translate(w.Name, langId, TranslationTextTypes.MasterData), w.CaseTemplateId.ToString()))
                .OrderBy(w => w.Name);
        }

        private string Translate(string translate, int languageId, int? tt = null)
        {
            return _translateCacheService.GetTextTranslation(translate, languageId, tt);
        }
    }
}

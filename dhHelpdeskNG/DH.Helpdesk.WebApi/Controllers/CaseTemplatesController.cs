using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Models.Case;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/templates")]
    public class CaseTemplatesController : BaseApiController
    {
        private readonly ITranslateCacheService _translateCacheService;
        private readonly IBaseCaseSolutionService _caseSolutionService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly IProductAreaService _productAreaService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        
        public CaseTemplatesController(IBaseCaseSolutionService caseSolutionService, ITranslateCacheService translateCacheService,
            IFinishingCauseService finishingCauseService,
            IProductAreaService productAreaService,
            ICaseTypeService caseTypeService,
            IUserService userService,
            IMapper mapper)
        {
            _caseSolutionService = caseSolutionService;
            _translateCacheService = translateCacheService;
            _finishingCauseService = finishingCauseService;
            _productAreaService = productAreaService;
            _caseTypeService = caseTypeService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
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

        [HttpGet]
        [Route("{templateId:int}")]
        public async Task<IHttpActionResult> Get([FromUri]int templateId, [FromUri] int cid, [FromUri] int langId,
            [FromUri] bool mobileOnly = false)
        {
            var caseSolution = await _caseSolutionService.GetCaseSolutionAsync(templateId);

            if (caseSolution == null)
                return NotFound();

            // This strange logic I took from Edit() action
            caseSolution.NoMailToNotifier = caseSolution.NoMailToNotifier == 0 ? 1 : 0;

            // Check CaseType is Active
            if (caseSolution.CaseType_Id.HasValue)
            {
                var caseType = _caseTypeService.GetCaseType(caseSolution.CaseType_Id.Value);
                if (!(caseType != null && caseType.IsActive != 0))
                    caseSolution.CaseType_Id = null;
            }

            // Check ProductArea is Active
            if (caseSolution.ProductArea_Id.HasValue)
            {
                var productArea = _productAreaService.GetProductArea(caseSolution.ProductArea_Id.Value);
                if (!(productArea != null && productArea.IsActive != 0))
                    caseSolution.ProductArea_Id = null;
            }

            // Check Finishing Cause is Active
            if (caseSolution.FinishingCause_Id.HasValue)
            {
                var finishingCause = _finishingCauseService.GetFinishingCause(caseSolution.FinishingCause_Id.Value);
                if (!(finishingCause != null && finishingCause.IsActive != 0))
                    caseSolution.FinishingCause_Id = null;
            }

            if (caseSolution.SetCurrentUserAsPerformer == 1)
                caseSolution.PerformerUser_Id = UserId;

            if (caseSolution.SetCurrentUsersWorkingGroup == 1)
            {
                var userDefaultWgId = _userService.GetUserDefaultWorkingGroupId(UserId, cid);
                caseSolution.CaseWorkingGroup_Id = userDefaultWgId;
            }

            if (caseSolution.CaseType_Id.HasValue)
                caseSolution.WorkingGroup_Id = null;

            return Ok(_mapper.Map<CaseSolutionModel>(caseSolution));

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
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
        private readonly ICustomerUserService _customerUserService;
        private readonly IMapper _mapper;

        public CaseTemplatesController(IBaseCaseSolutionService caseSolutionService, ITranslateCacheService translateCacheService,
            IFinishingCauseService finishingCauseService,
            IProductAreaService productAreaService,
            ICaseTypeService caseTypeService,
            IUserService userService,
            IMapper mapper,
            ICustomerUserService customerUserService)
        {
            _caseSolutionService = caseSolutionService;
            _translateCacheService = translateCacheService;
            _finishingCauseService = finishingCauseService;
            _productAreaService = productAreaService;
            _caseTypeService = caseTypeService;
            _userService = userService;
            _mapper = mapper;
            _customerUserService = customerUserService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IList<CustomerCaseSolution>> Get([FromUri]int langId, [FromUri] bool mobileOnly = false)
        {

            var model = new List<CustomerCaseSolution>();
            var customers = _customerUserService.GetCustomerUsersForHomeIndexPage(UserId);
            if (!customers.Any())
                return model;

            var caseSolutions = await _caseSolutionService.GetCustomersMobileCaseSolutionsAsync(customers.Select(c => c.Customer.Customer_Id).ToList());

            //var translatedCaseSolutions = caseSolutions.Apply(item =>
            //{
            //    item.Name = _translateCacheService.GetMasterDataTextTranslation(item.Name, langId);
            //    item.CategoryName = _translateCacheService.GetMasterDataTextTranslation(item.CategoryName, langId);
            //    //item.Name = "Kattas trans";
            //    ////item.CategoryName = "Kattas Kategori";
            //});

            foreach (var caseSol in caseSolutions)
            {
                var solName = _caseSolutionService.GetCaseSolutionTranslation(caseSol.CaseSolutionId, langId);
                if (solName != null)
                {
                    caseSol.Name = solName.CaseSolutionName;
                }
                if (caseSol.CategoryId != null && caseSol.CategoryId.HasValue)
                {
                    var catName = _caseSolutionService.GetCaseSolutionCategoryTranslation(caseSol.CategoryId.Value, langId);
                    if (catName != null)
                    {
                        caseSol.CategoryName = catName.CaseSolutionCategoryName;
                    }
                }
            }


            model = customers.Where(c => caseSolutions.Any(cs => cs.CustomerId == c.Customer.Customer_Id))
                .Select(c => new CustomerCaseSolution()
            {
                CustomerId = c.Customer.Customer_Id,
                CustomerName = c.Customer.Customer.Name,
                Items = new List<CustomerCaseSolutionOverviewItem>()
            }).ToList();

            model.ForEach(m => m.Items = caseSolutions
                    .Where(cs => cs.CustomerId == m.CustomerId)
                    .OrderBy(c => c.Name)
                .Select(cs => new CustomerCaseSolutionOverviewItem()
                {
                    Id = cs.CaseSolutionId,
                    Name = cs.Name,
                    CategoryId = cs.CategoryId,
                    CategoryName = cs.CategoryName
                })
                .ToList());

            return model.OrderBy(c => c.CustomerName).ToList();
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
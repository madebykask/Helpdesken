using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Models.Case.Field;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Logic.Case;

namespace DH.Helpdesk.WebApi.Controllers
{
    //[Route( "api/v{version:apiVersion}" )]
    [RoutePrefix("api/Case")]
    public class CaseController : BaseApiController
    {
        private readonly ICaseService _caseService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IBaseCaseSolutionService _caseSolutionService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ISettingService _customerSettingsService;
        private readonly IMapper _mapper;
        private readonly ICaseEditModeCalcStrategy _caseEditModeCalcStrategy;
        private readonly ICaseSolutionSettingService _caseSolutionSettingService;
        private readonly IUserService _userService;
        private readonly ICaseFieldsCreator _caseFieldsCreator;
        private readonly ICustomerService _customerService;

        public CaseController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService,
            IBaseCaseSolutionService caseSolutionService, ICustomerUserService customerUserService,
            ISettingService customerSettingsService, IMapper mapper, ICaseEditModeCalcStrategy caseEditModeCalcStrategy,
            ICaseSolutionSettingService caseSolutionSettingService, IUserService userService, ICaseFieldsCreator caseFieldsCreator,
            ICustomerService customerService)
        {
            _caseService = caseService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseSolutionService = caseSolutionService;
            _customerUserService = customerUserService;
            _customerSettingsService = customerSettingsService;
            _mapper = mapper;
            _caseEditModeCalcStrategy = caseEditModeCalcStrategy;
            _caseSolutionSettingService = caseSolutionSettingService;
            _userService = userService;
            _caseFieldsCreator = caseFieldsCreator;
            _customerService = customerService;
        }

        /// <summary>
        /// Get case data.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="langId"></param>
        /// <param name="caseId"></param>
        /// <param name="cid"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("{caseId:int}")]
        public async Task<CaseEditOutputModel> Get([FromUri]int langId, [FromUri]int caseId, [FromUri]int cid, [FromUri]string sessionId = null)
        {
            var model = new CaseEditOutputModel();

            var userId = UserId;
            var languageId = langId;
            var currentCase = await _caseService.GetDetachedCaseByIdAsync(caseId);
            var currentCid = cid;

            if (currentCase.Customer_Id != currentCid)
                throw new Exception($"Case customer({currentCase.Customer_Id}) and current customer({currentCid}) are different"); //TODO: how to react?

            var customerUserSetting = await _customerUserService.GetCustomerUserSettingsAsync(currentCid, userId);
            if (customerUserSetting == null)
                throw new Exception($"No customer settings for this customer '{currentCid}' and user '{userId}'");

            var customerSettings = _customerSettingsService.GetCustomerSettings(currentCid);
            var userOverview = await _userService.GetUserOverviewAsync(UserId);// TODO: use cached version!
            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(currentCid);
            var caseFieldTranslations = await _caseFieldSettingService.GetCustomerCaseTranslationsAsync(currentCid);

            model.Fields = new List<IBaseCaseField>();

            //TODO: Move to mapper
            model.Id = currentCase.Id;
            model.CaseNumber = currentCase.CaseNumber;
            model.CaseGuid = currentCase.CaseGUID;

            //case solution
            if (currentCase.CurrentCaseSolution_Id.HasValue)
            {
                var caseSolutionId = currentCase.CurrentCaseSolution_Id.Value;
                var caseSolution = _caseSolutionService.GetCaseSolution(caseSolutionId);
                //_caseSolutionSettingService.GetCaseSolutionSettingOverviews(caseSolutionId);//TODO: Case solution settings participate in visibility check
                if (caseSolution != null)
                    model.CaseSolution = _mapper.Map<CaseSolutionInfo>(caseSolution);
            }

            //if (!string.IsNullOrWhiteSpace(currentCase.ReportedBy))//TODO:
            //{
            //    var reportedByUser = _computerService.GetComputerUserByUserID(currentCase.ReportedBy);
            //    if (reportedByUser?.ComputerUsersCategoryID != null)
            //    {
            //        var initiatorComputerUserCategory = _computerService.GetComputerUserCategoryByID(reportedByUser.ComputerUsersCategoryID.Value);
            //        if (initiatorComputerUserCategory != null)
            //        {
            //            model.InitiatorReadOnly = initiatorComputerUserCategory.IsReadOnly;
            //        }
            //    }
            //}

            _caseFieldsCreator.CreateInitiatorSection(cid, customerUserSetting, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model);
            _caseFieldsCreator.CreateRegardingSection(cid, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model);
            _caseFieldsCreator.CreateComputerInfoSection(cid, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model);
            _caseFieldsCreator.CreateCaseInfoSection(cid, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model, customerUserSetting);
            _caseFieldsCreator.CreateCaseManagementSection(cid, userOverview, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model, customerUserSetting, customerSettings);
            _caseFieldsCreator.CreateCommunicationSection(cid, userOverview, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model, customerSettings);

            //calc case edit mode
            model.EditMode = _caseEditModeCalcStrategy.CalcEditMode(cid, UserId, currentCase); // remember to apply isCaseLocked check on client

            _caseService.MarkAsRead(caseId);

            return model;
        }

        [HttpGet]
        [Route("new/{templateId:int}")]
        [CheckUserPermissions(UserPermission.CreateCasePermission)]
        public async Task<IHttpActionResult> New([FromUri]int langId, [FromUri]int cid, [FromUri]int? templateId)
        { 
            var customerSettings = _customerSettingsService.GetCustomerSettings(cid);
            if (!templateId.HasValue && customerSettings.DefaultCaseTemplateId != 0)
                templateId = customerSettings.DefaultCaseTemplateId;

            if(!templateId.HasValue)
                throw new Exception("No template id found. Template id should be in parameter or set as default for customer.");

            var caseTemplate = _caseSolutionService.GetCaseSolution(templateId.Value);
            if (caseTemplate == null)
                throw new Exception($"Template '{templateId.Value}' can't be found.");

            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);
            var caseTemplateSettings = _caseSolutionSettingService.GetCaseSolutionSettingOverviews(templateId.Value);
            var caseFieldTranslations = await _caseFieldSettingService.GetCustomerCaseTranslationsAsync(cid);
            var customerUserSetting = await _customerUserService.GetCustomerUserSettingsAsync(cid, UserId);
            var customerDefaults = _customerService.GetCustomerDefaults(cid);
            var userOverview = await _userService.GetUserOverviewAsync(UserId);// TODO: use cached version!

            var model = new CaseEditOutputModel()
            {
                CaseGuid = Guid.NewGuid(),
                Fields = new List<IBaseCaseField>(),
                CaseSolution = _mapper.Map<CaseSolutionInfo>(caseTemplate)
            };

            _caseFieldsCreator.CreateInitiatorSection(cid, customerUserSetting, caseFieldSettings, caseTemplateSettings, null, caseTemplate, langId,
                caseFieldTranslations, model, customerDefaults);
            _caseFieldsCreator.CreateRegardingSection(cid, caseFieldSettings, caseTemplateSettings, null, caseTemplate, langId, caseFieldTranslations, model);
            _caseFieldsCreator.CreateComputerInfoSection(cid, caseFieldSettings, caseTemplateSettings, null, caseTemplate, langId, caseFieldTranslations, model);
            _caseFieldsCreator.CreateCaseInfoSection(cid, caseFieldSettings, caseTemplateSettings, null, caseTemplate, langId, caseFieldTranslations,
                model, customerUserSetting, customerDefaults);
            _caseFieldsCreator.CreateCaseManagementSection(cid, userOverview, caseFieldSettings, caseTemplateSettings, null, caseTemplate, langId,
                caseFieldTranslations, model, customerUserSetting, customerSettings, customerDefaults);
            _caseFieldsCreator.CreateCommunicationSection(cid, userOverview, caseFieldSettings, caseTemplateSettings, null, caseTemplate, langId,
                caseFieldTranslations, model, customerSettings);

            model.EditMode = Web.Common.Enums.Case.AccessMode.FullAccess;
            return Ok(model);
        }
    }
}
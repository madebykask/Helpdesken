using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Models.Case.Field;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using DH.Helpdesk.WebApi.Infrastructure.Filters;
using DH.Helpdesk.WebApi.Logic.Case;
using DH.Helpdesk.WebApi.Models.Case;

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
        private readonly ITranslateCacheService _translateCacheService;

        public CaseController(ICaseService caseService, ICaseFieldSettingService caseFieldSettingService,
            IBaseCaseSolutionService caseSolutionService, ICustomerUserService customerUserService,
            ISettingService customerSettingsService, IMapper mapper, ICaseEditModeCalcStrategy caseEditModeCalcStrategy,
            ICaseSolutionSettingService caseSolutionSettingService, IUserService userService, ICaseFieldsCreator caseFieldsCreator,
            ICustomerService customerService,
            ITranslateCacheService translateCacheService)
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
            _translateCacheService = translateCacheService;
        }

        /// <summary>
        /// Get case data.
        /// </summary>
        /// <param name="langId"></param>
        /// <param name="caseId"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet]
        [SkipCustomerAuthorization]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("{caseId:int}")]
        public async Task<CaseEditOutputModel> Get([FromUri]int langId, [FromUri]int caseId, [FromUri]string sessionId = null)
        {
            var model = new CaseEditOutputModel();

            var userId = UserId;
            var languageId = langId;
            var currentCase = await _caseService.GetDetachedCaseByIdAsync(caseId);
            var currentCid = currentCase.Customer_Id;
            if (!_userService.UserHasCustomerId(currentCid))
                Forbidden("User not authorized.");

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
            model.CustomerId = currentCase.Customer_Id;
            model.CaseNumber = currentCase.CaseNumber;
            model.CaseGuid = currentCase.CaseGUID;

            //case solution
            if (currentCase.CaseSolution_Id.HasValue)
            {
                var caseSolutionId = currentCase.CaseSolution_Id.Value;
                var caseSolution = _caseSolutionService.GetCaseSolution(caseSolutionId);
                //_caseSolutionSettingService.GetCaseSolutionSettingOverviews(caseSolutionId);//TODO: Case solution settings participate in visibility check
                if (caseSolution != null)
                {
                    if(_caseSolutionService.GetCaseSolutionTranslation(caseSolutionId, langId) != null)
                    {
                        caseSolution.Name = _caseSolutionService.GetCaseSolutionTranslation(caseSolutionId, langId).CaseSolutionName;
                    }
                    model.CaseSolution = _mapper.Map<CaseSolutionInfo>(caseSolution);
                }
                
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

            _caseFieldsCreator.CreateInitiatorSection(currentCid, customerUserSetting, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model);
            _caseFieldsCreator.CreateRegardingSection(currentCid, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model);
            _caseFieldsCreator.CreateComputerInfoSection(currentCid, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model);
            _caseFieldsCreator.CreateCaseInfoSection(currentCid, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model, customerUserSetting);
            _caseFieldsCreator.CreateCaseManagementSection(currentCid, userOverview, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model, customerUserSetting, customerSettings);
            _caseFieldsCreator.CreateCommunicationSection(currentCid, userOverview, caseFieldSettings, null, currentCase, null, languageId, caseFieldTranslations, model, customerSettings);

            //calc case edit mode
            model.EditMode = _caseEditModeCalcStrategy.CalcEditMode(currentCid, UserId, currentCase); // remember to apply isCaseLocked check on client

            model.ChildCasesIds = _caseService.GetChildCasesFor(caseId).Select(c => c.Id).ToList();
            model.ParentCaseId = _caseService.GetParentInfo(caseId)?.ParentId;

            //Todo - Check for merged cases

            _caseService.MarkAsRead(caseId);

            var stateSecondaryId = currentCase?.StateSecondary_Id ?? 0;

            model.ExtendedCaseData =
                GetExtendedCaseModel(currentCid, currentCase.Id, currentCase.CaseSolution_Id ?? 0, stateSecondaryId, userOverview.UserGUID.ToString(), langId);
            
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
            if(_caseSolutionService.GetCaseSolutionTranslation(templateId.Value, langId)!= null)
            {
                caseTemplate.Name = _caseSolutionService.GetCaseSolutionTranslation(templateId.Value, langId).CaseSolutionName;
            }
            if(!String.IsNullOrEmpty(caseTemplate.Description))
            {
                caseTemplate.Description = caseTemplate.Description.Replace("\r\n", "<br />");
            }
            if (!String.IsNullOrEmpty(caseTemplate.Text_External))
            {
                caseTemplate.Text_External = caseTemplate.Text_External.Replace("\r\n", "<br />");
            }
            if (!String.IsNullOrEmpty(caseTemplate.Text_Internal))
            {
                caseTemplate.Text_Internal = caseTemplate.Text_Internal.Replace("\r\n", "<br />");
            }

            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);
            var caseTemplateSettings = _caseSolutionSettingService.GetCaseSolutionSettingOverviews(templateId.Value);
            var caseFieldTranslations = await _caseFieldSettingService.GetCustomerCaseTranslationsAsync(cid);
            var customerUserSetting = await _customerUserService.GetCustomerUserSettingsAsync(cid, UserId);
            var customerDefaults = _customerService.GetCustomerDefaults(cid);
            var userOverview = await _userService.GetUserOverviewAsync(UserId);// TODO: use cached version!

            var model = new CaseEditOutputModel()
            {
                CaseGuid = Guid.NewGuid(),
                CustomerId = caseTemplate.Customer_Id,
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

            model.EditMode = Web.Common.Enums.Case.AccessMode.FullAccess; //todo: check mode calculation

            var stateSecondaryId = caseTemplate?.StateSecondary_Id ?? 0;

            model.ExtendedCaseData =
                GetExtendedCaseModel(cid, 0, caseTemplate?.Id ?? 0, stateSecondaryId, userOverview.UserGUID.ToString(), langId);

            return Ok(model);
        }

        private ExtendedCaseModel GetExtendedCaseModel(int customerId, int caseId, int caseSolutionId, int stateSecondaryId, string userGuid, int langId)
        {
            ExtendedCaseModel model = null;

            var exCaseData = 
                _caseService.GetCaseExtendedCaseForm(caseSolutionId, customerId, caseId, userGuid, stateSecondaryId);

            if (exCaseData != null)
            {
                model = new ExtendedCaseModel
                {
                    ExtendedCaseFormId = exCaseData.ExtendedCaseFormId,
                    ExtendedCaseGuid = exCaseData.ExtendedCaseGuid,
                    ExtendedCaseName = _translateCacheService.GetMasterDataTextTranslation(exCaseData.ExtendedCaseFormName, langId)
                };
            }
            return model;
        }
    }
}
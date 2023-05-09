using DH.Helpdesk.BusinessData.Models.Case.CaseLogs;
using DH.Helpdesk.BusinessData.Models.Email;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums.FileViewLog;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.SelfService.Controllers.Behaviors;
using DH.Helpdesk.SelfService.Entites;
using DH.Helpdesk.SelfService.Infrastructure.Attributes;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Services.Utils;
using log4net;
using System.Threading;
using WebGrease.Css.Extensions;

namespace DH.Helpdesk.SelfService.Controllers
{
    using BusinessData.Models.ExtendedCase;
    using Common.Extensions.Integer;
    using DH.Helpdesk.BusinessData.Enums.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.SelfService.Infrastructure;
    using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.SelfService.Infrastructure.Extensions;
    using DH.Helpdesk.SelfService.Infrastructure.Tools;
    using DH.Helpdesk.SelfService.Models.Case;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using Models.Message;
    using Models.Shared;
    using Newtonsoft.Json.Linq;
    using Services.Infrastructure;
    using Services.Services.UniversalCase;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.WebPages;
    using static DH.Helpdesk.BusinessData.OldComponents.GlobalEnums;

    public class CaseController : BaseController
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CaseController));

        private readonly ICustomerService _customerService;
        private readonly IInfoService _infoService;
        private readonly ICaseService _caseService;
        private readonly ILogService _logService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IUserTemporaryFilesStorage _userTemporaryFilesStorage;
        private readonly ICaseFileService _caseFileService;
        private readonly ILogFileService _logFileService;
        private readonly IRegionService _regionService;
        private readonly IDepartmentService _departmentService;
        private readonly IOUService _ouService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IProductAreaService _productAreaService;
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IImpactService _impactService;
        private readonly ICategoryService _categoryService;
        private readonly ICurrencyService _currencyService;
        private readonly ISupplierService _supplierService;
        private readonly ISettingService _settingService;
        private readonly IComputerService _computerService;
        private readonly IContractCategoryService _contractCategoryService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly ICaseSolutionSettingService _caseSolutionSettingService;
        private readonly IEmailService _emailService;
        private readonly IMasterDataService _masterDataService;
        private readonly ICaseExtraFollowersService _caseExtraFollowersService;
        private readonly IUserEmailsSearchService _userEmailsSearchService;
        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;
        private readonly IPriorityService _priorityService;
        private readonly IExtendedCaseService _extendedCaseService;
        private readonly IUniversalCaseService _universalCaseService;
        private readonly IWatchDateCalendarService _watchDateCalendarService;
        private readonly IInventoryService _inventoryService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IStatusService _statusService;
        private readonly ICaseSectionService _caseSectionService;


        private const string ParentPathDefaultValue = "--";
        private const string EnterMarkup = "<br />";
        private readonly IOrganizationService _orgService;
        private readonly OrganizationJsonService _orgJsonService;
        private readonly char[] EMAIL_SEPARATOR = new char[] { ';' };
        private readonly CaseControllerBehavior _caseControllerBehavior;
        private const string ShowRegistrationMessageKey = "showRegistrationMessage";
        private readonly IFileViewLogService _fileViewLogService;
        private readonly IFeatureToggleService _featureToggleService;
        private readonly IFinishingCauseService _finishingCauseService;

        public CaseController(
            ICaseService caseService,
            ICaseFieldSettingService caseFieldSettingService,
            IMasterDataService masterDataService,
            ILogService logService,
            IInfoService infoService,
            IUserTemporaryFilesStorageFactory userTemporaryFilesStorageFactory,
            ICaseFileService caseFileService,
            IRegionService regionService,
            IDepartmentService departmentService,
            IOUService ouService,
            ICaseTypeService caseTypeService,
            IProductAreaService productAreaService,
            ISystemService systemService,
            IUrgencyService urgencyService,
            IImpactService impactService,
            ICategoryService categoryService,
            ICurrencyService currencyService,
            ISupplierService supplierService,
            ICustomerService customerService,
            ISettingService settingService,
            IComputerService computerService,
            ICaseSettingsService caseSettingService,
            ICaseSearchService caseSearchService,
            IContractCategoryService contractCategoryService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IStateSecondaryService stateSecondaryService,
            ILogFileService logFileService,
            ICaseSolutionService caseSolutionService,
            IOrganizationService orgService,
            OrganizationJsonService orgJsonService,
            ICaseSolutionSettingService caseSolutionSettingService,
            IEmailService emailService,
            ICaseExtraFollowersService caseExtraFollowersService,
            IUserEmailsSearchService userEmailsSearchService,
            IRegistrationSourceCustomerService registrationSourceCustomerService,
            IPriorityService priorityService,
            IExtendedCaseService extendedCaseService,
            IUniversalCaseService universalCaseService,
            IInventoryService inventoryService,
            ICustomerUserService customerUserService,
            IGlobalSettingService globalSettingService,
            IWatchDateCalendarService watchDateCalendarService,
            ISelfServiceConfigurationService configurationService,
            IStatusService statusService,
            ICaseSectionService caseSectionService,
            IFileViewLogService fileViewLogService,
            IFeatureToggleService featureToggleService,
            IFinishingCauseService finishingCauseService)
            : base(configurationService, masterDataService, caseSolutionService)
        {
            _caseControllerBehavior = new CaseControllerBehavior(masterDataService, caseService, caseSearchService,
                caseSettingService, caseFieldSettingService,
                productAreaService, configurationService,
                computerService, featureToggleService);

            _masterDataService = masterDataService;
            _caseService = caseService;
            _logService = logService;
            _caseFieldSettingService = caseFieldSettingService;
            _infoService = infoService;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.Create(ModuleName.Cases);
            _caseFileService = caseFileService;
            _regionService = regionService;
            _departmentService = departmentService;
            _ouService = ouService;
            _caseTypeService = caseTypeService;
            _productAreaService = productAreaService;
            _currencyService = currencyService;
            _logFileService = logFileService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _systemService = systemService;
            _settingService = settingService;
            _computerService = computerService;
            _customerService = customerService;
            _workingGroupService = workingGroupService;
            _contractCategoryService = contractCategoryService;
            _userService = userService;
            _stateSecondaryService = stateSecondaryService;
            _caseSolutionService = caseSolutionService;
            _orgService = orgService;
            _orgJsonService = orgJsonService;
            _emailService = emailService;
            _urgencyService = urgencyService;
            _impactService = impactService;
            _caseSolutionSettingService = caseSolutionSettingService;
            _caseExtraFollowersService = caseExtraFollowersService;
            _userEmailsSearchService = userEmailsSearchService;
            _registrationSourceCustomerService = registrationSourceCustomerService;
            _priorityService = priorityService;
            _extendedCaseService = extendedCaseService;
            _universalCaseService = universalCaseService;
            _watchDateCalendarService = watchDateCalendarService;
            _inventoryService = inventoryService;
            _customerUserService = customerUserService;
            _globalSettingService = globalSettingService;
            _statusService = statusService;
            _caseSectionService = caseSectionService;
            _fileViewLogService = fileViewLogService;
            _featureToggleService = featureToggleService;
            _finishingCauseService = finishingCauseService; 
    }

        [HttpGet]
        public ActionResult Index(string id, bool showRegistrationMessage = false)
        {
            if (string.IsNullOrEmpty(id))
            {
                ErrorGenerator.MakeError("Id was not specified!", 210);
                return RedirectToAction("Index", "Error");
            }

            var caseId = 0;
            var allowAnonymousAccess = false;

            if (id.Is<Guid>())
            {
                allowAnonymousAccess = true;
                var guid = new Guid(id);
                caseId = _caseService.GetCaseIdByEmailGUID(guid);
                if (caseId == 0)
                {
                    TempData["NotFound"] = "Link is not valid!";
                    return View(new CaseOverviewModel());
                }

                // open extended case by guid
                var data = _extendedCaseService.GetExtendedCaseFromCase(caseId);
                if (data != null)
                {
                    var curCase = _caseService.GetCaseById(caseId);
                    if (curCase.CaseSolution.AvailableTabsSelfsevice != "case-tab")
                    {
                        return RedirectToAction("ExtendedCasePublic", new { id = data.ExtendedCaseGuid });
                    }

                }

                //check dynamic case
                var dynamicCase = _caseService.GetDynamicCase(caseId);
                if (dynamicCase != null)
                {
                    //todo: refactor to use common class
                    var dynamicUrl = $"/{dynamicCase.FormPath}";
                    if (!string.IsNullOrEmpty(dynamicUrl)) // Show Case in eform
                    {
                        var urlStr = dynamicUrl.SetUrlParameters(caseId);
                        var url = $"{Request.Url.Scheme}://{Request.Url.Authority}{urlStr}";
                        return Redirect(url);
                    }
                }
            }
            else if (!int.TryParse(id, out caseId))
            {
                ErrorGenerator.MakeError("Case Id is not valid!");
                return RedirectToAction("Index", "Error");
            }

            var currentCase = _caseService.GetCaseById(caseId);
            if (currentCase == null)
            {
                TempData["NotFound"] = "Case not found!";
                return View(new CaseOverviewModel());
            }

            //check if case is among customer cases only if its not anonymous access and not opened via link in email
            var isAnonymousMode = ConfigurationService.AppSettings.LoginMode == LoginMode.Anonymous;
            if (!isAnonymousMode && !allowAnonymousAccess)
            {
                var hasAccess = UserHasAccessToCase(currentCase);
                if (!hasAccess)
                {
                    ErrorGenerator.MakeError("Case not found among your cases!");
                    return RedirectToAction("Index", "Error");
                }
            }

            if (currentCase.CaseExtendedCaseDatas.Any())
            {
                //New check here if Only Extended case should see Case-tab
                if (currentCase.CaseSolution.AvailableTabsSelfsevice != "case-tab")
                {
                    var caseTemplateId = currentCase.CaseSolution.Id;
                    return RedirectToAction("ExtendedCase", new { caseTemplateId, caseId = currentCase.Id });
                }
            }

            var globalSettings = _globalSettingService.GetGlobalSettings().FirstOrDefault();
            var isMultiCustomerMode = globalSettings.MultiCustomersSearch.ToBool();
            // check only if multi customer is not enabled. Allow user to see own cases for different customers.
            if (!isMultiCustomerMode && currentCase.Customer_Id != SessionFacade.CurrentCustomerID)
            {
                ErrorGenerator.MakeError("Selected Case doesn't belong to current customer!");
                return null;
            }

            var currentCustomer = SessionFacade.CurrentCustomer;
            if (isMultiCustomerMode)
            {
                //override current customer when openning case from another customer in multicustomer search mode
                currentCustomer = currentCase.Customer;
                //SessionFacade.CurrentCustomer = currentCustomer;
            }

            var languageId = SessionFacade.CurrentLanguageId;
            var caseReceipt = GetCaseReceiptModel(currentCase, languageId);

            caseReceipt.ShowRegistringMessage = showRegistrationMessage;
            caseReceipt.ExLogFileGuid = currentCase.CaseGUID.ToString();
            caseReceipt.MailGuid = id;

            _userTemporaryFilesStorage.DeleteFiles(caseReceipt.ExLogFileGuid);
            _userTemporaryFilesStorage.DeleteFiles(id);

            if (id.Is<Guid>())
            {
                if (currentCase.StateSecondary_Id.HasValue && caseReceipt.CasePreview.FinishingDate == null)
                {
                    var stateSecondary = _stateSecondaryService.GetStateSecondary(currentCase.StateSecondary_Id.Value);
                    if (stateSecondary.NoMailToNotifier == 1)
                        caseReceipt.CasePreview.FinishingDate = DateTime.UtcNow;
                }
                caseReceipt.CanAddExternalNote = true;
            }
            else
            {
                if (showRegistrationMessage)
                    caseReceipt.CaseRegistrationMessage = GetCaseRegistrationMessage(languageId);

                caseReceipt.CanAddExternalNote = false;
            }

            var cs = _settingService.GetCustomerSetting(currentCustomer.Id);
            ViewBag.AttachmentPlacement = cs.AttachmentPlacement;

            var appSettings = ConfigurationService.AppSettings;
            ViewBag.ShowCommunicationForSelfService = appSettings.ShowCommunicationForSelfService;
            ViewBag.caseEmailGuid = id.Is<Guid>() ? Guid.Parse(id).ToString() : "";

            return View(caseReceipt);
        }

        [HttpGet]
        public ActionResult NewCase(int customerId, int? caseTemplateId)
        {
            // *** New Case ***
            var currentCustomer = SessionFacade.CurrentCustomer;

            //todo: move to attribute
            if (currentCustomer == null)
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return RedirectToAction("Index", "Error");
            }

            var languageId = SessionFacade.CurrentLanguageId;
            var appSettings = ConfigurationService.AppSettings;
            var caseFieldSetting =
                _caseFieldSettingService.ListToShowOnCasePage(customerId, languageId)
                    .Where(c => c.ShowExternal == 1)
                    .ToList();

            var cs = _settingService.GetCustomerSetting(currentCustomer.Id);

            var model = GetNewCaseModel(currentCustomer, SessionFacade.CurrentUserIdentity, caseFieldSetting, cs, appSettings);

            #region Load template info

            if (caseTemplateId != null && caseTemplateId.Value > 0)
            {
                var caseTemplate = _caseSolutionService.GetCaseSolution(caseTemplateId.Value);

                if (caseTemplate.Status == 0 || !caseTemplate.ShowInSelfService)
                {
                    ErrorGenerator.MakeError("Selected template is not available anymore!");
                    return RedirectToAction("Index", "Error");
                }

                var caseTemplateSettings = _caseSolutionSettingService.GetCaseSolutionSettingOverviews(caseTemplateId.Value).ToList();
                var jsFieldSettings = GetFieldSettingsModel(caseFieldSetting, caseTemplateSettings);
                model.JsFieldSettings = jsFieldSettings;

                model.CaseTemplateId = caseTemplateId.Value;

                // Apply template values to case:
                ApplyTemplate(caseTemplate, model.NewCase);

                model.NewCase.CaseSolution_Id = caseTemplateId;
                model.NewCase.CurrentCaseSolution_Id = caseTemplateId;

                model.Information = caseTemplate.Information;
                var caseSolutionTranslation = _caseSolutionService.GetCaseSolutionTranslation(caseTemplateId.Value, languageId);
                if (caseSolutionTranslation != null)
                {
                    model.Information = caseSolutionTranslation.Information;
                }

                if (!string.IsNullOrEmpty(caseTemplate.Text_External) ||
                    !string.IsNullOrEmpty(caseTemplate.Text_Internal) || caseTemplate.FinishingCause_Id.HasValue)
                {
                    model.CaseLog = new CaseLog
                    {
                        LogType = 0,
                        LogGuid = Guid.NewGuid(),
                        TextExternal = caseTemplate.Text_External.Replace("\r\n", "<br />"),
                        TextInternal = caseTemplate.Text_Internal.Replace("\r\n", "<br />"),
                        FinishingType = caseTemplate.FinishingCause_Id
                    };
                }
                model.NewCase.Description = caseTemplate.Description.Replace("\r\n", "<br />");
            }

            #endregion // Load Case Template

            //Translate product area path
            if (model.NewCase.ProductArea_Id.HasValue)
            {
                var p = _productAreaService.GetProductArea(model.NewCase.ProductArea_Id.GetValueOrDefault());
                if (p != null)
                {
                    var pathTexts = _productAreaService.GetParentPath(p.Id, currentCustomer.Id).ToList();
                    var translatedText = pathTexts;
                    if (pathTexts.Any())
                    {
                        translatedText = new List<string>();
                        foreach (var pathText in pathTexts.ToList())
                        {
                            translatedText.Add(Translation.Get(pathText));
                        }
                    }
                    model.ProductAreaParantPath = string.Join(" - ", translatedText);
                }
            }

            //Translate Category Path
            if (model.NewCase.Category_Id.HasValue)
            {
                var c = _categoryService.GetCategory(model.NewCase.Category_Id.GetValueOrDefault(), currentCustomer.Id);
                if (c != null)
                {
                    var pathTexts = _categoryService.GetParentPath(c.Id, currentCustomer.Id).ToList();
                    var translatedText = pathTexts;
                    if (pathTexts.Any())
                    {
                        translatedText = new List<string>();
                        foreach (var pathText in pathTexts.ToList())
                        {
                            translatedText.Add(Translation.Get(pathText));
                        }
                    }
                    model.CategoryParentPath = string.Join(" - ", translatedText);
                }
            }

            if (model.NewCase.CaseType_Id > 0)
            {
                var ct = _caseTypeService.GetCaseType(model.NewCase.CaseType_Id);
                var tempCTs = new List<CaseType> { ct };

                tempCTs = CaseTypeTreeTranslation(tempCTs).ToList();
                model.CaseTypeParantPath = tempCTs[0].getCaseTypeParentPath();
            }

            if (model.NewCase.Department_Id.HasValue)
            {
                if (!model.NewCase.Region_Id.HasValue)
                {
                    var reg = _departmentService.GetDepartment(model.NewCase.Department_Id.Value);
                    if (reg != null)
                    {
                        model.NewCase.Region_Id = reg.Region_Id; //todo: review
                        if (model.NewCase.Region_Id.HasValue)
                            model.Departments = model.Departments.Where(d => d.Region_Id.HasValue && d.Region_Id == model.NewCase.Region_Id.Value).ToList();
                    }

                    var ous = _orgService.GetOUs(model.NewCase.Department_Id);
                    model.OrganizationUnits = ous;
                }
                else
                {
                    model.Departments = model.Departments.Where(d => d.Region_Id.HasValue && d.Region_Id == model.NewCase.Region_Id.Value).ToList();
                    if (model.Departments.Select(d => d.Id).Contains(model.NewCase.Department_Id.Value))
                    {
                        var ous = _orgService.GetOUs(model.NewCase.Department_Id.Value);
                        model.OrganizationUnits = ous;
                    }
                }
            }
            else
            {
                if (model.NewCase.Region_Id.HasValue)
                    model.Departments = model.Departments.Where(d => d.Region_Id.HasValue && d.Region_Id == model.NewCase.Region_Id.Value).ToList();
            }

            return View("NewCase", model);
        }

        [HttpGet]
        public ActionResult ExtendedCasePublic(Guid id)
        {
            _logger.Warn($"ExtendedCasePublic: {id}");
            //var uniqueId = Guid.Empty;
            /*
            if (!Guid.TryParse(id, out uniqueId))
            {
                _logger.Warn($"ExtendedCasePublic: failed to parse - {id}");
                ErrorGenerator.MakeError("UniqueId value must be specified!", 210);
                return RedirectToAction("Index", "Error");
            }
            */
            bool both = false;
            var caseId = _extendedCaseService.GetCaseIdByExtendedCaseGuid(id);
            if (caseId <= 0)
            {
                ErrorGenerator.MakeError("Extended case data not found!", 210);
                return RedirectToAction("Index", "Error");
            }
            var currentCase = _caseService.GetCaseById(caseId);
            var model = GetExtendedCaseViewModel(currentCase.CaseSolution.Id, caseId);
            var languageId = SessionFacade.CurrentLanguageId;
            var currentCaseModel = GetCaseReceiptModel(currentCase, languageId);
            model.CaseOverviewModel = currentCaseModel;
            ViewBag.AttachmentPlacement = model.AttachmentPlacement;

            if (currentCase.CaseSolution.AvailableTabsSelfsevice == "both")
            {
                model.ActiveTab = currentCase.CaseSolution.ActiveTabSelfservice;
                both = true;
            }

            if (ErrorGenerator.HasError())
                return RedirectToAction("Index", "Error");

            //return url after save
            ViewBag.ReturnUrl = Url.Action("ExtendedCasePublic", new { id });
            ViewBag.caseEmailGuid = id;

            return both ? View("ExtendedCaseWithCase", model) : View("ExtendedCase", model);
        }

        [HttpGet]
        [NoCache] // prevent caching when go back after post
        public ActionResult ExtendedCase(int? caseTemplateId = null, int? caseId = null)
        {
            var model = GetExtendedCaseViewModel(caseTemplateId, caseId);
            if (ErrorGenerator.HasError())
                return RedirectToAction("Index", "Error");
            bool both = false;

            if (!caseId.IsNew())
            {
                var showRegistrationMessage = TempData.GetSafe<bool>(ShowRegistrationMessageKey);
                if (showRegistrationMessage)
                {
                    model.ShowRegistrationMessage = true;
                    model.CaseRegistrationMessage = GetCaseRegistrationMessage(SessionFacade.CurrentLanguageId);
                }

                int caseExistId = caseId ?? 0;
                if (caseExistId != 0)
                {
                    //New to get the case also even if it's an extended case
                    var currentCase = _caseService.GetCaseById(caseExistId);
                    var languageId = SessionFacade.CurrentLanguageId;
                    var currentCaseModel = GetCaseReceiptModel(currentCase, languageId);
                    model.CaseOverviewModel = currentCaseModel;
                    ViewBag.AttachmentPlacement = model.AttachmentPlacement;
                    if (currentCase.CaseSolution.AvailableTabsSelfsevice == "both")
                    {
                        model.ActiveTab = currentCase.CaseSolution.ActiveTabSelfservice;
                        both = true;
                    }
                }

            }

            return both ? View("ExtendedCaseWithCase", model) : View("ExtendedCase", model);

        }

        [HttpPost]
        public ActionResult ApplyWorkflow(int caseId, int templateId)
        {
            var caseTemplate = _caseSolutionService.GetCaseSolution(templateId);

            if (caseTemplate.Status == 0)
            {
                return Json(new { success = false, Error = "Selected template is not available anymore!" });
            }

            var caseModel = _universalCaseService.GetCase(caseId);

            // Apply template values to case:
            ApplyTemplate(caseTemplate, caseModel, true);

            var localUserId = SessionFacade.CurrentLocalUser?.Id ?? 0;
            var auxModel = new AuxCaseModel(SessionFacade.CurrentLanguageId,
                localUserId,
                SessionFacade.CurrentUserIdentity.UserId,
                ConfigurationService.AppSettings.HelpdeskPath,
                CreatedByApplications.SelfService5,
                TimeZoneInfo.Local);

            decimal caseNum;
            var res = _universalCaseService.SaveCaseCheckSplit(caseModel, auxModel, out caseId, out caseNum);

            return Json(new { success = res.IsSucceed, Error = res.LastMessage });
        }

        [HttpGet]
        public ActionResult GetWorkflowFinishingCause(int templateId)
        {
            var caseTemplate = _caseSolutionService.GetCaseSolution(templateId);

            if (caseTemplate == null)
            {
                return Json(new { success = false, Error = "Selected template is not available anymore!", Data = 0 }, JsonRequestBehavior.AllowGet);
            }

            if (caseTemplate.Status == 0)
            {
                return Json(new { success = false, Error = "Selected template is not available anymore!", Data = 0 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, Error = "", Data = caseTemplate.FinishingCause_Id }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetWorkflowSteps(int caseId, int templateId)
        {
            var customerId = caseId > 0 ? _caseService.GetCaseCustomerId(caseId) : SessionFacade.CurrentCustomerID;
            //Check if closed
            var steps = GetWorkflowStepModel(customerId, caseId, templateId);
            return Json(new { sucess = true, items = steps }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExtendedCase(ExtendedCaseViewModel model, string returnUrl = null)
        {
            var isNewCase = model.CaseDataModel.Id == 0;

            var localUserId = SessionFacade.CurrentLocalUser?.Id ?? 0;
            var auxModel = new AuxCaseModel(model.LanguageId,
                                            localUserId,
                                            SessionFacade.CurrentUserIdentity.UserId,
                                            ConfigurationService.AppSettings.HelpdeskPath, //RequestExtension.GetAbsoluteUrl(),
                                            CreatedByApplications.ExtendedCase,
                                            TimeZoneInfo.Local);

            //LogWithContext($"ExtendedCase.Post: Saving extended case data. LocalUserId: {auxModel.CurrentUserId}, url: {auxModel.AbsolutreUrl}.");

            var templateId = model.SelectedWorkflowStep ?? 0;
            if (templateId > 0)
            {
                var caseTemplate = _caseSolutionService.GetCaseSolution(templateId);
                if (caseTemplate == null || caseTemplate.Status == 0)
                {
                    ErrorGenerator.MakeError("Selected template is not available anymore!");
                    return RedirectToAction("Index", "Error");
                }

                ApplyTemplate(caseTemplate, model.CaseDataModel, true);
            }

            var caseId = -1;
            decimal caseNum;

            //TODO: Refactor
            model.CaseDataModel.ExtendedCaseData_Id = model.ExtendedCaseDataModel.Id;
            model.CaseDataModel.ExtendedCaseForm_Id = model.ExtendedCaseDataModel.ExtendedCaseFormId;

            var res = _universalCaseService.SaveCaseCheckSplit(model.CaseDataModel, auxModel, out caseId, out caseNum);

            if (res.IsSucceed)
            {
                var customerId = model.CustomerId;
                var caseEntity = res.Data as Case;
                if (caseEntity != null)
                {
                    customerId = caseEntity.Customer_Id;
                    caseId = caseEntity.Id;
                }
                else if (caseId > 0)
                {
                    customerId = _caseService.GetCaseCustomerId(caseId);
                }

                if (caseId > 0)
                {
                    SaveCaseFiles(model.CaseDataModel.CaseFileKey, customerId, caseId, localUserId);

                    if (ConfigurationService.AppSettings.ShowConfirmAfterCaseRegistration)
                    {
                        return RedirectToCaseConfirmation("Your case has been successfully registered.", string.Empty);
                    }
                    else
                    {
                        TempData[ShowRegistrationMessageKey] = isNewCase;

                        if (string.IsNullOrEmpty(returnUrl))
                            return new RedirectResult(Url.Action("ExtendedCase", "Case", new { caseId = caseId }) + "#no-back");
                        else
                            return Redirect(returnUrl + "#no-back");
                    }
                }
            }

            if (model.CaseDataModel.OU_Id.HasValue)
            {
                model.CaseOU = _ouService.GetOU(model.CaseDataModel.OU_Id.Value);
            }

            model.CurrentCustomer = SessionFacade.CurrentCustomer;
            model.Result = res;
            model.StatusBar = isNewCase ? new Dictionary<string, string>() : GetStatusBar(model);

            var cs = _settingService.GetCustomerSetting(model.CustomerId);
            ViewBag.AttachmentPlacement = cs.AttachmentPlacement;
            ViewBag.ShowCommunicationForSelfservice = ConfigurationService.AppSettings.ShowCommunicationForSelfService;

            model.CaseDataModel.FieldSettings =
                _caseFieldSettingService.ListToShowOnCasePage(model.CustomerId, model.LanguageId).Where(c => c.ShowExternal == 1).ToList();

            model.CaseLogsModel = GetCaseLogsModel(caseId);

            return View("ExtendedCase", model);
        }

        public ActionResult GetDepartmentsByRegion(int? id, int customerId, int departmentFilterFormat)
        {
            var list = _orgJsonService.GetActiveDepartmentForRegion(id, customerId, departmentFilterFormat);
            return Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrgUnitsByDepartments(int? id, int customerId)
        {
            var list = _orgJsonService.GetActiveOUForDepartmentAsIdName(id, customerId);
            return Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HandleError(RequestValidationResult result)
        {
            ErrorGenerator.MakeError(result.ErrorMessage, result.ErrorCode > 0 ? result.ErrorCode : (int?)null);
            return RedirectToErrorPage();
        }

        [HttpGet]
        public ActionResult UserCases(int customerId, string progressId = "")
        {
            if (ConfigurationService.AppSettings.LoginMode == LoginMode.Anonymous)
            {
                return Redirect("Index");
            }
            var res = _caseControllerBehavior.ValidateCustomer();
            if (!res.Valid) return HandleError(res);

            res = _caseControllerBehavior.ValidateCurrentUserIdentity();
            if (!res.Valid) return HandleError(res);

            var searchParameters = PrepareCaseSearchInputParameters();

            res = _caseControllerBehavior.ValidateSearchParameters(searchParameters);
            if (!res.Valid) return HandleError(res);

            var model = new UserCasesModel
            {
                CustomerId = customerId,

                ////////////////////////////////////////
                // search model
                PharasSearch = searchParameters.PharasSearch,
                ProgressId = searchParameters.ProgressId,
                ProgressItems = _caseControllerBehavior.PrepareProgressSelectItems(),
                ////////////////////////////////////////
            };

            return View("CaseOverview", model);
        }

        [HttpGet]
        public ActionResult MultiCustomerUserCases(string progressId = "")
        {
            if (ConfigurationService.AppSettings.LoginMode == LoginMode.Anonymous)
            {
                return Redirect("Index");
            }
            var globalSettings = _globalSettingService.GetGlobalSettings().First();
            if (globalSettings.MultiCustomersSearch == 0)
                return RedirectToAction("UserCases", new { customerId = SessionFacade.CurrentCustomerID });

            var res = _caseControllerBehavior.ValidateCustomer();
            if (!res.Valid) return HandleError(res);

            res = _caseControllerBehavior.ValidateCurrentUserIdentity();
            if (!res.Valid) return HandleError(res);

            var searchParameters = PrepareCaseSearchInputParameters();

            res = _caseControllerBehavior.ValidateSearchParameters(searchParameters);
            if (!res.Valid) return HandleError(res);

            var userId = SessionFacade.CurrentUserIdentity.UserId;
            var employeeNumber = SessionFacade.CurrentUserIdentity.EmployeeNumber;
            var employees = SessionFacade.CurrentCoWorkers != null && SessionFacade.CurrentCoWorkers.Any()
                ? SessionFacade.CurrentCoWorkers.Where(e => !string.IsNullOrEmpty(e.EmployeeNumber))
                    .Select(e => e.EmployeeNumber)
                    .ToList()
                : new List<string>();

            var customers = _customerUserService.ListCustomersByUserCases(userId, employeeNumber, employees,
                SessionFacade.CurrentCustomer, SessionFacade.CurrentUserIdentity.Email);

            var model = new MultiCustomerUserFilterModel
            {
                ////////////////////////////////////////
                // search model
                PharasSearch = searchParameters.PharasSearch,
                ProgressId = searchParameters.ProgressId,
                ProgressItems = _caseControllerBehavior.PrepareProgressSelectItems(),
                ////////////////////////////////////////
                Customers = customers
            };

            return View("MultiCustomerCaseOverview", model);
        }

        //TODO: should be refactored to methods with single responsibility!
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult _CaseLogNote(int caseId, string note, string logFileGuid, int? templateId)
        {
            SaveLogMessage(caseId, note, logFileGuid);
            //Also save Workflowsteps
            if (templateId.HasValue && templateId.Value > 0)
            {
                var caseTemplate = _caseSolutionService.GetCaseSolution(templateId.Value);

                if (caseTemplate.Status == 0)
                {
                    return Json(new { success = false, Error = "Selected template is not available anymore!" });
                }

                var caseModel = _universalCaseService.GetCase(caseId);

                // Apply template values to case:
                ApplyTemplate(caseTemplate, caseModel, true);

                var localUserId = SessionFacade.CurrentLocalUser?.Id ?? 0;
                var auxModel = new AuxCaseModel(SessionFacade.CurrentLanguageId,
                    localUserId,
                    SessionFacade.CurrentUserIdentity.UserId,
                    ConfigurationService.AppSettings.HelpdeskPath,
                    CreatedByApplications.SelfService5,
                    TimeZoneInfo.Local);

                decimal caseNum;
                var res = _universalCaseService.SaveCaseCheckSplit(caseModel, auxModel, out caseId, out caseNum);
            }

            var model = GetCaseLogsModel(caseId);
            return PartialView(model);
        }

        //TODO: should be moved to CaseService!


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewCase(
            Case newCase,
            CaseMailSetting caseMailSetting,
            string caseFileKey,
            string followerUsers,
            int? selectedWorkflowStep,
            CaseLog caseLog)
        {
            decimal caseNum;

            var templateId = selectedWorkflowStep ?? 0;
            if (templateId > 0)
            {
                var caseTemplate = _caseSolutionService.GetCaseSolution(templateId);
                ApplyTemplate(caseTemplate, newCase, true);
            }

            var isCaptchaActive = IsCaptchaActive();

            if (isCaptchaActive)
            {
                var isCaptchaValid = IsCaptchaValid();
                if (ModelState.IsValid && isCaptchaValid)
                {
                    Save(newCase, caseMailSetting, caseFileKey, followerUsers, caseLog, out caseNum);

                    if (ConfigurationService.AppSettings.ShowConfirmAfterCaseRegistration)
                    {
                        return RedirectToCaseConfirmation("Your case has been successfully registered.", $"You can follow up your case status via this number: {caseNum}");
                    }
                    else
                    {
                        return RedirectToAction("Index", "case", new { id = newCase.Id, showRegistrationMessage = true });
                    }
                }
                else
                {
                    ErrorGenerator.MakeError("Google Recaptcha is not valid", 210);
                    return RedirectToAction("Index", "Error");
                }
            }
            else
            {
                Save(newCase, caseMailSetting, caseFileKey, followerUsers, caseLog, out caseNum);

                if (ConfigurationService.AppSettings.ShowConfirmAfterCaseRegistration)
                {
                    return RedirectToCaseConfirmation("Your case has been successfully registered.", $"You can follow up your case status via this number: {caseNum}");
                }
                else
                {
                    return RedirectToAction("Index", "case", new { id = newCase.Id, showRegistrationMessage = true });
                }
            }
        }

        [HttpPost]
        public ActionResult UrgentInfoMessage(int urgentId, int impactId)
        {
            var result = _priorityService.GetPriorityInfoTextByImpactAndUrgency(impactId, urgentId, SessionFacade.CurrentLanguageId);
            return Json(result);
        }

        [HttpPost]
        public ActionResult SearchUser(string query, int customerId, string searchKey)
        {
            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(customerId, SessionFacade.CurrentLanguageId)
                                                          .Where(c => c.ShowExternal == 1)
                                                          .ToList();

            var fieldsVisibility = new
            {
                Name = caseFieldSetting.Select(f => f.Name).Contains(GlobalEnums.TranslationCaseFields.Persons_Name.ToString()),
                Email = caseFieldSetting.Select(f => f.Name).Contains(GlobalEnums.TranslationCaseFields.Persons_EMail.ToString()),
                Phone = caseFieldSetting.Select(f => f.Name).Contains(GlobalEnums.TranslationCaseFields.Persons_Phone.ToString()),
                Department = caseFieldSetting.Select(f => f.Name).Contains(GlobalEnums.TranslationCaseFields.Department_Id.ToString()),
                UserCode = caseFieldSetting.Select(f => f.Name).Contains(GlobalEnums.TranslationCaseFields.UserCode.ToString())
            };
            var result = _computerService.SearchComputerUsers(customerId, query, null, null, !SessionFacade.CurrentCustomer.UseInitiatorAutocomplete);
            return Json(new { searchKey = searchKey, result = result, fieldsVisibility = fieldsVisibility });
        }

        [HttpPost]
        public ActionResult SearchComputer(string query, int customerId)
        {
            var result = _computerService.SearchComputer(customerId, query);
            return Json(result);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CaseSearchUserEmails(string query, string searchKey)
        {
            var searchScope = new EmailSearchScope()
            {
                SearchInInitiators = true
            };
            var models = _userEmailsSearchService.GetUserEmailsForCaseSend(SessionFacade.CurrentCustomer.Id, query, searchScope);
            return Json(new { searchKey = searchKey, result = models });
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Communication(int caseId)
        {
            var model = GetCaseLogsModel(caseId);
            return PartialView("_Communication", model);
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult CaseLogNote(int caseId)
        {
            var model = GetCaseLogsModel(caseId);
            return PartialView("_CaseLogNote", model);
        }

        [HttpPost]
        public JsonResult GetProductAreaByCaseType(int? caseTypeId)
        {
            var pa = _productAreaService.GetTopProductAreas(SessionFacade.CurrentCustomer.Id).Where(p => p.ShowOnExternalPage != 0).ToList();
            TranslateProductArea(pa);

            /*TODO: This part does not cover all states and needs to be fixed*/
            if (caseTypeId.HasValue)
            {
                var ctProductAreas = _caseTypeService.GetCaseType(caseTypeId.Value).CaseTypeProductAreas.Select(x => x.ProductArea.GetParent()).ToList();
                var paNoCaseType = pa.Where(x => x.CaseTypeProductAreas == null || !x.CaseTypeProductAreas.Any()).ToList();
                ctProductAreas.AddRange(paNoCaseType.Where(p => !ctProductAreas.Select(c => c.Id).Contains(p.Id)));
                ctProductAreas = ctProductAreas.OrderBy(p => p.Name).ToList();

                if (ctProductAreas.Any())
                {
                    var paIds = ctProductAreas.Select(x => x.Id).ToList();
                    foreach (var ctProductArea in ctProductAreas)
                    {
                        paIds.AddRange(GetSubProductAreasIds(ctProductArea));
                    }
                    var drString = HtmlHelperExtension.ProductAreaDropdownString(ctProductAreas);
                    return Json(new { success = true, data = drString, paIds });
                }
            }

            pa = pa.OrderBy(p => p.Name).ToList();
            var praIds = pa.Select(x => x.Id).ToList();
            foreach (var ctProductArea in pa)
            {
                praIds.AddRange(GetSubProductAreasIds(ctProductArea));
            }
            var dropString = HtmlHelperExtension.ProductAreaDropdownString(pa);
            return Json(new { success = true, data = dropString, praIds });
        }

        [HttpGet]
        public ActionResult ChangeSystem(int? id)
        {
            var emptyResult = Json(new { }, JsonRequestBehavior.AllowGet);
            if (!id.HasValue) return emptyResult;

            int? ret = null;
            var e = _systemService.GetSystem(id.Value);
            if (e != null)
                ret = (e.Urgency_Id.HasValue && e.Urgency_Id.Value != 0) ? e.Urgency_Id.Value : new int?();
            return ret.HasValue ?
                Json(new { urgencyId = ret.Value }, JsonRequestBehavior.AllowGet) :
                emptyResult;
        }

        #region private
        private string GetCaseRegistrationMessage(int languageId)
        {
            var registrationInfoText = _infoService.GetInfoText((int)InfoTextType.SelfServiceRegistrationMessage, SessionFacade.CurrentCustomer.Id, languageId);
            return registrationInfoText?.Name ?? string.Empty;
        }

        private void ApplyTemplate(CaseSolution caseTemplate, Case caseEntity, bool isWorkflowChange = false)
        {
            if (caseEntity.IsAbout == null)
                caseEntity.IsAbout = new CaseIsAboutEntity();

            _caseSolutionService.ApplyCaseSolution(caseEntity, caseTemplate);

            if (isWorkflowChange)
            {
                caseEntity.CurrentCaseSolution_Id = caseTemplate.Id;
            }
        }

        private void ApplyTemplate(CaseSolution caseTemplate, CaseModel model, bool isWorkflowChange = false)
        {
            _caseSolutionService.ApplyCaseSolution(model, caseTemplate);

            if (isWorkflowChange)
            {
                model.CurrentCaseSolution_Id = caseTemplate.Id;
            }

            model.Text_External = caseTemplate.Text_External;
            model.Text_Internal = caseTemplate.Text_Internal;
            model.FinishingType_Id = caseTemplate.FinishingCause_Id.IfNullThenElse(model.FinishingType_Id);
        }

        private ExtendedCaseViewModel GetExtendedCaseViewModel(int? caseTemplateId = null, int? caseId = null, bool allowAnonymousAccess = false)
        {
            if (caseTemplateId.IsNew() && caseId.IsNew())
            {
                ErrorGenerator.MakeError("Template or Case must be specified!", 210);
                return null;
            }

            var caseModel = new CaseModel
            {
                RegUserId = SessionFacade.CurrentUserIdentity.UserId,
                RegUserDomain = SessionFacade.CurrentUserIdentity.Domain,
                RegUserName = $"{SessionFacade.CurrentUserIdentity.FirstName} {SessionFacade.CurrentUserIdentity.LastName}".Trim(),
                IpAddress = Request.GetIpAddress(),
                CaseSolution_Id = caseTemplateId,
                CurrentCaseSolution_Id = caseTemplateId,
                CaseFileKey = Guid.NewGuid().ToString()
            };

            CaseSolution caseTemplate = null;
            if (caseTemplateId.HasValue && caseTemplateId.Value > 0)
                caseTemplate = _caseSolutionService.GetCaseSolution(caseTemplateId.Value);

            if (caseId.HasValue)
                caseModel = _universalCaseService.GetCase(caseId.Value);

            if (caseModel == null && caseTemplate == null)
            {
                ErrorGenerator.MakeError("Template or Case must be specified!", 211);
                return null;
            }

            var isAnonymousMode = ConfigurationService.AppSettings.LoginMode == LoginMode.Anonymous;
            if (!isAnonymousMode && !allowAnonymousAccess && caseId.HasValue)
            {
                var hasAccess = UserHasAccessToCase(caseModel);
                if (!hasAccess)
                {
                    ErrorGenerator.MakeError("Case not found among your cases!");
                    return null;
                }
            }

            var cusId = caseModel != null && caseModel.Customer_Id > 0 ? caseModel.Customer_Id : (caseTemplate?.Customer_Id ?? 0);
            var globalSettings = _globalSettingService.GetGlobalSettings().First();

            // check only if multi customer is not enabled. Allow user to see own cases for different customers.
            if (!globalSettings.MultiCustomersSearch.ToBool() && !caseId.IsNew() && cusId != SessionFacade.CurrentCustomerID)
            {
                ErrorGenerator.MakeError("Selected Case doesn't belong to current customer!");
                return null;
            }

            var customer = SessionFacade.CurrentCustomer;

            if (globalSettings.MultiCustomersSearch.ToBool() && !caseId.IsNew())
            {
                //override current customer when openning a case from another customer in multicustomer search mode
                customer = _customerService.GetCustomer(cusId);
                //SessionFacade.CurrentCustomer = customer;
            }

            var customerId = customer.Id;
            var languageId = SessionFacade.CurrentLanguageId;
            var appSettings = ConfigurationService.AppSettings;
            var cs = _settingService.GetCustomerSetting(customerId);

            caseModel.FieldSettings =
                _caseFieldSettingService.ListToShowOnCasePage(customerId, languageId).Where(c => c.ShowExternal == 1).ToList();

            if (caseId.IsNew())
            {
                if (caseTemplate == null || caseTemplate.Status == 0 || !caseTemplate.ShowInSelfService || caseTemplate.Customer_Id != customerId)
                {
                    ErrorGenerator.MakeError("Selected template is not available anymore!");
                    return null;
                }
            }

            if (caseId == null || caseId == 0)
            {
                caseModel.Customer_Id = customerId;

                var registrationSource =
                    _registrationSourceCustomerService.GetCustomersActiveRegistrationSources(customerId).FirstOrDefault(x => x.SystemCode == (int)CaseRegistrationSource.SelfService);

                if (registrationSource != null)
                    caseModel.RegistrationSourceCustomer_Id = registrationSource.Id;

                if (caseTemplate != null)
                {
                    _caseSolutionService.ApplyCaseSolution(caseModel, caseTemplate);

                    caseModel.Text_External = caseTemplate.Text_External;
                    caseModel.Text_Internal = caseTemplate.Text_Internal;
                    caseModel.FinishingType_Id = caseTemplate.FinishingCause_Id;
                }
            }

            /*Get StateSecondaryId if existing*/
            var caseStateSecondaryId = 0;
            if (caseModel?.StateSecondary_Id != null)
            {
                var ss = _stateSecondaryService.GetStateSecondary(caseModel.StateSecondary_Id.Value);
                caseStateSecondaryId = ss?.StateSecondaryId ?? 0;
                caseModel.StateSecondaryName = ss?.Name;
            }

            if (caseModel?.WorkingGroup_Id != null)
            {
                var curWorkingGroup = _workingGroupService.GetWorkingGroup(caseModel.WorkingGroup_Id.Value);
                caseModel.WorkingGroupName = curWorkingGroup?.WorkingGroupName;
            }

            var initData = new InitExtendedForm(customerId, languageId, SessionFacade.CurrentUserIdentity.UserId, caseTemplateId, caseId, UserRoleType.LineManager, caseStateSecondaryId);
            var lastError = string.Empty;

            var extendedCaseDataModel = _extendedCaseService.GenerateExtendedFormModel(initData, out lastError);
            if (extendedCaseDataModel == null)
            {
                ErrorGenerator.MakeError(lastError);
                return null;
            }

            var model = new ExtendedCaseViewModel
            {
                CaseId = initData.CaseId,
                CaseTemplateId = initData.CaseSolutionId,
                CustomerId = initData.CustomerId,
                LanguageId = initData.LanguageId,
                ExtendedCaseDataModel = extendedCaseDataModel,
                CurrentUser = IsLineManagerApplication() ? SessionFacade.CurrentUserIdentity.EmployeeNumber : SessionFacade.CurrentSystemUser,
                CurrentCustomer = customer,
                UserRole = initData.UserRole,
                StateSecondaryId = caseStateSecondaryId,
                CaseOU = caseModel.OU_Id.HasValue ? _ouService.GetOU(caseModel.OU_Id.Value) : null,
                CaseDataModel = caseModel,
                LogFileGuid = Guid.NewGuid().ToString(),
                ApplicationType = CurrentApplicationType,
                AttachmentPlacement = cs.AttachmentPlacement,
                ShowCommunicationForSelfservice = appSettings.ShowCommunicationForSelfService,
                ShowCaseActionsPanelOnTop = customer.ShowCaseActionsPanelOnTop,
                ShowCaseActionsPanelAtBottom = customer.ShowCaseActionsPanelAtBottom,
                ClosedCaseAlertModel = new ClosedCaseAlertModel()
                {
                    FinishingDate = caseModel.FinishingDate,
                    CaseComplaintDays = cs.CaseComplaintDays
                },
                CaseLogsModel = GetCaseLogsModel(initData.CaseId)
            };

            model.WhiteFilesList = _globalSettingService.GetFileUploadWhiteList();
            model.MaxFileSize = 36700160;

            if (string.IsNullOrEmpty(model.ExtendedCaseDataModel.FormModel.Name))
            {
                if (caseTemplate == null)
                    caseTemplate = _caseSolutionService.GetCaseSolution(caseModel.CaseSolution_Id ?? (caseTemplateId ?? 0));

                if (caseTemplate != null)
                    model.ExtendedCaseDataModel.FormModel.Name = caseTemplate.Name;
            }

            model.StatusBar = caseId.IsNew() ? new Dictionary<string, string>() : GetStatusBar(model);

            return model;
        }

        private void SaveCaseFiles(string caseFileKey, int customerId, int caseId, int userId)
        {
            //Get from baseCase path
            var basePath = _masterDataService.GetFilePath(customerId);

            if (!string.IsNullOrWhiteSpace(caseFileKey))
            {
                var temporaryFiles = _userTemporaryFilesStorage.GetFiles(caseFileKey, ModuleName.Cases);
                var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseId, userId)).ToList();

                var paths = new List<KeyValuePair<CaseFileDto, string>>();
                _caseFileService.AddFiles(newCaseFiles, paths);

                if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                {
                    foreach (var file in paths)
                    {
                        var result = SessionFacade.CurrentUser != null
                            ? _fileViewLogService.Log(caseId, SessionFacade.CurrentUser.Id, file.Key.FileName, file.Value, FileViewLogFileSource.Selfservice, FileViewLogOperation.Add)
                            : _fileViewLogService.Log(caseId, GetUserName(), file.Key.FileName, file.Value, FileViewLogFileSource.Selfservice, FileViewLogOperation.Add);
                    }
                }

                // delete temp folders                
                _userTemporaryFilesStorage.DeleteFiles(caseFileKey);
            }
        }

        private ActionResult RedirectToCaseConfirmation(string title, string details)
        {
            SessionFacade.LastMessageDialog = MessageDialogModel.Success(title, details);
            return RedirectToAction("Index", "Message");
        }

        private CaseLogsModel GetCaseLogsModel(int? caseId)
        {
            var customer = caseId.HasValue && caseId > 0 ? _caseService.GetCaseCustomer(caseId.Value) : SessionFacade.CurrentCustomer;
            var isTwoAttachmentsMode = IsTwoAttachmentsModeEnabled(customer.Id);
            var useInternalLogs = customer.UseInternalLogNoteOnExternalPage.ToBool();

            var caseLogModels = new List<CaseLogModel>();
            if (caseId > 0)
            {
                var caseLogs =
                    _logService.GetLogsByCaseId(caseId.Value, useInternalLogs, isTwoAttachmentsMode && useInternalLogs).OrderByDescending(l => l.RegTime).ToList();

                var logsFilter =
                    useInternalLogs
                        ? (Func<CaseLogData, bool>)(l => !string.IsNullOrEmpty(l.InternalText.Trim()))
                        : (Func<CaseLogData, bool>)(l => !string.IsNullOrEmpty(l.ExternalText.Trim()));

                // filter case logs
                caseLogModels = caseLogs.Where(logsFilter).Select(data => new CaseLogModel
                {
                    Id = data.Id,
                    CaseId = data.CaseId,
                    UserId = data.UserId,
                    UserFirstName = data.UserFirstName,
                    UserSurName = data.UserSurName,
                    RegUserName = data.RegUserName,
                    InternalText = data.InternalText,
                    ExternalText = data.ExternalText,
                    LogDate = data.LogDate,
                    RegTime = data.RegTime,

                    Files = data.Files?.Select(f => new Models.Case.LogFileModel
                    {
                        Id = f.Id,
                        FileName = f.FileName,
                        LogType = f.LogType
                    }).ToList()
                }).ToList();
            }

            bool allowAttachments;
            if (useInternalLogs)
            {
                allowAttachments = isTwoAttachmentsMode;
            }
            else
            {
                var fieldName = GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString().GetCaseFieldName();
                allowAttachments = _caseFieldSettingService.GetCaseFieldSetting(customer.Id, fieldName)?.ShowExternal.ToBool() ?? false;
            }


            var model = new CaseLogsModel(
                            caseId ?? 0,
                            caseLogModels,
                            SessionFacade.CurrentSystemUser,
                            useInternalLogs,
                            allowAttachments);
            return model;
        }

        private void SaveLogMessage(int caseId, string extraNote, string logFileGuid)
        {
            IDictionary<string, string> errors;
            var currentCase = _caseService.GetCaseById(caseId);
            var customer = _customerService.GetCustomer(currentCase.Customer_Id);
            var cs = _settingService.GetCustomerSetting(customer.Id);
            var caseIsActivated = false;
            var useInternalLog = customer.UseInternalLogNoteOnExternalPage.ToBool();
            var appSettings = ConfigurationService.AppSettings;

            // save case history

            // unread/status flag update if not case is closed

            if (currentCase.FinishingDate.HasValue)
            {
                var adUser = SessionFacade.CurrentSystemUser; // global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                _caseService.Activate(currentCase.Id, 0, adUser, CreatedByApplications.SelfService5, out errors);
                caseIsActivated = true;
                currentCase = _caseService.GetCaseById(caseId);
            }

            if (!currentCase.FinishingDate.HasValue)
                currentCase.Unread = 1;

            int[] departmentIds = null;
            if (currentCase.Department_Id.HasValue)
                departmentIds = new int[] { currentCase.Department_Id.Value };

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZoneId);

            //todo: should be injected via DI
            var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                ManualDependencyResolver.Get<IHolidayService>(),
                customer.WorkingDayStart,
                customer.WorkingDayEnd,
                timeZone);

            var utcNow = DateTime.UtcNow;
            var workTimeCalc = workTimeCalcFactory.Build(currentCase.RegTime, utcNow, departmentIds);

            var possibleWorktime =
                workTimeCalc.CalculateWorkTime(currentCase.RegTime, utcNow, currentCase.Department_Id);

            int externalTimeToAdd = 0;

            // if statesecondary has ResetOnExternalUpdate
            if (currentCase.StateSecondary_Id.HasValue)
            {
                //get substatus
                var casestatesecundary = _stateSecondaryService.GetStateSecondary(currentCase.StateSecondary_Id.Value);

                if (casestatesecundary.ResetOnExternalUpdate == 1)
                    currentCase.StateSecondary_Id = null;

                if (casestatesecundary.IncludeInCaseStatistics == 0)
                {
                    externalTimeToAdd =
                        workTimeCalc.CalculateWorkTime(currentCase.ChangeTime, utcNow, currentCase.Department_Id);

                    currentCase.ExternalTime += externalTimeToAdd;
                }
            }

            var oldLeadTime = currentCase.LeadTime;
            currentCase.LeadTime = possibleWorktime - currentCase.ExternalTime;

            currentCase.ChangeTime = DateTime.UtcNow;

            var extraFields = new ExtraFieldCaseHistory()
            {
                ActionExternalTime = externalTimeToAdd,
                ActionLeadTime = currentCase.LeadTime - oldLeadTime,
                LeadTime = currentCase.LeadTime
            };
            //This only if changed workflow is not true?
            var caseHistoryId =
                _caseService.SaveCaseHistory(currentCase, 0, currentCase.PersonsEmail, CreatedByApplications.SelfService5, out errors, SessionFacade.CurrentUserIdentity.UserId, extraFields);

            // save log
            var caseLog = new CaseLog
            {
                CaseHistoryId = caseHistoryId,
                CaseId = caseId,
                LogGuid = Guid.NewGuid(),
                TextExternal = !useInternalLog ? extraNote.Replace("\n", "\r\n") : string.Empty,
                UserId = null,
                TextInternal = useInternalLog ? extraNote.Replace("\n", "\r\n") : string.Empty,
                WorkingTime = 0,
                EquipmentPrice = 0,
                Price = 0,
                Charge = false,
                RegUser = SessionFacade.CurrentSystemUser,
                SendMailAboutCaseToNotifier = false,
                SendMailAboutLog = true
            };

            if (currentCase.WorkingGroup_Id != null)
            {
                var curWorkingGroup = _workingGroupService.GetWorkingGroup(currentCase.WorkingGroup_Id.Value);
                if (curWorkingGroup.AllocateCaseMail == 1) // Send To Users Working Group EMail
                {
                    // Send Mail to all working group users
                    var usersInWorkingGroup = _userService.GetUsersForWorkingGroup(currentCase.WorkingGroup_Id.Value).Where(u => u.Email.Trim() != string.Empty).ToList();
                    var emailTo = new List<string>();
                    if (usersInWorkingGroup != null && usersInWorkingGroup.Count > 0)
                    {
                        if (currentCase.Department_Id.HasValue)
                        {
                            foreach (var user_ in usersInWorkingGroup)
                            {
                                var departments = _departmentService.GetDepartmentsByUserPermissions(user_.Id, currentCase.Customer_Id);

                                if (departments != null && departments.FirstOrDefault(x => x.Id == currentCase.Department_Id.Value) != null)
                                {
                                    emailTo.Add(user_.Email);
                                }

                                if (departments == null)
                                    emailTo.Add(user_.Email);
                            }
                        }
                        else
                        {
                            emailTo = usersInWorkingGroup.Select(u => u.Email).ToList();
                        }
                    }
                    if (emailTo.Count > 0)
                        caseLog.EmailRecepientsExternalLog = string.Join(Environment.NewLine, emailTo);
                }
            }

            var caseMailSetting = new CaseMailSetting(customer.NewCaseEmailList,
                customer.HelpdeskEmail,
                appSettings.HelpdeskPath,
                cs.DontConnectUserToWorkingGroup
            );

            var temporaryLogFiles = new List<WebTemporaryFile>();
            var userTimeZone = TimeZoneInfo.Local;

            if (!string.IsNullOrEmpty(logFileGuid))
            {
                var logFileType = LogFileType.External;

                if (IsTwoAttachmentsModeEnabled(customer.Id) && useInternalLog)
                {
                    logFileType = LogFileType.Internal;
                }

                temporaryLogFiles = _userTemporaryFilesStorage.GetFiles(logFileGuid, logFileType.GetFolderPrefix());
                caseLog.Id = _logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);

                // save log files
                var basePath = _masterDataService.GetFilePath(currentCase.Customer_Id);
                var newLogFiles = temporaryLogFiles.Select(f => new CaseLogFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id, null, logFileType, null)).ToList();

                var paths = new List<KeyValuePair<CaseLogFileDto, string>>();
                _logFileService.AddFiles(newLogFiles, paths);

                if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                {
                    foreach (var path in paths)
                    {
                        var result = SessionFacade.CurrentUser != null
                            ? _fileViewLogService.Log(caseId, SessionFacade.CurrentUser.Id, path.Key.FileName, path.Value, FileViewLogFileSource.Selfservice, FileViewLogOperation.Add)
                            : _fileViewLogService.Log(caseId, GetUserName(), path.Key.FileName, path.Value, FileViewLogFileSource.Selfservice, FileViewLogOperation.Add);
                    }
                }

                // send emails                
                _caseService.SendSelfServiceCaseLogEmail(currentCase.Id, caseMailSetting, caseHistoryId, caseLog, basePath, userTimeZone, newLogFiles, caseIsActivated);
                _userTemporaryFilesStorage.DeleteFiles(logFileGuid);
            }
            else
            {
                caseLog.Id = _logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);
                _caseService.SendSelfServiceCaseLogEmail(currentCase.Id, caseMailSetting, caseHistoryId, caseLog, string.Empty, userTimeZone, null, caseIsActivated);
            }
        }

        private void TranslateProductArea(ICollection<ProductArea> pa)
        {
            pa.ForEach(p =>
            {
                p.Name = Translation.Get(p.Name);
                if (p.SubProductAreas != null && p.SubProductAreas.Any())
                    TranslateProductArea(p.SubProductAreas);
            });
        }

        private Dictionary<string, string> GetStatusBar(ExtendedCaseViewModel model)
        {
            var values = new Dictionary<string, string>();
            var templateText = "<strong>{0}</strong>&nbsp;{1}&nbsp;|&nbsp;";
            var settings = _caseFieldSettingService.GetCaseFieldSettings(model.CustomerId)
                .Where(c => c.ShowExternalStatusBar == true)
                .Select(c => c.Name)
                .ToList();

            var defaultValue = "";
            var dateFormat = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            foreach (var setting in settings)
            {
                var value = "";
                if (setting == GlobalEnums.TranslationCaseFields.ReportedBy.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.CostCentre.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.Place.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.UserCode.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.InventoryLocation.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.CaseNumber.ToString() ||
                   //setting == GlobalEnums.TranslationCaseFields.RegTime.ToString() ||
                   //setting == GlobalEnums.TranslationCaseFields.ChangeTime.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.InventoryNumber.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.Caption.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.Available.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.SolutionRate.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString())
                {
                    var property = model.CaseDataModel.GetType().GetProperty(setting);
                    value = defaultValue;
                    if (property != null)
                    {
                        var propValue = property.GetValue(model.CaseDataModel, null);
                        if (propValue != null)
                            value = propValue.ToString();
                    }

                }
                else if (setting == GlobalEnums.TranslationCaseFields.Persons_Name.ToString())
                {
                    value = model.CaseDataModel.PersonsName;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString())
                {
                    value = model.CaseDataModel.PersonsEmail;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Persons_Phone.ToString())
                {
                    value = model.CaseDataModel.PersonsPhone;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString())
                {
                    value = model.CaseDataModel.PersonsCellphone;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Region_Id.ToString() && model.CaseDataModel.Region_Id.HasValue)
                {
                    var region = _regionService.GetRegion(model.CaseDataModel.Region_Id.Value);
                    value = region != null ? region.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Department_Id.ToString() && model.CaseDataModel.Department_Id.HasValue)
                {
                    var department = _departmentService.GetDepartment(model.CaseDataModel.Department_Id.Value);
                    value = department != null ? department.DepartmentName : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.OU_Id.ToString() && model.CaseDataModel.OU_Id.HasValue)
                {
                    var ou = _ouService.GetOU(model.CaseDataModel.OU_Id.Value);
                    value = ou != null ? ou.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString())
                {
                    value = model.CaseDataModel.IsAbout_PersonsName;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString())
                {
                    value = model.CaseDataModel.IsAbout_PersonsEmail;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString())
                {
                    value = model.CaseDataModel.IsAbout_PersonsPhone;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString())
                {
                    value = model.CaseDataModel.IsAbout_PersonsCellPhone;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString() && model.CaseDataModel.IsAbout_Region_Id.HasValue)
                {
                    var region = _regionService.GetRegion(model.CaseDataModel.IsAbout_Region_Id.Value);
                    value = region != null ? region.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString() && model.CaseDataModel.IsAbout_Department_Id.HasValue)
                {
                    var department = _departmentService.GetDepartment(model.CaseDataModel.IsAbout_Department_Id.Value);
                    value = department != null ? department.DepartmentName : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString() && model.CaseDataModel.IsAbout_OU_Id.HasValue)
                {
                    var ou = _ouService.GetOU(model.CaseDataModel.IsAbout_OU_Id.Value);
                    value = ou != null ? ou.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString())
                {
                    value = model.CaseDataModel.IsAbout_CostCentre;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString())
                {
                    value = model.CaseDataModel.IsAbout_Place;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString())
                {
                    value = model.CaseDataModel.IsAbout_UserCode;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString())
                {
                    value = model.CaseDataModel.InventoryType;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString() && model.CaseDataModel.RegistrationSourceCustomer_Id.HasValue)
                {
                    var source = _registrationSourceCustomerService.GetRegistrationSouceCustomer(model.CaseDataModel.RegistrationSourceCustomer_Id.Value);
                    value = source != null ? Translation.Get(source.SourceName) : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.User_Id.ToString())
                {
                    //Not used in self service
                }
                else if (setting == GlobalEnums.TranslationCaseFields.CaseType_Id.ToString())
                {
                    var caseType = _caseTypeService.GetCaseType(model.CaseDataModel.CaseType_Id);

                    value = caseType != null ? Translation.Get(caseType.Name) : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString() && model.CaseDataModel.ProductArea_Id.HasValue)
                {
                    var prodArea = _productAreaService.GetProductArea(model.CaseDataModel.ProductArea_Id.Value);

                    value = prodArea != null ? Translation.Get(prodArea.Name) : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.System_Id.ToString() && model.CaseDataModel.System_Id.HasValue)
                {
                    var system = _systemService.GetSystem(model.CaseDataModel.System_Id.Value);
                    value = system != null ? system.SystemName : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Urgency_Id.ToString() && model.CaseDataModel.Urgency_Id.HasValue)
                {
                    var urgency = _urgencyService.GetUrgency(model.CaseDataModel.Urgency_Id.Value);
                    value = urgency != null ? urgency.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Impact_Id.ToString() && model.CaseDataModel.Impact_Id.HasValue)
                {
                    var impact = _impactService.GetImpact(model.CaseDataModel.Impact_Id.Value);
                    value = impact != null ? impact.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Category_Id.ToString() && model.CaseDataModel.Category_Id.HasValue)
                {
                    var cat = _categoryService.GetCategoryById(model.CaseDataModel.Category_Id.Value);
                    value = cat != null ? cat.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Supplier_Id.ToString() && model.CaseDataModel.Supplier_Id.HasValue)
                {
                    var sup = _supplierService.GetSupplier(model.CaseDataModel.Supplier_Id.Value);
                    value = sup != null ? sup.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.AgreedDate.ToString())
                {
                    value = model.CaseDataModel.AgreedDate.HasValue ? model.CaseDataModel.AgreedDate.Value.ToString(dateFormat) : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Cost.ToString())
                {
                    value = string.Format("{0}/{1} {2}", model.CaseDataModel.Cost, model.CaseDataModel.OtherCost, model.CaseDataModel.Currency);
                }
                else if (setting == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString() && model.CaseDataModel.WorkingGroup_Id.HasValue)
                {
                    //not used in selfservice
                }
                else if (setting == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString())
                {
                    //not used in selfservice
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString())
                {
                    //not used in selfservice
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Priority_Id.ToString())
                {
                    //not used in selfservice
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Status_Id.ToString() && model.CaseDataModel.Status_Id.HasValue)
                {
                    var status = _statusService.GetStatus(model.CaseDataModel.Status_Id.Value);
                    value = status?.Name ?? defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString() && model.CaseDataModel.StateSecondary_Id.HasValue)
                {
                    var subStatus = _stateSecondaryService.GetStateSecondary(model.CaseDataModel.StateSecondary_Id.Value);
                    value = subStatus?.Name ?? defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.PlanDate.ToString())
                {
                    value = model.CaseDataModel.PlanDate?.ToString(dateFormat) ?? defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.WatchDate.ToString())
                {
                    value = model.CaseDataModel.WatchDate?.ToString(dateFormat) ?? defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Verified.ToString())
                {
                    value = Translation.Get(model.CaseDataModel.Verified == 1 ? "Yes" : "No");
                }
                else if (setting == GlobalEnums.TranslationCaseFields.CausingPart.ToString())
                {
                    //not used in selfservice
                }
                else if (setting == GlobalEnums.TranslationCaseFields.FinishingDate.ToString())
                {
                    value = model.CaseDataModel.FinishingDate?.ToString(dateFormat) ?? defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.ClosingReason.ToString())
                {
                    //not used in selfservice
                }
                var template = string.Format(templateText, Translation.GetForCase(setting, model.CustomerId), value);
                values.Add(setting, template);
            }

            return values;
        }

        private bool UserHasAccessToCase(Case currentCase)
        {
            var curUser = SessionFacade.CurrentUserIdentity.UserId;
            

            //Hide this to next release #57742
            if (currentCase.CaseType.ShowOnExtPageCases == 0 || (currentCase.ProductArea != null && currentCase.ProductArea.ShowOnExtPageCases == 0))
            {
                _logger.Warn("UserHasAccessToCase: ShowOnExtPageCases == 0");
                return false;
            }

            var criteria = _caseControllerBehavior.GetCaseOverviewCriteria();


            /*User creator*/
            if (criteria.MyCasesRegistrator && !string.IsNullOrEmpty(criteria.UserId) && !string.IsNullOrEmpty(currentCase.RegUserId))
            {
                if (currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                //Is this neccecary??
                else
                {
                    _logger.Warn($"UserHasAccessToCase: currentCase.RegUserId ('{currentCase.RegUserId}') != CurrentUserIdenity.UserId ('{criteria.UserId}')");
                }
            }

            /*User initiator*/
            if (criteria.MyCasesInitiator && !string.IsNullOrEmpty(currentCase.ReportedBy) &&
                (!string.IsNullOrEmpty(criteria.UserId) || !string.IsNullOrEmpty(criteria.UserEmployeeNumber)))
            {
                if (!string.IsNullOrEmpty(criteria.UserId) &&
                    currentCase.ReportedBy.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase))
                    return true;

                if (!string.IsNullOrEmpty(criteria.UserEmployeeNumber) &&
                    currentCase.ReportedBy.Equals(criteria.UserEmployeeNumber, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            // Compare with email
            if (criteria.MyCasesInitiator && !string.IsNullOrEmpty(currentCase.PersonsEmail) &&
                !string.IsNullOrEmpty(criteria.PersonEmail))
            {
                if (!string.IsNullOrEmpty(criteria.PersonEmail) &&
                    currentCase.PersonsEmail.Equals(criteria.PersonEmail, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            /*User follower*/
            if (criteria.MyCasesFollower)
            {
                if ((currentCase.CaseFollowers != null && currentCase.CaseFollowers.Any()) && (!string.IsNullOrEmpty(criteria.PersonEmail)))
                {
                    if (currentCase.CaseFollowers.Where(m => m.Follower.Equals(criteria.PersonEmail, StringComparison.CurrentCultureIgnoreCase)).Any())
                    { 
                        return true; 
                    }
                }

            }

            /*User Group*/
            if (criteria.MyCasesUserGroup)
            {
                if (criteria.GroupMember != null && criteria.GroupMember.Any())
                {
                    if ((string.IsNullOrEmpty(currentCase.ReportedBy) && currentCase.RegUserId != null &&
                        currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase)) ||
                        criteria.GroupMember.Where(m => m.Equals(currentCase.ReportedBy, StringComparison.CurrentCultureIgnoreCase)).Any())
                        return true;
                }
                else
                {
                    if (string.IsNullOrEmpty(currentCase.ReportedBy) && currentCase.RegUserId != null &&
                        currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }
            }

            if (criteria.MyCasesInitiatorDepartmentId.HasValue &&
                currentCase.Department_Id == criteria.MyCasesInitiatorDepartmentId.Value)
                return true;

            return false;
        }

        private bool UserHasAccessToCase(CaseModel currentCase)
        {
            var curUser = SessionFacade.CurrentUserIdentity.UserId;

            //Hide this to next release #57742
            var caseType = _caseTypeService.GetCaseType(currentCase.CaseType_Id);
            var productArea = currentCase.ProductArea_Id.HasValue ? _productAreaService.GetProductArea(currentCase.ProductArea_Id.Value) : null;

            if (caseType.ShowOnExtPageCases == 0 || (productArea != null && productArea.ShowOnExtPageCases == 0))
                return false;

            var criteria = _caseControllerBehavior.GetCaseOverviewCriteria();
            if (String.IsNullOrEmpty(criteria.PersonEmail))
            {
                //Get User Email from logged in user if exists in tblComputerUsers
                var user = _computerService.GetComputerUserByUserID(curUser);
                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    criteria.PersonEmail = user.Email;
                }
                else //If no computeruser - seach in tblUsers
                {
                    var hdUser = _userService.GetUserByLogin(curUser, SessionFacade.CurrentCustomerID);
                    if (hdUser != null && !string.IsNullOrEmpty(hdUser.Email))
                    {
                        criteria.PersonEmail = hdUser.Email;
                    }
                    else
                    {
                        criteria.PersonEmail = "";
                    }
                }
            }
            

            /*User creator*/
            if (criteria.MyCasesRegistrator && !string.IsNullOrEmpty(criteria.UserId) && !string.IsNullOrEmpty(currentCase.RegUserId))
            {
                if (currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            /*User initiator*/
            if (criteria.MyCasesInitiator && !string.IsNullOrEmpty(currentCase.ReportedBy) &&
                (!string.IsNullOrEmpty(criteria.UserId) || !string.IsNullOrEmpty(criteria.UserEmployeeNumber)))
            {
                if (!string.IsNullOrEmpty(criteria.UserId) &&
                    currentCase.ReportedBy.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase))
                    return true;

                if (!string.IsNullOrEmpty(criteria.UserEmployeeNumber) &&
                    currentCase.ReportedBy.Equals(criteria.UserEmployeeNumber, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            // Compare with email
            if (criteria.MyCasesInitiator && !string.IsNullOrEmpty(currentCase.PersonsEmail) &&
                !string.IsNullOrEmpty(criteria.PersonEmail))
            {
                if (!string.IsNullOrEmpty(criteria.PersonEmail) &&
                    currentCase.PersonsEmail.Equals(criteria.PersonEmail, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            /*User follower*/
            if (criteria.MyCasesFollower)
            {
                var caseFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(currentCase.Id);

                /*User follower*/
                if (criteria.MyCasesFollower)
                {
                    if ((caseFollowers != null && caseFollowers.Any()) && (!string.IsNullOrEmpty(criteria.PersonEmail)))
                    {
                        if (caseFollowers.Where(m => m.Follower.Equals(criteria.PersonEmail, StringComparison.CurrentCultureIgnoreCase)).Any())
                        {
                            return true;
                        }
                    }

                }

            }

            /*User Group*/
            if (criteria.MyCasesUserGroup)
            {
                if (criteria.GroupMember != null && criteria.GroupMember.Any())
                {
                    if ((string.IsNullOrEmpty(currentCase.ReportedBy) && currentCase.RegUserId != null &&
                        currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase)) ||
                        criteria.GroupMember.Where(m => m.Equals(currentCase.ReportedBy, StringComparison.CurrentCultureIgnoreCase)).Any())
                        return true;
                }
                else
                {
                    if (string.IsNullOrEmpty(currentCase.ReportedBy) && currentCase.RegUserId != null &&
                        currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }
            }

            if (criteria.MyCasesInitiatorDepartmentId.HasValue &&
                currentCase.Department_Id == criteria.MyCasesInitiatorDepartmentId.Value)
                return true;

            return false;
        }

        private IEnumerable<int> GetSubProductAreasIds(ProductArea ctProductArea)
        {
            var result = new List<int>();
            if (ctProductArea.SubProductAreas != null && ctProductArea.SubProductAreas.Any())
            {
                foreach (var subProductArea in ctProductArea.SubProductAreas)
                {
                    if (subProductArea.IsActive == 1)
                    {
                        result.Add(subProductArea.Id);
                        if (subProductArea.SubProductAreas != null && subProductArea.SubProductAreas.Any())
                        {
                            result.AddRange(GetSubProductAreasIds(subProductArea));
                        }
                    }
                }
            }
            return result;
        }

        private bool IsCaptchaActive()
        {
            var recaptchaSiteKey = ConfigurationManager.AppSettings[AppSettingsKey.ReCaptchaSiteKey] != null ?
                           ConfigurationManager.AppSettings[AppSettingsKey.ReCaptchaSiteKey].ToString() : "";
            if (recaptchaSiteKey == "#{reCaptchaSiteKey}")
            {
                recaptchaSiteKey = "";
            }
            if (recaptchaSiteKey != "")
                return true;

            return false;
        }
        private bool IsCaptchaValid()
        {
            //Validate Google recaptcha here
            var response = Request["g-recaptcha-response"];
            string secretKey = ConfigurationManager.AppSettings[AppSettingsKey.ReCaptchaSecretKey];
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");

            return status;
        }

        private int Save(
                Case newCase,
                CaseMailSetting caseMailSetting,
                string caseFileKey,
                string followerUsers,
                CaseLog caseLog,
                out decimal caseNumber)
        {
            IDictionary<string, string> errors;
            var utcNow = DateTime.UtcNow;

            // save case and case history
            if (newCase.User_Id <= 0)
                newCase.User_Id = null;

            var mailSenders = new MailSenders
            {
                SystemEmail = caseMailSetting.HelpdeskMailFromAdress
            };

            var basePath = _masterDataService.GetFilePath(newCase.Customer_Id);
            newCase.LatestSLACountDate = CalculateLatestSLACountDate(newCase.StateSecondary_Id);

            var ei = new CaseExtraInfo()
            {
                CreatedByApp = CreatedByApplications.SelfService5,
                LeadTimeForNow = 0,
                ActionLeadTime = 0,
                ActionExternalTime = 0
            };

            if (newCase.Urgency_Id.HasValue && newCase.Impact_Id.HasValue)
            {
                var priorityImpactUrgencies = _urgencyService.GetPriorityImpactUrgencies(newCase.Customer_Id);
                var prioInfo = priorityImpactUrgencies.FirstOrDefault(p => p.Impact_Id == newCase.Impact_Id && p.Urgency_Id == newCase.Urgency_Id);
                if (prioInfo != null)
                    newCase.Priority_Id = prioInfo.Priority_Id;
            }

            // All values should be taken from the template, no rules 2017-09-29, only if template workinggroup is null this rule will happend (Höganäs)(2017-11-01)
            if (newCase.ProductArea_Id.HasValue)
            {
                var productArea = _productAreaService.GetProductArea(newCase.ProductArea_Id.Value);

                if (!newCase.WorkingGroup_Id.HasValue && productArea?.WorkingGroup_Id != null)
                    newCase.WorkingGroup_Id = productArea.WorkingGroup_Id;
                if (!newCase.Priority_Id.HasValue && productArea?.Priority_Id != null)
                    newCase.Priority_Id = productArea.Priority_Id;
            }

            if (newCase.Department_Id.HasValue && newCase.Priority_Id.HasValue)
            {
                var dept = _departmentService.GetDepartment(newCase.Department_Id.Value);
                var priority = _priorityService.GetPriorities(newCase.Customer_Id).Where(it => it.Id == newCase.Priority_Id && it.IsActive == 1).FirstOrDefault();

                if (dept?.WatchDateCalendar_Id != null && priority != null && priority.SolutionTime == 0)
                {
                    newCase.WatchDate =
                        _watchDateCalendarService.GetClosestDateTo(dept.WatchDateCalendar_Id.Value, utcNow);
                }
            }

            if (newCase.CaseType_Id > 0)
            {
                var caseType = _caseTypeService.GetCaseType(newCase.CaseType_Id);
                if (!newCase.WorkingGroup_Id.HasValue && caseType?.WorkingGroup_Id != null)
                    newCase.WorkingGroup_Id = caseType.WorkingGroup_Id;

                if (!newCase.Performer_User_Id.HasValue && caseType?.User_Id != null)
                    newCase.Performer_User_Id = caseType.User_Id;
            }

            if (newCase.WorkingGroup_Id.HasValue)
            {
                var wg = _workingGroupService.GetWorkingGroup(newCase.WorkingGroup_Id.Value);
                if (!string.IsNullOrWhiteSpace(wg?.EMail) && _emailService.IsValidEmail(wg.EMail))
                    mailSenders.WGEmail = wg.EMail;

                if (!newCase.StateSecondary_Id.HasValue && wg?.StateSecondary_Id != null)
                    newCase.StateSecondary_Id = wg.StateSecondary_Id.Value;
            }

            caseMailSetting.CustomeMailFromAddress = mailSenders;

            if (newCase.CaseSolution_Id == 0)
                newCase.CaseSolution_Id = null;

            var localUserId = SessionFacade.CurrentLocalUser?.Id ?? 0;

            // SAVE CASE:
            var caseHistoryId =
                _caseService.SaveCase(newCase, caseLog, localUserId, SessionFacade.CurrentUserIdentity.UserId, ei, out errors);

            // save log
            caseLog.CaseId = newCase.Id;
            caseLog.CaseHistoryId = caseHistoryId;

            if (caseLog.UserId <= 0 && localUserId > 0)
                caseLog.UserId = localUserId;

            if (string.IsNullOrWhiteSpace(caseLog.RegUser))
                caseLog.RegUser = SessionFacade.CurrentUserIdentity?.UserId ?? string.Empty;

            caseLog.Id = _logService.SaveLog(caseLog, 0, out errors);

            // save case files
            SaveCaseFiles(caseFileKey, newCase.Customer_Id, newCase.Id, localUserId);

            var oldCase = new Case();

            // send emails
            var userTimeZone = TimeZoneInfo.Local;

            //save extra followers            
            if (!string.IsNullOrEmpty(followerUsers))
            {
                var extraFollowers = followerUsers.Split(EMAIL_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                _caseExtraFollowersService.SaveExtraFollowers(newCase.Id, extraFollowers, null);
            }

            _caseService.SendCaseEmail(newCase.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase, caseLog);

            caseNumber = newCase.CaseNumber;
            return newCase.Id;
        }

        private DateTime? CalculateLatestSLACountDate(int? newSubStateId)
        {
            DateTime? ret = null;
            /* -1: Blank | 0: Non-Counting | 1: Counting */
            //var oldSubStateMode = -1;
            var newSubStateMode = -1;

            if (newSubStateId.HasValue)
            {
                var newSubStatus = _stateSecondaryService.GetStateSecondary(newSubStateId.Value);
                if (newSubStatus != null)
                    newSubStateMode = newSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (newSubStateMode == 0)
                ret = DateTime.UtcNow;
            else
                ret = null;

            return ret;
        }

        private string GetUserName()
        {
            return SessionFacade.CurrentUserIdentity?.UserId ??
                   SessionFacade.CurrentLocalUser?.UserId ?? "AnonymousUser";
        }

        private CaseOverviewModel GetCaseReceiptModel(Case currentCase, int languageId)
        {
            var currentCustomer = currentCase.Customer ?? SessionFacade.CurrentCustomer;
            var caseFieldSetting =
                _caseFieldSettingService.ListToShowOnCasePage(currentCase.Customer_Id, languageId)
                    .Where(c => c.ShowExternal == 1 ||
                                c.Name == GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString() ||
                                c.Name == GlobalEnums.TranslationCaseFields.CaseNumber.ToString() ||
                                c.Name == GlobalEnums.TranslationCaseFields.RegTime.ToString())
                    .ToList();

            var caseSectionModels = _caseSectionService.GetCaseSections(currentCase.Customer_Id, SessionFacade.CurrentLanguageId);
            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);
            var infoText = _infoService.GetInfoText((int)InfoTextType.SelfServiceInformation, currentCase.Customer_Id, languageId);

            var appSettings = ConfigurationService.AppSettings;

            // get customersettings
            var customersettings = _settingService.GetCustomerSetting(currentCase.Customer_Id);

            var regions = _regionService.GetRegions(currentCase.Customer_Id);
            var suppliers = _supplierService.GetSuppliers(currentCase.Customer_Id);
            var systems = _systemService.GetSystems(currentCase.Customer_Id, true, currentCase.System_Id);

            if (currentCase.CaseType != null)
            {
                var tempCTs = new List<CaseType>();
                tempCTs.Add(currentCase.CaseType);
                tempCTs = CaseTypeTreeTranslation(tempCTs).ToList();
                currentCase.CaseType.Name = tempCTs[0].getCaseTypeParentPath();
            }

            if (currentCase.ProductArea_Id.HasValue && currentCase.ProductArea != null)
            {
                var pathTexts = _productAreaService.GetParentPath(currentCase.ProductArea_Id.Value, currentCase.Customer_Id).ToList();
                var translatedText = pathTexts.Select(pathText => Translation.Get(pathText)).ToList();
                currentCase.ProductArea.Name = string.Join(" - ", translatedText);
            }

            if (currentCase.Category_Id.HasValue && currentCase.Category != null)
            {
                var pathTexts = _categoryService.GetParentPath(currentCase.Category_Id.Value, currentCase.Customer_Id).ToList();
                var translatedText = pathTexts.Select(pathText => Translation.Get(pathText)).ToList();
                currentCase.Category.Name = string.Join(" - ", translatedText);
            }

            var caseId = currentCase.Id;

            if (currentCase.Impact_Id.HasValue && currentCase.Impact == null)
                currentCase.Impact = _impactService.GetImpact(currentCase.Impact_Id.Value);

            var caseFolowerUsers = _caseExtraFollowersService.GetCaseExtraFollowers(currentCase.Id).Select(x => x.Follower).ToArray();
            var followerUsers = caseFolowerUsers.Any() ? string.Join(";", caseFolowerUsers) + ";" : string.Empty;

            var whiteList = _globalSettingService.GetFileUploadWhiteList();
            var mergedFinishingCauseForCustomer = _finishingCauseService.GetMergedFinishingCause(SessionFacade.CurrentCustomerID);
            //
            var model = new CaseOverviewModel
            {
                CaseId = caseId,
                CustomerId = currentCustomer.Id,
                InfoText = infoText?.Name,
                CasePreview = currentCase,
                CaseFieldGroups = caseFieldGroups,
                FieldSettings = caseFieldSetting,
                CaseSectionSettings = caseSectionModels,
                Regions = regions,
                Suppliers = suppliers,
                Systems = systems,
                FollowerUsers = followerUsers,

                //logs
                LogFileGuid = Guid.NewGuid().ToString(),
                LogFilesModel = new FilesModel(),
                CaseLogsModel = GetCaseLogsModel(caseId),

                ClosedCaseAlertModel = new ClosedCaseAlertModel()
                {
                    FinishingDate = currentCase?.FinishingDate,
                    CaseComplaintDays = customersettings.CaseComplaintDays,
                    FinishingCause = currentCase.FinishingDescription
                },

                AttachmentPlacement = customersettings.AttachmentPlacement,
                ShowCaseActionsPanelOnTop = currentCustomer.ShowCaseActionsPanelOnTop,
                ShowCaseActionsPanelAtBottom = currentCustomer.ShowCaseActionsPanelAtBottom,
                ApplicationType = CurrentApplicationType,
                ShowCommunicationForSelfService = appSettings.ShowCommunicationForSelfService,
                FileUploadWhiteList = whiteList
            };

            if(!String.IsNullOrEmpty(model.ClosedCaseAlertModel.FinishingCause) && model.ClosedCaseAlertModel.FinishingCause == mergedFinishingCauseForCustomer.Name)
            {
                model.ClosedCaseAlertModel.IsMerged = true;
                model.ClosedCaseAlertModel.MergedParentInfo = _caseService.GetMergedParentInfo(caseId);
            }
            
            return model;
        }

        private NewCaseModel GetNewCaseModel(Customer currentCustomer,
                IUserIdentity currentUserIdentity,
                List<CaseListToCase> caseFieldSetting,
                Setting customerSettings,
                IApplicationSettings appSettings)
        {
            var customerId = currentCustomer.Id;
            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);
            var caseSectionSettings = _caseSectionService.GetCaseSections(customerId, SessionFacade.CurrentLanguageId);
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            var caseFieldSettingsWithLanguages = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);

            //Case Type tree            
            var caseTypes = _caseTypeService.GetCaseTypes(customerId).Where(c => c.ShowOnExternalPage != 0).ToList();
            caseTypes = CaseTypeTreeTranslation(caseTypes).ToList();

            //Product Area tree            
            var productAreas = _productAreaService.GetTopProductAreas(customerId).Where(p => p.ShowOnExternalPage != 0).OrderBy(s => s.Name).ToList();
            var traversedData = ProductAreaTreeTranslation(productAreas);
            productAreas = traversedData.Item1.ToList();

            var whiteList = _globalSettingService.GetFileUploadWhiteList();

            ComputerUserCategory regardingComputerUserCategory = GetRegardingComputerUserCategory(customerId);

            var model = new NewCaseModel
            {
                NewCase = new Case { Customer_Id = customerId },
                CustomerId = customerId,
                CurrentLanguageId = SessionFacade.CurrentLanguageId,
                CaseFieldGroups = caseFieldGroups,
                FieldSettings = caseFieldSetting,
                CaseFieldSettings = caseFieldSettings,
                CaseFieldSettingWithLangauges = caseFieldSettingsWithLanguages,
                CaseFilesModel = new FilesModel { Id = Guid.NewGuid().ToString() },
                CaseSectionSettings = caseSectionSettings,
                Regions = _regionService.GetRegions(customerId),
                Departments = _departmentService.GetDepartments(customerId),
                OrganizationUnits = _orgService.GetOUs(customerId).ToList(),
                CaseTypes = caseTypes,
                ProductAreas = productAreas,
                Systems = _systemService.GetSystems(customerId, true),
                Urgencies = _urgencyService.GetUrgencies(customerId),
                Impacts = _impactService.GetImpacts(customerId),
                Categories = _categoryService.GetActiveParentCategories(customerId),
                Currencies = _currencyService.GetCurrencies(),
                Suppliers = _supplierService.GetSuppliers(customerId),

                CaseMailSetting = new CaseMailSetting(
                                    currentCustomer.NewCaseEmailList,
                                    currentCustomer.HelpdeskEmail,
                                    appSettings.HelpdeskPath,
                                    customerSettings.DontConnectUserToWorkingGroup),

                JsApplicationOptions = new JsApplicationOptions()
                {
                    customerId = customerId,
                    departmentFilterFormat = customerSettings.DepartmentFilterFormat,
                    departmentsURL = Url.Action("GetDepartmentsByRegion", "Case"),
                    orgUnitURL = Url.Action("GetOrgUnitsByDepartments", "Case")
                },

                ShowCaseActionsPanelOnTop = currentCustomer.ShowCaseActionsPanelOnTop,
                ShowCaseActionsPanelAtBottom = currentCustomer.ShowCaseActionsPanelAtBottom,
                CaseTypeParantPath = ParentPathDefaultValue,
                ProductAreaParantPath = ParentPathDefaultValue,
                CategoryParentPath = ParentPathDefaultValue,
                CaseFileKey = Guid.NewGuid().ToString(),
                ProductAreaChildren = traversedData.Item2.ToList(),
                SendToDialogModel = new SendToDialogModel(),

                CaseTypeRelatedFields =
                    _caseTypeService.GetCaseTypesRelatedFields(customerId, true, false)
                        .Select(c => new KeyValuePair<int, string>(c.Id, c.RelatedField))
                        .ToList(),

                ExLogFileGuid = Guid.NewGuid().ToString(),
                AttachmentPlacement = customerSettings.AttachmentPlacement,
                ApplicationType = CurrentApplicationType,
                ShowCommunicationForSelfService = appSettings.ShowCommunicationForSelfService,
                FileUploadWhiteList = whiteList,
                ComputerUserCategories = _computerService.GetComputerUserCategoriesByCustomerID(customerId, true),
                RegardingComputerUserCategory = regardingComputerUserCategory
            };

            if (currentUserIdentity != null)
            {

                var isMicrosoftMode = ConfigurationService.AppSettings.LoginMode == LoginMode.Microsoft;
                model.NewCase =
                    _caseService.InitCase(currentCustomer.Id,
                        0,
                        SessionFacade.CurrentLanguageId,
                        Request.GetIpAddress(),
                        CaseRegistrationSource.SelfService,
                        customerSettings,
                        currentUserIdentity.UserId);

                model.NewCase.Customer = currentCustomer;
                model.NewCase.RegUserId = currentUserIdentity.UserId;
                model.NewCase.RegUserDomain = currentUserIdentity.Domain;

                //set default admin
                model.NewCase.Performer_User_Id = customerSettings.DefaultAdministratorExternal;

                // populate notifier fields
                var notifier = _computerService.GetInitiatorByUserId(currentUserIdentity.UserId, customerId);
                if (isMicrosoftMode && notifier == null)
                {
                    notifier = _masterDataService.GetInitiatorByMail(currentUserIdentity.UserId, customerId);
                    model.NewCase.RegUserId = null;
                    model.NewCase.PersonsEmail = currentUserIdentity.UserId;
                }

                if (notifier != null)
                {
                    model.NewCase.ReportedBy = notifier.UserId;
                    model.NewCase.PersonsName = $"{notifier.FirstName} {notifier.LastName}".Trim();
                    model.NewCase.PersonsEmail = notifier.Email;
                    model.NewCase.PersonsPhone = notifier.Phone;
                    model.NewCase.PersonsCellphone = notifier.CellPhone;
                    model.NewCase.Department_Id = notifier.DepartmentId;
                    model.NewCase.OU_Id = notifier.OrganizationUnitId;
                    model.NewCase.Place = notifier.Place;
                    model.NewCase.UserCode = notifier.Code;
                    model.NewCase.CostCentre = notifier.CostCentre;
                }
            }

            if (currentCustomer.FetchPcNumber)
            {
                var pcNumber = Request.GetComputerName();
                if (!string.IsNullOrEmpty(pcNumber))
                {
                    model.NewCase.InventoryNumber = pcNumber;
                    var inventory = _inventoryService.GetWorkstationByNumber(pcNumber, currentCustomer.Id);
                    if (inventory != null)
                    {
                        model.NewCase.InventoryType = inventory.WorkstationFields?.ComputerTypeName;
                        model.NewCase.InventoryLocation = inventory.WorkstationFields?.Location;
                    }
                }
            }

            var registrationSource =
                _registrationSourceCustomerService.GetCustomersActiveRegistrationSources(customerId)
                    .FirstOrDefault(x => x.SystemCode == (int)CaseRegistrationSource.SelfService);

            if (registrationSource != null)
            {
                model.NewCase.RegistrationSourceCustomer_Id = registrationSource.Id;
            }

            return model;
        }

        private ComputerUserCategory GetRegardingComputerUserCategory(int customerId)
        {
            var defaultCategoryId = ComputerUserCategory.EmptyCategoryId;
            ComputerUserCategory regardingComputerUserCategory = null;
            var customerFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            var regFieldSettings = customerFieldSettings.getCaseSettingsValue(TranslationCaseFields.IsAbout_UserSearchCategory_Id.ToString());

            //set default value only for new case
            if (Int32.TryParse(regFieldSettings.DefaultValue, out defaultCategoryId))
            {
                regardingComputerUserCategory = _computerService.GetComputerUserCategoryByID(defaultCategoryId);
            }

            return regardingComputerUserCategory;
        }

        private List<string> GetVisibleFieldGroups(List<CaseListToCase> fieldList)
        {
            List<string> ret = new List<string>();
            var t = GlobalEnums.TranslationCaseFields.Persons_Phone.ToString();
            string[] userInformationGroup = new string[]
                        {
                            GlobalEnums.TranslationCaseFields.ReportedBy.ToString(),
                            GlobalEnums.TranslationCaseFields.Persons_Name.ToString(),
                            GlobalEnums.TranslationCaseFields.Persons_EMail.ToString(),
                            GlobalEnums.TranslationCaseFields.Persons_Phone.ToString(),
                            GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString(),
                            GlobalEnums.TranslationCaseFields.Customer_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.Region_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.Department_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.OU_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.Place.ToString(),
                            GlobalEnums.TranslationCaseFields.UserCode.ToString(),
                            GlobalEnums.TranslationCaseFields.CostCentre.ToString()
                        };

            string[] computerInformationGroup = new string[]
                        {
                            GlobalEnums.TranslationCaseFields.InventoryNumber.ToString(),
                            GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.InventoryLocation.ToString()
                        };

            string[] caseRegardingGroup = new string[]
            {
                            GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString(),
                            GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString(),
                            GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString(),
                            GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString(),
                            GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString()
            };

            string[] caseInfoGroup = new string[]
                        {
                            GlobalEnums.TranslationCaseFields.CaseNumber.ToString(),
                            GlobalEnums.TranslationCaseFields.RegTime.ToString(),
                            GlobalEnums.TranslationCaseFields.ChangeTime.ToString(),
                            GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.System_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.Urgency_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.Impact_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.Category_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.Supplier_Id.ToString(),
                            GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString(),
                            GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString(),
                            GlobalEnums.TranslationCaseFields.Caption.ToString(),
                            GlobalEnums.TranslationCaseFields.Description.ToString(),
                            GlobalEnums.TranslationCaseFields.Miscellaneous.ToString(),
                            GlobalEnums.TranslationCaseFields.ContactBeforeAction.ToString(),
                            GlobalEnums.TranslationCaseFields.SMS.ToString(),
                            GlobalEnums.TranslationCaseFields.AgreedDate.ToString(),
                            GlobalEnums.TranslationCaseFields.Available.ToString(),
                            GlobalEnums.TranslationCaseFields.Cost.ToString(),
                            GlobalEnums.TranslationCaseFields.Filename.ToString(),
                        };

            string[] otherGroup = new string[]
                        {
                            GlobalEnums.TranslationCaseFields.PlanDate.ToString(),
                            GlobalEnums.TranslationCaseFields.WatchDate.ToString(),
                            GlobalEnums.TranslationCaseFields.Verified.ToString(),
                            GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString(),
                            GlobalEnums.TranslationCaseFields.SolutionRate.ToString()

                        };

            string[] caseLogGroup = new string[]
                        {
                            GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString(),
                            GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(),
                            GlobalEnums.TranslationCaseFields.FinishingDescription.ToString(),
                            GlobalEnums.TranslationCaseFields.FinishingDate.ToString(),
                        };


            foreach (var field in fieldList)
            {
                if (userInformationGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.UserInformation);

                if (computerInformationGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.ComputerInformation);

                if (caseRegardingGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.CaseRegarding);

                if (caseInfoGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.CaseInfo);

                if (otherGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.Other);

                if (caseLogGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.CaseLog);
            }

            return ret;
        }

        private List<FieldSettingJSModel> GetFieldSettingsModel(List<CaseListToCase> customerFieldSettings, List<CaseSolutionSettingOverview> templateSettings)
        {
            var ret = new List<FieldSettingJSModel>();
            foreach (var field in customerFieldSettings)
            {
                var isVisible = field.ShowExternal.ToBool();
                var isRequired = field.Required.ToBool();
                var isReadonly = false;

                if (templateSettings != null && templateSettings.Any())
                {
                    var curFieldName = field.Name.ToLower();
                    var templateField = templateSettings.Where(t => t.CaseSolutionField.MapToCaseField().ToString().ToLower() == curFieldName).SingleOrDefault();
                    if (templateField != null)
                    {
                        isReadonly = templateField.CaseSolutionMode == Common.Enums.Settings.CaseSolutionModes.ReadOnly;
                    }

                    if (templateField != null && isVisible)
                    {
                        isVisible = templateField.CaseSolutionMode != Common.Enums.Settings.CaseSolutionModes.Hide;
                    }
                }

                ret.Add(new FieldSettingJSModel(field.Name, isVisible, isReadonly, isRequired));
            }

            return ret;
        }

        private Tuple<IList<ProductArea>, IList<ProductAreaChild>> ProductAreaTreeTranslation(IList<ProductArea> productAreas)
        {
            var paChildren = new List<ProductAreaChild>();

            foreach (var pa in productAreas)
            {
                pa.Name = Translation.Get(pa.Name, Enums.TranslationSource.TextTranslation);
                if (pa.SubProductAreas.Where(sp => sp.IsActive != 0 && sp.ShowOnExternalPage != 0).Any())
                {
                    paChildren.Add(new ProductAreaChild(pa.Id, true));
                    var newList = ProductAreaTreeTranslation(pa.SubProductAreas.Where(sp => sp.IsActive != 0 && sp.ShowOnExternalPage != 0).ToList());
                    pa.SubProductAreas = newList.Item1;
                    if (newList.Item2.Any())
                        paChildren.AddRange(newList.Item2);
                }
                else
                    paChildren.Add(new ProductAreaChild(pa.Id, false));
            }

            var item1 = productAreas.OrderBy(p => p.Name).ToList();
            return new Tuple<IList<ProductArea>, IList<ProductAreaChild>>(item1, paChildren);
        }

        private IList<CaseType> CaseTypeTreeTranslation(IList<CaseType> caseTypes)
        {
            foreach (var ct in caseTypes)
            {
                ct.Name = Translation.Get(ct.Name, Enums.TranslationSource.TextTranslation);
                if (ct.SubCaseTypes.Any())
                {
                    ct.SubCaseTypes = CaseTypeTreeTranslation(ct.SubCaseTypes.ToList());
                }
            }

            return caseTypes.OrderBy(p => p.Name).ToList();
        }

        private List<WorkflowStepModel> GetWorkflowStepModel(int customerId, int caseId, int templateId)
        {
            IList<WorkflowStepModel> res = new List<WorkflowStepModel>();
            Case caseEntity;

            if (caseId == 0)
            {
                caseEntity = _caseService.InitCase(
                                customerId,
                                0,
                                SessionFacade.CurrentLanguageId,
                                Request.GetIpAddress(),
                                CaseRegistrationSource.SelfService,
                                null,
                                string.Empty);

                var defaultStateSecondary = _stateSecondaryService.GetDefaultOverview(customerId);
                if (defaultStateSecondary != null)
                {
                    caseEntity.StateSecondary_Id = int.Parse(defaultStateSecondary.Value);
                }
            }
            else
            {
                caseEntity = _caseService.GetCaseById(caseId);
            }

            var workflowCaseSolutions = _caseSolutionService.GetWorkflowCaseSolutionIds(customerId);
            int lang = SessionFacade.CurrentLanguageId;
            if (lang == SessionFacade.CurrentCustomer.Language_Id)
            {
                lang = 0;
            }

            if (caseEntity != null && caseEntity.FinishingDate == null)
            {
                var isRelatedCase = caseId > 0 && _caseService.IsRelated(caseId);

                res = _caseSolutionService.GetWorkflowSteps(
                    customerId,
                    caseEntity,
                    workflowCaseSolutions,
                    isRelatedCase,
                    null,
                    ApplicationType.LineManager, // this is used for purpose since its comapred against ApplicationTypes table values where Selfservice = 2
                    templateId,
                    lang);
            }

            if (res.Any())
            {
                res.Apply(x => x.Name = Translation.Get(x.Name));
            }

            return res.ToList();
        }

        private CaseSearchInputParameters PrepareCaseSearchInputParameters(string progressId = "")
        {
            var pharaseSearch = string.Empty;
            if (string.IsNullOrEmpty(progressId))
            {
                if (SessionFacade.CurrentCaseSearch != null)
                {
                    var lastprogressId = SessionFacade.CurrentCaseSearch.caseSearchFilter?.CaseProgress;
                    progressId = string.IsNullOrEmpty(lastprogressId) ? CaseProgressFilter.CasesInProgress : lastprogressId;
                    pharaseSearch = SessionFacade.CurrentCaseSearch.caseSearchFilter?.FreeTextSearch;
                }
                else
                {
                    progressId = CaseProgressFilter.CasesInProgress;
                }
            }

            return new CaseSearchInputParameters
            {
                CustomerId = SessionFacade.CurrentCustomer.Id,
                LanguageId = SessionFacade.CurrentLanguageId,
                IdentityUser = SessionFacade.CurrentUserIdentity.UserId,
                ProgressId = progressId,
                PharasSearch = pharaseSearch,
                MaxRecords = 20,
                SortBy = "Casenumber",
                SortAscending = false
            };
        }


        //// keep for diagnostic purposes
        //private void LogWithContext(string msg)
        //{
        //    var customerId = SessionFacade.CurrentCustomerID;
        //    var userIdentityEmail = SessionFacade.CurrentUserIdentity?.Email;
        //    var userIdentityEmployeeNumber = SessionFacade.CurrentUserIdentity?.EmployeeNumber;
        //    var userIdentityUserId = SessionFacade.CurrentUserIdentity?.UserId;
        //    var localUserPkId = SessionFacade.CurrentLocalUser?.Id;
        //    var localUserId = SessionFacade.CurrentLocalUser?.UserId;
        //}

        private bool IsTwoAttachmentsModeEnabled(int customerId)
        {
            var fieldName = GlobalEnums.TranslationCaseFields.tblLog_Filename_Internal.ToString().GetCaseFieldName();
            var isChecked = _caseFieldSettingService.GetCaseFieldSetting(customerId, fieldName)?.ShowExternal.ToBool() ?? false;
            return isChecked;
        }

        #endregion
    }
}

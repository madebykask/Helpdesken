using System.Diagnostics;
using System.Threading;
using System.Web.SessionState;
using DH.Helpdesk.BusinessData.Models.Email;
using DH.Helpdesk.BusinessData.Models.WorktimeCalculator;
using DH.Helpdesk.SelfService.Controllers.Behaviors;
using DH.Helpdesk.SelfService.Entites;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using log4net;
using log4net.Core;
using WebGrease.Css.Extensions;

namespace DH.Helpdesk.SelfService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;

    using DH.Helpdesk.BusinessData.Enums.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.SelfService.Infrastructure;
    using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.SelfService.Infrastructure.Extensions;
    using DH.Helpdesk.SelfService.Infrastructure.Tools;
    using DH.Helpdesk.SelfService.Models;
    using DH.Helpdesk.SelfService.Models.Case;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using Models.Shared;
    using BusinessData.Models.ExtendedCase;
    using Services.Services.UniversalCase;
    using Common.Extensions.Integer;
    using Common.Extensions.String;
    using Common.Extensions.DateTime;
    using Infrastructure.Helpers;
    using BusinessData.Models.Shared;
    using Models.Message;

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
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly ICaseSolutionSettingService _caseSolutionSettingService;
        private readonly IEmailService _emailService;        
        private readonly IMasterDataService _masterDataService;
        private readonly ICaseExtraFollowersService _caseExtraFollowersService;
        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;
        private readonly IPriorityService _priorityService;
        private readonly IExtendedCaseService _extendedCaseService;
        private readonly IUniversalCaseService _universalCaseService;
        private readonly IWatchDateCalendarService _watchDateCalendarService;
        private readonly IInventoryService _inventoryService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IStatusService _statusService;


        private const string ParentPathDefaultValue = "--";
        private const string EnterMarkup = "<br />";
        private readonly IOrganizationService _orgService;
        private readonly OrganizationJsonService _orgJsonService;
        private readonly char[] EMAIL_SEPARATOR = new char[] { ';' };
        private readonly CaseControllerBehavior _caseControllerBehavior;
        private const string ShowRegistrationMessageKey = "showRegistrationMessage";

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
            IRegistrationSourceCustomerService registrationSourceCustomerService,
            IPriorityService priorityService,
            IExtendedCaseService extendedCaseService,
            IUniversalCaseService universalCaseService,
            IInventoryService inventoryService,
            ICustomerUserService customerUserService,
            IGlobalSettingService globalSettingService,
            IWatchDateCalendarService watchDateCalendarService,
            ISelfServiceConfigurationService configurationService,
            IStatusService statusService)
            : base(configurationService, masterDataService, caseSolutionService)
        {
            _caseControllerBehavior = new CaseControllerBehavior(masterDataService, caseService, caseSearchService,
                caseSettingService, caseFieldSettingService, 
                productAreaService, configurationService);

            _masterDataService = masterDataService;
            _caseService = caseService;
            _logService = logService;
            _caseFieldSettingService = caseFieldSettingService;
            _infoService = infoService;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.Create("Case");
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
            _caseSettingService = caseSettingService;
            _caseSearchService = caseSearchService;
            _workingGroupService = workingGroupService;
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
            _registrationSourceCustomerService = registrationSourceCustomerService;
            _priorityService = priorityService;
            _extendedCaseService = extendedCaseService;
            _universalCaseService = universalCaseService;
            _watchDateCalendarService = watchDateCalendarService;
            _inventoryService = inventoryService;
            _customerUserService = customerUserService;
            _globalSettingService = globalSettingService;
            _statusService = statusService;
        }

        [HttpGet]
        public ActionResult Index(string id, bool showRegistrationMessage = false)
        {
            if(string.IsNullOrEmpty(id))
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
                    ErrorGenerator.MakeError("Link is not valid!");
                    return RedirectToAction("Index", "Error");
                }

                // open extended case by guid
                var data = _extendedCaseService.GetExtendedCaseFromCase(caseId);
                if (data != null)
                {
                    return RedirectToAction("ExtendedCasePublic", new { id = data.ExtendedCaseGuid });
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
                ErrorGenerator.MakeError("Case not found!");
                return RedirectToAction("Index", "Error");
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
                return RedirectToAction("ExtendedCase", new { caseId = currentCase.Id });

            currentCase.Description = currentCase.Description.Replace("\n", EnterMarkup);
            
            Customer currentCustomer;
            if (SessionFacade.CurrentCustomer != null)
            {
                currentCustomer = SessionFacade.CurrentCustomer;
            }
            else
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return RedirectToAction("Index", "Error");                
            }

            currentCase.Customer = currentCustomer;

            var languageId = SessionFacade.CurrentLanguageId;
            var caseReceipt = GetCaseReceiptModel(currentCase, languageId);

            caseReceipt.ShowRegistringMessage = showRegistrationMessage;
            caseReceipt.ExLogFileGuid = currentCase.CaseGUID.ToString();
            caseReceipt.MailGuid = id;

            _userTemporaryFilesStorage.DeleteFiles(caseReceipt.ExLogFileGuid);
            _userTemporaryFilesStorage.DeleteFiles(id);
                        
            if (id.Is<Guid>())
            {
                if(currentCase.StateSecondary_Id.HasValue && caseReceipt.CasePreview.FinishingDate == null)
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

            var cs = _settingService.GetCustomerSetting(currentCase.Customer.Id);
            ViewBag.AttachmentPlacement = cs.AttachmentPlacement;

            var appSettings = ConfigurationService.AppSettings;
            ViewBag.ShowCommunicationForSelfService = appSettings.ShowCommunicationForSelfService;

            return View(caseReceipt);
        }

        private string GetCaseRegistrationMessage(int languageId)
        {
            var registrationInfoText = _infoService.GetInfoText((int)InfoTextType.SelfServiceRegistrationMessage, SessionFacade.CurrentCustomer.Id, languageId);
            return registrationInfoText?.Name ?? string.Empty;
        }

        [HttpGet]
        public ActionResult NewCase(int customerId, int? caseTemplateId)
        {
            // *** New Case ***
            var currentCustomer = default(Customer);
            if (SessionFacade.CurrentCustomer != null)
                currentCustomer = SessionFacade.CurrentCustomer;
            else
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return RedirectToAction("Index", "Error");
            }
            var languageId = SessionFacade.CurrentLanguageId;

            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(customerId, languageId)
                                                          .Where(c => c.ShowExternal == 1)
                                                          .ToList();

            
            var model = GetNewCaseModel(currentCustomer.Id, languageId, caseFieldSetting);
            model.ExLogFileGuid = Guid.NewGuid().ToString();

            var cs = _settingService.GetCustomerSetting(currentCustomer.Id);
            ViewBag.AttachmentPlacement = cs.AttachmentPlacement;

            var appSettings = ConfigurationService.AppSettings;
            ViewBag.ShowCommunicationForSelfService = appSettings.ShowCommunicationForSelfService;

            if (SessionFacade.CurrentUserIdentity != null)
            {                
                model.NewCase = _caseService.InitCase(
                    currentCustomer.Id,
                    0,
                    SessionFacade.CurrentLanguageId,
                    Request.GetIpAddress(),
                    CaseRegistrationSource.SelfService,
                    cs, SessionFacade.CurrentUserIdentity.UserId);

                model.NewCase.Customer = currentCustomer;
                model.CaseMailSetting = new CaseMailSetting(
                    currentCustomer.NewCaseEmailList,
                    currentCustomer.HelpdeskEmail,
                    appSettings.HelpdeskPath,
                    cs.DontConnectUserToWorkingGroup);

                model.NewCase.RegUserId = SessionFacade.CurrentUserIdentity.UserId;
                model.NewCase.RegUserDomain = SessionFacade.CurrentUserIdentity.Domain;
            }

            model.CaseTypeParantPath = ParentPathDefaultValue;
            model.ProductAreaParantPath = ParentPathDefaultValue;
            model.CategoryParentPath = ParentPathDefaultValue;

            // Load template info
            if(caseTemplateId != null && caseTemplateId.Value > 0)
            {
                var caseTemplate = _caseSolutionService.GetCaseSolution(caseTemplateId.Value);
                var caseTemplateSettings = _caseSolutionSettingService.GetCaseSolutionSettingOverviews(caseTemplateId.Value).ToList();

                var jsFieldSettings = GetFieldSettingsModel(caseFieldSetting, caseTemplateSettings);
                model.JsFieldSettings = jsFieldSettings;

                if (caseTemplate.Status == 0 || !caseTemplate.ShowInSelfService)
                {
                    ErrorGenerator.MakeError("Selected template is not available anymore!");
                    return RedirectToAction("Index", "Error");
                }
                if(caseTemplate.CaseType_Id != null)
                {
                    model.NewCase.CaseType_Id = caseTemplate.CaseType_Id.Value;
                }

                var notifier = _computerService.GetInitiatorByUserId(SessionFacade.CurrentUserIdentity.UserId, customerId);

                model.NewCase.ReportedBy = string.IsNullOrEmpty(caseTemplate.ReportedBy)? notifier?.UserId : caseTemplate.ReportedBy;
                model.NewCase.PersonsName = string.IsNullOrEmpty(caseTemplate.PersonsName)
                    ? string.Format("{0} {1}", notifier?.FirstName, notifier?.LastName)
                    : caseTemplate.PersonsName;

                model.NewCase.PersonsEmail = string.IsNullOrEmpty(caseTemplate.PersonsEmail) ? notifier?.Email: caseTemplate.PersonsEmail;
                model.NewCase.PersonsPhone = string.IsNullOrEmpty(caseTemplate.PersonsPhone) ? notifier?.Phone : caseTemplate.PersonsPhone;
                model.NewCase.PersonsCellphone = string.IsNullOrEmpty(caseTemplate.PersonsCellPhone) ? notifier?.CellPhone : caseTemplate.PersonsCellPhone;
                model.NewCase.Region_Id = caseTemplate.Region_Id;
                model.NewCase.Department_Id = caseTemplate.Department_Id.HasValue ? caseTemplate.Department_Id.Value : notifier?.DepartmentId;
                model.NewCase.OU_Id = caseTemplate.OU_Id.HasValue ? caseTemplate.OU_Id.Value : notifier?.OrganizationUnitId;
                model.NewCase.Place = string.IsNullOrEmpty(caseTemplate.Place) ? notifier?.Place : caseTemplate.Place;
                model.NewCase.UserCode = string.IsNullOrEmpty(caseTemplate.UserCode) ? notifier?.Code : caseTemplate.UserCode;
                model.NewCase.CostCentre = string.IsNullOrEmpty(caseTemplate.CostCentre) ? notifier?.CostCentre : caseTemplate.CostCentre;

                var inventoryNumber = caseTemplate.InventoryNumber;
                var inventoryType = caseTemplate.InventoryType;
                var inventoryLocation = caseTemplate.InventoryLocation;
                if (currentCustomer.FetchPcNumber)
                {
                    var pcNumber = Request.GetComputerName();
                    if (!string.IsNullOrEmpty(pcNumber))
                    {
                        var inventory = _inventoryService.GetWorkstationByNumber(pcNumber, currentCustomer.Id);
                        inventoryNumber = string.IsNullOrEmpty(inventoryNumber) ? pcNumber : inventoryNumber;
                        if (inventory != null)
                        {
                            inventoryType = string.IsNullOrEmpty(inventoryType) ? inventory.WorkstationFields.ComputerTypeName : inventoryType;
                            inventoryLocation = string.IsNullOrEmpty(inventoryLocation) ? inventory.WorkstationFields.Location : inventoryLocation;
                        }
                    }
                }
                model.NewCase.InventoryNumber = inventoryNumber;
                model.NewCase.InventoryType = inventoryType;
                model.NewCase.InventoryLocation = inventoryLocation;

                model.NewCase.ProductArea_Id = caseTemplate.ProductArea_Id;
                model.NewCase.System_Id = caseTemplate.System_Id;
                model.NewCase.Caption = caseTemplate.Caption;
                model.NewCase.Description = caseTemplate.Description;
                model.NewCase.Priority_Id = caseTemplate.Priority_Id;
                model.NewCase.Project_Id = caseTemplate.Project_Id;
                model.NewCase.Urgency_Id = caseTemplate.Urgency_Id;
                model.NewCase.Impact_Id = caseTemplate.Impact_Id;
                model.NewCase.Category_Id = caseTemplate.Category_Id;
                model.NewCase.Supplier_Id = caseTemplate.Supplier_Id;

                model.NewCase.InvoiceNumber = caseTemplate.InvoiceNumber;
                model.NewCase.ReferenceNumber = caseTemplate.ReferenceNumber;
                model.NewCase.Miscellaneous = caseTemplate.Miscellaneous;
                model.NewCase.ContactBeforeAction = caseTemplate.ContactBeforeAction;
                model.NewCase.SMS = caseTemplate.SMS;
                model.NewCase.AgreedDate = caseTemplate.AgreedDate;
                model.NewCase.Available = caseTemplate.Available;
                model.NewCase.Cost = caseTemplate.Cost;
                model.NewCase.OtherCost = caseTemplate.OtherCost;
                model.NewCase.Currency = caseTemplate.Currency;
                model.NewCase.Change_Id = caseTemplate.Change_Id;
                model.NewCase.FinishingDescription = caseTemplate.FinishingDescription;

                //Hidden fields
                model.NewCase.CausingPartId = caseTemplate.CausingPartId;
                model.NewCase.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id;
                model.NewCase.Project_Id = caseTemplate.Project_Id;
                model.NewCase.Problem_Id = caseTemplate.Problem_Id;
                model.NewCase.PlanDate = caseTemplate.PlanDate;
                model.NewCase.WatchDate = caseTemplate.WatchDate;

                var defaultAdmin = cs.DefaultAdministratorExternal.HasValue ? cs.DefaultAdministratorExternal : caseTemplate.PerformerUser_Id;
                model.NewCase.Performer_User_Id = caseTemplate.PerformerUser_Id.HasValue ? caseTemplate.PerformerUser_Id : defaultAdmin;

                if (model.NewCase.IsAbout == null)
                    model.NewCase.IsAbout = new CaseIsAboutEntity();

                model.NewCase.IsAbout.Id = 0;
                model.NewCase.IsAbout.ReportedBy = caseTemplate.IsAbout_ReportedBy;
                model.NewCase.IsAbout.Person_Name = caseTemplate.IsAbout_PersonsName;
                model.NewCase.IsAbout.Person_Email = caseTemplate.IsAbout_PersonsEmail;
                model.NewCase.IsAbout.Person_Phone = caseTemplate.IsAbout_PersonsPhone;
                model.NewCase.IsAbout.Person_Cellphone = caseTemplate.IsAbout_PersonsCellPhone;
                model.NewCase.IsAbout.Region_Id = caseTemplate.IsAbout_Region_Id;
                model.NewCase.IsAbout.Department_Id = caseTemplate.IsAbout_Department_Id;
                model.NewCase.IsAbout.OU_Id = caseTemplate.IsAbout_OU_Id;
                model.NewCase.IsAbout.CostCentre = caseTemplate.IsAbout_CostCentre;
                model.NewCase.IsAbout.Place = caseTemplate.IsAbout_Place;
                model.NewCase.IsAbout.UserCode = caseTemplate.UserCode;

                model.NewCase.Status_Id = caseTemplate.Status_Id;
                model.NewCase.StateSecondary_Id = caseTemplate.StateSecondary_Id;
                model.NewCase.Verified = caseTemplate.Verified;
                model.NewCase.VerifiedDescription = caseTemplate.VerifiedDescription;
                model.NewCase.SolutionRate = caseTemplate.SolutionRate;

                model.Information = caseTemplate.Information;

                if (!string.IsNullOrEmpty(caseTemplate.Text_External) ||
                    !string.IsNullOrEmpty(caseTemplate.Text_Internal) || caseTemplate.FinishingCause_Id.HasValue)
                {
                    model.CaseLog = new CaseLog
                    {
                        LogType = 0,
                        LogGuid = Guid.NewGuid(),
                        TextExternal = caseTemplate.Text_External,
                        TextInternal = caseTemplate.Text_Internal,
                        FinishingType = caseTemplate.FinishingCause_Id
                    };
                }

                if (caseTemplate.RegistrationSource.HasValue)
                {
                    model.NewCase.RegistrationSourceCustomer_Id = caseTemplate.RegistrationSource.Value;
                }
                else
                {
                    var registrationSource = _registrationSourceCustomerService.GetCustomersActiveRegistrationSources(customerId)
                            .FirstOrDefault(x => x.SystemCode == (int)CaseRegistrationSource.SelfService);
                    if (registrationSource != null)
                        model.NewCase.RegistrationSourceCustomer_Id = registrationSource.Id;
                }

                if(model.NewCase.ProductArea_Id.HasValue)
                {
                    var p = _productAreaService.GetProductArea(model.NewCase.ProductArea_Id.GetValueOrDefault());
                    if(p != null)
                    {
                        var pathTexts = _productAreaService.GetParentPath(p.Id, currentCustomer.Id).ToList();
                        var translatedText = pathTexts;
                        if (pathTexts.Any())
                        {
                            translatedText = new List<string>();
                            foreach (var pathText in pathTexts.ToList())
                                translatedText.Add(Translation.Get(pathText, Enums.TranslationSource.TextTranslation));
                        }
                        model.ProductAreaParantPath = string.Join(" - ", translatedText);
                    }
                }

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
                                translatedText.Add(Translation.Get(pathText));
                        }
                        model.CategoryParentPath = string.Join(" - ", translatedText);
                    }
                }

                if (model.NewCase.CaseType_Id > 0)
                {
                    var ct = _caseTypeService.GetCaseType(model.NewCase.CaseType_Id);
                    var tempCTs = new List<CaseType>();
                    tempCTs.Add(ct);
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
                            model.NewCase.Region_Id = reg.Region_Id;
                            if (model.NewCase.Region_Id.HasValue)
                                model.Departments = model.Departments.Where(d => d.Region_Id.HasValue && d.Region_Id == model.NewCase.Region_Id.Value).ToList();
                        }
                            
                        var ous = _orgService.GetOUs(model.NewCase.Department_Id);
                        model.OrganizationUnits = ous;
                    }
                    else
                    {
                        model.Departments = model.Departments.Where(d => d.Region_Id.HasValue && d.Region_Id == model.NewCase.Region_Id.Value).ToList();
                        if (model.Departments.Select(d=> d.Id).Contains(model.NewCase.Department_Id.Value))
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

            } // Load Case Template

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

            var caseId = _extendedCaseService.GetCaseIdByExtendedCaseGuid(id);
            if (caseId <= 0)
            {
                ErrorGenerator.MakeError("Extended case data not found!", 210);
                return RedirectToAction("Index", "Error");
            }

            var model = GetExtendedCaseViewModel(null, caseId);
            if (ErrorGenerator.HasError())
                return RedirectToAction("Index", "Error");

            //return url after save
            ViewBag.ReturnUrl = Url.Action("ExtendedCasePublic", new { id });

            return View("ExtendedCase", model);
        }

        [HttpGet]
        public ActionResult ExtendedCase(int? caseTemplateId = null, int? caseId = null)
        {
            var model = GetExtendedCaseViewModel(caseTemplateId, caseId);
            if (ErrorGenerator.HasError())
                return RedirectToAction("Index", "Error");

            if (!caseId.IsNew())
            {
                var showRegistrationMessage = TempData.GetSafe<bool>(ShowRegistrationMessageKey);
                if (showRegistrationMessage)
                {
                    model.ShowRegistrationMessage = true;
                    model.CaseRegistrationMessage = GetCaseRegistrationMessage(SessionFacade.CurrentLanguageId);
                }
            }

            return View("ExtendedCase", model);
        }

        private ExtendedCaseViewModel GetExtendedCaseViewModel(int? caseTemplateId = null, int? caseId = null)
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
                RegUserName = string.Format("{0} {1}", SessionFacade.CurrentUserIdentity.FirstName, SessionFacade.CurrentUserIdentity.LastName),
                IpAddress = Request.GetIpAddress(),
                CaseSolution_Id = caseTemplateId,
                CaseFileKey = Guid.NewGuid().ToString()
            };

            CaseSolution caseTemplate = null;
            if (caseTemplateId.HasValue)
                caseTemplate = _caseSolutionService.GetCaseSolution(caseTemplateId.Value);

            if (caseId.HasValue)
                caseModel = _universalCaseService.GetCase(caseId.Value);

            if (caseModel == null && caseTemplate == null)
            {
                ErrorGenerator.MakeError("Template or Case must be specified!", 211);
                return null;
            }

            var isAnonymousMode = ConfigurationService.AppSettings.LoginMode == LoginMode.Anonymous;

            if (!isAnonymousMode && caseId.HasValue && !UserHasAccessToCase(caseModel))
            {
                ErrorGenerator.MakeError("Case not found among your cases!");
                return null;
            }

            if (SessionFacade.CurrentCustomer == null)
            {
                var cusId = caseModel != null && caseModel.Customer_Id > 0
                    ? caseModel.Customer_Id
                    : (caseTemplate?.Customer_Id ?? 0);

                SessionFacade.CurrentCustomer = _customerService.GetCustomer(cusId);
            }

            if (SessionFacade.CurrentCustomer == null)
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return null;
            }
        
            var languageId = SessionFacade.CurrentLanguageId;
            var customerId = SessionFacade.CurrentCustomer.Id;
            var appSettings = ConfigurationService.AppSettings;
            var globalSettings = _globalSettingService.GetGlobalSettings().First();
            var cs = _settingService.GetCustomerSetting(customerId);
            
            ViewBag.AttachmentPlacement = cs.AttachmentPlacement;
            ViewBag.ShowCommunicationForSelfservice = appSettings.ShowCommunicationForSelfService;
            
            caseModel.FieldSettings = _caseFieldSettingService.ListToShowOnCasePage(customerId, languageId)
                .Where(c => c.ShowExternal == 1)
                .ToList();

            if (caseId.IsNew())
            {
                if (caseTemplate == null || caseTemplate.Status == 0 ||
                    !caseTemplate.ShowInSelfService || caseTemplate.Customer_Id != customerId)
                {
                    ErrorGenerator.MakeError("Selected template is not available anymore!");
                    return null;
                }
            }

            // check only if multi customer is not enabled. Allow user to see own cases for different customers.
            if (globalSettings.MultiCustomersSearch == 0 &&
                caseId.IsNew() == false && caseModel.Customer_Id != customerId)
            {
                ErrorGenerator.MakeError("Selected Case doesn't belong to current customer!");
                return null;
            }

            if (caseId == null || caseId == 0)
            {
                caseModel.Customer_Id = customerId;
                caseModel = LoadTemplateToCase(caseModel, caseTemplate);
            }

            /*Get StateSecondaryId if existing*/
            int caseStateSecondaryId = 0;
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
            var isRelatedCase = caseId > 0 && _caseService.IsRelated(caseId ?? 0);
            var customerCaseSolutions =
                _caseSolutionService.GetCustomerCaseSolutionsOverview(customerId, userId: null);
            
            var model = new ExtendedCaseViewModel
            {
                CaseId = initData.CaseId,
                CaseTemplateId = initData.CaseSolutionId,
                CustomerId = initData.CustomerId,
                LanguageId = initData.LanguageId,
                CustomerSettings = cs,
                ExtendedCaseDataModel = extendedCaseDataModel,
                CurrentUser = SessionFacade.CurrentUserIdentity.EmployeeNumber,
                UserRole = initData.UserRole,
                StateSecondaryId = caseStateSecondaryId,
                CaseOU = caseModel.OU_Id.HasValue ? _ouService.GetOU(caseModel.OU_Id.Value) : null,
                WorkflowSteps = GetWorkflowStepModel(customerId, caseId ?? 0, caseTemplateId ?? 0, customerCaseSolutions, isRelatedCase),
                CaseDataModel = caseModel,
                LogFileGuid = Guid.NewGuid().ToString(),
                CaseLogs = caseId.HasValue ? _logService.GetLogsByCaseId(caseId.Value).OrderByDescending(l => l.LogDate).ToList() : new List<Log>()
            };

            if (string.IsNullOrEmpty(model.ExtendedCaseDataModel.FormModel.Name))
            {
                if (caseTemplate == null)
                    caseTemplate = _caseSolutionService.GetCaseSolution(caseModel.CaseSolution_Id ?? (caseTemplateId.HasValue ? caseTemplateId.Value : 0));

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
                _caseFileService.AddFiles(newCaseFiles);

                // delete temp folders                
                _userTemporaryFilesStorage.DeleteFiles(caseFileKey);
            }
        }

        [HttpPost]
        public ActionResult ExtendedCase(ExtendedCaseViewModel model, string returnUrl = null)
        {
            var isNewCase = model.CaseDataModel.Id == 0;

            var localUserId = SessionFacade.CurrentLocalUser?.Id ?? 0;
            var auxModel = new AuxCaseModel(model.LanguageId, 
                                            localUserId, 
                                            SessionFacade.CurrentUserIdentity.UserId,
                                            RequestExtension.GetAbsoluteUrl(),
                                            CreatedByApplications.ExtendedCase,
                                            TimeZoneInfo.Local);

            //LogWithContext($"ExtendedCase.Post: Saving extended case data. LocalUserId: {auxModel.CurrentUserId}, url: {auxModel.AbsolutreUrl}.");

            if (model.SelectedWorkflowStep.HasValue && model.SelectedWorkflowStep.Value > 0)            
                model.CaseDataModel = ApplyNextWorkflowStepOnCase(model.CaseDataModel, model.SelectedWorkflowStep.Value);
            
            var caseId = -1;
            decimal caseNum;
            
            //TODO: Refactor
            model.CaseDataModel.ExtendedCaseData_Id = model.ExtendedCaseDataModel.Id;
            model.CaseDataModel.ExtendedCaseForm_Id = model.ExtendedCaseDataModel.ExtendedCaseFormId;
            
            var res = _universalCaseService.SaveCaseCheckSplit(model.CaseDataModel, auxModel, out caseId, out caseNum);
            if (res.IsSucceed && caseId > 0)
            {
                var case_ = _universalCaseService.GetCase(caseId);

                SaveCaseFiles(model.CaseDataModel.CaseFileKey, case_.Customer_Id, caseId, localUserId);

                if (ConfigurationService.AppSettings.ShowConfirmAfterCaseRegistration)
                {
                    return RedirectToCaseConfirmation("Your case has been successfully registered.", string.Empty);
                }
                else
                {
                    TempData[ShowRegistrationMessageKey] = isNewCase;

                    //return RedirectToAction("UserCases", new { customerId = model.CustomerId });
                    if (string.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("ExtendedCase", new { caseId = caseId });
                    else
                        return Redirect(returnUrl);
                }
            }

            if (model.CaseDataModel.OU_Id.HasValue)            
                model.CaseOU = _ouService.GetOU(model.CaseDataModel.OU_Id.Value);

            model.Result = res;
            model.StatusBar = isNewCase ? new Dictionary<string, string>() : GetStatusBar(model);

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
            var res = _caseControllerBehavior.ValidateCustomer();
            if (!res.Valid) return HandleError(res);

            res = _caseControllerBehavior.ValidateCurrentUserIdentity();
            if (!res.Valid) return HandleError(res);

            var searchParameters = PrepareCaseSearchInputParameters();

            res = _caseControllerBehavior.ValidateSearchParameters(searchParameters);
            if (!res.Valid) return HandleError(res);

            var model = new UserCasesModel
            {
                CustomerId  = customerId,

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

            var customers = _customerUserService.ListCustomersByInitiatorCases(userId);

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

        [HttpGet]
        public FileContentResult DownloadFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
               fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, id, "");
            else
            {
                var c = _caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                fileContent = _caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }

            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public FileContentResult DownloadNewCaseFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
                fileContent =  _userTemporaryFilesStorage.GetFileContent(fileName, id, "");
            else
            {
                var c = _caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                fileContent = _caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }
            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpPost]
        public void UploadFile(string id, string name)
        {
            var uploadedFile = Request.Files[0];
            if(uploadedFile != null)
            {
                var uploadedData = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

                if(GuidHelper.IsGuid(id))
                {
                    if(_userTemporaryFilesStorage.FileExists(name, id))
                    {
                        throw new HttpException((int)HttpStatusCode.Conflict, null);
                    }
                    else
                    {
                        _userTemporaryFilesStorage.AddFile(uploadedData, name, id);
                    }
                }
            }
        }

        [HttpPost]
        public void NewCaseUploadFile(string id, string name)
        {
            var uploadedFile = Request.Files[0];
            if(uploadedFile != null)
            {
                var uploadedData = new byte[uploadedFile.InputStream.Length];
                uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

                if(GuidHelper.IsGuid(id))
                {
                    if(_userTemporaryFilesStorage.FileExists(name, id))
                    {
                        throw new HttpException((int)HttpStatusCode.Conflict, null);
                    }
                    else
                    {
                        _userTemporaryFilesStorage.AddFile(uploadedData, name, id);
                    }
                }
            }
        }

        [HttpPost]
        public void DeleteFile(string id, string fileName)
        {
            if(GuidHelper.IsGuid(id))
            {
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id);
            }
            else
            {
                var log = _logService.GetLogById(int.Parse(id));
                Case c = null;
                var basePath = string.Empty;
                if (log != null)
                {
                    c = _caseService.GetCaseById(log.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.Customer_Id);
                }

                _logFileService.DeleteByLogIdAndFileName(int.Parse(id), basePath, fileName.Trim());
            }
        }

        [HttpPost]
        public void DeleteNewCaseFile(string id, string fileName)
        {
            if(GuidHelper.IsGuid(id))
            {
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id);
            }
            else
            {
                var c = _caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                _caseFileService.DeleteByCaseIdAndFileName(int.Parse(id), basePath, fileName.Trim());
            }
        }

        [HttpGet]
        public JsonResult Files(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                                ? _userTemporaryFilesStorage.GetFileNames(id)
                                : _logFileService.FindFileNamesByLogId(int.Parse(id));

            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult NewCaseFiles(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                                ? _userTemporaryFilesStorage.GetFileNames(id)
                                : _caseFileService.FindFileNamesByCaseId(int.Parse(id));

            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult DownloadLogFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
                fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Log);
            else
            {
                var log = _logService.GetLogById(int.Parse(id));
                Case c = null;
                var basePath = string.Empty;
                if (log != null)
                {
                    c = _caseService.GetCaseById(log.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.Customer_Id);
                } 

                fileContent = _logFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }

            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public ActionResult _CaseLogNote(int caseId, string note, string logFileGuid)
        {
            SaveExternalMessage(caseId, note, logFileGuid);            
            CaseLogModel model = new CaseLogModel()
                  {
                      CaseId = caseId,
                      CaseLogs = _logService.GetLogsByCaseId(caseId).OrderByDescending(l=> l.RegTime).ToList()                      
                  };            
            return PartialView(model);
        }

        private void SaveExternalMessage(int caseId, string extraNote, string logFileGuid) 
        { 
            IDictionary<string, string> errors;            
            var currentCase = _caseService.GetCaseById(caseId);
            var currentCustomer = _customerService.GetCustomer(currentCase.Customer_Id);
            var cs = _settingService.GetCustomerSetting(currentCustomer.Id);
            var caseIsActivated = false;

            var appSettings = ConfigurationService.AppSettings;

            // save case history

            // unread/status flag update if not case is closed
            if (!currentCase.FinishingDate.HasValue)
                currentCase.Unread = 1;

            if (currentCase.FinishingDate.HasValue)
            {
                string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                this._caseService.Activate(currentCase.Id, 0, adUser, CreatedByApplications.SelfService5, out errors);
                caseIsActivated = true;
            }
                

            // if statesecondary has ResetOnExternalUpdate
            if (currentCase.StateSecondary_Id.HasValue)
            {
                //get substatus
                var casestatesecundary = _stateSecondaryService.GetStateSecondary(currentCase.StateSecondary_Id.Value);

                if (casestatesecundary.ResetOnExternalUpdate == 1)
                    currentCase.StateSecondary_Id = null;
            }


            currentCase.ChangeTime = DateTime.UtcNow;
            int caseHistoryId = _caseService.SaveCaseHistory(currentCase, 0, currentCase.PersonsEmail, CreatedByApplications.SelfService5,  out errors, SessionFacade.CurrentUserIdentity.UserId);            
            // save log
            var caseLog = new CaseLog
                              {
                                  CaseHistoryId = caseHistoryId,
                                  CaseId = caseId,
                                  LogGuid = Guid.NewGuid(),
                                  TextExternal = (currentCustomer.UseInternalLogNoteOnExternalPage == (int)Enums.LogNote.UseExternalLogNote? extraNote : string.Empty),
                                  UserId = null,
                                  TextInternal = (currentCustomer.UseInternalLogNoteOnExternalPage == (int)Enums.LogNote.UseInternalLogNote? extraNote : string.Empty),
                                  WorkingTime = 0,
                                  EquipmentPrice = 0,
                                  Price = 0,
                                  Charge = false,
                                  RegUser = SessionFacade.CurrentSystemUser,
                                  SendMailAboutCaseToNotifier = true,
                                  SendMailAboutLog = true                                 
                              };
            
            if(currentCase.WorkingGroup_Id != null)
            {
                var curWorkingGroup = _workingGroupService.GetWorkingGroup(currentCase.WorkingGroup_Id.Value);
                if(curWorkingGroup.AllocateCaseMail == 1) // Send To Users Working Group EMail
                {
                    // Send Mail to all working group users
                    var usersInWorkingGroup = _userService.GetUsersForWorkingGroup(currentCase.WorkingGroup_Id.Value).Where(u => u.Email.Trim() != string.Empty).ToList();
                    var emailTo = new List<string>();                    
                    if(usersInWorkingGroup != null && usersInWorkingGroup.Count > 0)
                    {
                        if(currentCase.Department_Id.HasValue)
                        {
                            foreach(var user in usersInWorkingGroup)
                            {
                                var departments = _departmentService.GetDepartmentsByUserPermissions(user.Id, currentCase.Customer_Id);

                                if(departments != null && departments.FirstOrDefault(x=>x.Id == currentCase.Department_Id.Value) != null)
                                {
                                    emailTo.Add(user.Email);
                                }

                                if(departments == null)
                                    emailTo.Add(user.Email);
                            }
                        }
                        else
                        {                            
                            emailTo = usersInWorkingGroup.Select(u => u.Email).ToList();
                        }
                    }                    
                    if(emailTo.Count > 0)
                        caseLog.EmailRecepientsExternalLog = string.Join(Environment.NewLine, emailTo);
                }
            }

            var caseMailSetting = new CaseMailSetting(currentCustomer.NewCaseEmailList,
                currentCustomer.HelpdeskEmail,
                appSettings.HelpdeskPath,
                cs.DontConnectUserToWorkingGroup
            );

            var temporaryLogFiles = new List<WebTemporaryFile>();
            var userTimeZone = TimeZoneInfo.Local;
            if (!string.IsNullOrEmpty(logFileGuid))
            {
                temporaryLogFiles = _userTemporaryFilesStorage.GetFiles(logFileGuid, ModuleName.Log);
                caseLog.Id = _logService.SaveLog(caseLog, temporaryLogFiles.Count, out errors);
                var basePath = _masterDataService.GetFilePath(currentCase.Customer_Id);
                // save log files
                var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id)).ToList();
                _logFileService.AddFiles(newLogFiles);
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

        [HttpPost]
        public ActionResult NewCase(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey, string followerUsers, CaseLog caseLog)
        {
            decimal caseNum;
            int caseId = Save(newCase, caseMailSetting, caseFileKey, followerUsers, caseLog, out caseNum);

            if (ConfigurationService.AppSettings.ShowConfirmAfterCaseRegistration)
            {
                return RedirectToCaseConfirmation("Your case has been successfully registered.", $"You can follow up your case status via this number: {caseNum}");
            }
            else
            {
                return RedirectToAction("Index", "case", new { id = newCase.Id, showRegistrationMessage = true });
            }
        }

        private ActionResult RedirectToCaseConfirmation(string title, string details)
        {
            SessionFacade.LastMessageDialog = MessageDialogModel.Success(title, details);
            return RedirectToAction("Index", "Message");
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
            var result = _computerService.SearchComputerUsers(customerId, query);
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
            var models = _caseSearchService.GetUserEmailsForCaseSend(SessionFacade.CurrentCustomer.Id, query, searchScope);
            return Json(new { searchKey = searchKey, result = models });
        }

        public List<CaseSolution> GetCaseSolutions(int customerId)
        {
            return _caseSolutionService.GetCaseSolutions(customerId).Where(t => t.ShowInSelfService).ToList();
        }   

        [HttpPost]
        public void UploadLogFile(string id, string name)
        {
            var uploadedFile = Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                if (_userTemporaryFilesStorage.FileExists(name, id, ModuleName.Log))
                {
                    //return;
                    //this.userTemporaryFilesStorage.DeleteFile(name, id, ModuleName.Log); 
                    //throw new HttpException((int)HttpStatusCode.Conflict, null); because it take a long time.
                }
                _userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Log);
            }
        }

        [HttpGet]
        public JsonResult GetLogFiles(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                                ? _userTemporaryFilesStorage.GetFileNames(id, ModuleName.Log)
                                : _logFileService.FindFileNamesByLogId(int.Parse(id));

            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteLogFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Log);            
        }

        public ViewResult AddCommentPopup(int casePreviewId)
        {
            return View("_AddCommentPopup");
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Communication(int caseId)
        {
            var model = GetCaseLogModel(caseId);
            return PartialView("_Communication", model);
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult CaseLogNote(int caseId)
        {
            var model = GetCaseLogModel(caseId);
            return PartialView("_CaseLogNote", model);
        }

        private CaseLogModel GetCaseLogModel(int? caseId)
        {
            var caseLogs = new List<Log>();
            if (caseId > 0)
            {
                caseLogs = _logService.GetLogsByCaseId(caseId.Value).OrderByDescending(l => l.RegTime).ToList();
            }

            var model = new CaseLogModel
            {
                CaseId = caseId ?? 0,
                CaseLogs = caseLogs
            };
            return model;
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

        private void TranslateProductArea(ICollection<ProductArea> pa)
        {
            pa.ForEach(p =>
            {
                p.Name = Translation.Get(p.Name);
                if(p.SubProductAreas != null && p.SubProductAreas.Any())
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
            var userEmployeeNumber = SessionFacade.CurrentUserIdentity.EmployeeNumber;

            //Hide this to next release #57742
            if (currentCase.CaseType.ShowOnExtPageCases == 0 || currentCase.ProductArea?.ShowOnExtPageCases == 0)
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

            /*User Group*/
            if (criteria.MyCasesUserGroup)
            {
                if (criteria.GroupMember != null && criteria.GroupMember.Any())
                {
                    if ((string.IsNullOrEmpty(currentCase.ReportedBy) && !(currentCase.RegUserId == null) &&
                        currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase)) ||
                        criteria.GroupMember.Where(m => m.Equals(currentCase.ReportedBy, StringComparison.CurrentCultureIgnoreCase)).Any())
                        return true;
                }
                else
                {
                    if (string.IsNullOrEmpty(currentCase.ReportedBy) && !(currentCase.RegUserId == null) &&
                        currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        private bool UserHasAccessToCase(CaseModel currentCase)
        {
            var curUser = SessionFacade.CurrentUserIdentity.UserId;
            var userEmployeeNumber = SessionFacade.CurrentUserIdentity.EmployeeNumber;

            //Hide this to next release #57742
            var caseType = _caseTypeService.GetCaseType(currentCase.CaseType_Id);
            var productArea = currentCase.ProductArea_Id.HasValue? _productAreaService.GetProductArea(currentCase.ProductArea_Id.Value) : null;

            if (caseType.ShowOnExtPageCases == 0 || productArea?.ShowOnExtPageCases == 0)
                return false;

            var criteria = _caseControllerBehavior.GetCaseOverviewCriteria();

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

            /*User Group*/
            if (criteria.MyCasesUserGroup)
            {
                if (criteria.GroupMember != null && criteria.GroupMember.Any())
                {
                    if ((string.IsNullOrEmpty(currentCase.ReportedBy) && !(currentCase.RegUserId == null) &&
                        currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase)) ||
                        criteria.GroupMember.Where(m => m.Equals(currentCase.ReportedBy, StringComparison.CurrentCultureIgnoreCase)).Any())
                        return true;
                }
                else
                {
                    if (string.IsNullOrEmpty(currentCase.ReportedBy) && !(currentCase.RegUserId == null) &&
                        currentCase.RegUserId.Equals(criteria.UserId, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }
            }

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

        private int Save(Case newCase, CaseMailSetting caseMailSetting, string caseFileKey, string followerUsers, CaseLog caseLog, out decimal caseNumber)
        {
            IDictionary<string, string> errors;
            caseNumber = 0;
            // save case and case history
            if (newCase.User_Id <= 0)
                newCase.User_Id = null;

            var mailSenders = new MailSenders();
            mailSenders.SystemEmail = caseMailSetting.HelpdeskMailFromAdress;
            if (newCase.WorkingGroup_Id.HasValue)
            {
                var curWG = _workingGroupService.GetWorkingGroup(newCase.WorkingGroup_Id.Value);
                if (curWG != null)
                    if (!string.IsNullOrWhiteSpace(curWG.EMail) && _emailService.IsValidEmail(curWG.EMail))
                        mailSenders.WGEmail = curWG.EMail;
            }

            caseMailSetting.CustomeMailFromAddress = mailSenders;

            var basePath = _masterDataService.GetFilePath(newCase.Customer_Id);
            newCase.LatestSLACountDate = CalculateLatestSLACountDate(newCase.StateSecondary_Id);
            var ei = new CaseExtraInfo() { CreatedByApp = CreatedByApplications.SelfService5, LeadTimeForNow = 0, ActionLeadTime = 0, ActionExternalTime = 0};

            if (newCase.Urgency_Id.HasValue && newCase.Impact_Id.HasValue)
            {
                var prio_Impact_Urgent = _urgencyService.GetPriorityImpactUrgencies(newCase.Customer_Id);
                var prioInfo = prio_Impact_Urgent.FirstOrDefault(p=> p.Impact_Id == newCase.Impact_Id && p.Urgency_Id == newCase.Urgency_Id);
                if (prioInfo != null)
                {
                    newCase.Priority_Id = prioInfo.Priority_Id;
                }
            }

            // All values should be taken from the template, no rules 2017-09-29, only if template workinggroup is null this rule will happend (Höganäs)(2017-11-01)
            if (newCase.ProductArea_Id.HasValue)
            {
                var productArea = _productAreaService.GetProductArea(newCase.ProductArea_Id.Value);

                if (!newCase.WorkingGroup_Id.HasValue)
                {
                    if (productArea != null && productArea.WorkingGroup_Id.HasValue)
                    {
                        newCase.WorkingGroup_Id = productArea.WorkingGroup_Id;
                    }
                }
            }

            if (newCase.Department_Id.HasValue && newCase.Priority_Id.HasValue)
            {
                var dept = this._departmentService.GetDepartment(newCase.Department_Id.Value);
                var priority = this._priorityService.GetPriorities(newCase.Customer_Id).Where(it => it.Id == newCase.Priority_Id && it.IsActive == 1).FirstOrDefault();

                if (dept != null && dept.WatchDateCalendar_Id.HasValue && priority != null && priority.SolutionTime == 0)
                {
                    newCase.WatchDate =
                    this._watchDateCalendarService.GetClosestDateTo(
                    dept.WatchDateCalendar_Id.Value,
                    DateTime.UtcNow);
                }
            }

            var localUserId = SessionFacade.CurrentLocalUser?.Id ?? 0; 
            int caseHistoryId = _caseService.SaveCase(newCase, caseLog, localUserId, SessionFacade.CurrentUserIdentity.UserId, ei, out errors);

            // save log
            caseLog.CaseId = newCase.Id;
            caseLog.CaseHistoryId = caseHistoryId;

            if (caseLog.UserId <= 0 && localUserId > 0)
                caseLog.UserId = localUserId;

            if (string.IsNullOrWhiteSpace(caseLog.RegUser))
                caseLog.RegUser = SessionFacade.CurrentUserIdentity?.UserId ?? string.Empty;

            caseLog.Id = this._logService.SaveLog(caseLog, 0, out errors);

            // save case files
            SaveCaseFiles(caseFileKey, newCase.Customer_Id, newCase.Id, localUserId);

            var oldCase = new Case();            
            // send emails
            var userTimeZone = TimeZoneInfo.Local;

            //save extra followers            
            if (!string.IsNullOrEmpty(followerUsers))
            {
                var _followerUsers = followerUsers.Split(EMAIL_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                _caseExtraFollowersService.SaveExtraFollowers(newCase.Id, _followerUsers, null);
            }

            _caseService.SendCaseEmail(newCase.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase);

            caseNumber = newCase.CaseNumber;
            return newCase.Id;
        }

        private DateTime? CalculateLatestSLACountDate(int? newSubStateId)
        {
            DateTime? ret = null;
            /* -1: Blank | 0: Non-Counting | 1: Counting */
            var oldSubStateMode = -1;
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

        private CaseOverviewModel GetCaseReceiptModel(Case currentCase, int languageId)
        {
            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(currentCase.Customer_Id, languageId)
                                                           .Where(c => c.ShowExternal == 1 ||
                                                                       c.Name == GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString() ||
                                                                       c.Name == GlobalEnums.TranslationCaseFields.CaseNumber.ToString() ||
                                                                       c.Name == GlobalEnums.TranslationCaseFields.RegTime.ToString()) 
                                                           .ToList();
            
            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);            
            var infoText = _infoService.GetInfoText((int) InfoTextType.SelfServiceInformation, currentCase.Customer_Id, languageId);

            // get customersettings
            var customersettings = _settingService.GetCustomerSetting(currentCase.Customer_Id);

            var regions = _regionService.GetRegions(currentCase.Customer_Id);
            var suppliers = _supplierService.GetSuppliers(currentCase.Customer_Id);
            var systems = _systemService.GetSystems(currentCase.Customer_Id);
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
                var translatedText = pathTexts;
                if (pathTexts.Any())
                {
                    translatedText = new List<string>();
                    foreach (var pathText in pathTexts.ToList())
                        translatedText.Add(Translation.Get(pathText, Enums.TranslationSource.TextTranslation));
                }
                currentCase.ProductArea.Name = string.Join(" - ", translatedText);                
            }

            if (currentCase.Category_Id.HasValue && currentCase.Category != null)
            {
                var pathTexts = _categoryService.GetParentPath(currentCase.Category_Id.Value, currentCase.Customer_Id).ToList();
                var translatedText = pathTexts;
                if (pathTexts.Any())
                {
                    translatedText = new List<string>();
                    foreach (var pathText in pathTexts.ToList())
                        translatedText.Add(Translation.Get(pathText));
                }
                currentCase.Category.Name = string.Join(" - ", translatedText);
            }

            var newLogFile = new FilesModel();

            CaseOverviewModel model = null;

            if(currentCase != null)
            {
                var caselogs = _logService.GetLogsByCaseId(currentCase.Id).OrderByDescending(l => l.LogDate).ToList();
                if (currentCase.Impact_Id.HasValue && currentCase.Impact == null)
                    currentCase.Impact = _impactService.GetImpact(currentCase.Impact_Id.Value);

                model = new CaseOverviewModel
                {
                    InfoText = infoText != null ? infoText.Name : null,
                    CasePreview = currentCase,
                    CaseFieldGroups = caseFieldGroups,
                    CaseLogs = caselogs,
                    FieldSettings = caseFieldSetting,
                    LogFilesModel = newLogFile,
                    Regions = regions,
                    Suppliers = suppliers,
                    Systems = systems,
                    LogFileGuid = Guid.NewGuid().ToString(),
                    CustomerSettings = customersettings
                };
            }

            var caseFolowerUsers = _caseExtraFollowersService.GetCaseExtraFollowers(currentCase.Id).Select(x => x.Follower).ToArray();
            var followerUsers = caseFolowerUsers.Any() ? string.Join(";", caseFolowerUsers) + ";" : string.Empty;
            model.FollowerUsers = followerUsers;

            return model;
        }

        private NewCaseModel GetNewCaseModel(int customerId, int languageId, List<CaseListToCase> caseFieldSetting)
        {           
            var caseFieldGroups = GetVisibleFieldGroups(caseFieldSetting);

            var newCase = new Case { Customer_Id = customerId };
            var caseFile = new FilesModel { Id = Guid.NewGuid().ToString() };

            //Region list            
            var regions = _regionService.GetRegions(customerId);

            //Department list
            var departments = _departmentService.GetDepartments(customerId);

            //Organization unit list
            var orgUnits = _orgService.GetOUs(customerId).ToList();

            //Case Type tree            
            var caseTypes = _caseTypeService.GetCaseTypes(customerId).Where(c=> c.ShowOnExternalPage != 0).ToList();
            caseTypes = CaseTypeTreeTranslation(caseTypes).ToList();

            //Product Area tree            
            var productAreas = _productAreaService.GetTopProductAreas(customerId).Where(p=> p.ShowOnExternalPage != 0).OrderBy(s => s.Name).ToList();
            var traversedData = ProductAreaTreeTranslation(productAreas);
            productAreas = traversedData.Item1.ToList();

            //System list            
            var systems = _systemService.GetSystems(customerId);

            //Urgent List
            var ugencies = _urgencyService.GetUrgencies(customerId);

            //Impact List
            var impacts = _impactService.GetImpacts(customerId);

            //Category List
            var categories = _categoryService.GetActiveParentCategories(customerId);

            //Currency List
            var currencies = _currencyService.GetCurrencies();

            //Country list
            var suppliers = _supplierService.GetSuppliers(customerId);

            //Field Settings
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

            var caseFieldSettingsWithLanguages = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);

            var cs = _settingService.GetCustomerSetting(customerId);
            
            var model = new NewCaseModel(
                newCase, 
                regions, 
                departments,
                orgUnits,
                caseTypes, 
                productAreas, 
                systems,
                ugencies,
                impacts,
                categories, 
                currencies, 
                suppliers, 
                caseFieldGroups, 
                caseFieldSetting, 
                caseFile, 
                caseFieldSettings,
                new JsApplicationOptions()
                    {
                        customerId = customerId,
                        departmentFilterFormat = cs.DepartmentFilterFormat,
                        departmentsURL = Url.Content("~/Case/GetDepartmentsByRegion"),
                        orgUnitURL = Url.Content("~/Case/GetOrgUnitsByDepartments")
                    },
                caseFieldSettingsWithLanguages);

            model.CaseTypeParantPath = "--";
            model.ProductAreaParantPath = "--";
            model.CategoryParentPath = "--";
            model.CaseFileKey = Guid.NewGuid().ToString();
            model.ProductAreaChildren = traversedData.Item2.ToList();
            model.SendToDialogModel = new SendToDialogModel();            

            var _caseTypes = _caseTypeService.GetAllCaseTypes(customerId).Where(c => c.ShowOnExternalPage != 0).ToList();
            model.CaseTypeRelatedFields = _caseTypes.Select(c => new KeyValuePair<int, string>(c.Id, c.RelatedField)).ToList();
            return model;
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

            
            foreach(var field in fieldList)
            {                
                if(userInformationGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.UserInformation);

                if(computerInformationGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.ComputerInformation);

                if(caseInfoGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.CaseInfo);

                if(otherGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.Other);

                if(caseLogGroup.Contains(field.Name))
                    ret.Add(Enums.CaseFieldGroups.CaseLog);
            }

            return ret;
        }
       
        private List<FieldSettingJSModel> GetFieldSettingsModel(List<CaseListToCase> customerFieldSettings, List<CaseSolutionSettingOverview> templateSettings)
        {
            var ret = new List<FieldSettingJSModel>();
            foreach (var field in customerFieldSettings)
            {
                var isVisible = field.ShowExternal.ConvertIntToBool();
                var isRequired = field.Required.ConvertIntToBool();
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
                if (pa.SubProductAreas.Where(sp=> sp.IsActive != 0 && sp.ShowOnExternalPage != 0).Any())
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

        

        private CaseModel LoadTemplateToCase(CaseModel model, CaseSolution caseTemplate)
        {
            if (model == null)
                return null;

            if (caseTemplate == null)
                return model;

            if (caseTemplate.CaseType_Id != null)
            {
                model.CaseType_Id = caseTemplate.CaseType_Id.Value;
            }            

            model.ReportedBy = caseTemplate.ReportedBy;
            model.PersonsName = caseTemplate.PersonsName;

            model.PersonsEmail = caseTemplate.PersonsEmail;
            model.PersonsPhone = caseTemplate.PersonsPhone;
            model.PersonsCellphone = caseTemplate.PersonsCellPhone;
            model.Region_Id = caseTemplate.Region_Id;
            model.Department_Id = caseTemplate.Department_Id;
            model.OU_Id = caseTemplate.OU_Id;
            model.Place = caseTemplate.Place;
            model.UserCode = caseTemplate.UserCode;
            model.CostCentre = caseTemplate.CostCentre;

            model.InventoryNumber = caseTemplate.InventoryNumber;
            model.InventoryType = caseTemplate.InventoryType;
            model.InventoryLocation = caseTemplate.InventoryLocation;

            model.ProductArea_Id = caseTemplate.ProductArea_Id;
            model.System_Id = caseTemplate.System_Id;
            model.Caption = caseTemplate.Caption;
            model.Description = caseTemplate.Description;
            model.Priority_Id = caseTemplate.Priority_Id;
            model.Project_Id = caseTemplate.Project_Id;
            model.Urgency_Id = caseTemplate.Urgency_Id;
            model.Impact_Id = caseTemplate.Impact_Id;
            model.Category_Id = caseTemplate.Category_Id;
            model.Supplier_Id = caseTemplate.Supplier_Id;

            model.InvoiceNumber = caseTemplate.InvoiceNumber;
            model.ReferenceNumber = caseTemplate.ReferenceNumber;
            model.Miscellaneous = caseTemplate.Miscellaneous;
            model.ContactBeforeAction = caseTemplate.ContactBeforeAction;
            model.SMS = caseTemplate.SMS;
            model.AgreedDate = caseTemplate.AgreedDate;
            model.Available = caseTemplate.Available;
            model.Cost = caseTemplate.Cost;
            model.OtherCost = caseTemplate.OtherCost;
            model.Currency = caseTemplate.Currency;
            
            model.Performer_User_Id = caseTemplate.PerformerUser_Id;
            model.CausingPartId = caseTemplate.CausingPartId;
            model.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id;
            model.Project_Id = caseTemplate.Project_Id;
            model.Problem_Id = caseTemplate.Problem_Id;
            model.PlanDate = caseTemplate.PlanDate;
            model.WatchDate = caseTemplate.WatchDate;
            
            model.IsAbout_ReportedBy = caseTemplate.IsAbout_ReportedBy;
            model.IsAbout_PersonsName = caseTemplate.IsAbout_PersonsName;
            model.IsAbout_PersonsEmail = caseTemplate.IsAbout_PersonsEmail;
            model.IsAbout_PersonsPhone = caseTemplate.IsAbout_PersonsPhone;
            model.IsAbout_PersonsCellPhone = caseTemplate.IsAbout_PersonsCellPhone;
            model.IsAbout_Region_Id = caseTemplate.IsAbout_Region_Id;
            model.IsAbout_Department_Id = caseTemplate.IsAbout_Department_Id;
            model.IsAbout_OU_Id = caseTemplate.IsAbout_OU_Id;
            model.IsAbout_CostCentre = caseTemplate.IsAbout_CostCentre;
            model.IsAbout_Place = caseTemplate.IsAbout_Place;
            model.IsAbout_UserCode = caseTemplate.UserCode;

            model.Status_Id = caseTemplate.Status_Id;
            model.StateSecondary_Id = caseTemplate.StateSecondary_Id;
            model.Verified = caseTemplate.Verified;
            model.VerifiedDescription = caseTemplate.VerifiedDescription;
            model.SolutionRate = caseTemplate.SolutionRate;

            model.Text_External = caseTemplate.Text_External;
            model.Text_Internal = caseTemplate.Text_Internal;
            model.FinishingType_Id = caseTemplate.FinishingCause_Id;
                
            if (caseTemplate.RegistrationSource.HasValue && caseTemplate.RegistrationSource.Value > 0)
            {
                model.RegistrationSourceCustomer_Id = caseTemplate.RegistrationSource.Value;
            }
            else
            {
                var registrationSource = _registrationSourceCustomerService.GetCustomersActiveRegistrationSources(model.Customer_Id)
                        .FirstOrDefault(x => x.SystemCode == (int)CaseRegistrationSource.SelfService);
                if (registrationSource != null)
                    model.RegistrationSourceCustomer_Id = registrationSource.Id;
            }

            return model;
        }

        private CaseModel ApplyNextWorkflowStepOnCase(CaseModel model, int stepId)
        {
            if (model == null)
                return null;

            var caseTemplate = _caseSolutionService.GetCaseSolution(stepId);
            if (caseTemplate == null)
                return model;

            if (caseTemplate.CaseType_Id != null)
            {
                model.CaseType_Id = caseTemplate.CaseType_Id.Value;
            }            

            model.ReportedBy = caseTemplate.ReportedBy.IfNullThenElse(model.ReportedBy);
            model.PersonsName = caseTemplate.PersonsName.IfNullThenElse(model.PersonsName);
            model.PersonsEmail = caseTemplate.PersonsEmail.IfNullThenElse(model.PersonsEmail);
            model.PersonsPhone = caseTemplate.PersonsPhone.IfNullThenElse(model.PersonsPhone);
            model.PersonsCellphone = caseTemplate.PersonsCellPhone.IfNullThenElse(model.PersonsCellphone);
            model.Region_Id = caseTemplate.Region_Id.IfNullThenElse(model.Region_Id);
            model.Department_Id = caseTemplate.Department_Id.IfNullThenElse(model.Department_Id);
            model.OU_Id = caseTemplate.OU_Id.IfNullThenElse(model.OU_Id);
            model.Place = caseTemplate.Place.IfNullThenElse(model.Place);
            model.UserCode = caseTemplate.UserCode.IfNullThenElse(model.UserCode);
            model.CostCentre = caseTemplate.CostCentre.IfNullThenElse(model.CostCentre);

            model.InventoryNumber = caseTemplate.InventoryNumber.IfNullThenElse(model.InventoryNumber);
            model.InventoryType = caseTemplate.InventoryType.IfNullThenElse(model.InventoryType);
            model.InventoryLocation = caseTemplate.InventoryLocation.IfNullThenElse(model.InventoryLocation);

            model.ProductArea_Id = caseTemplate.ProductArea_Id.IfNullThenElse(model.ProductArea_Id);
            model.System_Id = caseTemplate.System_Id.IfNullThenElse(model.System_Id);
            model.Caption = caseTemplate.Caption.IfNullThenElse(model.Caption);
            model.Description = caseTemplate.Description.IfNullThenElse(model.Description);
            model.Priority_Id = caseTemplate.Priority_Id.IfNullThenElse(model.Priority_Id);
            model.Project_Id = caseTemplate.Project_Id.IfNullThenElse(model.Project_Id);
            model.Urgency_Id = caseTemplate.Urgency_Id.IfNullThenElse(model.Urgency_Id);
            model.Impact_Id = caseTemplate.Impact_Id.IfNullThenElse(model.Impact_Id);
            model.Category_Id = caseTemplate.Category_Id.IfNullThenElse(model.Category_Id);
            model.Supplier_Id = caseTemplate.Supplier_Id.IfNullThenElse(model.Supplier_Id);

            model.InvoiceNumber = caseTemplate.InvoiceNumber.IfNullThenElse(model.InvoiceNumber);
            model.ReferenceNumber = caseTemplate.ReferenceNumber.IfNullThenElse(model.ReferenceNumber);
            model.Miscellaneous = caseTemplate.Miscellaneous.IfNullThenElse(model.Miscellaneous);
            model.ContactBeforeAction = caseTemplate.ContactBeforeAction;
            model.SMS = caseTemplate.SMS;
            model.AgreedDate = caseTemplate.AgreedDate.IfNullThenElse(model.AgreedDate);
            model.Available = caseTemplate.Available.IfNullThenElse(model.Available);
            model.Cost = caseTemplate.Cost;
            model.OtherCost = caseTemplate.OtherCost;
            model.Currency = caseTemplate.Currency.IfNullThenElse(model.Currency);

            model.Performer_User_Id = caseTemplate.PerformerUser_Id.IfNullThenElse(model.Performer_User_Id);
            model.CausingPartId = caseTemplate.CausingPartId.IfNullThenElse(model.CausingPartId);
            model.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id.IfNullThenElse(model.WorkingGroup_Id);
            model.Project_Id = caseTemplate.Project_Id.IfNullThenElse(model.Project_Id);
            model.Problem_Id = caseTemplate.Problem_Id.IfNullThenElse(model.Problem_Id);
            model.PlanDate = caseTemplate.PlanDate.IfNullThenElse(model.PlanDate);
            model.WatchDate = caseTemplate.WatchDate.IfNullThenElse(model.WatchDate);

            model.IsAbout_ReportedBy = caseTemplate.IsAbout_ReportedBy.IfNullThenElse(model.IsAbout_ReportedBy);
            model.IsAbout_PersonsName = caseTemplate.IsAbout_PersonsName.IfNullThenElse(model.IsAbout_PersonsName);
            model.IsAbout_PersonsEmail = caseTemplate.IsAbout_PersonsEmail.IfNullThenElse(model.IsAbout_PersonsEmail);
            model.IsAbout_PersonsPhone = caseTemplate.IsAbout_PersonsPhone.IfNullThenElse(model.IsAbout_PersonsPhone);
            model.IsAbout_PersonsCellPhone = caseTemplate.IsAbout_PersonsCellPhone.IfNullThenElse(model.IsAbout_PersonsCellPhone);
            model.IsAbout_Region_Id = caseTemplate.IsAbout_Region_Id.IfNullThenElse(model.IsAbout_Region_Id);
            model.IsAbout_Department_Id = caseTemplate.IsAbout_Department_Id.IfNullThenElse(model.IsAbout_Department_Id);
            model.IsAbout_OU_Id = caseTemplate.IsAbout_OU_Id.IfNullThenElse(model.IsAbout_OU_Id);
            model.IsAbout_CostCentre = caseTemplate.IsAbout_CostCentre.IfNullThenElse(model.IsAbout_CostCentre);
            model.IsAbout_Place = caseTemplate.IsAbout_Place.IfNullThenElse(model.IsAbout_Place);
            model.IsAbout_UserCode = caseTemplate.UserCode.IfNullThenElse(model.IsAbout_UserCode);

            model.Status_Id = caseTemplate.Status_Id.IfNullThenElse(model.Status_Id);
            model.StateSecondary_Id = caseTemplate.StateSecondary_Id.IfNullThenElse(model.StateSecondary_Id);
            model.Verified = caseTemplate.Verified;
            model.VerifiedDescription = caseTemplate.VerifiedDescription.IfNullThenElse(model.VerifiedDescription);
            model.SolutionRate = caseTemplate.SolutionRate.IfNullThenElse(model.SolutionRate);

            model.Text_External = caseTemplate.Text_External.IfNullThenElse(model.Text_External);
            model.Text_Internal = caseTemplate.Text_Internal.IfNullThenElse(model.Text_Internal);
            model.FinishingType_Id = caseTemplate.FinishingCause_Id.IfNullThenElse(model.FinishingType_Id);

            if (caseTemplate.RegistrationSource.HasValue && caseTemplate.RegistrationSource.Value > 0)
            {
                model.RegistrationSourceCustomer_Id = caseTemplate.RegistrationSource.Value;
            }
           
            return model;
        }

        private List<WorkflowStepModel> GetWorkflowStepModel(int customerId, int caseId, int templateId, IList<CaseSolutionOverview> customerCaseSolutions, bool isRealtedCase)
        {
            IList<WorkflowStepModel> res = new List<WorkflowStepModel>();
            Case caseEntity = null;
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

            if (caseEntity != null)
                res = _caseSolutionService.GetWorkflowSteps(customerId, caseEntity, customerCaseSolutions, isRealtedCase, null, ApplicationType.LineManager, templateId);            

            return res.ToList();
        }

        #region Helper Methods

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


		// keep for diagnostic purposes
		private void LogWithContext(string msg)
		{
			var customerId = SessionFacade.CurrentCustomerID;
			var userIdentityEmail = SessionFacade.CurrentUserIdentity?.Email;
			var userIdentityEmployeeNumber = SessionFacade.CurrentUserIdentity?.EmployeeNumber;
			var userIdentityUserId = SessionFacade.CurrentUserIdentity?.UserId;
			var localUserPkId = SessionFacade.CurrentLocalUser?.Id;
			var localUserId = SessionFacade.CurrentLocalUser?.UserId;
		}        

        #endregion
    }
}

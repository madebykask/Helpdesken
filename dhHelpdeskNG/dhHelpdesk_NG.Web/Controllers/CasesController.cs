using DH.Helpdesk.BusinessData.Models.Paging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DH.Helpdesk.BusinessData.Models.ExternalInvoice;
using DH.Helpdesk.BusinessData.Models.Logs;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Common.Enums.FileViewLog;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Enums.Settings;
using DH.Helpdesk.Common.Exceptions;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Extensions;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Web.Models.Invoice;
using DH.Helpdesk.Domain.Interfaces;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Services.BusinessLogic.Cases;
using DH.Helpdesk.Services.Services.CaseStatistic;
using DH.Helpdesk.Services.Services.Orders;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.Web.Common.Models.Case;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.Web.Infrastructure.Behaviors;
using DH.Helpdesk.Web.Infrastructure.Logger;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
using DH.Helpdesk.BusinessData.Models.Case.Input;
using Microsoft.Ajax.Utilities;

namespace DH.Helpdesk.Web.Controllers
{
    using DH.Helpdesk.BusinessData.Enums.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Services.Services.Grid;
    using DH.Helpdesk.Services.Utils;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Infrastructure.Case;
    using DH.Helpdesk.Web.Infrastructure.CaseOverview;
    using DH.Helpdesk.Web.Infrastructure.Configuration;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.Grid;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.CaseLockMappers;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Case.ChildCase;
    using DH.Helpdesk.Web.Models.Case.Input;
    using DH.Helpdesk.Web.Models.Case.Output;
    using DH.Helpdesk.Web.Models.CaseLock;
    using DH.Helpdesk.Web.Models.Shared;
    using DH.Helpdesk.Common.Extensions.DateTime;

    using DHDomain = DH.Helpdesk.Domain;
    using ParentCaseInfo = DH.Helpdesk.BusinessData.Models.Case.ChidCase.ParentCaseInfo;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Services.Services.Reports;
    using DH.Helpdesk.Common.Enums.CaseSolution;
    using DH.Helpdesk.Common.Enums.BusinessRule;
    using Services.BusinessLogic.Admin.Users;
    using Services.BusinessLogic.Mappers.Users;
    using BusinessData.Enums.Admin.Users;
    using System.Diagnostics;
    using Models.CaseRules;
    using Infrastructure.ModelFactories.Case.Concrete;
    using static BusinessData.OldComponents.GlobalEnums;
    using System.Threading;
    using Models.WebApi;
    using Infrastructure.WebApi;
    using System.Configuration;
    using Infrastructure.Helpers;
    using Infrastructure.Cryptography;
    using Domain.Computers;
    using BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using Common.Tools.Files;
    using BusinessData.Models.FinishingCause;
    using Infrastructure.Mvc;
    using System.Net.Mime;
    using DH.Helpdesk.Common.Enums.Logs;
    using DH.Helpdesk.Common.Extensions;
    using System.IO;
    using System.Net;

    public partial class CasesController : BaseController
    {
        #region ***Constant/Variables***

        private const string ParentPathDefaultValue = "--";
        private const string ChildCasesHashTab = "childcases-tab";
        private const int MaxTextCharCount = 200;

        private readonly ICaseProcessor _caseProcessor;
        private readonly ICaseService _caseService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseFileService _caseFileService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ICaseFollowUpService _caseFollowUpService;
        private readonly ICategoryService _categoryService;
        private readonly IComputerService _computerService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IDepartmentService _departmentService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly IImpactService _impactService;
        private readonly IOUService _ouService;
        private readonly IProblemService _problemService;
        private readonly IPriorityService _priorityService;
        private readonly IProductAreaService _productAreaService;
        private readonly IRegionService _regionService;
        private readonly ISettingService _settingService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly IStatusService _statusService;
        private readonly ISupplierService _supplierService;
        private readonly IStandardTextService _standardTextService;
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IProjectService _projectService;
        private readonly IChangeService _changeService;
        private readonly ILogService _logService;
        private readonly ILogFileService _logFileService;
        private readonly ITemporaryFilesCache _userTemporaryFilesStorage;
        private readonly IEmailGroupService _emailGroupService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly IEmailService _emailService;
        private readonly ILanguageService _languageService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IMail2TicketService _mail2TicketService;
        private readonly ICausingPartService _causingPartService;
        private readonly IInvoiceArticlesModelFactory _invoiceArticlesModelFactory;
        private readonly ICaseNotifierModelFactory _caseNotifierModelFactory;
        private readonly INotifierService _notifierService;
        private readonly IWorkContext _workContext;
        private readonly IInvoiceArticleService _invoiceArticleService;
        private readonly ICaseSolutionSettingService _caseSolutionSettingService;
        private readonly ICaseModelFactory _caseModelFactory;
        private readonly CaseOverviewGridSettingsService _caseOverviewSettingsService;
        private readonly GridSettingsService _gridSettingsService;
        private readonly IOrganizationService _organizationService;
        private readonly IMasterDataService _masterDataService;
        private readonly OrganizationJsonService _orgJsonService;
        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;
        private readonly ICaseLockService _caseLockService;

        private readonly IWatchDateCalendarService _watchDateCalendarService;
        private readonly IReportServiceService _reportServiceService;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly IExternalInvoiceService _externalInvoiceService;
        private readonly ICaseExtraFollowersService _caseExtraFollowersService;
        private readonly ICaseRuleFactory _caseRuleFactory;
        private readonly IOrdersService _orderService;
        private readonly IOrderAccountService _orderAccountService;
        private readonly ICaseSectionService _caseSectionService;
        private readonly ICaseDocumentService _caseDocumentService;
        private readonly IExtendedCaseService _extendedCaseService;
        private readonly ISendToDialogModelFactory _sendToDialogModelFactory;
        private readonly ICaseStatisticService _caseStatService;
        private readonly IUserEmailsSearchService _userEmailsSearchService;
        private readonly IFeatureToggleService _featureToggleService;
        private readonly IFileViewLogService _fileViewLogService;
        private readonly IFilesStorage _filesStorage;
        private readonly ICaseDeletionService _caseDeletionService;

        #endregion

        private readonly int DefaultCaseLockBufferTime = 30; // Second
        private readonly int DefaultExtendCaseLockTime = 60; // Secondx§

        #region ***Constructor***

        public CasesController(
            ICaseProcessor caseProcessor,
            ICaseService caseService,
            ICaseSearchService caseSearchService,
            ICaseFieldSettingService caseFieldSettingService,
            ICaseFileService caseFileService,
            ICaseSettingsService caseSettingService,
            ICaseTypeService caseTypeService,
            ICaseFollowUpService caseFollowUpService,
            ICategoryService categoryService,
            IComputerService computerService,
            ICountryService countryService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            ICustomerUserService customerUserService,
            IDepartmentService departmentService,
            IFinishingCauseService finishingCauseService,
            IImpactService impactService,
            IOUService ouService,
            IProblemService problemService,
            IPriorityService priorityService,
            IProductAreaService productAreaService,
            IRegionService regionService,
            ISettingService settingService,
            IStateSecondaryService stateSecondaryService,
            IStatusService statusService,
            IStandardTextService standardTextService,
            ISupplierService supplierService,
            ISystemService systemService,
            IUrgencyService urgencyService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService,
            IProjectService projectService,
            IChangeService changeService,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            ICaseSolutionService caseSolutionService,
            ILogService logService,
            IEmailGroupService emailGroupService,
            IEmailService emailService,
            ILanguageService languageService,
            ILogFileService logFileService,
            IGlobalSettingService globalSettingService,
            IWorkContext workContext,
            ICaseNotifierModelFactory caseNotifierModelFactory,
            INotifierService notifierService,
            IInvoiceArticleService invoiceArticleService,
            IConfiguration configuration,
            ICaseSolutionSettingService caseSolutionSettingService,
            IInvoiceHelper invoiceHelper,
            ICaseModelFactory caseModelFactory,
            CaseOverviewGridSettingsService caseOverviewSettingsService,
            GridSettingsService gridSettingsService,
            IOrganizationService organizationService,
            OrganizationJsonService orgJsonService,
            IRegistrationSourceCustomerService registrationSourceCustomerService,
            ICaseLockService caseLockService,
            IMailTemplateService mailTemplateService,
            IMail2TicketService mail2TicketService,
            IWatchDateCalendarService watchDateCalendarServcie,
            ICausingPartService causingPartService,
            IInvoiceArticlesModelFactory invoiceArticlesModelFactory,
            IReportServiceService reportServiceService,
            IUserPermissionsChecker userPermissionsChecker,
            IExternalInvoiceService externalInvoiceService,
            ICaseExtraFollowersService caseExtraFollowersService,
            ICaseRuleFactory caseRuleFactory,
            IOrdersService orderService,
            IOrderAccountService orderAccountService,
            ICaseDocumentService caseDocumentService,
            ICaseSectionService caseSectionService,
            IExtendedCaseService extendedCaseService,
            ISendToDialogModelFactory sendToDialogModelFactory,
            ICaseStatisticService caseStatService,
            IUserEmailsSearchService userEmailsSearchService,
            IFeatureToggleService featureToggleService,
            IFileViewLogService fileViewLogService,
            IFilesStorage filesStorage,
            ICaseDeletionService caseDeletionService)
            : base(masterDataService)
        {
            _caseProcessor = caseProcessor;
            _masterDataService = masterDataService;
            _caseService = caseService;
            _caseSearchService = caseSearchService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseFileService = caseFileService;
            _caseSettingService = caseSettingService;
            _caseTypeService = caseTypeService;
            _caseFollowUpService = caseFollowUpService;
            _categoryService = categoryService;
            _computerService = computerService;
            _countryService = countryService;
            _currencyService = currencyService;
            _customerService = customerService;
            _customerUserService = customerUserService;
            _departmentService = departmentService;
            _finishingCauseService = finishingCauseService;
            _impactService = impactService;
            _ouService = ouService;
            _problemService = problemService;
            _priorityService = priorityService;
            _productAreaService = productAreaService;
            _regionService = regionService;
            _settingService = settingService;
            _stateSecondaryService = stateSecondaryService;
            _statusService = statusService;
            _standardTextService = standardTextService;
            _supplierService = supplierService;
            _systemService = systemService;
            _urgencyService = urgencyService;
            _userService = userService;
            _workingGroupService = workingGroupService;
            _projectService = projectService;
            _changeService = changeService;
            _logService = logService;
            _logFileService = logFileService;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
            _caseSolutionService = caseSolutionService;
            _emailGroupService = emailGroupService;
            _emailService = emailService;
            _languageService = languageService;
            _globalSettingService = globalSettingService;
            _workContext = workContext;
            _caseNotifierModelFactory = caseNotifierModelFactory;
            _notifierService = notifierService;
            _invoiceArticleService = invoiceArticleService;
            _caseSolutionSettingService = caseSolutionSettingService;
            _caseModelFactory = caseModelFactory;
            _caseOverviewSettingsService = caseOverviewSettingsService;
            _gridSettingsService = gridSettingsService;
            _organizationService = organizationService;
            _orgJsonService = orgJsonService;
            _registrationSourceCustomerService = registrationSourceCustomerService;
            _caseLockService = caseLockService;
            _watchDateCalendarService = watchDateCalendarServcie;
            _mailTemplateService = mailTemplateService;
            _mail2TicketService = mail2TicketService;
            _causingPartService = causingPartService;
            _reportServiceService = reportServiceService;
            _invoiceArticlesModelFactory = invoiceArticlesModelFactory;
            _userPermissionsChecker = userPermissionsChecker;
            _externalInvoiceService = externalInvoiceService;
            _caseExtraFollowersService = caseExtraFollowersService;
            _caseRuleFactory = caseRuleFactory;
            _orderService = orderService;
            _orderAccountService = orderAccountService;
            _caseDocumentService = caseDocumentService;
            _caseSectionService = caseSectionService;
            _extendedCaseService = extendedCaseService;
            _sendToDialogModelFactory = sendToDialogModelFactory;
            _caseStatService = caseStatService;
            _userEmailsSearchService = userEmailsSearchService;
            _featureToggleService = featureToggleService;
            _fileViewLogService = fileViewLogService;
            _filesStorage = filesStorage;
            _caseDeletionService = caseDeletionService;

            _advancedSearchBehavior = new AdvancedSearchBehavior(caseFieldSettingService,
                caseSearchService,
                userService,
                settingService,
                productAreaService,
                customerUserService,
                globalSettingService,
                caseLockService);
        }

        #endregion

        #region ***Public Methods***

        #region --Case Overview--

        public ActionResult InitFilter(
            int? customerId,
            bool? clearFilters = false,
            CasesCustomFilter customFilter = CasesCustomFilter.None,
            bool? useMyCases = false,
            bool? keepAdvancedSearch = false)
        {
            if (customerId.HasValue && (SessionFacade.CurrentCustomer == null || customerId.Value != SessionFacade.CurrentCustomer.Id))
            {
                SessionFacade.CurrentCustomer = _customerService.GetCustomer(customerId.Value);
                SessionFacade.CaseOverviewGridSettings = null;
                if (!keepAdvancedSearch.HasValue || !keepAdvancedSearch.Value)
                    SessionFacade.CurrentAdvancedSearch = null;
                SessionFacade.ClearSearches();
            }
            else
            {
                if (SessionFacade.CurrentCustomer == null)
                {
                    SessionFacade.CurrentCustomer = _customerService.GetCustomer(SessionFacade.CurrentUser.CustomerId);
                    SessionFacade.CaseOverviewGridSettings = null;
                    if (!keepAdvancedSearch.HasValue || !keepAdvancedSearch.Value)
                        SessionFacade.CurrentAdvancedSearch = null;
                    SessionFacade.ClearSearches();
                }
            }

            if (SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            var caseSearchModel = (clearFilters.HasValue && clearFilters.Value)
                                      ? this.InitEmptySearchModel(
                        SessionFacade.CurrentCustomer.Id,
                                          SessionFacade.CurrentUser.Id)
                                      : this.InitCaseSearchModel(
                                          SessionFacade.CurrentCustomer.Id,
                                          SessionFacade.CurrentUser.Id);
            if (useMyCases.HasValue && useMyCases == true)
            {
                caseSearchModel.CaseSearchFilter.SearchInMyCasesOnly = true;
                caseSearchModel.CaseSearchFilter.UserPerformer = string.Empty;
                caseSearchModel.CaseSearchFilter.CaseProgress = CaseSearchFilter.InProgressCases;
            }

            switch (customFilter)
            {
                case CasesCustomFilter.UnreadCases:
                    caseSearchModel.CaseSearchFilter.CaseProgress = CaseSearchFilter.UnreadCases;
                    caseSearchModel.CaseSearchFilter.UserPerformer = string.Empty;
                    break;
                case CasesCustomFilter.HoldCases:
                    caseSearchModel.CaseSearchFilter.CaseProgress = CaseSearchFilter.HoldCases;
                    caseSearchModel.CaseSearchFilter.UserPerformer = string.Empty;
                    break;
                case CasesCustomFilter.InProcessCases:
                    caseSearchModel.CaseSearchFilter.CaseProgress = CaseSearchFilter.InProgressCases;
                    caseSearchModel.CaseSearchFilter.UserPerformer = string.Empty;
                    break;
            }

            caseSearchModel.CaseSearchFilter.CustomFilter = customFilter;
            SessionFacade.CurrentCaseSearch = caseSearchModel;

            return new RedirectResult("~/cases");
        }

        [UserCasePermissions]
        public ActionResult Documents(string id, string fileName, CaseFileType type, int? logId = null)
        {
            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }
            var userId = SessionFacade.CurrentUser.Id;

            //var caseLockViewModel = GetCaseLockModel(int.Parse(id), userId, false, "");

            var customerId = _caseService.GetCaseCustomerId(int.Parse(id));

            //var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            //CaseInputViewModel m = this.GetCaseInputViewModel(userId, customerId, int.Parse(id), caseLockViewModel, caseFieldSettings, "", "", null, null, false);
            var cc = _caseService.GetCaseById(int.Parse(id));
            var editMode = CalcEditMode(customerId, userId, cc);
            // User has not access to case
            if (editMode == AccessMode.NoAccess)
                return RedirectToAction("index", "home");

            //Ok to se case
            var c = _caseService.GetCaseBasic(int.Parse(id));
            var pathToFile = string.Empty;
            if (c != null)
            {
                var basePath = _masterDataService.GetFilePath(c.CustomerId) + "\\";

                if (logId != null)
                {
                    var prefix = "";
                    if (type == CaseFileType.LogExternal)
                    {
                        prefix = DH.Helpdesk.Common.Enums.ModuleName.Log;
                    }
                    else
                    {
                        //Check permission to see internal lognotes.
                        if (!SessionFacade.CurrentUser.CaseInternalLogPermission.ToBool())
                            return RedirectToAction("index", "home");

                        prefix = DH.Helpdesk.Common.Enums.ModuleName.LogInternal;
                    }
                    var logFolder = $"{prefix}{logId}";
                    pathToFile = basePath + logFolder + "\\" + fileName;
                }
                else
                {
                    pathToFile = basePath + c.CaseNumber + "\\" + fileName;
                }

            }
            _fileViewLogService.Log(int.Parse(id), userId, fileName, pathToFile, FileViewLogFileSource.Helpdesk, FileViewLogOperation.View);

            string mimeType = MimeMapping.GetMimeMapping(fileName);
            byte[] fileBytes = _filesStorage.GetFileByteContent(pathToFile);
            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName.Replace(",", ""));
            return File(fileBytes, mimeType);
        }
        public AccessMode CalcEditMode(int customerId, int userId, Case @case, bool temporaryHasAccessToWG = false)
        {
            var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
            var accessToWorkinggroups = _userService.GetWorkinggroupsForUserAndCustomer(userId, customerId);
            var departmensForUser = _departmentService.GetDepartmentsByUserPermissions(userId, customerId);

            var currentUser = _userService.GetUserOverview(userId);
            if (departmensForUser != null)
            {
                var accessToDepartments = departmensForUser.Select(d => d.Id).ToList();

                if (currentUser.UserGroupId < (int)BusinessData.Enums.Admin.Users.UserGroup.CustomerAdministrator)
                {
                    if (accessToDepartments.Count > 0 && @case.Department_Id.HasValue)
                    {
                        if (!accessToDepartments.Contains(@case.Department_Id.Value))
                        {
                            return AccessMode.NoAccess;
                        }
                    }
                }
            }

            // In new case shouldn't check
            /*Updated in this way:*/
            /*If user does not have access to WG, if last action was "Save", user can see the Case in readonly mode 
             * there is no ticket. (Per knows more info)
             */
            if (accessToWorkinggroups != null && @case.Id != 0)
            {
                if (currentUser.UserGroupId < (int)BusinessData.Enums.Admin.Users.UserGroup.CustomerAdministrator)
                {
                    if (accessToWorkinggroups.Any() && @case.WorkingGroup_Id.HasValue)
                    {
                        var wg = accessToWorkinggroups.FirstOrDefault(w => w.WorkingGroup_Id == @case.WorkingGroup_Id.Value);
                        if (wg == null && (gs != null && gs.LockCaseToWorkingGroup == 1))
                        {
                            return temporaryHasAccessToWG ? AccessMode.ReadOnly : AccessMode.NoAccess;
                        }

                        if (wg != null && wg.RoleToUWG == 1)
                        {
                            return AccessMode.ReadOnly;
                        }
                    }
                }
            }

            if (@case.FinishingDate.HasValue)
            {
                return AccessMode.ReadOnly;
            }

            //case lock condition will be checked on client separately
            //if (lockModel != null && lockModel.IsLocked) return AccessMode.ReadOnly;

            return AccessMode.FullAccess;
        }
        public ActionResult Index()
        {
            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            ApplicationFacade.UpdateLoggedInUser(Session.SessionID, string.Empty);

            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;

            if (SessionFacade.CaseOverviewGridSettings == null)
            {
                SessionFacade.CaseOverviewGridSettings =
                   _gridSettingsService.GetForCustomerUserGrid(
                        customerId,
                        SessionFacade.CurrentUser.UserGroupId,
                        userId,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);
            }

            if (SessionFacade.CurrentCaseSearch == null)
                SessionFacade.CurrentCaseSearch = this.InitCaseSearchModel(customerId, userId);

            var m = new JsonCaseIndexViewModel();
            var lang = SessionFacade.CurrentLanguageId;

            var customerUser = _customerUserService.GetCustomerUserSettings(customerId, userId);
            m.CaseSearchFilterData = CreateCaseSearchFilterData(customerId, SessionFacade.CurrentUser, customerUser, SessionFacade.CurrentCaseSearch);
            m.CaseTemplateTreeButton = GetCaseTemplateTreeModel(customerId, userId, CaseSolutionLocationShow.OnCaseOverview, lang);
            _caseSettingService.GetCaseSettingsWithUser(customerId, userId, SessionFacade.CurrentUser.UserGroupId);

            m.CaseSetting = GetCaseSettingModel(customerId, userId);
            m.CaseSearchFilterData.IsAboutEnabled = m.CaseSetting.ColumnSettingModel.CaseFieldSettings.GetIsAboutEnabled();
            m.CaseInputViewModel = this.PopulateBulkCaseEditModal(m.CaseSetting.CustomerId);
            var user = _userService.GetUser(userId);

            SessionFacade.CaseOverviewGridSettings.pageOptions.pageIndex =
                SessionFacade.CurrentCaseSearch.CaseSearchFilter.PageInfo.PageNumber;

            //create page settings model
            m.PageSettings = new PageSettingsModel()
            {
                searchFilter = JsonCaseSearchFilterData.MapFrom(m.CaseSetting),
                userFilterFavorites = GetMyFavorites(customerId, userId),

                gridSettings =
                    JsonGridSettingsMapper.ToJsonGridSettingsModel(
                        SessionFacade.CaseOverviewGridSettings,
                        SessionFacade.CurrentCustomer.Id,
                        m.CaseSetting.ColumnSettingModel.AvailableColumns.Count(),
                        CaseColumnsSettingsModel.PageSizes.Select(x => x.Value).ToArray()),

                refreshContent = user.RefreshContent,
                messages = new Dictionary<string, string>()
                {
                    { "information", Translation.GetCoreTextTranslation("Information") },
                    { "records_limited_msg", Translation.GetCoreTextTranslation("Antal ärende som visas är begränsade till 500.") },
                },
                HelperRegTime = Translation.Get(GlobalEnums.TranslationCaseFields.RegTime.ToString(), Enums.TranslationSource.CaseTranslation, SessionFacade.CurrentCustomer.Id),
                HelperCaption = Translation.Get(GlobalEnums.TranslationCaseFields.Caption.ToString(), Enums.TranslationSource.CaseTranslation, SessionFacade.CurrentCustomer.Id)
            };

            return View("Index", m);
        }

        [ValidateInput(false)]
        public ActionResult SearchAjax(FormCollection frm)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            #region Code from old method. TODO: code review wanted
            var f = new CaseSearchFilter();
            f.CustomerId = SessionFacade.CurrentCustomer.Id;
            f.UserId = SessionFacade.CurrentUser.Id;
            f.Initiator = frm.ReturnFormValue(CaseFilterFields.InitiatorNameAttribute);
            CaseInitiatorSearchScope initiatorSearchScope;
            if (Enum.TryParse(frm.ReturnFormValue(CaseFilterFields.InitiatorSearchScopeAttribute), out initiatorSearchScope))
            {
                f.InitiatorSearchScope = initiatorSearchScope;
            }
            f.CaseType = frm.ReturnFormValue(CaseFilterFields.CaseTypeIdNameAttribute).ToInt();
            f.ProductArea = frm.ReturnFormValue(CaseFilterFields.ProductAreaIdNameAttribute).ReturnCustomerUserValue();
            f.Region = frm.ReturnFormValue(CaseFilterFields.RegionNameAttribute);
            f.User = frm.ReturnFormValue(CaseFilterFields.RegisteredByNameAttribute);
            f.Category = frm.ReturnFormValue(CaseFilterFields.CategoryNameAttribute);
            f.WorkingGroup = frm.ReturnFormValue(CaseFilterFields.WorkingGroupNameAttribute);
            f.UserResponsible = frm.ReturnFormValue(CaseFilterFields.ResponsibleNameAttribute);
            f.UserPerformer = frm.ReturnFormValue(CaseFilterFields.PerformerNameAttribute);

            f.Priority = frm.ReturnFormValue(CaseFilterFields.PriorityNameAttribute);
            f.Status = frm.ReturnFormValue(CaseFilterFields.StatusNameAttribute);
            f.StateSecondary = frm.ReturnFormValue(CaseFilterFields.StateSecondaryNameAttribute);

            f.CaseRegistrationDateStartFilter = frm.GetDate(CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute);
            f.CaseRegistrationDateEndFilter = frm.GetDate(CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute).GetEndOfDay();

            f.CaseWatchDateStartFilter = frm.GetDate(CaseFilterFields.CaseWatchDateStartFilterNameAttribute);
            f.CaseWatchDateEndFilter = frm.GetDate(CaseFilterFields.CaseWatchDateEndFilterNameAttribute).GetEndOfDay();
            f.CaseClosingDateStartFilter = frm.GetDate(CaseFilterFields.CaseClosingDateStartFilterNameAttribute);
            f.CaseClosingDateEndFilter = frm.GetDate(CaseFilterFields.CaseClosingDateEndFilterNameAttribute).GetEndOfDay();
            f.CaseClosingReasonFilter = frm.ReturnFormValue(CaseFilterFields.ClosingReasonNameAttribute).ReturnCustomerUserValue();
            f.SearchInMyCasesOnly = frm.IsFormValueTrue("SearchInMyCasesOnly");
            f.IsConnectToParent = frm.IsFormValueTrue(CaseFilterFields.IsConnectToParent);
            f.ToBeMerged = frm.IsFormValueTrue(CaseFilterFields.ToBeMerged);

            if (f.IsConnectToParent)
            {
                var id = frm.ReturnFormValue(CaseFilterFields.CurrentCaseId);
                int currentCaseId;
                if (!string.IsNullOrEmpty(id) && int.TryParse(id, out currentCaseId))
                {
                    f.CurrentCaseId = currentCaseId;
                }
            }

            f.CaseProgress = frm.ReturnFormValue(CaseFilterFields.FilterCaseProgressNameAttribute);
            f.CaseFilterFavorite = frm.ReturnFormValue(CaseFilterFields.CaseFilterFavoriteNameAttribute);
            f.FreeTextSearch = frm.ReturnFormValue(CaseFilterFields.FreeTextSearchNameAttribute);

            var departments_OrganizationUnits = frm.ReturnFormValue(CaseFilterFields.DepartmentNameAttribute);

            f.Department = GetDepartmentsFrom(departments_OrganizationUnits);
            f.OrganizationUnit = GetOrganizationUnitsFrom(departments_OrganizationUnits);

            f.CaseRemainingTime = frm.ReturnFormValue(CaseFilterFields.CaseRemainingTimeAttribute);

            if (!string.IsNullOrEmpty(f.CaseRemainingTime))
            {
                int remainingTimeId;
                if (int.TryParse(f.CaseRemainingTime, out remainingTimeId))
                {
                    var timeTable = GetRemainigTimeById((RemainingTimes)remainingTimeId);
                    if (timeTable != null)
                    {
                        f.CaseRemainingTimeFilter = timeTable.RemaningTime;
                        f.CaseRemainingTimeUntilFilter = timeTable.RemaningTimeUntil;
                        f.CaseRemainingTimeMaxFilter = timeTable.MaxRemaningTime;
                        f.CaseRemainingTimeHoursFilter = timeTable.IsHour;
                    }
                }
            }

            int caseRemainingTimeFilter;
            if (int.TryParse(frm.ReturnFormValue("CaseRemainingTime"), out caseRemainingTimeFilter))
            {
                f.CaseRemainingTimeFilter = caseRemainingTimeFilter;
            }

            int caseRemainingTimeUntilFilter;
            if (int.TryParse(frm.ReturnFormValue("CaseRemainingTimeUntil"), out caseRemainingTimeUntilFilter))
            {
                f.CaseRemainingTimeUntilFilter = caseRemainingTimeUntilFilter;
            }

            int caseRemainingTimeMaxFilter;
            if (int.TryParse(frm.ReturnFormValue("CaseRemainingTimeMax"), out caseRemainingTimeMaxFilter))
            {
                f.CaseRemainingTimeMaxFilter = caseRemainingTimeMaxFilter;
            }

            bool caseRemainingTimeHoursFilter;
            if (bool.TryParse(frm.ReturnFormValue("CaseRemainingTimeHours"), out caseRemainingTimeHoursFilter))
            {
                f.CaseRemainingTimeHoursFilter = caseRemainingTimeHoursFilter;
            }

            CaseSearchModel sm;
            if (SessionFacade.CurrentCaseSearch == null
                || SessionFacade.CurrentCaseSearch.CaseSearchFilter.CustomerId != f.CustomerId || f.IsConnectToParent)
            {
                sm = InitCaseSearchModel(f.CustomerId, f.UserId);
            }
            else
            {
                sm = SessionFacade.CurrentCaseSearch;
                f.CustomFilter = sm.CaseSearchFilter.CustomFilter;
            }

            ResolveParentPathesForFilter(f);
            sm.CaseSearchFilter = f;
            if (SessionFacade.CaseOverviewGridSettings == null)
            {
                if (f.IsConnectToParent)
                {
                    var connectToParentGrid = _gridSettingsService.GetForCustomerUserGrid(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id,
                        GridSettingsService.CASE_CONNECTPARENT_GRID_ID);
                    var userSelectedGrid = _gridSettingsService.GetForCustomerUserGrid(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);

                    var existingColumns = connectToParentGrid.columnDefs.Where(a => userSelectedGrid.columnDefs.Any(b => b.name == a.name)).ToList();
                    var otherColumns = userSelectedGrid.columnDefs.Where(a => !existingColumns.Any(b => b.name == a.name)).ToList();

                    var maxColumns = 7;
                    if (existingColumns.Count < maxColumns)
                    {
                        var missingAmount = Math.Abs(maxColumns - existingColumns.Count);
                        existingColumns.AddRange(otherColumns.Take(missingAmount));
                        connectToParentGrid.columnDefs = existingColumns;
                    }
                    SessionFacade.CaseOverviewGridSettings = connectToParentGrid;
                }
                else
                {
                    SessionFacade.CaseOverviewGridSettings = _gridSettingsService.GetForCustomerUserGrid(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);
                }

            }
            else
            {
                // TODO: (alexander.semenischev): put validation for sortOpt.sortBy
                var sortBy = frm.ReturnFormValue(CaseFilterFields.OrderColumnNum);
                var sort = frm.ReturnFormValue(CaseFilterFields.OrderColumnDir);
                var sortDir = !string.IsNullOrEmpty(sort)
                    ? GridSortOptions.SortDirectionFromString(sort)
                    : SortingDirection.Asc;


                if (sortBy != SessionFacade.CaseOverviewGridSettings.sortOptions.sortBy
                    || sortDir != SessionFacade.CaseOverviewGridSettings.sortOptions.sortDir)
                {
                    SessionFacade.CaseOverviewGridSettings.sortOptions.sortBy = sortBy;
                    SessionFacade.CaseOverviewGridSettings.sortOptions.sortDir = sortDir;
                    _gridSettingsService.SaveCaseoviewSettings(
                        SessionFacade.CaseOverviewGridSettings,
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.Id,
                        SessionFacade.CurrentUser.UserGroupId);
                }
            }

            var m = new CaseSearchResultModel
            {
                GridSettings = f.IsConnectToParent
                    ? _caseOverviewSettingsService.GetSettings(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id, GridSettingsService.CASE_CONNECTPARENT_GRID_ID)
                    : _caseOverviewSettingsService.GetSettings(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id)
            };


            var gridSettings = SessionFacade.CaseOverviewGridSettings;

            sm.Search.SortBy = gridSettings.sortOptions.sortBy;
            sm.Search.Ascending = gridSettings.sortOptions.sortDir == SortingDirection.Asc;

            m.caseSettings = _caseSettingService.GetCaseSettingsWithUser(f.CustomerId, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId);
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(f.CustomerId).ToArray();
            var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;
            CaseRemainingTimeData remainingTimeData;
            CaseAggregateData aggregateData;
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            f.MaxTextCharacters = MaxTextCharCount;

            //Show Parent/child icons with hint on Case overview
            f.FetchInfoAboutParentChild = true;

            int recPerPage;
            int pageStart;
            if (int.TryParse(frm.ReturnFormValue(CaseFilterFields.PageSize), out recPerPage) && int.TryParse(frm.ReturnFormValue(CaseFilterFields.PageStart), out pageStart))
            {
                f.PageInfo = new PageInfo
                {
                    PageSize = recPerPage,
                    PageNumber = recPerPage != 0 ? pageStart / recPerPage : 0
                };
                if (!f.IsConnectToParent)
                    SessionFacade.CaseOverviewGridSettings.pageOptions.recPerPage = recPerPage;
            }

            var currentUserId = SessionFacade.CurrentUser.Id;
            var customerId = SessionFacade.CurrentCustomer.Id;
            var customerUserSettings = _customerUserService.GetCustomerUserSettings(customerId, currentUserId);

            var searchResult = _caseSearchService.Search(
                f,
                m.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                customerUserSettings.RestrictedCasePermission,
                sm.Search,
                _workContext.Customer.WorkingDayStart,
                _workContext.Customer.WorkingDayEnd,
                userTimeZone,
                ApplicationTypes.Helpdesk,
                showRemainingTime,
                out remainingTimeData,
                out aggregateData);

            m.cases = searchResult.Items;

            m.cases = CommonHelper.TreeTranslate(m.cases, f.CustomerId, _productAreaService);
            sm.Search.IdsForLastSearch = GetIdsFromSearchResult(m.cases);

            var ouIds = f.OrganizationUnit.Split(',');
            if (ouIds.Any())
            {
                foreach (var id in ouIds)
                    if (!string.IsNullOrEmpty(id))
                    {
                        if (string.IsNullOrEmpty(sm.CaseSearchFilter.Department))
                            sm.CaseSearchFilter.Department += string.Format("-{0}", id);
                        else
                            sm.CaseSearchFilter.Department += string.Format(",-{0}", id);
                    }
            }

            if (!f.IsConnectToParent)
                SessionFacade.CurrentCaseSearch = sm;
            #endregion

            var customerSettings = GetCustomerSettings(f.CustomerId);

            var outputFormatter = new OutputFormatter(customerSettings.IsUserFirstLastNameRepresentation == 1, userTimeZone);

            var data = BuildSearchResultData(m.cases, gridSettings, outputFormatter);

            var remainingView = string.Empty;
            string statisticsView = null;

            if (!f.IsConnectToParent)
            {
                if (SessionFacade.CurrentUser.ShowSolutionTime)
                {
                    remainingView = RenderPartialViewToString("CaseRemainingTime", _caseModelFactory.GetCaseRemainingTimeModel(remainingTimeData, _workContext));
                }

                var statisticsFields = m.GridSettings.CaseFieldSettings.Where(cf => cf.ShowOnStartPage != 0 &&
                                                                                    (cf.Name == GlobalEnums.TranslationCaseFields.Status_Id.ToString() ||
                                                                                     cf.Name == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()))
                    .Select(cf => cf.Name).ToList();
                var showCaseStatistics = SessionFacade.CurrentUser.ShowCaseStatistics && statisticsFields.Any();
                if (showCaseStatistics)
                {
                    var expandedGroup = frm.ReturnFormValue("expandedGroup");
                    if (string.IsNullOrEmpty(expandedGroup))
                        expandedGroup = SessionFacade.CaseOverviewGridSettings.ExpandedStatistics;

                    statisticsView = this.RenderPartialViewToString("_Statistics", GetCaseStatisticsModel(aggregateData, searchResult.Count, expandedGroup, statisticsFields));
                }
            }

            sw.Stop();
            var duration = sw.ElapsedMilliseconds;
            return Json(new
            {
                result = "success",
                data = data,
                remainingView = remainingView,
                statisticsView = statisticsView,
                recordsTotal = searchResult.Count,
                recordsFiltered = searchResult.Count,
                processDuration = duration
            }, JsonRequestBehavior.AllowGet);
        }


        [ValidateInput(false)]
        public ActionResult SearchAjaxSimple(FormCollection frm)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            int selectedCustomer = Int32.Parse(frm.ReturnFormValue("lstfilterCustomersCustom"));

            var f = new CaseSearchFilter();
            f.CustomerId = selectedCustomer; //SessionFacade.CurrentCustomer.Id;
            f.UserId = SessionFacade.CurrentUser.Id;

            if (f.IsConnectToParent)
            {
                var id = frm.ReturnFormValue(CaseFilterFields.CurrentCaseId);
                int currentCaseId;
                if (!string.IsNullOrEmpty(id) && int.TryParse(id, out currentCaseId))
                {
                    f.CurrentCaseId = currentCaseId;
                }
            }


            f.FreeTextSearch = frm.ReturnFormValue(CaseFilterFields.FreeTextSearchNameAttribute);



            CaseSearchModel sm;
            sm = SessionFacade.CurrentCaseSearch;
            f.CustomFilter = sm.CaseSearchFilter.CustomFilter;

            ResolveParentPathesForFilter(f);
            sm.CaseSearchFilter = f;


            var sortBy = frm.ReturnFormValue(CaseFilterFields.OrderColumnNum);
            var sort = frm.ReturnFormValue(CaseFilterFields.OrderColumnDir);
            var sortDir = !string.IsNullOrEmpty(sort)
                ? GridSortOptions.SortDirectionFromString(sort)
                : SortingDirection.Asc;


            if (sortBy != SessionFacade.CaseOverviewGridSettings.sortOptions.sortBy
                || sortDir != SessionFacade.CaseOverviewGridSettings.sortOptions.sortDir)
            {
                SessionFacade.CaseOverviewGridSettings.sortOptions.sortBy = sortBy;
                SessionFacade.CaseOverviewGridSettings.sortOptions.sortDir = sortDir;
                _gridSettingsService.SaveCaseoviewSettings(
                    SessionFacade.CaseOverviewGridSettings,
                    selectedCustomer, //SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentUser.Id,
                    SessionFacade.CurrentUser.UserGroupId);
            }

            var m = new CaseSearchResultModel
            {
                GridSettings = f.IsConnectToParent
                    ? _caseOverviewSettingsService.GetSettings(
                        selectedCustomer, //SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id, GridSettingsService.CASE_CONNECTPARENT_GRID_ID)
                    : _caseOverviewSettingsService.GetSettings(
                        selectedCustomer, //SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id)
            };


            var gridSettings = SessionFacade.CaseOverviewGridSettings;

            sm.Search.SortBy = gridSettings.sortOptions.sortBy;
            sm.Search.Ascending = gridSettings.sortOptions.sortDir == SortingDirection.Asc;

            m.caseSettings = _caseSettingService.GetCaseSettingsWithUser(selectedCustomer /*f.CustomerId*/, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId);

            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(selectedCustomer/*f.CustomerId*/).ToArray();
            var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;
            CaseRemainingTimeData remainingTimeData;
            CaseAggregateData aggregateData;
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            f.MaxTextCharacters = MaxTextCharCount;

            //Show Parent/child icons with hint on Case overview
            f.FetchInfoAboutParentChild = true;

            int recPerPage;
            int pageStart;
            if (int.TryParse(frm.ReturnFormValue(CaseFilterFields.PageSize), out recPerPage) && int.TryParse(frm.ReturnFormValue(CaseFilterFields.PageStart), out pageStart))
            {
                f.PageInfo = new PageInfo
                {
                    PageSize = recPerPage,
                    PageNumber = recPerPage != 0 ? pageStart / recPerPage : 0
                };
                if (!f.IsConnectToParent)
                    SessionFacade.CaseOverviewGridSettings.pageOptions.recPerPage = recPerPage;
            }

            var currentUserId = SessionFacade.CurrentUser.Id;
            var customerId = selectedCustomer;//SessionFacade.CurrentCustomer.Id;
            var customerUserSettings = _customerUserService.GetCustomerUserSettings(customerId, currentUserId);

            var searchResult = _caseSearchService.Search(
                f,
                m.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                customerUserSettings.RestrictedCasePermission,
                sm.Search,
                _workContext.Customer.WorkingDayStart,
                _workContext.Customer.WorkingDayEnd,
                userTimeZone,
                ApplicationTypes.Helpdesk,
                showRemainingTime,
                out remainingTimeData,
                out aggregateData);

            m.cases = searchResult.Items;

            m.cases = CommonHelper.TreeTranslate(m.cases, f.CustomerId, _productAreaService);
            sm.Search.IdsForLastSearch = GetIdsFromSearchResult(m.cases);




            var customerSettings = GetCustomerSettings(f.CustomerId);

            var outputFormatter = new OutputFormatter(customerSettings.IsUserFirstLastNameRepresentation == 1, userTimeZone);

            var data = BuildSearchResultData(m.cases, gridSettings, outputFormatter);

            var remainingView = string.Empty;
            string statisticsView = null;



            sw.Stop();
            var duration = sw.ElapsedMilliseconds;
            return Json(new
            {
                result = "success",
                data = data,
                remainingView = remainingView,
                statisticsView = statisticsView,
                recordsTotal = searchResult.Count,
                recordsFiltered = searchResult.Count,
                processDuration = duration
            });
        }

        private IList<Dictionary<string, object>> BuildSearchResultData(IList<CaseSearchResult> caseSearchResults, GridSettingsModel gridSettings, OutputFormatter outputFormatter)
        {
            var data = new List<Dictionary<string, object>>();
            var ids = caseSearchResults.Select(o => o.Id).ToArray();
            var globalSettings = _globalSettingService.GetGlobalSettings().FirstOrDefault();

            var casesLocks = _caseLockService.GetLockedCasesToOverView(ids, globalSettings, this.DefaultCaseLockBufferTime).ToList();

            var workinggroupsForUserAndCustomer = _userService
                                    .GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, SessionFacade.CurrentCustomer.Id);

            foreach (var searchRow in caseSearchResults)
            {
                var caseId = searchRow.Id;

                var jsRow = new Dictionary<string, object>
                {
                    {"case_id", searchRow.Id},
                    {"caseIconTitle", searchRow.CaseIcon.CaseIconTitle()},
                    {"caseIconUrl", $"/Content/icons/{searchRow.CaseIcon.CaseIconSrc()}"},
                    {"isUnread", searchRow.IsUnread},
                    {"isUrgent", searchRow.IsUrgent},
                    {"isClosed", searchRow.IsClosed},
                    {"isParent", searchRow.IsParent},
                    {"ParentId", searchRow.ParentId},
                    {"IsMergeParent", searchRow.IsMergeParent},
                    {"IsMergeChild", searchRow.IsMergeChild},
                    {"case_caption", searchRow.CaseCaption }
                };

                var caseLock = casesLocks.Where(x => x.CaseId == caseId).FirstOrDefault();

                //this specific case is locked
                if (caseLock != null)
                {
                    jsRow.Add("isCaseLocked", true);
                    jsRow.Add("caseLockedIconTitle", $"{caseLock.User.FirstName} {caseLock.User.LastName} ({caseLock.User.UserId})");
                    jsRow.Add("caseLockedIconUrl", $"/Content/icons/{CaseIcon.Locked.CaseIconSrc()}");
                }

                if(searchRow.ExtendedSearchInfo != null)
                {
                    var wg = workinggroupsForUserAndCustomer.FirstOrDefault(x => x.WorkingGroup_Id == searchRow.ExtendedSearchInfo.WorkingGroupId);
                    if (wg != null)
                    {
                        var cc = _caseService.GetCaseById(caseId);
                        var accessMode = CalcEditMode(searchRow.ExtendedSearchInfo.CustomerId, SessionFacade.CurrentUser.Id, cc);

                        if (!wg.IsMemberOfGroup && wg.WorkingGroup_Id > 0 
                            && accessMode != AccessMode.FullAccess)
                        {
                            jsRow.Add("isNotMemberOfGroup", true);
                        }
                    }
                }

                foreach (var col in gridSettings.columnDefs)
                {
                    var searchCol = searchRow.Columns.FirstOrDefault(it => it.Key == col.name);
                    if (searchCol != null)
                    {
                        if (searchCol.Key == "Description")
                        {
                            jsRow.Add(col.name, outputFormatter.StripHTML(searchCol.StringValue));
                        }
                        else
                        {
                            jsRow.Add(col.name, outputFormatter.FormatField(searchCol));
                        }
                    }
                    else
                    {
                        jsRow.Add(col.name, string.Empty);
                    }

                }

                data.Add(jsRow);
            }

            return data;
        }

        #endregion

        #region --Favorite--

        [HttpPost]
        public JsonResult SaveFavorite(int favoriteId, string favoriteName, MyFavoriteFilterJSFields favoriteFilter)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
                return Json("Please logout and login again!");

            var jsFilter = new MyFavoriteFilterJSModel(favoriteId, favoriteName, favoriteFilter);

            var favoriteBM = new CaseFilterFavorite(jsFilter.Id,
                                                       SessionFacade.CurrentCustomer.Id,
                                                       SessionFacade.CurrentUser.Id,
                                                       jsFilter.Name,
                                                       jsFilter.GetFavoriteFields());

            return Json(_caseService.SaveFavorite(favoriteBM));
        }

        [HttpPost]
        public JsonResult DeleteFavorite(int favoriteId)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
                return Json("Please logout and login again!");

            return Json(_caseService.DeleteFavorite(favoriteId));
        }

        [HttpPost]
        public JsonResult LoadFavorites()
        {
            var favorites = new List<MyFavoriteFilterJSModel>();
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
                return Json(new List<MyFavoriteFilterJSModel>());
            else
                return Json(GetMyFavorites(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id));
        }

        #endregion

        #region --Case Lock--

        public JsonResult UnLockCase(string lockGUID)
        {
            if (!string.IsNullOrEmpty(lockGUID))
                _caseLockService.UnlockCaseByGUID(new Guid(lockGUID));
            return Json("Success");
        }

        public JsonResult UnLockCaseByCaseId(int caseId)
        {
            _caseLockService.UnlockCaseByCaseId(caseId);
            return Json("Success");
        }

        public JsonResult IsCaseAvailable(int caseId, DateTime caseChangedTime, string lockGuid)
        {
            var caseLock = _caseLockService.GetCaseLockOverviewByCaseId(caseId);

            if (caseLock != null &&
                caseLock.LockGUID == new Guid(lockGuid) &&
                caseLock.ExtendedTime >= DateTime.Now)
            {
                // Case still is locked by me
                return Json(true);
            }

            if (caseLock == null || !(caseLock.ExtendedTime >= DateTime.Now))
            {
                //case is not locked by me or is not locked at all 
                var curCase = _caseService.GetDetachedCaseById(caseId);
                if (curCase != null && curCase.ChangeTime.RoundTick() == caseChangedTime.RoundTick())
                    return Json(true);//case is not updated yet by any other
                return Json(false);
            }

            return Json(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockGuid"></param>
        /// <param name="extendValue"></param>
        /// <param name="caseId"></param>
        /// <returns>empty string if success. Name of user who locked the case if not</returns>
        public JsonResult ReExtendCaseLock(string lockGuid, int extendValue, int caseId)
        {
            var isExtended = _caseLockService.ReExtendLockCase(new Guid(lockGuid), extendValue);
            if (!isExtended)
            {
                var lockInfo = _caseLockService.GetCaseLock(caseId);
                if (lockInfo != null)
                {
                    return Json(
                        $"{lockInfo.User.FirstName ?? ""} {lockInfo.User.SurName ?? ""} ({lockInfo.User.UserID ?? ""})");
                }
            }
            return Json(string.Empty);
        }

        #endregion

        #region --Case Save/Delete--

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult New(CaseEditInput m, int? templateId)
        {
            int caseId = Save(m);
            CheckTemplateParameters(templateId, caseId);
            return RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = m.updateNotifierInformation, activeTab = m.ActiveTab });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectResult NewAndClose(CaseEditInput m, int? templateId, string BackUrl)
        {
#pragma warning disable 0618
            var newChild = m.case_.Id == 0 && m.ParentId != null;
#pragma warning restore 0618

            m.ActiveTab = "";

            int caseId = this.Save(m);
            CheckTemplateParameters(templateId, caseId);
            string url;

            if (BackUrl == null)
                BackUrl = "";


            if (BackUrl != "")
            {
                url = BackUrl;
            }
            else if (m.ParentId.HasValue && newChild)
            {
                url = this.GetLinkWithHash(ChildCasesHashTab, new { id = m.ParentId }, "Edit");
            }
            else
            {
#pragma warning disable 0618
                url = this.GetLinkWithHash(string.Empty, new { customerId = m.case_.Customer_Id }, "Index");
#pragma warning restore 0618
            }

            return this.Redirect(url);
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult NewAndAddCase(CaseEditInput m, int? templateId)
        {
            int caseId = this.Save(m);
            CheckTemplateParameters(templateId, caseId);
#pragma warning disable 0618
            return this.RedirectToAction("new", "cases", new { customerId = m.case_.Customer_Id });
#pragma warning restore 0618
        }

        [UserCasePermissions]
        [HttpPost]
        public JsonResult EditCaseProperties(BulkEditCase inputData)
        {
            var caseToUpdate = _caseService.GetCaseById(inputData.Id);
            var oldCase = new Case();
            var caseLog = new CaseLog();
            oldCase = _caseService.GetDetachedCaseById(inputData.Id);

            CaseLockModel caseLockViewModel = null;

            try
            {
                if (caseToUpdate == null)
                {
                    string errorMsg = Translation.Get("Ärendet hittades inte!");
                    throw new Exception(errorMsg);
                }

                if (SessionFacade.CurrentUser == null)
                {
                    string errorMsg = Translation.Get("Åtkomst nekad");
                    throw new Exception(errorMsg);
                }

                var userId = SessionFacade.CurrentUser.Id;

                caseLockViewModel = GetCaseLockModel(inputData.Id, userId, true);

                if(caseLockViewModel != null)
                {
                    if(caseLockViewModel.IsLocked) 
                    {
                        //string errorMsg = Translation.GetCoreTextTranslation("Låst");

                        string errorMsg = Translation.GetCoreTextTranslation("OBS! Detta ärende är öppnat av") + " <name>.";
                        errorMsg = errorMsg.Replace("<name>", caseLockViewModel.User.FirstName + " " + caseLockViewModel.User.SurName);
                        throw new Exception(errorMsg);
                    }
                }

                IDictionary<string, string> errors;
                var mailSenders = new MailSenders();
                var customer = _customerService.GetCustomer(caseToUpdate.Customer_Id);
                var caseMailSetting = new CaseMailSetting(string.Empty, customer.HelpdeskEmail, RequestExtension.GetAbsoluteUrl(), 1);

                mailSenders.SystemEmail = caseMailSetting.HelpdeskMailFromAdress;

                // Positive: Send Mail to...
                //caseMailSetting.DontSendMailToNotifier = caseMailSetting.DontSendMailToNotifier == false;

                if (caseToUpdate.DefaultOwnerWG_Id.HasValue && caseToUpdate.DefaultOwnerWG_Id.Value > 0)
                {
                    var defaultWGEmail = _workingGroupService.GetWorkingGroup(caseToUpdate.DefaultOwnerWG_Id.Value).EMail;
                    mailSenders.DefaultOwnerWGEMail = defaultWGEmail;
                }

                bool updateCase = false;

                if (inputData.WorkingGroup_Id > 0 && caseToUpdate.WorkingGroup_Id != inputData.WorkingGroup_Id)
                {
                    var curWG = _workingGroupService.GetWorkingGroup(inputData.WorkingGroup_Id);
                    if (curWG != null)
                        if (!string.IsNullOrWhiteSpace(curWG.EMail) && _emailService.IsValidEmail(curWG.EMail))
                            mailSenders.WGEmail = curWG.EMail;

                    caseToUpdate.WorkingGroup_Id = inputData.WorkingGroup_Id;
                    updateCase = true;
                }

                if (inputData.Performer_User_Id > 0 && caseToUpdate.Performer_User_Id != inputData.Performer_User_Id)
                {
                    caseToUpdate.Performer_User_Id = inputData.Performer_User_Id;
                    updateCase = true;
                }

                //if (inputData.Priority_Id > 0 && caseToUpdate.Priority_Id != inputData.Priority_Id)
                //{
                //    caseToUpdate.Priority_Id = inputData.Priority_Id;
                //    updateCase = true;
                //}

                if (inputData.StateSecondary_Id > 0 && caseToUpdate.StateSecondary_Id != inputData.StateSecondary_Id)
                {
                    caseToUpdate.StateSecondary_Id = inputData.StateSecondary_Id;
                    updateCase = true;
                }

                if (inputData.Problem_Id > 0 && caseToUpdate.Problem_Id != inputData.Problem_Id)
                {
                    caseToUpdate.Problem_Id = inputData.Problem_Id;
                    updateCase = true;
                }

                if (!String.IsNullOrEmpty(inputData.FinishDescription) && caseToUpdate.FinishingDescription != inputData.FinishDescription)
                {
                    caseToUpdate.FinishingDescription = inputData.FinishDescription;
                    updateCase = true;
                }
                

                if (inputData.FinishTypeId > 0)
                {
                    var lastLog = caseToUpdate.Logs.OrderByDescending(o => o.Id).FirstOrDefault() ?? new Log();
                    if (!lastLog.FinishingType.HasValue || lastLog.FinishingType.Value != inputData.FinishTypeId)
                    {

                        var childrenCases = _caseService.GetChildCasesFor(inputData.Id);

                        if (childrenCases != null)
                        {
                            var childrenNotIncludedForDeletion = childrenCases.Where(x=> !inputData.CasesToBeUpdated.Contains(x.Id));
                            if(childrenNotIncludedForDeletion.Any(x=> x.ClosingDate == null && !x.Indepandent))
                            {
                                string errorMsg = Translation.Get("Ärendet har underärende som ej är avslutade");
                                throw new Exception(errorMsg);
                            }
                        }

                        caseLog.CaseId = inputData.Id;
                        caseLog.FinishingType = inputData.FinishTypeId;

                        DateTime validatedDate;
                        DateTime finishDate = DateTime.UtcNow;
                        if (DateTime.TryParse(inputData.FinishDate.ToString(), out validatedDate))
                        {
                            finishDate = validatedDate;
                        }
                        caseLog.FinishingDate = finishDate;
                        caseToUpdate.FinishingDate = finishDate;
                        updateCase = true;
                    }
                }

                if (updateCase)
                {
                    caseMailSetting.CustomeMailFromAddress = mailSenders;

                    var currentLoggedInUser = _userService.GetUser(SessionFacade.CurrentUser.Id);
                    var basePath = _masterDataService.GetFilePath(caseToUpdate.Customer_Id);
                    var caseHistoryId = _caseService.SaveCase(caseToUpdate, caseLog, SessionFacade.CurrentUser.Id, User.Identity.Name, new CaseExtraInfo(), out errors);
                    caseLog.CaseHistoryId = caseHistoryId;
                    var logId = _logService.SaveLog(caseLog, 0, out errors);
                    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
                    // send emails
                    _caseService.SendCaseEmail(caseToUpdate.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase, caseLog, null, currentLoggedInUser);
                }

                //if (caseLockViewModel.IsLocked && !string.IsNullOrEmpty(caseLockViewModel.LockGUID))
                //{
                //_caseLockService.UnlockCaseByCaseId(caseToUpdate.Id);
                //_caseLockService.UnlockCaseByGUID(new Guid(caseLockViewModel.LockGUID));
                //}

                return Json(new CaseOperationResult() { Success = true, Message = Translation.Get("Uppdaterad", Enums.TranslationSource.TextTranslation), CaseId = inputData.Id, CaseNumber = caseToUpdate.CaseNumber.ToString()} );
            }

            catch(Exception e)
            {
                return Json(new CaseOperationResult() { Success = false, Message = e.Message, CaseId = inputData.Id, CaseNumber = caseToUpdate.CaseNumber.ToString() });
            }
            finally
            {
                var unlocked = false;
                if(caseLockViewModel != null)
                {
                    if (!string.IsNullOrEmpty(caseLockViewModel.LockGUID))
                    {
                        _caseLockService.UnlockCaseByGUID(new Guid(caseLockViewModel.LockGUID));
                        unlocked = true;
                    }
                }
                if (!unlocked && caseToUpdate != null)
                {
                    _caseLockService.UnlockCaseByCaseId(caseToUpdate.Id);
                }
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult Edit(CaseEditInput m)
        {
            // Save current case
            int caseId = Save(m);
            #region Case Split

            // Create a split child if specified
            if (m.SplitToCaseSolution_Id.HasValue)
            {
                if (SessionFacade.CurrentCustomer == null)
                {
                    return this.RedirectToAction("~/Error/Unathorized");
                }

                var customerId = SessionFacade.CurrentCustomer.Id;

                SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
                if (SessionFacade.CurrentUser != null)
                {
                    var userId = SessionFacade.CurrentUser.Id;
                    var caseLockModel = new CaseLockModel();
                    caseLockModel.ActiveTab = "non";
                    var customerCaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

                    var template = _caseSolutionService.GetCaseSolution(m.SplitToCaseSolution_Id.Value);

                    var identity = global::System.Security.Principal.WindowsIdentity.GetCurrent();
                    var windowsUser = identity != null ? identity.Name : null;
                    var child = _caseService.Copy(
                            caseId,
                            userId,
                            SessionFacade.CurrentLanguageId,
                            this.Request.GetIpAddress(),
                            CaseRegistrationSource.Administrator,
                            windowsUser);

                    child.Id = 0;


                    if (template.OU_Id.HasValue)
                        child.OU_Id = template.OU_Id.Value;

                    if (template.Status_Id.HasValue)
                        child.Status_Id = template.Status_Id.Value;

                    if (template.ProductArea_Id.HasValue)
                        child.ProductArea_Id = template.ProductArea_Id.Value;

                    if (template.Priority_Id.HasValue)
                        child.Priority_Id = template.Priority_Id.Value;

                    if (template.StateSecondary_Id.HasValue)
                        child.StateSecondary_Id = template.StateSecondary_Id.Value;

                    if (template.CaseWorkingGroup_Id.HasValue)
                        child.WorkingGroup_Id = template.CaseWorkingGroup_Id.Value;

                    IDictionary<string, string> errors; // = new Dictionary<string, string>();

                    var parentCase = _caseService.GetCaseById(caseId);

                    _caseService.SaveCase(child,
                        new CaseLog(),
                        userId,
                        windowsUser,
                        new CaseExtraInfo(),
                        out errors,
                        parentCase);

                    _caseService.SetIndependentChild(child.Id, true);


                    var data = _extendedCaseService.GetExtendedCaseFromCase(parentCase.Id);
                    if (data != null)
                        _extendedCaseService.CopyExtendedCaseToCase(data.Id, child.Id, SessionFacade.CurrentUser.UserId);

                    //editModel.case_.Ou = null;

                    ///this.Save(m);


                    //var input = this.GetCaseInputViewModel(0, 0, 0, null, null, null, null, m.SplitToCaseSolution_Id.Value, caseId, true, 1, caseId);

                }
                else
                {
                    // Todo: proper error handling
                    this.RedirectToAction("~/Error/Unathorized");
                }

                // Create new case
            }

            #endregion

            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = m.updateNotifierInformation, updateState = false, activeTab = m.ActiveTab });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectResult EditAndClose(CaseEditInput m, string BackUrl)
        {
            m.ActiveTab = "";
            this.Save(m);
#pragma warning disable 0618
            return string.IsNullOrEmpty(BackUrl) ? this.Redirect(Url.Action("index", "cases", new { customerId = m.case_.Customer_Id })) : this.Redirect(BackUrl);
#pragma warning restore 0618
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult EditAndAddCase(CaseEditInput m)
        {
            this.Save(m);
#pragma warning disable 0618
            return this.RedirectToAction("new", "cases", new { customerId = m.case_.Customer_Id });
#pragma warning restore 0618
        }

        [HttpPost]
        public RedirectResult DeleteCase(
            int caseId,
            int customerId,
            int? parentCaseId,
            string backUrl
            )
        {
            var basePath = _masterDataService.GetFilePath(customerId);
            var caseGuid = _caseDeletionService.Delete(caseId, basePath, parentCaseId);
            _userTemporaryFilesStorage.ResetCacheForObject(caseGuid.ToString());
            if (parentCaseId.HasValue)
            {
                var parentCase = _caseService.GetCaseById(parentCaseId.Value);
                if (parentCase.Performer_User_Id == SessionFacade.CurrentUser.Id)
                {
                    var url = this.GetLinkWithHash(ChildCasesHashTab, new { id = parentCaseId.Value }, "Edit");
                    return this.Redirect(url);
                }
            }
            return string.IsNullOrEmpty(backUrl) ? this.Redirect(Url.Action("index", "cases", new { customerId = customerId })) : this.Redirect(backUrl);
        }

        //TODO: REVIEW 
        [HttpPost]
        public RedirectToRouteResult DeleteLog(int id, int caseId)
        {
            var currentuser = _userService.GetUser(SessionFacade.CurrentUser.Id);
            string err = "";

            var tmpLog = _logService.GetLogById(id);
            var logFiles = _logService.GetLogFilesByLogId(id);

            var logFileStr = string.Empty;
            if (logFiles.Any())
            {
                if (currentuser.DeleteAttachedFilePermission != 0)
                {
                    logFileStr = string.Format("{0}{1}", StringTags.LogFile, string.Join(StringTags.Seperator, logFiles.Select(f => f.FileName).ToArray()));
                }
                else
                {
                    err = Translation.GetCoreTextTranslation("Du kan inte ta bort noteringen, eftersom du saknar behörighet att ta bort bifogade filer") + ".";
                    TempData["PreventError"] = err;
                    return this.RedirectToAction("editlog", "cases", new { id = id, customerId = SessionFacade.CurrentCustomer.Id });
                }
            }

            var mail2tickets = _mail2TicketService.GetCaseMail2Tickets(caseId);
            var mail2Ticket = mail2tickets.Where(x => x.Log_Id == id);
            if (mail2Ticket != null)
                _mail2TicketService.DeleteByLogId(id);

            var c = _caseService.GetCaseById(caseId);
            var basePath = _masterDataService.GetFilePath(c.Customer_Id);
            var logGuid = _logService.Delete(id, basePath);
            LogLogFilesDelete(caseId, logFiles, c, basePath);
            _userTemporaryFilesStorage.ResetCacheForObject(logGuid.ToString());

            IDictionary<string, string> errors;
            string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            var logStr = string.Format("{0} {1} {2} {3} {4} {5}",
                                        StringTags.Delete,
                                        StringTags.ExternalLog,
                                        tmpLog.TextExternal,
                                        StringTags.InternalLog,
                                        tmpLog.TextInternal,
                                        logFileStr);

            var extraField = new ExtraFieldCaseHistory { CaseLog = logStr };
            _caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, CreatedByApplications.Helpdesk5, out errors, string.Empty, extraField);

            return this.RedirectToAction("edit", "cases", new { id = caseId });

        }

        private void LogLogFilesDelete(int caseId, List<LogFile> logFiles, Case c, string basePath)
        {
            if (logFiles.Any())
            {
                if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
                {
                    foreach (var f in logFiles)
                    {
                        if (!f.ParentLog_Id.HasValue) // delete only non reference files
                        {
                            var subFolder = f.GetFolderPrefix();
                            var path = _filesStorage.ComposeFilePath(subFolder, decimal.ToInt32(c.CaseNumber), basePath,
                                "");
                            _fileViewLogService.Log(caseId, SessionFacade.CurrentUser.Id, f.FileName, path,
                                FileViewLogFileSource.Helpdesk,
                                FileViewLogOperation.Delete);
                        }
                    }
                }
            }
        }
        #endregion

        #region --New/Edit--

        public ActionResult New(
           int? customerId,
           int? templateId,
           int? copyFromCaseId,
           int? caseLanguageId,
           int? templateistrue)
        {
            CaseInputViewModel m = null;
            ViewBag.Title = " " + Translation.GetForJS("Nytt ärende", Enums.TranslationSource.TextTranslation);
            if (!customerId.HasValue)
            {
                if (SessionFacade.CurrentCustomer == null)
                {
                    return new RedirectResult("~/Error/Unathorized");
                }

                customerId = SessionFacade.CurrentCustomer.Id;
            }

            //Check if customer has a default casetemplate
            if (!templateId.HasValue && customerId.HasValue)
            {
                var setting = GetCustomerSettings(customerId.Value);
                if (setting.DefaultCaseTemplateId != 0)
                {
                    templateId = setting.DefaultCaseTemplateId;
                    templateistrue = 1;
                }
            }

            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            if (SessionFacade.CurrentUser != null)
            {
                if (SessionFacade.CurrentUser.CreateCasePermission == 1 || templateistrue == 1)
                {
                    var userId = SessionFacade.CurrentUser.Id;
                    var caseLockModel = new CaseLockModel();
                    caseLockModel.ActiveTab = (templateId.HasValue ? _caseSolutionService.GetCaseSolution(templateId.Value).DefaultTab : "case-tab");
                    var customerCaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId.Value);

                    var activeTab = GetActiveTab(templateId, 0);

                    m = this.GetCaseInputViewModel(
                        userId,
                        customerId.Value,
                        0,
                        caseLockModel,
                        customerCaseFieldSettings,
                        string.Empty,
                        null,
                        templateId,
                        copyFromCaseId,
                        false,
                        templateistrue, null, activeTab);

                    var caseParam = new NewCaseParams
                    {
                        customerId = customerId.Value,
                        templateId = templateId,
                        copyFromCaseId = copyFromCaseId,
                        caseLanguageId = caseLanguageId
                    };

                    m.NewModeParams = caseParam;
                    AddViewDataValues();

                    // Positive: Send Mail to...
                    if (m.CaseMailSetting.DontSendMailToNotifier == false) m.CaseMailSetting.DontSendMailToNotifier = true;
                    else m.CaseMailSetting.DontSendMailToNotifier = false;

                    var moduleCaseInvoice = GetCustomerSettings(customerId.Value).ModuleCaseInvoice;
                    if (moduleCaseInvoice == 1)
                    {
#pragma warning disable 0618
                        var caseInvoices = _invoiceArticleService.GetCaseInvoicesWithTimeZone(m.case_.Id, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                        var invoiceArticles = _invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
                        m.InvoiceModel = new CaseInvoiceModel(customerId.Value, m.case_.Id, invoiceArticles, "", m.CaseKey, m.LogKey);
#pragma warning restore 0618
                    }

                    return this.View(m);
                }
            }

            return this.RedirectToAction("index", "cases", new { id = customerId });
        }

        [UserCasePermissions]
        public ActionResult Edit(int id,
            string redirectFrom = "",
            int? moveToCustomerId = null,
            bool? uni = null,
            bool updateState = true,
            string backUrl = null,
            bool retToCase = true,
            string activeTab = "")
        {
            CaseInputViewModel m = null;
            var c = _caseService.GetCaseById(id);
            int caseNumber = Convert.ToInt32(c.CaseNumber);

            int maxIndex = c.Caption.Length < 10 ? c.Caption.Length : 10;
            string caseDescription = c.Caption.Substring(0, maxIndex);
            //ViewBag.Title = " " + caseNumber;
            ViewBag.Title = " " + caseNumber + " " + caseDescription;

            if (!_caseService.IsCaseExist(id))
            {
                TempData["NotFound"] = Translation.Get("Ärendet hittades inte!");
                return View(m);
            }

            if (SessionFacade.CurrentUser != null)
            {

                //var isAuthorised = _userService.VerifyUserCasePermissions(SessionFacade.CurrentUser, id);
                //if(!isAuthorised)
                //{
                //    return new RedirectResult("~/Error/Unathorized");
                //}
                /* Used for Extended Case */
                TempData["Case_Id"] = id;

                _userTemporaryFilesStorage.ResetCacheForObject(id);

                var userId = SessionFacade.CurrentUser.Id;

                var caseLockViewModel = GetCaseLockModel(id, userId, true, activeTab);

                //todo: check if GetCaseById can be used in model!
                var customerId = moveToCustomerId.HasValue ? moveToCustomerId.Value : _caseService.GetCaseCustomerId(id);



                var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
                m = this.GetCaseInputViewModel(userId, customerId, id, caseLockViewModel, caseFieldSettings, redirectFrom, backUrl, null, null, updateState);

                m.NumberOfCustomers = _masterDataService.GetCustomers(userId).Count;
                

                m.ActiveTab = (!string.IsNullOrEmpty(caseLockViewModel.ActiveTab) ? caseLockViewModel.ActiveTab : activeTab);
#pragma warning disable 0618
                m.ActiveTab = (m.ActiveTab == "") ? GetActiveTab(m.case_.CaseSolution_Id, id) : activeTab; //Fallback to casesolution
#pragma warning restore 0618

                if (uni.HasValue)
                {
                    m.UpdateNotifierInformation = uni.Value;
                }

                //if move case always fullaccess
                if (moveToCustomerId.HasValue)
                    m.EditMode = AccessMode.FullAccess;

                ApplicationFacade.UpdateLoggedInUser(Session.SessionID, string.Empty);

                // If user logged in from link in email
                var currentCustomerId = SessionFacade.CurrentCustomer.Id;
                var currentCase = _caseService.GetCaseById(id);
                if (currentCustomerId != currentCase.Customer_Id)
                    this.InitFilter(currentCase.Customer_Id, false, CasesCustomFilter.None, false, true);

                //Get order
                m.OrderId = _orderService.GetOrderByCase(id)?.Id;

                //Get account id
                var account = _orderAccountService.GetAccountByCaseNumber(currentCase.CaseNumber);
                if (account != null)
                {
                    m.AccountId = account.Item1;
                    m.AccountActivityId = account.Item2;
                }

                // User has not access to case
                if (m.EditMode == AccessMode.NoAccess)
                    return this.RedirectToAction("index", "home");

                // move case to another customer
                if (moveToCustomerId.HasValue)
                {
#pragma warning disable 0618
                    m.MovedFromCustomerId = m.case_.Customer_Id;
                    m.case_.Customer_Id = moveToCustomerId.Value;
                    m.case_.CaseType_Id = 0;
                    m.case_.CaseSolution_Id = 0;
                    currentCase.CaseSolution_Id = 0;
                    m.ActiveTab = "";
                    m.ContainsExtendedCase = false;
                    m.case_.ProductArea_Id = null;
                    m.case_.Category_Id = null;
                    m.case_.Change_Id = null;
                    m.case_.ChangeByUser_Id = null;
                    m.case_.Department_Id = null;
                    m.case_.Impact_Id = null;
                    m.case_.LockCaseToWorkingGroup_Id = null;
                    m.case_.OU_Id = null;
                    m.case_.Performer_User_Id = 0;
                    m.case_.Priority_Id = null;
                    m.case_.Problem_Id = null;
                    m.case_.ProductAreaQuestionVersion_Id = null;
                    m.case_.ProductAreaSetDate = null;
                    m.case_.Project_Id = null;
                    m.case_.Region_Id = null;
                    m.case_.StateSecondary_Id = null;
                    m.case_.Status_Id = null;
                    m.case_.Supplier_Id = null;
                    m.case_.System_Id = null;
                    m.case_.Urgency_Id = null;
                    m.case_.User_Id = null;
                    m.case_.WorkingGroup_Id = null;
                    m.ParantPath_CaseType = ParentPathDefaultValue;
                    m.ParantPath_ProductArea = ParentPathDefaultValue;
                    m.ParantPath_Category = ParentPathDefaultValue;
                    m.ParantPath_OU = ParentPathDefaultValue;

                }
                
                var caseCustomerSettings = GetCustomerSettings(m.case_.Customer_Id);
                var moduleCaseInvoice = caseCustomerSettings.ModuleCaseInvoice;
                if (moduleCaseInvoice == 1)
                {
                    var caseInvoices = _invoiceArticleService.GetCaseInvoicesWithTimeZone(id, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    var invoiceArticles = _invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
                    m.InvoiceModel = new CaseInvoiceModel(m.case_.Customer_Id, m.case_.Id, invoiceArticles, "", m.CaseKey, m.LogKey);
                }

                m.CustomerSettings = _workContext.Customer.Settings; //current customer settings
                m.CustomerSettings.ModuleCaseInvoice = Convert.ToBoolean(caseCustomerSettings.ModuleCaseInvoice); // TODO FIX
                Customer cus = _customerService.GetCustomer(m.case_.Customer_Id);
                m.CaseLog.AutoCheckPerformerCheckbox = cus.CommunicateWithPerformer.ToBool();
                m.CaseLog.AutoCheckNotifyerCheckbox = cus.CommunicateWithNotifier.ToBool();
#pragma warning restore 0618
                #region ConnectToParentModel

                m.ConnectToParentModel = new JsonCaseIndexViewModel();
                if (!m.IsItChildCase())
                {
                    var customerUser = _customerUserService.GetCustomerUserSettings(currentCustomerId, userId);
                    m.ConnectToParentModel.CaseSearchFilterData = this.CreateCaseSearchFilterData(currentCustomerId, SessionFacade.CurrentUser, customerUser, this.InitCaseSearchModel(customerId, userId));

                    //todo:
                    m.ConnectToParentModel.CaseSetting = this.GetCaseSettingModel(currentCustomerId, userId, GridSettingsService.CASE_CONNECTPARENT_GRID_ID, caseFieldSettings);

                    var gridSettings = _gridSettingsService.GetForCustomerUserGrid(
                        customerId,
                        SessionFacade.CurrentUser.UserGroupId,
                        userId,
                        GridSettingsService.CASE_CONNECTPARENT_GRID_ID);

                    var connectToParentGrid = gridSettings;
                    var userSelectedGrid = _gridSettingsService.GetForCustomerUserGrid(
                            SessionFacade.CurrentCustomer.Id,
                            SessionFacade.CurrentUser.UserGroupId,
                            SessionFacade.CurrentUser.Id,
                            GridSettingsService.CASE_OVERVIEW_GRID_ID);

                    var existingColumns = connectToParentGrid.columnDefs.Where(a => userSelectedGrid.columnDefs.Any(b => b.name == a.name)).ToList();
                    var otherColumns = userSelectedGrid.columnDefs.Where(a => !existingColumns.Any(b => b.name == a.name)).ToList();

                    var maxColumns = 7;
                    if (existingColumns.Count < maxColumns)
                    {
                        var missingAmount = Math.Abs(maxColumns - existingColumns.Count);
                        existingColumns.AddRange(otherColumns.Take(missingAmount));
                        connectToParentGrid.columnDefs = existingColumns;
                    }

                    gridSettings = connectToParentGrid;

                    m.ConnectToParentModel.PageSettings = new PageSettingsModel()
                    {
                        searchFilter = JsonCaseSearchFilterData.MapFrom(m.ConnectToParentModel.CaseSetting),
                        userFilterFavorites = GetMyFavorites(currentCustomerId, userId),
                        gridSettings = JsonGridSettingsMapper.ToJsonGridSettingsModel(
                            gridSettings,
                            SessionFacade.CurrentCustomer.Id,
                            m.ConnectToParentModel.CaseSetting.ColumnSettingModel.AvailableColumns.Count(),
                            new[] { "5", "10", "15" }),
                        messages = new Dictionary<string, string>()
                        {
                            {"information", Translation.GetCoreTextTranslation("Information")},
                            {
                                "records_limited_msg",
                                Translation.GetCoreTextTranslation("Antal ärende som visas är begränsade till 500.")
                            },
                        },
                        HelperRegTime = Translation.Get(GlobalEnums.TranslationCaseFields.RegTime.ToString(), Enums.TranslationSource.CaseTranslation, SessionFacade.CurrentCustomer.Id),
                        HelperCaption = Translation.Get(GlobalEnums.TranslationCaseFields.Caption.ToString(), Enums.TranslationSource.CaseTranslation, SessionFacade.CurrentCustomer.Id)
                    };

                }

                #endregion

            }
            AddViewDataValues();

            // Positive: Send Mail to...
            m.CaseMailSetting.DontSendMailToNotifier = m.CaseMailSetting.DontSendMailToNotifier == false;
            m.IsReturnToCase = retToCase;

            //If case has been moved to customer with no access, extended case has been removed and therefore active tab should be "case-tab"
            //Template still has DefaultTab "extendedcase-tab" in db (tblcasesolution)
            if (m.ContainsExtendedCase == false)
            {
                m.ActiveTab = "";
            }

            return this.View(m);
        }

        private bool CheckInternalLogFilesAccess(int customerId, UserOverview user, IList<CaseFieldSetting> caseFieldSettings = null)
        {
            var internalFileFieldSetting =
                caseFieldSettings == null
                ? _caseFieldSettingService.GetCaseFieldSetting(customerId, TranslationCaseFields.tblLog_Filename_Internal.ToString().getCaseFieldName())
                : caseFieldSettings.getCaseSettingsValue(TranslationCaseFields.tblLog_Filename_Internal.ToString());

            var isTwoAttachmentsModeEnabled = internalFileFieldSetting?.IsActive ?? false;
            if (isTwoAttachmentsModeEnabled)
                return _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(user), UserPermission.CaseInternalLogPermission);

            return true;
        }

        public ActionResult EditLog(int id, int customerId, bool newLog = false, bool editLog = false, bool isCaseReopened = false)
        {
            CaseInputViewModel m = null;

            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                var cu = _customerUserService.GetCustomerUserSettings(customerId, userId);
                var cs = GetCustomerSettings(customerId);
                var customer = _customerService.GetCustomer(customerId);
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);

                if (cu != null)
                {
                    m = new CaseInputViewModel();
                    m.CaseLog = _logService.GetLogById(id);
                    m.LogKey = m.CaseLog.Id.ToString();
                    m.customerUserSetting = cu;
                    m.caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
                    m.CaseFieldSettingWithLangauges = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);
                    m.CaseSectionModels = _caseSectionService.GetCaseSections(customerId, SessionFacade.CurrentLanguageId);
                    m.finishingCauses = _finishingCauseService.GetFinishingCausesWithChilds(customerId).Where(x => x.Merged == false).ToList();
#pragma warning disable 0618
                    m.case_ = _caseService.GetCaseById(m.CaseLog.CaseId);
#pragma warning restore 0618
                    m.IsCaseReopened = isCaseReopened;

#pragma warning disable 0618
                    var lastLog = m.case_.Logs.OrderByDescending(o => o.Id).FirstOrDefault();
#pragma warning restore 0618
                    if (lastLog.FinishingType.HasValue)
                    {
                        var finishingCausesInfos = _finishingCauseService.GetFinishingCauseInfos(customerId).ToArray();
                        m.FinishingCause = CommonHelper.GetFinishingCauseFullPath(finishingCausesInfos, lastLog.FinishingType);
                    }

                    var virtualDirPath = _masterDataService.GetVirtualDirectoryPath(customerId);
                    var useVD = !string.IsNullOrEmpty(virtualDirPath);
                    m.CaseFilesUrlBuilder = new CaseFilesUrlBuilder(virtualDirPath, RequestExtension.GetAbsoluteUrl());

                    //prepare log files
                    m.CaseInternalLogAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseInternalLogPermission);
                    m.EnableTwoAttachments = m.caseFieldSettings.getCaseSettingsValue(TranslationCaseFields.tblLog_Filename_Internal.ToString())?.IsActive ?? false;

                    //get all log files
                    var logFiles = _logFileService.GetLogFileNamesByLogId(id, true);

                    m.LogFilesModel =
                        new FilesModel(id.ToString(), logFiles.Where(f => f.LogType == LogFileType.External).ToList(), useVD);

                    var includeInternalFiles = CheckInternalLogFilesAccess(customerId, SessionFacade.CurrentUser, m.caseFieldSettings);
                    if (includeInternalFiles)
                    {
                        m.LogInternalFilesModel = new FilesModel(id.ToString(), logFiles.Where(f => f.LogType == LogFileType.Internal).ToList(), useVD);
                    }

                    const bool isAddEmpty = true;
#pragma warning disable 0618
                    var responsibleUsersAvailable = _userService.GetAvailablePerformersOrUserId(customerId, m.case_.CaseResponsibleUser_Id);
#pragma warning restore 0618
                    m.OutFormatter = new OutputFormatter(cs.IsUserFirstLastNameRepresentation == 1, userTimeZone);
                    m.ResponsibleUsersAvailable = responsibleUsersAvailable.MapToSelectList(cs, isAddEmpty);

                    m.SendToDialogModel = _sendToDialogModelFactory.CreateNewSendToDialogModel(customerId, responsibleUsersAvailable.ToList(), cs, _emailGroupService, _workingGroupService, _emailService);
                    m.MinWorkingTime = cs.MinRegWorkingTime;
                    m.CaseMailSetting = new CaseMailSetting(
                        customer.NewCaseEmailList,
                        customer.HelpdeskEmail,
                        RequestExtension.GetAbsoluteUrl(),
                        cs.DontConnectUserToWorkingGroup);

                    // check department info
                    m.ShowInvoiceFields = 0;
#pragma warning disable 0618
                    if (m.case_.Department_Id > 0 && m.case_.Department_Id.HasValue)
                    {
                        var d = _departmentService.GetDepartment(m.case_.Department_Id.Value);
                        if (d != null)
                        {
                            m.ShowInvoiceFields = d.Charge;
                            m.ShowExternalInvoiceFields = d.ShowInvoice;
                            m.TimeRequired = d.ChargeMandatory.ToBool();
                        }
                    }
#pragma warning restore 0618

                    m.CaseLog.SendMailAboutCaseToNotifier = !string.IsNullOrEmpty(m.CaseLog.TextExternal);// customer.CommunicateWithNotifier.ToBool();
                    

                    // check state secondary info
                    m.Disable_SendMailAboutCaseToNotifier = false;
#pragma warning disable 0618
                    if (m.case_.StateSecondary_Id > 0)
                        if (m.case_.StateSecondary != null)
                        {
                            m.Disable_SendMailAboutCaseToNotifier = m.case_.StateSecondary.NoMailToNotifier == 1;
                            if (m.case_.StateSecondary.NoMailToNotifier == 1)
                                m.CaseLog.SendMailAboutCaseToNotifier = false;
                            else
                                m.CaseLog.SendMailAboutCaseToNotifier = true;
                        }
#pragma warning restore 0618

                    m.stateSecondaries = _stateSecondaryService.GetStateSecondaries(customerId);

                    var acccessToGroups = _userService.GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, customerId);
                    var deps = _departmentService.GetDepartmentsByUserPermissions(userId, customerId);

                    // TODO: Should mix CustomerSettings & Setting 
                    m.CustomerSettings = _workContext.Customer.Settings;
                    m.Setting = cs;
                    m.EditMode = EditMode(m, ModuleName.Log, deps, acccessToGroups, true);
                    m.LogFileNames = string.Join("|", m.LogFilesModel.Files.Select(x => x.Name).ToArray());
                    AddViewDataValues();
                    SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;

                    // User has not access to case/log
                    if (m.EditMode == AccessMode.NoAccess)
                        return this.RedirectToAction("index", "home");

                    m.editLog = editLog;
                    m.newLog = newLog;

                    if (newLog)
                    {
                        m.CaseLog.OldLog_Id = m.CaseLog.Id;
                        m.CaseLog.Id = 0;
                    }
                }
            }

            return this.View(m);
        }


        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult EditLog(Case case_, CaseLog caseLog)
        {
            this.NewCaselog(case_, caseLog);
            //this.UpdateCaseLogForCase(case_, caseLog);
            if (caseLog.FinishingType > 0)
            {
                return this.RedirectToAction("index", "cases", new { id = case_.Customer_Id });
            }
            else
            {
                return this.RedirectToAction("edit", "cases", new { id = caseLog.CaseId });
            }
        }

        [HttpPost]
        public JsonResult SetCaseDataChanged(int id)
        {
            SessionFacade.IsCaseDataChanged = true;
            return Json(new { success = true });
        }

        [HttpGet]
        public JsonResult GetCaseInfo(int caseId)
        {
            LogManager.Session.Debug($"GetCaseInfo called. CaseId: {caseId}, IsCaseDataChanged: {SessionFacade.IsCaseDataChanged}");

            if (!SessionFacade.IsCaseDataChanged)
                return Json(new { needUpdate = false, shouldReload = false, newData = "" }, JsonRequestBehavior.AllowGet);

            var _case = _caseService.GetCaseById(caseId);
            if (_case == null)
                return Json(new { needUpdate = false, shouldReload = false, newData = "" }, JsonRequestBehavior.AllowGet);

            if (_case.FinishingDate != null)
            {
                SessionFacade.IsCaseDataChanged = false;
                return Json(new { needUpdate = true, shouldReload = true, newData = "" }, JsonRequestBehavior.AllowGet);
            }

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);

            var caseInfo = new CaseCurrentDataModelJS()
            {
                Id = caseId,
                DateFormat = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                ReportedBy = _case.ReportedBy,
                PersonsName = _case.PersonsName,
                PersonsPhone = _case.PersonsPhone,
                PlanDateJS = _case.PlanDate.HasValue ? _case.PlanDate.Value.ToShortDateString() : "",
                WatchDateJS = _case.WatchDate.HasValue ? _case.WatchDate.Value.ToShortDateString() : "",
                Region_Id = _case.Region_Id,
                RegionName = _case.Region?.Name,
                Department_Id = _case.Department_Id,
                DepartmentName = _case.Department?.DepartmentName,
                OU_Id = _case.OU_Id,
                OUName = _case.Ou != null ? _case.Ou.Parent != null ?
                                string.Format("{0} - {1}", _case.Ou.Parent.Name, _case.Ou.Name) : _case.Ou.Name : string.Empty,
                CaseType_Id = _case.CaseType_Id,
                CaseTypeName = _case.CaseType?.Name,
                Priority_Id = _case.Priority_Id,
                PriorityName = _case.Priority?.Name,
                ProductArea_Id = _case.ProductArea_Id,
                ProductAreaName = _case.ProductArea?.ResolveFullName(),
                StateSecondary_Id = _case.StateSecondary_Id,
                SubStateName = _case.StateSecondary?.Name,
                Status_Id = _case.Status_Id,
                StatusName = _case.Status?.Name,
                WorkingGroup_Id = _case.WorkingGroup_Id,
                WorkingGroupName = _case.Workinggroup?.WorkingGroupName,
                Project_Id = _case.Project_Id
            };

            LogManager.Session.Debug($"GetCaseInfo: case should be updated.");

            SessionFacade.IsCaseDataChanged = false;
            return Json(new { needUpdate = true, shouldReload = false, newData = caseInfo }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region --Auto Complete Fields--

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Search_User(string query, int customerId, string searchKey, int? categoryID = null)
        {
            IList<int> departmentIds = new List<int>();
            var applyUserSearchRestriction = GetCustomerSettings(customerId).ComputerUserSearchRestriction == 1;
            if (applyUserSearchRestriction)
            {
                departmentIds = _departmentService.GetDepartmentsIdsByUserPermissions(SessionFacade.CurrentUser.Id, customerId);
                //user has no departments checked == access to all departments. TODO: change getdepartmentsbyuserpermissions to actually reflect the "none selected"
                if (departmentIds.Count == 0)
                {
                    departmentIds = _departmentService.GetDepartmentsIds(customerId);
                }
            }

            var result = _computerService.SearchComputerUsers(customerId, query, categoryID, departmentIds);
            return Json(new { searchKey = searchKey, result = result });
        }

        [HttpPost]
        public JsonResult GetExtendedCaseUrlForCategoryAndSection(int categoryID, int caseSectionType)
        {
            string url = null;
            Guid guid = Guid.Empty;

            if (categoryID == 0)
                return Json(new { guid, url });

            var category = _computerService.GetComputerUserCategoryByID(categoryID);
            var customerId = SessionFacade.CurrentCustomer.Id;

            var currentUserID = SessionFacade.CurrentUser.Id;
            var currentUserGroupID = SessionFacade.CurrentUser.UserGroupId;

            var userRole = GetUserRole(currentUserID, currentUserGroupID, customerId);

            if (customerId != category.CustomerID)
                throw new HttpException(403, "Not a valid category for customer");

            var userGuid = SessionFacade.CurrentUser.UserGUID.ToString();

            var extendedCasePathMask = _globalSettingService.GetGlobalSettings().FirstOrDefault().ExtendedCasePath;
            if (!string.IsNullOrEmpty(extendedCasePathMask) && category.CaseSolutionID.HasValue)
            {
                var extendedCaseData =
                    _caseService.GetCaseSectionExtendedCaseForm(category.CaseSolutionID.Value, customerId, 0, caseSectionType, userGuid, 0);

                if (extendedCaseData != null)
                {
                    guid = extendedCaseData.ExtendedCaseGuid;
                    url = ExtendedCaseUrlBuilder.BuildExtendedCaseUrl(extendedCasePathMask, new ExtededCaseUrlParams
                    {
                        formId = extendedCaseData.ExtendedCaseFormId,
                        caseStatus = extendedCaseData.StateSecondaryId,
                        userRole = userRole,
                        userGuid = userGuid,
                        customerId = customerId,
                        autoLoad = true
                    });
                }
            }

            return Json(new { guid, url });
        }

        private int GetUserRole(int currentUserID, int currentUserGroupID, int customerID)
        {
            int userRole;
            if (currentUserGroupID >= 3)
            {
                userRole = 99;

            }
            else
            {
                //Take the highest workinggroupId with "Admin" access (UserRole)
                var userWorkingGroup = _workingGroupService.GetWorkingGroupsAdmin(customerID, currentUserID)
                    .OrderByDescending(x => x.WorkingGroupId)
                    .FirstOrDefault();

                int userWorkingGroupId = 0;

                if (userWorkingGroup != null)
                {
                    userWorkingGroupId = userWorkingGroup.WorkingGroupId;
                }
                userRole = userWorkingGroupId;
            }

            return userRole;
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CaseSearchUserEmails(string query, string searchKey, bool isInternalLog = false)
        {
            var searchScope = new BusinessData.Models.Email.EmailSearchScope()
            {
                SearchInInitiators = true,
                SearchInAdmins = true,
                SearchInUsers = true,
                SearchInEmailGrs = true,
                SearchInWorkingGrs = true
            };

            var models = _userEmailsSearchService.GetUserEmailsForCaseSend(SessionFacade.CurrentCustomer.Id, query, searchScope);
            return Json(new { searchKey = searchKey, result = models });
        }

        [HttpPost]
        public JsonResult Get_User(int id)
        {
            var cu = _computerService.GetComputerUser(id);
            var u = new
            {
                num = cu.UserId,
                name = cu.FirstName + ' ' + cu.SurName,
                email = cu.Email,
                place = cu.Location,
                phone = cu.Phone,
                usercode = cu.UserCode,
                cellphone = cu.Cellphone,
                regionid = (cu.Department != null) ? cu.Department.Region_Id == null ? string.Empty : cu.Department.Region_Id.Value.ToString() : string.Empty,
                regionname = (cu.Department != null) ? cu.Department.Region != null ? cu.Department.Region.Name : string.Empty : string.Empty,
                departmentid = cu.Department_Id,
                departmentname = (cu.Department != null ? cu.Department.DepartmentName : string.Empty),
                ouid = cu.OU_Id,
                ouname = cu.OU != null ? (cu.OU.Parent != null ? cu.OU.Parent.Name + " - " : string.Empty) + cu.OU.Name : string.Empty,
                costcentre = string.IsNullOrEmpty(cu.CostCentre) ? string.Empty : cu.CostCentre
            };
            return this.Json(u);
        }

        [HttpPost]
        public ActionResult Search_Computer(string query, int customerId)
        {
            var result = _computerService.SearchPcNumber(customerId, query);
            foreach (var inv in result)
            {
                if (inv.NeedTranslate)
                    inv.TypeName = Translation.GetCoreTextTranslation(inv.TypeName);
            }
            return this.Json(result);
        }

        [HttpPost]
        public ActionResult Search_PcNumber(int userId, int customerId)
        {
            var result = _computerService.SearchPcNumberByUserId(customerId, userId);
            return this.Json(result);
        }

        #endregion

        #region --Change Depended Field--

        public JsonResult ChangeRegion(int? id, int customerId, int departmentFilterFormat)
        {
            if (SessionFacade.CurrentUser == null)
            {
                return this.Json(new { success = false, message = "Access denied" }, JsonRequestBehavior.AllowGet);
            }

            var list = _orgJsonService.GetActiveDepartmentForUserByRegion(id, SessionFacade.CurrentUser.Id, customerId, departmentFilterFormat);
            return this.Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeWorkingGroupFilterUser(int? id, int customerId)
        {
            IList<BusinessData.Models.User.CustomerUserInfo> performersList;
            var customerSettings = GetCustomerSettings(customerId);
            if (customerSettings.DontConnectUserToWorkingGroup == 0 && id > 0)
            {
                performersList = _userService.GetAvailablePerformersForWorkingGroup(customerId, id);
            }
            else
            {
                performersList = _userService.GetAvailablePerformersOrUserId(customerId);
            }

            if (customerSettings.IsUserFirstLastNameRepresentation == 1)
            {
                return
                    this.Json(
                        new
                        {
                            list =
                                    performersList.OrderBy(it => it.FirstName)
                                        .ThenBy(it => it.SurName)
                                        .Select(
                                            it =>
                                            new IdName
                                            {
                                                id = it.Id,
                                                name = string.Format("{0} {1}", it.FirstName, it.SurName)
                                            })
                        });
            }

            return
                this.Json(
                    new
                    {
                        list =
                                performersList.OrderBy(it => it.SurName)
                                    .ThenBy(it => it.FirstName)
                                    .Select(
                                        it =>
                                        new IdName
                                        {
                                            id = it.Id,
                                            name = string.Format("{0} {1}", it.SurName, it.FirstName)
                                        })
                    });
        }

        public JsonResult GetWatchDateByDepartment(int departmentId)
        {
            var dept = _departmentService.GetDepartment(departmentId);
            DateTime? res = null;
            if (dept != null && dept.WatchDateCalendar_Id.HasValue)
            {
                res = _watchDateCalendarService.GetClosestDateTo(dept.WatchDateCalendar_Id.Value, DateTime.UtcNow);
            }

            return this.Json(new { result = "success", data = res }, JsonRequestBehavior.AllowGet);
        }

        public int ChangeWorkingGroupSetStateSecondary(int? id)
        {
            int ret = 0;
            if (id.HasValue)
            {
                DHDomain.WorkingGroupEntity w = _workingGroupService.GetWorkingGroup(id.Value);
                ret = w != null ? w.StateSecondary_Id.HasValue ? w.StateSecondary_Id.Value : 0 : 0;
            }
            return ret;
        }

        public JsonResult GetDepartmentInvoiceParameters(int? departmentId, int? ouId)
        {
            return departmentId.HasValue ? GetInvoiceTime(departmentId.Value, ouId) : null;
        }

        public JsonResult ChangeDepartment(int? id, int customerId)
        {
            var list = _orgJsonService.GetActiveOUForDepartmentAsIdName(id, customerId);
            return this.Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeCountry(int? id, int customerId, int departmentFilterFormat)
        {
            var list = id.HasValue ? _supplierService.GetSuppliersByCountry(customerId, id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name }) : _supplierService.GetSuppliers(customerId).Where(x => x.IsActive == 1).Select(x => new { id = x.Id, name = x.Name });
            return this.Json(new { list });
        }

        public JsonResult GetCaseFields()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var CaseFields = _caseFieldSettingService.GetCaseFieldSettings(customerId);

            return this.Json(new
            {
                Result = CaseFields.Select(x => new
                {
                    Name = x.Name,
                    Show = x.ShowOnStartPage,
                    Required = x.Required
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangePriority(int? id, string textExternalLogNote)
        {
            string ret = string.Empty;
            if (id.HasValue)
            {
                var p = _priorityService.GetPriority(id.Value);
                if (textExternalLogNote == string.Empty)
                {
                    ret = p != null ? p.LogText : string.Empty;
                }
                else if (p.LogText != null)
                {
                    ret = p.LogText;
                }
                else
                    ret = textExternalLogNote;
            }
            else
            {
                ret = textExternalLogNote;
            }
            return Json(new { ExternalLogText = ret });
        }

        public int GetPriorityIdForImpactAndUrgency(int? impactId, int? urgencyId)
        {
            int ret = 0;
            if (impactId.HasValue && urgencyId.HasValue)
            {
                ret = _priorityService.GetPriorityIdByImpactAndUrgency(impactId.Value, urgencyId.Value);
            }
            return ret;
        }

        [HttpPost]
        public ActionResult ChangeCaseType(int? id)
        {

            CaseType caseType = new CaseType();
            if (id.HasValue)
            {
                caseType = _caseTypeService.GetCaseType(id.Value);
            }
            if (caseType == null)
                return new HttpNotFoundResult();

            int userId = caseType.User_Id ?? 0;
            var performerUser = caseType.Administrator;
            return Json(new
            {
                Id = caseType.Id,
                Name = caseType.Name,
                ParentId = caseType.Parent_CaseType_Id,
                UserId = userId > 0 ? caseType.User_Id : null,
                UserName = performerUser != null ? string.Format("{0} {1}", performerUser.FirstName, performerUser.SurName) : "",
                ShowOnExternalPage = caseType.ShowOnExternalPage,
                ShowOnExtPageCases = caseType.ShowOnExtPageCases,
                IsActive = caseType.IsActive,
                Selectable = caseType.Selectable,
                WorkingGroupId = caseType.WorkingGroup_Id,
                WorkingGroupName = caseType.WorkingGroup_Id != null ? caseType.WorkingGroup.WorkingGroupName : null
            });
        }

        [HttpPost]
        public string ChangeSystem(int? id)
        {
            string ret = null;
            if (id.HasValue)
            {
                var e = _systemService.GetSystem(id.Value);
                if (e != null)
                    ret = e.Urgency_Id.HasValue ? e.Urgency_Id != 0 ? e.Urgency_Id.Value.ToString() : null : null;
            }
            return ret;
        }

        [HttpPost]
        public JsonResult ChangeProductArea(int? id)
        {
            int workinggroupId = 0;
            string workinggroupName = string.Empty;
            int priorityId = 0;
            int hasChild = 0;
            string priorityName = string.Empty;
            int sla = 0;

            if (id.HasValue)
            {
                var productArea = _productAreaService.GetProductArea(id.Value);
                if (productArea != null)
                {
                    workinggroupId = productArea.WorkingGroup_Id.HasValue ? productArea.WorkingGroup_Id.Value : 0;
                    if (workinggroupId > 0)
                    {
                        var wg = _workingGroupService.GetWorkingGroup(workinggroupId);
                        workinggroupName = wg != null ? wg.WorkingGroupName : string.Empty;
                    }

                    priorityId = productArea.Priority_Id.HasValue ? productArea.Priority_Id.Value : 0;
                    if (priorityId > 0)
                    {
                        var prio = _priorityService.GetPriority(priorityId);
                        priorityName = prio != null ? prio.Name : string.Empty;
                        sla = prio.SolutionTime;
                    }

                    if (productArea.SubProductAreas != null && productArea.SubProductAreas.Where(s => s.IsActive != 0).Any())
                        hasChild = 1;
                }
            }

            return Json(new
            {
                WorkingGroup_Id = workinggroupId,
                WorkingGroup_Name = workinggroupName,
                Priority_Id = priorityId,
                PriorityName = priorityName,
                SLA = sla,
                HasChild = hasChild
            });
        }
        [HttpPost]
        public JsonResult ChangeFinishingType(int? id)
        {
            int hasChild = 0;

            if (id.HasValue)
            {
                var finishingType = _finishingCauseService.GetFinishingCause(id.Value);
                if (finishingType != null)
                {

                    if (finishingType.SubFinishingCauses != null && finishingType.SubFinishingCauses.Where(s => s.IsActive != 0).Any())
                        hasChild = 1;
                }
            }

            return Json(new
            {
                HasChild = hasChild
            });
        }
        [HttpGet]
        public JsonResult GetStateSecondary(int id)
        {
            var state = _caseService.GetCaseSubStatus(id) ?? new StateSecondary();
            return Json(new
            {
                state.Id,
                state.StateSecondaryId,
                StateSecondaryName = state.Name
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(int? id)
        {
            int workinggroupId = 0;
            int stateSecondaryId = 0;

            if (id.HasValue)
            {
                var e = _statusService.GetStatus(id.Value);
                if (e != null)
                {
                    workinggroupId = e.WorkingGroup_Id.HasValue ? e.WorkingGroup_Id.Value : 0;
                    stateSecondaryId = e.StateSecondary_Id.HasValue ? e.StateSecondary_Id.Value : 0;
                }
            }
            return Json(new { WorkingGroup_Id = workinggroupId, StateSecondary_Id = stateSecondaryId });
        }

        public JsonResult ChangeStateSecondary(int? id)
        {
            int workinggroupId = 0;
            int noMailToNotifier = 0;
            int reCalculateWatchDate = 0;

            if (id.HasValue)
            {
                var s = _stateSecondaryService.GetStateSecondary(id.Value);
                reCalculateWatchDate = s != null ? s.RecalculateWatchDate : 0;
                noMailToNotifier = s != null ? s.NoMailToNotifier : 0;
                workinggroupId = s != null ? s.WorkingGroup_Id.HasValue ? s.WorkingGroup_Id.Value : 0 : 0;
            }
            return Json(new
            {
                NoMailToNotifier = noMailToNotifier,
                WorkingGroup_Id = workinggroupId,
                ReCalculateWatchDate = reCalculateWatchDate
            });
        }

        public JsonResult GetProductAreaByCaseType(int? caseTypeId, int customerId, int? productAreaIdToInclude)
        {
            var productAreas =
                _productAreaService.GetTopProductAreasForUserOnCase(customerId, productAreaIdToInclude, caseTypeId, SessionFacade.CurrentUser).ToList();

            //sort
            productAreas = productAreas.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList();

            //build tree with Ids
            var praIds = productAreas.Select(x => x.Id).ToList();
            foreach (var ctProductArea in productAreas)
            {
                var subAreaIds = GetSubProductAreasIds(ctProductArea);
                praIds.AddRange(subAreaIds);
            }

            var dropString = Infrastructure.Extensions.HtmlHelperExtension.ProductAreaDropdownString(productAreas, true, productAreaIdToInclude);
            return Json(new { success = true, data = dropString, praIds });
        }

        private IEnumerable<int> GetSubProductAreasIds(ProductAreaOverview ctProductArea)
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
                            var subAreaIds = GetSubProductAreasIds(subProductArea);
                            result.AddRange(subAreaIds);
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region --Validation--

        [HttpPost]
        public JsonResult CheckCaseForActiveData(CaseMasterDataFieldsModel caseMasterDataFields)
        {
            var inactiveFields = GetInactiveFieldsValue(caseMasterDataFields);
            if (inactiveFields.Any())
            {
                var publicMessage = Translation.Get("Ärendet kunde inte sparas pga inaktiva värden(n). Var vänlig kontrollera ärendet.");
                var fieldNames = String.Join("<br/> ", inactiveFields.ToArray());
                return Json(string.Format("{0}<br/> {1}", publicMessage, fieldNames));
            }
            else
                return Json("valid");
        }

        [HttpGet]
        public JsonResult ProductAreaHasChild(int pId)
        {
            var res = "false";
            var productArea = _productAreaService.GetProductArea(pId);
            if (productArea != null && productArea.SubProductAreas != null &&
                productArea.SubProductAreas.Where(p => p.IsActive != 0).ToList().Count > 0)
                res = "true";

            return Json(res);
        }

        [HttpGet]
        public JsonResult FinishingCauseHasChild(int fId)
        {
            var res = "false";
            var finishingCause = _finishingCauseService.GetFinishingCause(fId);
            if (finishingCause != null && finishingCause.SubFinishingCauses != null &&
                finishingCause.SubFinishingCauses.Where(f => f.IsActive != 0).ToList().Count > 0)
                res = "true";

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsFinishingDateValid(DateTime changedTime, DateTime finishingTime)
        {
            if (finishingTime.ToShortDateString() == DateTime.Today.ToShortDateString() ||
                finishingTime.ToShortDateString() == changedTime.ToShortDateString())
                return Json(true, JsonRequestBehavior.AllowGet);

            if (changedTime > finishingTime)
                return Json(false, JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region --Overview Setting--

        public void SaveSetting(FormCollection frm)
        {
            int customerId = int.Parse(frm["CustomerId"]);
            int userId = int.Parse(frm["UserId"]);

            bool regionCheck = frm.IsFormValueTrue("RegionCheck");
            var regions = (regionCheck)
                ? ((frm.ReturnFormValue("lstRegions") == string.Empty) ? "0" : frm.ReturnFormValue("lstRegions"))
                : string.Empty;

            bool departmentsCheck = frm.IsFormValueTrue("IsDepartmentChecked");
            var departments = departmentsCheck
                 ? ((frm.ReturnFormValue(CaseSettingModel.DepartmentsControlName) == string.Empty) ? "0" : frm.ReturnFormValue(CaseSettingModel.DepartmentsControlName))
                : string.Empty;

            bool registerByCheck = frm.IsFormValueTrue("RegisteredByCheck");
            var registerBy = (registerByCheck)
                ? ((frm.ReturnFormValue("lstRegisterBy") == string.Empty) ? "0" : frm.ReturnFormValue("lstRegisterBy"))
                : string.Empty;

            bool caseTypeCheck = frm.IsFormValueTrue("CaseTypeCheck");
            var caseType = caseTypeCheck
                ? ((frm.ReturnFormValue("CaseTypeId") == string.Empty) ? "0" : frm.ReturnFormValue("CaseTypeId"))
                : string.Empty;

            bool productAreaCheck = frm.IsFormValueTrue("ProductAreaCheck");
            var productArea = (productAreaCheck)
                ? ((frm.ReturnFormValue("ProductAreaId") == string.Empty) ? "0" : frm.ReturnFormValue("ProductAreaId"))
                : string.Empty;

            bool workingGroupCheck = frm.IsFormValueTrue("WorkingGroupCheck");
            var workingGroup = (workingGroupCheck)
                ? ((frm.ReturnFormValue("lstWorkingGroup") == string.Empty)
                    ? "0"
                    : frm.ReturnFormValue("lstWorkingGroup"))
                : string.Empty;

            bool responsibleCheck = frm.IsFormValueTrue("ResponsibleCheck");

            // we always show administrator filter field
            var administrator = (frm.ReturnFormValue("lstAdministrator") == string.Empty)
                ? " "
                : frm.ReturnFormValue("lstAdministrator");

            bool priorityCheck = frm.IsFormValueTrue("PriorityCheck");
            var priority = (priorityCheck)
                ? ((frm.ReturnFormValue("lstPriority") == string.Empty) ? "0" : frm.ReturnFormValue("lstPriority"))
                : string.Empty;

            bool categoryCheck = frm.IsFormValueTrue("CategoryCheck");
            var category = (categoryCheck)
                ? ((frm.ReturnFormValue("CategoryId") == string.Empty) ? "0" : frm.ReturnFormValue("CategoryId"))
                : string.Empty;

            var stateCheck = frm.IsFormValueTrue("StateCheck");
            var state = (stateCheck)
                ? ((frm.ReturnFormValue("lstStatus") == string.Empty) ? "0" : frm.ReturnFormValue("lstStatus"))
                : string.Empty;

            bool subStateCheck = frm.IsFormValueTrue("SubStateCheck");
            var subState = (subStateCheck)
                ? ((frm.ReturnFormValue("lstSubState") == string.Empty) ? "0" : frm.ReturnFormValue("lstSubState"))
                : string.Empty;

            var closingReasonCheck = frm.IsFormValueTrue("ClosingReasonCheck");
            var closingReason = closingReasonCheck
                ? ((frm.ReturnFormValue("ClosingReasonId") == string.Empty) ? "0" : frm.ReturnFormValue("ClosingReasonId"))
                : string.Empty;

            var caseRegistrationDateCheck = frm.IsFormValueTrue("CaseRegistrationDateFilterShow");
            var caseClosingDateCheck = frm.IsFormValueTrue("CaseClosingDateFilterShow");
            var caseWatchDateCheck = frm.IsFormValueTrue("CaseWatchDateFilterShow");

            bool caseRemainingTimeCheck = frm.IsFormValueTrue("CaseRemainingTimeChecked");
            var caseRemainingTime = caseRemainingTimeCheck
                 ? ((frm.ReturnFormValue("lstfilterCaseRemainingTime") == string.Empty) ? "0" : frm.ReturnFormValue("lstfilterCaseRemainingTime"))
                : string.Empty;

            var newCaseSetting = new UserCaseSetting(
                            customerId,
                            userId,
                            regions,
                            departments,
                            registerBy,
                            caseType,
                            productArea,
                            workingGroup,
                            responsibleCheck,
                            administrator,
                            priority,
                            state,
                            subState,
                            category,
                            (caseRegistrationDateCheck ? frm.GetDate("CaseRegistrationDateStartFilter") : null),
                            (caseRegistrationDateCheck ? frm.GetDate("CaseRegistrationDateEndFilter") : null),
                            (caseWatchDateCheck ? frm.GetDate("CaseWatchDateStartFilter") : null),
                            (caseWatchDateCheck ? frm.GetDate("CaseWatchDateEndFilter") : null),
                            (caseClosingDateCheck ? frm.GetDate("CaseClosingDateStartFilter") : null),
                            (caseClosingDateCheck ? frm.GetDate("CaseClosingDateEndFilter") : null),
                            frm.IsFormValueTrue("CaseRegistrationDateFilterShow"),
                            frm.IsFormValueTrue("CaseWatchDateFilterShow"),
                            frm.IsFormValueTrue("CaseClosingDateFilterShow"),
                            closingReason,
                            frm.IsFormValueTrue("CaseInitiatorFilterShow"),
                            caseRemainingTime);

            _customerUserService.UpdateUserCaseSetting(newCaseSetting);
            SessionFacade.CurrentCaseSearch = null;
        }

        /// <summary>
        /// Saving settings for case overview page
        /// </summary>
        /// <param name="inputSettings"></param>
        public void SaveColSetting(CaseOverviewSettingsInput inputSettings)
        {
            if (!inputSettings.FieldStyle.Any())
            {
                throw new ArgumentException("columns count can not be 0");
            }

            _gridSettingsService.SaveCaseoviewSettings(
                inputSettings.MapToGridSettingsModel(),
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserGroupId);
            SessionFacade.CaseOverviewGridSettings = null;
        }

        #endregion

        #region --Parent Child Case--
        public ActionResult MergeCase(int parentCaseId)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            if (SessionFacade.CurrentUser.MergeCasePermission != 1)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            if (SessionFacade.CurrentUser.CreateCasePermission != 1 || SessionFacade.CurrentUser.CreateSubCasePermission != 1)
            {
                return new RedirectResult("~/Error/Forbidden");
            }

            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;

            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;
            var caseLockModel = new CaseLockModel();
            var customerFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

            var m = this.GetCaseInputViewModel(userId, customerId, 0, caseLockModel, customerFieldSettings, string.Empty, null, null, null, false, null, parentCaseId);

            var caseParam = new NewCaseParams
            {
                customerId = SessionFacade.CurrentCustomer.Id,
                templateId = null,
                copyFromCaseId = null,
                caseLanguageId = SessionFacade.CurrentCaseLanguageId
            };

            m.NewModeParams = caseParam;
            AddViewDataValues();

            // Positive: Send Mail to...
            if (m.CaseMailSetting.DontSendMailToNotifier == false)
                m.CaseMailSetting.DontSendMailToNotifier = true;
            else
                m.CaseMailSetting.DontSendMailToNotifier = false;

            return this.View("New", m);
        }
        public ActionResult NewChildCase(int parentCaseId)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            if (SessionFacade.CurrentUser.CreateCasePermission != 1 || SessionFacade.CurrentUser.CreateSubCasePermission != 1)
            {
                return new RedirectResult("~/Error/Forbidden");
            }

            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;

            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;
            var caseLockModel = new CaseLockModel();
            var customerFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

            var m = this.GetCaseInputViewModel(userId, customerId, 0, caseLockModel, customerFieldSettings, string.Empty, null, null, null, false, null, parentCaseId);

            var caseParam = new NewCaseParams
            {
                customerId = SessionFacade.CurrentCustomer.Id,
                templateId = null,
                copyFromCaseId = null,
                caseLanguageId = SessionFacade.CurrentCaseLanguageId
            };

            m.NewModeParams = caseParam;
            AddViewDataValues();

            // Positive: Send Mail to...
            if (m.CaseMailSetting.DontSendMailToNotifier == false)
                m.CaseMailSetting.DontSendMailToNotifier = true;
            else
                m.CaseMailSetting.DontSendMailToNotifier = false;

            return this.View("New", m);
        }

        public ActionResult NewCloseAndSplit(CaseEditInput caseEditInput)
        {


            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            if (SessionFacade.CurrentUser.CreateCasePermission != 1 || SessionFacade.CurrentUser.CreateSubCasePermission != 1)
            {
                return new RedirectResult("~/Error/Forbidden");
            }

            this.Save(caseEditInput);

            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;

            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;

            var caseLockModel = new CaseLockModel();
            var customerFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

#pragma warning disable 0618
            var parentCaseID = caseEditInput.case_.Id;
#pragma warning restore 0618
            // Template to split to
            int? splitToCaseSolutionID;
            if (caseEditInput.CurrentCaseSolution_Id.HasValue)
            {
                var parentCaseSolution = _caseSolutionService.GetCaseSolution(caseEditInput.CurrentCaseSolution_Id.Value);
                splitToCaseSolutionID = parentCaseSolution.SplitToCaseSolution_Id;
            }
            else
                splitToCaseSolutionID = null;

            var m = this.GetCaseInputViewModel(userId, customerId, 0, caseLockModel, customerFieldSettings, string.Empty, null, splitToCaseSolutionID, null, false, null, parentCaseID);


            var caseParam = new NewCaseParams
            {
                customerId = SessionFacade.CurrentCustomer.Id,
                templateId = m.CaseTemplateSplitToCaseSolutionID,
                copyFromCaseId = parentCaseID,
                caseLanguageId = SessionFacade.CurrentCaseLanguageId
            };

            m.NewModeParams = caseParam;
            m.IndependentChild = true;

            AddViewDataValues();

            // TODO: Why invert? 
            // Positive: Send Mail to...
            if (m.CaseMailSetting.DontSendMailToNotifier == false)
                m.CaseMailSetting.DontSendMailToNotifier = true;
            else
                m.CaseMailSetting.DontSendMailToNotifier = false;


            return this.View("New", m);
        }


        public ActionResult EditCloseAndSplit(CaseEditInput caseEditInput)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            if (SessionFacade.CurrentUser.CreateCasePermission != 1 || SessionFacade.CurrentUser.CreateSubCasePermission != 1)
            {
                return new RedirectResult("~/Error/Forbidden");
            }

            this.Save(caseEditInput);

            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;

#pragma warning disable 0618
            var parentCaseID = caseEditInput.case_.Id;
#pragma warning restore 0618


            return RedirectToAction("Split", new { parentCaseID = parentCaseID });
        }

        public ActionResult Split(int parentCaseID)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;

            var caseLockModel = new CaseLockModel();
            var customerFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

            var parentCase = _caseService.GetCaseById(parentCaseID);

            // Template to split to
            //int? splitToSolutionCaseID = parentCase.CurrentCaseSolution?.SplitToCaseSolution_Id;

            //var m = this.GetCaseInputViewModel(userId, customerId, 0, caseLockModel, customerFieldSettings, string.Empty, null, splitToSolutionCaseID, null, false, null, parentCaseID);

            var identity = global::System.Security.Principal.WindowsIdentity.GetCurrent();
            var windowsUser = identity != null ? identity.Name : null;

            var parentCaseInfo = new ParentCaseInfo();
            var newCase = _caseService.InitChildCaseFromCase(
               parentCase.Id,
               userId,
               this.Request.GetIpAddress(),
               CaseRegistrationSource.Administrator,
               windowsUser,
               out parentCaseInfo);

            var utcNow = DateTime.UtcNow;

            // TODO: Proper case extra info and user info
            IDictionary<string, string> errors;
            var extraInfo = new CaseExtraInfo
            {
                ActionExternalTime = 0,
                ActionLeadTime = 0,
                CreatedByApp = CreatedByApplications.Helpdesk5,
                LeadTimeForNow = 0
            };

            var caseLog = new CaseLog();
            _caseService.SaveCase(newCase, caseLog, userId, windowsUser, extraInfo, out errors);
            _caseService.AddParentCase(newCase.Id, parentCase.Id, true);

            return Json(new { CaseNumber = newCase.CaseNumber });

        }

        public ActionResult ConnectToParentCase(int id, int parentCaseId, bool? tomerge = false, bool? nomail = false)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            if (SessionFacade.CurrentUser.CreateCasePermission != 1 || SessionFacade.CurrentUser.CreateSubCasePermission != 1)
            {
                return new RedirectResult("~/Error/Forbidden");
            }
            if (tomerge == false)
            {
                _caseService.AddParentCase(id, parentCaseId);
            }
            else
            {
                _caseService.MergeChildToParentCase(id, parentCaseId);


                Case mergeCase = _caseService.GetCaseById(id);
                Case parentCase = _caseService.GetCaseById(parentCaseId);

                //Move followers and initiator to mergeparent
                List<ExtraFollower> mergeCaseFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(id);
                List<ExtraFollower> mergeParentFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(parentCaseId);

                var exists = mergeParentFollowers.Where(x => x.Follower == mergeCase.PersonsEmail).FirstOrDefault();

                if (!String.IsNullOrEmpty(mergeCase.PersonsEmail) && mergeCase.PersonsEmail != parentCase.PersonsEmail && exists == null)
                {
                    var extraFollower = new ExtraFollower()
                    {
                        CaseId = parentCaseId,
                        Follower = mergeCase.PersonsEmail
                    };
                    mergeParentFollowers.Add(extraFollower);
                }
                mergeParentFollowers = mergeParentFollowers.Union(mergeCaseFollowers).Where(y => y.Follower != parentCase.PersonsEmail).DistinctBy(x => x.Follower).ToList();

                List<string> newParentFollowers = mergeParentFollowers.Select(x => x.Follower).ToList();
                List<string> ccEmailList = mergeCaseFollowers.Select(x => x.Follower).ToList();

                if (mergeParentFollowers.Count() > 0)
                {
                    _caseExtraFollowersService.SaveExtraFollowers(parentCaseId, newParentFollowers, _workContext.User.UserId);
                }

                IDictionary<string, string> errors;

                // Close case...
                CaseExtraInfo extraInfo = new CaseExtraInfo
                {
                    ActionExternalTime = 0,
                    ActionLeadTime = 0,
                    CreatedByApp = CreatedByApplications.Helpdesk5,
                    LeadTimeForNow = 0
                };

                CaseLog caseLog = new CaseLog();

                FinishingCause mergedFinishingCause = _finishingCauseService.GetMergedFinishingCause(mergeCase.Customer_Id);
                mergeCase.FinishingDescription = mergedFinishingCause.Name;
                mergeCase.FinishingDate = DateTime.UtcNow;
                caseLog.FinishingDate = DateTime.UtcNow;
                caseLog.FinishingType = mergedFinishingCause.Id;
                caseLog.FinishingTypeName = Translation.GetCoreTextTranslation(mergedFinishingCause.Name);
                caseLog.CaseId = mergeCase.Id;
                caseLog.UserId = SessionFacade.CurrentUser.Id;
                caseLog.LogGuid = Guid.NewGuid();
                caseLog.TextInternal = Translation.GetCoreTextTranslation(mergedFinishingCause.Name);



                int caseHistoryId = _caseService.SaveCase(mergeCase, caseLog, SessionFacade.CurrentUser.Id, null, extraInfo, out errors);
                int mergeParentHistoryId = _caseService.SaveCase(parentCase, null, SessionFacade.CurrentUser.Id, null, extraInfo, out errors);
                caseLog.CaseHistoryId = caseHistoryId;
                int caseLogId = _logService.SaveLog(caseLog, 0, out errors);

                Customer customer = _customerService.GetCustomer(mergeCase.Customer_Id);
                CaseMailSetting caseMailSetting = new CaseMailSetting(string.Empty, customer.HelpdeskEmail, RequestExtension.GetAbsoluteUrl(), 1);
                if(nomail == false)
                {
                    caseMailSetting.DontSendMailToNotifier = false;
                    MailSenders mailSenders = new MailSenders { SystemEmail = caseMailSetting.HelpdeskMailFromAdress };
                    caseMailSetting.CustomeMailFromAddress = mailSenders;

                    TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
                    User currentLoggedInUser = _userService.GetUser(SessionFacade.CurrentUser.Id);
                    _caseService.SendMergedCaseEmail(mergeCase, parentCase, caseMailSetting, caseHistoryId, userTimeZone, caseLog, ccEmailList);
                }
               
            }

            return this.RedirectToAction("Edit", "cases", new { id = parentCaseId });
        }

        [UserCasePermissions]
        public ActionResult RemoveChild(int id, int parentId)
        {
            _caseService.DeleteChildCaseFromParent(id, parentId);
            return this.RedirectToAction("Edit", "cases", new { id = parentId });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectResult NewAndGotoParentCase(CaseEditInput m)
        {
            if (m.ParentId == null)
            {
                throw new ArgumentException("bad input");
            }

            int id = this.Save(m);
            var url = this.GetLinkWithHash(ChildCasesHashTab, new { id = m.ParentId }, "Edit");
            return this.Redirect(url);
        }

        #endregion

        #region --Related Case--

        [HttpGet]
        public ViewResult RelatedCases(int caseId, string userId)
        {
            var relatedCases = _caseService.GetCaseRelatedCases(
                                                caseId,
                                                _workContext.Customer.CustomerId,
                                                userId,
                                                SessionFacade.CurrentUser);

            var model = _caseModelFactory.GetRelatedCasesModel(relatedCases, _workContext.Customer.CustomerId, userId);
            return this.View(model);
        }

        [HttpGet]
        public ViewResult RelatedCasesFull(int caseId, string userId)
        {
            var sortBy = CaseSortField.CaseNumber;
            var sortByAsc = true;
            if (SessionFacade.CurrentCaseSearch != null &&
                SessionFacade.CurrentCaseSearch.Search != null)
            {
                sortBy = SessionFacade.CurrentCaseSearch.Search.SortBy;
                sortByAsc = SessionFacade.CurrentCaseSearch.Search.Ascending;
            }

            var model = _caseModelFactory.GetRelatedCasesFullModel(
                                                null,
                                                userId,
                                                caseId,
                                                sortBy,
                                                sortByAsc);
            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult RelatedCasesFullContent(int caseId, string userId, string sortBy, string sortByAsc)
        {
            var model = this.GetRelatedCasesViewModel(sortBy, sortByAsc, caseId, userId);
            return this.PartialView(model);
        }

        [ValidateInput(false)]
        [HttpGet]
        public JsonResult RelatedCasesCount(int caseId, string userId)
        {
            var count = _caseService.GetCaseRelatedCasesCount(
                                                caseId,
                                                _workContext.Customer.CustomerId,
                                                userId,
                                                SessionFacade.CurrentUser);
            return this.Json(count, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Related Inventory

        [ValidateInput(false)]
        [HttpGet]
        public JsonResult RelatedInventoryCount(string userId)
        {
            var count = _workContext.Customer.Settings.ModuleInventory ?
                _caseService.GetCaseRelatedInventoryCount(_workContext.Customer.CustomerId, userId, SessionFacade.CurrentUser) :
                0;
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region --Othres--

        [HttpGet]
        public ViewResult CaseByIds(string caseIds, string sortBy, string sortByAsc)
        {
            var cases = this.GetUnfilteredCases(sortBy, sortByAsc, null, null, caseIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
            var model = new CaseByIdsViewModel(cases, sortBy, sortByAsc.ToBool(), caseIds);
            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult CaseByIdsContent(string caseIds, string sortBy, string sortByAsc)
        {
            var cases = this.GetUnfilteredCases(sortBy, sortByAsc, null, null, caseIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
            var model = new CaseByIdsViewModel(cases, sortBy, sortByAsc.ToBool(), caseIds);
            return this.PartialView(model);
        }

        #endregion

        #region --General Methods--

        [HttpGet]
        public RedirectToRouteResult ChangeCurrentLanguage(int languageId)
        {
            SessionFacade.CurrentLanguageId = languageId;

            var language = _languageService.GetLanguage(languageId);

            if (language != null)
            {
                SessionFacade.CurrentLanguageCode = language.LanguageID;
            }

            var prevInfo = this.ExtractPreviousRouteInfo();
            var refreshRouteInfo = new RouteValueDictionary();

            if (prevInfo != null && prevInfo.Any())
            {
                foreach (var p in prevInfo)
                {
                    if (p.Key.ToLower() == "language")
                        refreshRouteInfo.Add(p.Key, language.LanguageID);
                    else
                        refreshRouteInfo.Add(p.Key, p.Value);
                }
            }

            var res = new RedirectToRouteResult(refreshRouteInfo);
            return res;
        }

        #endregion

        #region --Developer Test--
        /****  Case SLA Calculation simulator ***/
        // ** This function is using for developing test **//
        /*public JsonResult CalculateSLA(int caseId, int userId, DateTime simulateTime)
        {
            var utcNow = simulateTime;
            var case_ = _caseService.GetCaseById(caseId);
            if (case_ == null || case_.Id < 1)
                return Json("There is no case info for case id:" + caseId.ToString());

            var user = _userService.GetUser(userId);
            if (user == null || user.Id < 1)
                return Json("User number is not exist:" + userId.ToString());

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

            var curCustomer = _customerService.GetCustomer(case_.Customer_Id);
            var caseRegTime = DateTime.SpecifyKind(case_.RegTime, DateTimeKind.Utc);

            bool edit = true;
            // get case as it was before edit           
            var curCase = _caseService.GetCaseHistoryByCaseId(caseId).Where(ch => ch.CreatedDate < simulateTime)
                                                                              .OrderByDescending(x => x.CreatedDate)
                                                                              .FirstOrDefault();
            var oldCase = _caseService.GetCaseHistoryByCaseId(caseId).Where(ch => ch.CreatedDate < curCase.CreatedDate)
                                                                            .OrderByDescending(x => x.CreatedDate)
                                                                            .FirstOrDefault();
            var externalTime = 0;
            if (edit)
            {
                #region Editing existing case
                //oldCase = _caseService.GetDetachedCaseById(case_.Id);


                if (curCase == null)
                    return Json("There is no history about case");

                var oldCase_StateSecondary_Id = oldCase.StateSecondary_Id;
                if (oldCase_StateSecondary_Id.HasValue)
                {
                    var caseSubState = _stateSecondaryService.GetStateSecondary(oldCase_StateSecondary_Id.Value);

                    // calculating time spent in "inactive" state since last changing every save
                    if (caseSubState.IncludeInCaseStatistics == 0)
                    {
                        var workTimeCalcFactory =
                            new WorkTimeCalculatorFactory(
                                ManualDependencyResolver.Get<IHolidayService>(),
                                curCustomer.WorkingDayStart,
                                curCustomer.WorkingDayEnd,
                                TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId));
                        int[] deptIds = null;

                        var caseDepartmentId = curCase.Department_Id;
                        if (caseDepartmentId.HasValue)
                        {
                            deptIds = new int[] { caseDepartmentId.Value };
                        }

                        var oldCase_ChangeTime = oldCase.CreatedDate;
                        var oldCase_DepartmentId = oldCase.Department_Id;
                        var workTimeCalc = workTimeCalcFactory.Build(oldCase_ChangeTime, utcNow, deptIds);
                        externalTime = workTimeCalc.CalculateWorkTime(
                            oldCase_ChangeTime,
                            utcNow,
                            oldCase_DepartmentId) + oldCase.ExternalTime;
                    }
                }


                #endregion
            }

            var case_FinishingDate = simulateTime;

            var workTimeCalcFactory2 = new WorkTimeCalculatorFactory(
                ManualDependencyResolver.Get<IHolidayService>(),
                curCustomer.WorkingDayStart,
                curCustomer.WorkingDayEnd,
                TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId));
            int[] deptIds2 = null;

            if (curCase.Department_Id.HasValue)
            {
                deptIds2 = new int[] { curCase.Department_Id.Value };
            }

            var workTimeCalc2 = workTimeCalcFactory2.Build(caseRegTime, case_FinishingDate, deptIds2);
            var leadTime = workTimeCalc2.CalculateWorkTime(
                caseRegTime,
                case_FinishingDate,
                curCase.Department_Id) - externalTime;

            var msg = string.Format("System >> LeadTime:{0} ExternalTime:{1} <br/> Simulator >> LeadTime:{2} ExternalTime:{3}",
                                    case_.LeadTime, case_.ExternalTime, leadTime, externalTime);

            return Json(msg);
        }*/

        #endregion

        #region --Invoice--



        #endregion

        #region --Ajax--

        public JsonResult SaveStatisticsStateInSession(string value)
        {
            SessionFacade.CaseOverviewGridSettings.ExpandedStatistics = value;
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetToken()
        {
            if (SessionFacade.CurrentUser != null)
            {
                var user = _userService.GetUser(SessionFacade.CurrentUser.Id);
                if (user != null)
                {
                    var token = GetSimpleToken(user.UserID, user.Password);
                    if (token != null)
                    {
                        return Json(new { result = true, accessToken = token.access_token, refreshToken = token.refresh_token }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public JsonResult MoveCaseToExternalCustomer(int caseId, int customerId, Guid lockGuid)
        {
            var userId = _workContext.User.UserId;
            try
            {
                _caseProcessor.MoveCaseToExternalCustomer(caseId, userId, customerId);

                //Unlock Case
                _caseLockService.UnlockCaseByGUID(lockGuid);
            }
            catch (HelpdeskException e)
            {
                var err = Translation.GetCoreTextTranslation(e.Message);
                return Json(new { Success = false, Error = err });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Error = $"Unknown error. {e.Message}".ToHtmlString() });
            }

            return Json(new { Success = true, Location = Url.Action("Index", "Cases") });
        }

        [HttpGet]
        public ActionResult CaseMoveResult(int id)
        {
            var @case = _caseService.GetCaseById(id);
            if (@case == null)
            {
                return HttpNotFound($"Case (Id={id}) was not found.");
            }

            var customer = @case.Customer;
            var model = new CaseOperationResult
            {
                CaseId = id,
                CaseNumber = @case.CaseNumber.ToString(),
                Success = true,
                Message = $"Case has been moved to customer <b>{customer.Name}</b>"
            };

            return View("CaseOperationResult", model);
        }

        [ChildActionOnly]
        public PartialViewResult MoveCaseModal(int caseId, int customerId)
        {
            var userId = _workContext.User.UserId;

            var customers =
                _customerService.GetCustomersForCaseMove(userId)
                    .Where(x => x.Id != customerId)
                    .ToList();

            var hasExtendedCase =
                _caseService.HasExtendedCase(caseId, customerId);

            var model = new CaseMoveModel()
            {
                CaseCustomerId = customerId,
                Customers = customers,
                HasExtendedCase = hasExtendedCase
            };

            return PartialView("_MoveCase", model);
        }

        /*	[HttpGet]
            //[Route("Files/{type:string}/{id:int}/filename:string}")]
            public UnicodeFileContentResult CaseFiles(string caseId, string filename)
            {
                return DownloadFile(caseId, filename);
            }

            public UnicodeFileContentResult CaseLogFiles(string type, string logId, string filename)
            {
                LogFileType logFileType;
                if (Enum.TryParse(type, out logFileType))
                {
                    return (UnicodeFileContentResult)DownloadLogFile(logId, filename, logFileType);
                }

                throw new ArgumentException("Invalid log file type", "type");
            }  test */


        #endregion

        #region ***Private Methods***

        #region --Case Template--

        private void CheckTemplateParameters(int? templateId, int caseId)
        {
            if (templateId.HasValue)
            {
                // check template parameters
                var template = _caseSolutionService.GetCaseSolution(templateId.Value);

                // if formGuid this indicates that we want to initate a form by this guid.
                if (template.FormGUID.HasValue)
                {
                    _caseSolutionService.SaveEmptyForm(template.FormGUID.Value, caseId);
                }
            }
        }

        private string GetActiveTab(int? templateId, int caseId)
        {
            if (templateId.HasValue)
            {
                // check template parameters
                var template = _caseSolutionService.GetCaseSolution(templateId.Value);

                return template.DefaultTab;
            }

            return "";
        }

        private CaseTemplateTreeModel GetCaseTemplateTreeModel(int customerId, int userId, CaseSolutionLocationShow location, int? languageId)
        {
            var model = new CaseTemplateTreeModel
            {
                CustomerId = customerId,
                CaseTemplateCategoryTree =
                    _caseSolutionService.GetCaseSolutionCategoryTree(customerId, userId, location, languageId)
                        .Where(c => c.CaseTemplates == null || (c.CaseTemplates != null && c.CaseTemplates.Any())).ToList()
            };

            return model;
        }

        private CaseInputViewModel PopulateBulkCaseEditModal(
         int customerId)
        {
            CaseInputViewModel m = null;

            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                var caseLockModel = new CaseLockModel();

                var customerCaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

                m = this.GetCaseInputViewModel(
                    userId,
                    customerId,
                    0,
                    caseLockModel,
                    customerCaseFieldSettings,
                    string.Empty,
                    null,
                    null,
                    null,
                    false,
                    null, null, null);

                var caseParam = new NewCaseParams
                {
                    customerId = customerId,
                    templateId = null,
                    copyFromCaseId = null,
                    caseLanguageId = null
                };

                m.NewModeParams = caseParam;
                AddViewDataValues();

                // Positive: Send Mail to...
                if (m.CaseMailSetting.DontSendMailToNotifier == false) m.CaseMailSetting.DontSendMailToNotifier = true;
                else m.CaseMailSetting.DontSendMailToNotifier = false;

                var moduleCaseInvoice = GetCustomerSettings(customerId).ModuleCaseInvoice;
                if (moduleCaseInvoice == 1)
                {
#pragma warning disable 0618
                    var caseInvoices = _invoiceArticleService.GetCaseInvoicesWithTimeZone(m.case_.Id, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    var invoiceArticles = _invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
                    m.InvoiceModel = new CaseInvoiceModel(customerId, m.case_.Id, invoiceArticles, "", m.CaseKey, m.LogKey);
#pragma warning restore 0618
                }
                m.Performer_Id = 0;
                return m;
            }

            return null;
        }

        #endregion

        #region --Save/Update--

        /// <summary>
        /// @TODO: this method should defenitely be moved to service in future
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private int Save(CaseEditInput m)
        {
            var utcNow = DateTime.UtcNow;
            var movedFromCustomerId = m.MovedFromCustomerId;
#pragma warning disable 0618
            var case_ = m.case_;
#pragma warning restore 0618
            var caseLog = m.caseLog;
            var caseMailSetting = m.caseMailSetting;
            var updateNotifierInformation = m.updateNotifierInformation;
            m.caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(case_.Customer_Id);
            case_.Performer_User_Id = m.Performer_Id.HasValue && m.Performer_Id == 0 ? null : m.Performer_Id;
            case_.CaseResponsibleUser_Id = m.ResponsibleUser_Id.HasValue && m.ResponsibleUser_Id == 0 ? null : m.ResponsibleUser_Id;
            case_.RegistrationSourceCustomer_Id = m.customerRegistrationSourceId;
            case_.Ou = null;
            case_.Department = null;
            case_.Region = null;
            case_.CaseSolution_Id = m.CaseSolution_Id.HasValue && m.CaseSolution_Id == 0 ? null : m.CaseSolution_Id;
            case_.CurrentCaseSolution_Id = m.CurrentCaseSolution_Id;

            //case_.CurrentCaseSolution_Id = m.
            //case_.ActiveTab = m.ActiveTab;

            bool edit = case_.Id != 0;
            var isItChildCase = m.ParentId.HasValue;
            Case parentCase = null;
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);

            if (isItChildCase)
            {
                parentCase = _caseService.GetCaseById(m.ParentId.Value);
                if (parentCase == null)
                {
                    throw new ArgumentException("parentCaseId has an invalid value");
                }
            }

            IDictionary<string, string> errors;
            var mailSenders = new MailSenders();
            if (case_.RegLanguage_Id == 0)
            {
                case_.RegLanguage_Id = SessionFacade.CurrentLanguageId;
            }

            if (case_.IsAbout != null)
                case_.IsAbout.Id = case_.Id;

            // Positive: Send Mail to...
            caseMailSetting.DontSendMailToNotifier = caseMailSetting.DontSendMailToNotifier == false;

            mailSenders.SystemEmail = caseMailSetting.HelpdeskMailFromAdress;

            if (case_.WorkingGroup_Id.HasValue)
            {
                var curWG = _workingGroupService.GetWorkingGroup(case_.WorkingGroup_Id.Value);
                if (curWG != null)
                    if (!string.IsNullOrWhiteSpace(curWG.EMail) && _emailService.IsValidEmail(curWG.EMail))
                        mailSenders.WGEmail = curWG.EMail;
            }

            if (case_.DefaultOwnerWG_Id.HasValue && case_.DefaultOwnerWG_Id.Value > 0)
            {
                var defaultWGEmail = _workingGroupService.GetWorkingGroup(case_.DefaultOwnerWG_Id.Value).EMail;
                mailSenders.DefaultOwnerWGEMail = defaultWGEmail;
            }

            // get case as it was before edit
            var curCustomer = _customerService.GetCustomer(case_.Customer_Id);
            if (curCustomer == null)
            {
                throw new ArgumentException("Case customer has an invalid value");
            }

            var customerSetting = GetCustomerSettings(curCustomer.Id);

            // offset in Minute
            var customerTimeOffset = customerSetting.TimeZone_offset;
            var actionExternalTime = 0;

            //If Persons_Email field not show on case, then the field should not be saved on case in db
            var customerfieldSettings = _caseFieldSettingService.GetCaseFieldSettings(case_.Customer_Id);

            if (customerfieldSettings.Where(fs =>
                    fs.Name == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString() &&
                    fs.ShowOnStartPage == 0).Any())
            {
                case_.PersonsEmail = string.Empty;
            }

            var oldCase = new Case();
            var oldCaseSubstateCount = true;
            if (edit)
            {
                #region Editing existing case

                oldCase = _caseService.GetDetachedCaseById(case_.Id);
                var cu = _customerUserService.GetCustomerUserSettings(case_.Customer_Id, SessionFacade.CurrentUser.Id);
                if (cu != null)
                {
                    if (cu.UserInfoPermission == 0)
                    {
                        case_.ReportedBy = oldCase.ReportedBy;
                        case_.Place = oldCase.Place;
                        case_.PersonsName = oldCase.PersonsName;
                        case_.PersonsEmail = oldCase.PersonsEmail;
                        case_.PersonsPhone = oldCase.PersonsPhone;
                        case_.PersonsCellphone = oldCase.PersonsCellphone;
                        case_.Region_Id = oldCase.Region_Id;
                        case_.Department_Id = oldCase.Department_Id;
                        case_.OU_Id = oldCase.OU_Id;
                        case_.UserCode = oldCase.UserCode;
                    }
                }

                if (oldCase.StateSecondary_Id.HasValue)
                {
                    var subState = _stateSecondaryService.GetStateSecondary(oldCase.StateSecondary_Id.Value);

                    // calculating time spent in "inactive" state since last changing every save
                    if (subState.IncludeInCaseStatistics == 0)
                    {
                        oldCaseSubstateCount = false;

                        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(curCustomer.TimeZoneId);

                        var workTimeCalcFactory =
                            new WorkTimeCalculatorFactory(
                                ManualDependencyResolver.Get<IHolidayService>(),
                                curCustomer.WorkingDayStart,
                                curCustomer.WorkingDayEnd,
                                timeZone);
                        int[] deptIds = null;
                        if (case_.Department_Id.HasValue)
                        {
                            deptIds = new int[] { case_.Department_Id.Value };
                        }

                        var workTimeCalc = workTimeCalcFactory.Build(oldCase.RegTime, utcNow, deptIds);

                        var externalTime = workTimeCalc.CalculateWorkTime(
                            oldCase.ChangeTime,
                            utcNow,
                            oldCase.Department_Id);
                        var newExternalTime = oldCase.ExternalTime + externalTime;

                        case_.ExternalTime = newExternalTime;

                        case_.LeadTime = workTimeCalc.CalculateWorkTime(
                             oldCase.RegTime,
                             utcNow,
                             oldCase.Department_Id) - newExternalTime;

                        actionExternalTime = workTimeCalc.CalculateWorkTime(
                            oldCase.ChangeTime,
                            utcNow,
                            oldCase.Department_Id, customerTimeOffset);
                    }
                }

                case_.RegTime = DateTime.SpecifyKind(oldCase.RegTime, DateTimeKind.Utc);
                #endregion
            }
            else
            {
                #region NewCase

                case_.RegTime = utcNow;
                #endregion
            }

            if (case_.ProductArea_Id.HasValue && case_.ProductAreaSetDate == null)
                case_.ProductAreaSetDate = utcNow;

            if (movedFromCustomerId.HasValue)
                oldCase.ProductAreaSetDate = null;

            case_.LatestSLACountDate = _caseStatService.CalculateLatestSLACountDate(oldCase.StateSecondary_Id, case_.StateSecondary_Id, oldCase.LatestSLACountDate);

            //var leadTimeForHistory = 0;
            var actionLeadTime = 0;

            if (caseLog != null && caseLog.FinishingType > 0)
            {
                if (caseLog.FinishingDate == null)
                {
                    caseLog.FinishingDate = utcNow;
                }
                else
                {
                    // för att få med klockslag
                    if (caseLog.FinishingDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString())
                    {
                        caseLog.FinishingDate = utcNow;
                    }
                    else if (oldCase != null && oldCase.ChangeTime.ToShortDateString() == caseLog.FinishingDate.Value.ToShortDateString())
                    {
                        var lastChangedTime = new DateTime(oldCase.ChangeTime.Year, oldCase.ChangeTime.Month, oldCase.ChangeTime.Day, 22, 59, 59);
                        caseLog.FinishingDate = lastChangedTime;
                    }
                    else
                    {
                        //Manually set
                        caseLog.FinishingDate = caseLog.FinishingDate.Value.AddHours(8);
                    }
                }

                case_.FinishingDate = DatesHelper.Max(case_.RegTime, caseLog.FinishingDate.Value);
            }

            #region WorkingTime calculation

            actionLeadTime = CalculateActionLeadTime(curCustomer, case_, utcNow, oldCaseSubstateCount, oldCase,
                customerTimeOffset, actionExternalTime, caseLog != null && caseLog.FinishingType > 0);

            #endregion

            var childCasesIds = _caseService.GetChildCasesFor(case_.Id).Where(it => !it.ClosingDate.HasValue).Select(it => it.Id).ToArray();

            var ei = new CaseExtraInfo()
            {
                CreatedByApp = CreatedByApplications.Helpdesk5,
                LeadTimeForNow = case_.LeadTime,
                ActionLeadTime = actionLeadTime,
                ActionExternalTime = actionExternalTime
            };

            // save case and case history
            // TODO: better time calculation (move to a service)
            int caseHistoryId = _caseService.SaveCase(
                        case_,
                        caseLog,
                        SessionFacade.CurrentUser.Id,
                        User.Identity.Name,
                        ei,
                        out errors,
                        parentCase,
                        m.FollowerUsers);
            var mergeParent = _caseService.GetMergedParentInfo(case_.Id);
            if (isItChildCase && mergeParent == null)
            {
                _caseService.SetIndependentChild(case_.Id, m.IndependentChild);
            }

            if (updateNotifierInformation.HasValue && updateNotifierInformation.Value)
            {
                var names = case_.PersonsName.Split(' ');

                var fName = string.Empty;
                var lName = string.Empty;

                if (names.Length > 0)
                {
                    for (int i = 0; i < names.Length - 1; i++)
                        if (i == 0)
                            fName = names[i];
                        else
                            fName += " " + names[i];
                }

                if (names.Length > 1)
                    lName = names[names.Length - 1];

                int lng = case_.RegLanguage_Id;

                var caseNotifier = _caseNotifierModelFactory.Create(
                                                            case_.ReportedBy,
                                                            fName,
                                                            lName,
                                                            case_.PersonsEmail,
                                                            case_.PersonsPhone,
                                                            case_.PersonsCellphone,
                                                            case_.Department_Id,
                                                            case_.OU_Id,
                                                            case_.Place,
                                                            case_.UserCode,
                                                            case_.Customer_Id,
                                                            lng);
                //m.InitiatorCategory); //todo: check if category is required

                _notifierService.UpdateCaseNotifier(caseNotifier);
            }

            #region ExtendedCase sections

            if (m.ContainsExtendedCase)
            {
                /* Create Relationship between Case & ExtendedCase*/
                if (m.ExtendedCaseGuid != Guid.Empty)
                {
                    var exData = _caseService.GetExtendedCaseData(m.ExtendedCaseGuid);
                    _caseService.CreateExtendedCaseRelationship(case_.Id, exData.Id, exData.ExtendedCaseFormId);
                }
#pragma warning disable 0618
                if (m.case_.ReportedBy != null && m.ExtendedInitiatorGUID.HasValue)
                {
                    var exData = _caseService.GetExtendedCaseData(m.ExtendedInitiatorGUID.Value);
                    _caseService.CreateExtendedCaseSectionRelationship(case_.Id, exData.Id, CaseSectionType.Initiator, curCustomer.Id);
                }
                if (m.case_.IsAbout.ReportedBy != null && m.ExtendedRegardingGUID.HasValue)
                {
                    var exData = _caseService.GetExtendedCaseData(m.ExtendedRegardingGUID.Value);
                  
                    _caseService.CreateExtendedCaseSectionRelationship(case_.Id, exData.Id, CaseSectionType.Regarding, curCustomer.Id);
                }
#pragma warning restore 0618
            }
            if (edit) // If edit existing case
            {
                if (m.ExtendedInitiatorGUID.HasValue)
                {
#pragma warning disable 0618
                    var exData = _caseService.GetExtendedCaseData(m.ExtendedInitiatorGUID.Value);
                    _caseService.CheckAndUpdateExtendedCaseSectionData(exData.Id, m.case_.Id, m.case_.Customer_Id, CaseSectionType.Initiator);
#pragma warning restore 0618
                }
                else
                {
#pragma warning disable 0618
                    _caseService.RemoveAllExtendedCaseSectionData(case_.Id, m.case_.Customer_Id, CaseSectionType.Initiator);
#pragma warning restore 0618
                }

                if (m.ExtendedRegardingGUID.HasValue)
                {
#pragma warning disable 0618
                    var exData = _caseService.GetExtendedCaseData(m.ExtendedRegardingGUID.Value);
                    _caseService.CheckAndUpdateExtendedCaseSectionData(exData.Id, m.case_.Id, m.case_.Customer_Id, CaseSectionType.Regarding);
#pragma warning restore 0618
                }
                else
                {
#pragma warning disable 0618
                    _caseService.RemoveAllExtendedCaseSectionData(case_.Id, m.case_.Customer_Id, CaseSectionType.Regarding);
#pragma warning restore 0618

                }
            }

            #endregion

            var basePath = _masterDataService.GetFilePath(case_.Customer_Id);
            var disableLogFileView = _featureToggleService.Get(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE);

            // save case files
            var temporaryFiles = _userTemporaryFilesStorage.FindFiles(!edit ? case_.CaseGUID.ToString() : case_.Id.ToString(), ModuleName.Cases);
            var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, utcNow, case_.Id, _workContext.User.UserId)).ToList();

            var paths = new List<KeyValuePair<CaseFileDto, string>>();
            _caseFileService.AddFiles(newCaseFiles, paths);

            if (!disableLogFileView.Active)
            {
                foreach (var file in paths)
                {
                    var userId = SessionFacade.CurrentUser?.Id ?? 0;
                    _fileViewLogService.Log(case_.Id, userId, file.Key.FileName, file.Value, FileViewLogFileSource.Helpdesk, FileViewLogOperation.Add);
                }
            }

            #region Save Logs

            // save log
            caseLog.CaseId = case_.Id;
            caseLog.CaseHistoryId = caseHistoryId;

            var temporaryLogInternalFiles = new List<WebTemporaryFile>();
            var temporaryLogFiles = _userTemporaryFilesStorage.FindFiles(caseLog.LogGuid.ToString(), ModuleName.Log);
            var temporaryExLogFiles = _logFileService.GetExistingFileNamesByCaseId(case_.Id);

            /* #58573 Check that user have access to write to InternalLogNote */
            bool hasInternalLogAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseInternalLogPermission);
            if (hasInternalLogAccess)
            {
                // read internal log files if any
                temporaryLogInternalFiles = _userTemporaryFilesStorage.FindFiles(caseLog.LogGuid.ToString(), ModuleName.LogInternal);
            }
            else
            {
                // clear out internal log if no access
                m.caseLog.TextInternal = null;
            }

            var logFileCount = temporaryLogFiles.Count + temporaryExLogFiles.Count + temporaryLogInternalFiles.Count;
            var orginalInternalLog = caseLog.TextInternal;

            if (caseLog.SendLogToParentChildLog.HasValue &&
                caseLog.SendLogToParentChildLog.Value &&
                !string.IsNullOrEmpty(caseLog.TextInternal) &&
                childCasesIds.Length > 0)
            {
                var parentChildCasesMarker = Translation.GetCoreTextTranslation(CaseLog.ParentChildCasesMarker);
                caseLog.TextInternal = string.Format("[{0}]: {1}", parentChildCasesMarker, caseLog.TextInternal);
            }

            if (caseLog.SendLogToParentChildLog.HasValue && caseLog.SendLogToParentChildLog.Value && parentCase != null)
            {
                var childParentCasesMarker = Translation.GetCoreTextTranslation(CaseLog.ChildParentCasesMarker);
                caseLog.TextInternal = string.Format("[{0}]: {1}", childParentCasesMarker, caseLog.TextInternal);
            }

            // SAVE LOG:
            caseLog.Id = _logService.SaveLog(caseLog, logFileCount, out errors);

            caseLog.TextInternal = orginalInternalLog;

            if (caseLog != null &&
                caseLog.SendLogToParentChildLog.HasValue &&
                caseLog.SendLogToParentChildLog.Value &&
                !string.IsNullOrEmpty(caseLog.TextInternal))
            {
                if (parentCase != null)
                {
                    var parentCaseLog = new CaseLog
                    {
                        CaseId = parentCase.Id,
                        UserId = caseLog.UserId,
                        TextInternal = string.Format("[{0} #{1}]: {2}", Translation.GetCoreTextTranslation(CaseLog.ChildCaseMarker), case_.CaseNumber, caseLog.TextInternal)
                    };
                    UpdateCaseLogForCase(parentCase, parentCaseLog);
                }

                if (childCasesIds != null && childCasesIds.Length > 0)
                {
                    caseLog.TextInternal = string.Format("[{0}]: {1}", Translation.GetCoreTextTranslation(CaseLog.ParentCaseMarker), caseLog.TextInternal);
                    _logService.AddParentCaseLogToChildCases(childCasesIds, caseLog);
                }
            }

            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseLogFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id, _workContext.User.UserId, LogFileType.External, null)).ToList();
            if (temporaryLogInternalFiles.Any())
            {
                var internalLogFiles = temporaryLogInternalFiles.Select(f => new CaseLogFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id, _workContext.User.UserId, LogFileType.Internal, null)).ToList();
                newLogFiles.AddRange(internalLogFiles);
            }

            var logPaths = new List<KeyValuePair<CaseLogFileDto, string>>();

            _logFileService.AddFiles(newLogFiles, logPaths, temporaryExLogFiles, caseLog.Id);

            var allLogFiles =
                temporaryExLogFiles.Select(x =>
                    new CaseLogFileDto(basePath,
                        x.Name,
                        x.IsExistCaseFile ? Convert.ToInt32(case_.CaseNumber) : x.LogId.Value,
                        x.IsExistCaseFile)
                    {
                        LogType = x.IsInternalLogNote ? LogFileType.Internal : LogFileType.External,
                        ParentLogType = x.IsExistCaseFile ? (LogFileType?)null : x.LogType
                    })
                .ToList();
            allLogFiles.AddRange(newLogFiles);

            if (!disableLogFileView.Active)
            {
                foreach (var newLogFile in newLogFiles)
                {
                    var userId = SessionFacade.CurrentUser?.Id ?? 0;
                    var path = _filesStorage.ComposeFilePath((newLogFile.ParentLogType ?? newLogFile.LogType).GetFolderPrefix(),
                        caseLog.Id, basePath, "");
                    _fileViewLogService.Log(case_.Id, userId, newLogFile.FileName, path, FileViewLogFileSource.Helpdesk, FileViewLogOperation.Add);
                }
            }

            #endregion

            if (movedFromCustomerId.HasValue)
            {
                var fromBasePath = _masterDataService.GetFilePath(movedFromCustomerId.Value);
                _caseService.DeleteExCaseWhenCaseMove(case_.Id);

                if (!fromBasePath.Equals(basePath, StringComparison.CurrentCultureIgnoreCase))
                {
                    _caseFileService.MoveCaseFiles(case_.CaseNumber.ToString(), fromBasePath, basePath);
                    _logFileService.MoveLogFiles(case_.Id, fromBasePath, basePath);
                }
            }

            //save external invoices
            if (m.ExternalInvoices != null)
            {
                _externalInvoiceService.SaveExternalInvoices(case_.Id,
                    m.ExternalInvoices.Where(x => !String.IsNullOrWhiteSpace(x.Name)).Select(x => new ExternalInvoice
                    {
                        Id = x.Id,
                        InvoiceNumber = x.Name,
                        InvoicePrice = x.Amount,
                        CreatedDate = DateTime.UtcNow,
                        CreatedByUserId = _workContext.User.UserId,
                        Charge = true
                    }).ToList());
            }

            //save extra followers
            var followerUsers = new List<string>();
            if (edit)
            {
                if (!string.IsNullOrEmpty(m.FollowerUsers))
                {
                    followerUsers = m.FollowerUsers.Split(BRConstItem.Email_Char_Separator).Where(s => !string.IsNullOrWhiteSpace(s)).Select(x => x.Trim()).ToList();
                }
                _caseExtraFollowersService.SaveExtraFollowers(case_.Id, followerUsers, _workContext.User.UserId);
            }
            else
            {
                if (!string.IsNullOrEmpty(m.FollowerUsers))
                {
                    followerUsers = m.FollowerUsers.Split(BRConstItem.Email_Char_Separator).Where(s => !string.IsNullOrWhiteSpace(s)).Select(x => x.Trim()).ToList();
                    _caseExtraFollowersService.SaveExtraFollowers(case_.Id, followerUsers, _workContext.User.UserId);
                }
            }

            caseMailSetting.CustomeMailFromAddress = mailSenders;

            //get logged in user
            var currentLoggedInUser = _userService.GetUser(SessionFacade.CurrentUser.Id);

            // send emails
            _caseService.SendCaseEmail(case_.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase, caseLog, allLogFiles, currentLoggedInUser);

            var actions = _caseService.CheckBusinessRules(BREventType.OnSaveCase, case_, oldCase);
            if (actions.Any())
                _caseService.ExecuteBusinessActions(actions, case_.Id, caseLog, userTimeZone, caseHistoryId, basePath, SessionFacade.CurrentLanguageId, caseMailSetting, allLogFiles);

            //Unlock Case            
            if (m.caseLock != null && !string.IsNullOrEmpty(m.caseLock.LockGUID))
                _caseLockService.UnlockCaseByGUID(new Guid(m.caseLock.LockGUID));

            // delete temp folders                
            _userTemporaryFilesStorage.ResetCacheForObject(case_.Id);
            _userTemporaryFilesStorage.ResetCacheForObject(case_.CaseGUID.ToString());
            _userTemporaryFilesStorage.ResetCacheForObject(caseLog.LogGuid.ToString());

            return case_.Id;
        }

        private int CalculateActionLeadTime(Customer curCustomer, Case case_, DateTime utcNow,
            bool oldCaseSubstateCount,
            Case oldCase, int customerTimeOffset, int actionExternalTime, bool hasFinishingType)
        {
            var actionLeadTime = 0;

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(curCustomer.TimeZoneId);

            var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                ManualDependencyResolver.Get<IHolidayService>(),
                curCustomer.WorkingDayStart,
                curCustomer.WorkingDayEnd,
                timeZone);
            int[] deptIds = null;
            if (case_.Department_Id.HasValue)
            {
                deptIds = new int[] { case_.Department_Id.Value };
            }

            var workTimeCalc = workTimeCalcFactory.Build(case_.RegTime, utcNow, deptIds);

            // If should count time on old status and finish date is earlier than now, add external time for time in "finish" state to now
            if (hasFinishingType && oldCaseSubstateCount && case_.FinishingDate.HasValue && case_.FinishingDate.Value < utcNow)
            {
                case_.ExternalTime += workTimeCalc.CalculateWorkTime(case_.FinishingDate.Value, utcNow, case_.Department_Id);
            }

            var possibleWorkTime = workTimeCalc.CalculateWorkTime(
                case_.RegTime,
                utcNow,
                case_.Department_Id);
            var leadTime = possibleWorkTime - case_.ExternalTime;

            case_.LeadTime = leadTime;

            // ActionLeadTime Calc
            if (oldCase != null && oldCase.Id > 0)
            {
                deptIds = null;
                if (oldCase.Department_Id.HasValue)
                {
                    deptIds = new int[] { oldCase.Department_Id.Value };
                }

                var endTime = hasFinishingType ? case_.FinishingDate.Value.ToUniversalTime() : utcNow;
                workTimeCalc =
                    workTimeCalcFactory.Build(oldCase.ChangeTime, endTime, deptIds, customerTimeOffset);
                actionLeadTime = workTimeCalc.CalculateWorkTime(
                                     oldCase.ChangeTime, endTime,
                                     oldCase.Department_Id, customerTimeOffset) - actionExternalTime;
            }

            return actionLeadTime;
        }

        private void UpdateCaseLogForCase(Case @case, CaseLog caseLog)
        {
            if (@case == null || caseLog == null)
            {
                throw new ArgumentException("@case or/and caseLog is null");
            }

            IDictionary<string, string> errors;

            var c = _caseService.GetCaseById(caseLog.CaseId);

            _logService.AddChildCaseLogToParentCase(c.Id, caseLog);

            // save case and case history
            var ei = new CaseExtraInfo() { CreatedByApp = CreatedByApplications.Helpdesk5, LeadTimeForNow = 0, ActionLeadTime = 0, ActionExternalTime = 0 };
            int caseHistoryId = _caseService.SaveCase(c, caseLog, SessionFacade.CurrentUser.Id, this.User.Identity.Name, ei, out errors);
            caseLog.CaseHistoryId = caseHistoryId;
        }

        private void NewCaselog(Case @case, CaseLog caseLog)
        {
            if (caseLog == null)
            {
                throw new ArgumentException("caseLog is null");
            }

            IDictionary<string, string> errors;

            var oldCase = _caseService.GetCaseById(caseLog.CaseId);

            var customer = _customerService.GetCustomer(oldCase.Customer_Id);
            var customerSetting = GetCustomerSettings(oldCase.Customer_Id);
            var basePath = _masterDataService.GetFilePath(oldCase.Customer_Id);
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            var currentLoggedInUser = _userService.GetUser(SessionFacade.CurrentUser.Id);

            var caseMailSetting = new CaseMailSetting(
              customer.NewCaseEmailList,
              customer.HelpdeskEmail,
              RequestExtension.GetAbsoluteUrl(),
              customerSetting.DontConnectUserToWorkingGroup);

            var mailSenders = new MailSenders();

            // Positive: Send Mail to...
            if (caseMailSetting.DontSendMailToNotifier == false)
                caseMailSetting.DontSendMailToNotifier = true;
            else
                caseMailSetting.DontSendMailToNotifier = false;

            mailSenders.SystemEmail = caseMailSetting.HelpdeskMailFromAdress;

            if (oldCase.WorkingGroup_Id.HasValue)
            {
                var curWG = _workingGroupService.GetWorkingGroup(oldCase.WorkingGroup_Id.Value);
                if (curWG != null)
                    if (!string.IsNullOrWhiteSpace(curWG.EMail) && _emailService.IsValidEmail(curWG.EMail))
                        mailSenders.WGEmail = curWG.EMail;
            }

            if (oldCase.DefaultOwnerWG_Id.HasValue && oldCase.DefaultOwnerWG_Id.Value > 0)
            {
                var defaultWGEmail = _workingGroupService.GetWorkingGroup(oldCase.DefaultOwnerWG_Id.Value).EMail;
                mailSenders.DefaultOwnerWGEMail = defaultWGEmail;
            }
            caseMailSetting.CustomeMailFromAddress = mailSenders;

            //Count time
            var leadTime = 0;
            var actionLeadTime = 0;
            var customerTimeOffset = customerSetting.TimeZone_offset;
            var actionExternalTime = 0;

            if (oldCase.StateSecondary_Id.HasValue)
            {
                var caseSubState = _stateSecondaryService.GetStateSecondary(oldCase.StateSecondary_Id.Value);

                // calculating time spent in "inactive" state since last changing every save
                if (caseSubState.IncludeInCaseStatistics == 0)
                {
                    var timeZone = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZoneId);

                    var workTimeCalcFactory =
                        new WorkTimeCalculatorFactory(
                            ManualDependencyResolver.Get<IHolidayService>(),
                            customer.WorkingDayStart,
                            customer.WorkingDayEnd,
                            timeZone);
                    int[] deptIds = null;
                    if (@case.Department_Id.HasValue)
                    {
                        deptIds = new int[] { @case.Department_Id.Value };
                    }

                    var workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, DateTime.UtcNow, deptIds);
                    var externalTime = workTimeCalc.CalculateWorkTime(
                        oldCase.ChangeTime,
                        DateTime.UtcNow,
                        oldCase.Department_Id);

                    @case.ExternalTime = oldCase.ExternalTime + externalTime;
                    actionExternalTime = externalTime;

                    deptIds = null;
                    if (@case.Department_Id.HasValue)
                    {
                        deptIds = new int[] { @case.Department_Id.Value };
                    }
                }
            }

            oldCase.LatestSLACountDate = _caseStatService.CalculateLatestSLACountDate(oldCase.StateSecondary_Id, @case.StateSecondary_Id, oldCase.LatestSLACountDate);

            // save case and case history
            oldCase.StateSecondary_Id = @case.StateSecondary_Id;
            caseLog.UserId = currentLoggedInUser.Id;


            if (caseLog != null && caseLog.FinishingType > 0)
            {
                if (caseLog.FinishingDate == null)
                {
                    caseLog.FinishingDate = DateTime.UtcNow;
                }
                else
                {
                    // för att få med klockslag
                    if (caseLog.FinishingDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString())
                    {
                        caseLog.FinishingDate = DateTime.UtcNow;
                    }
                    else if (oldCase != null && oldCase.ChangeTime.ToShortDateString() == caseLog.FinishingDate.Value.ToShortDateString())
                    {
                        var lastChangedTime = new DateTime(oldCase.ChangeTime.Year, oldCase.ChangeTime.Month, oldCase.ChangeTime.Day, 22, 59, 59);
                        caseLog.FinishingDate = lastChangedTime;
                    }
                    else
                    {
                        caseLog.FinishingDate = DateTime.SpecifyKind(caseLog.FinishingDate.Value, DateTimeKind.Local).ToUniversalTime();
                    }
                }

                oldCase.FinishingDate = DatesHelper.Max(oldCase.RegTime, caseLog.FinishingDate.Value);

                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZoneId);

                var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                    ManualDependencyResolver.Get<IHolidayService>(),
                    customer.WorkingDayStart,
                    customer.WorkingDayEnd,
                    timeZone);
                int[] deptIds = null;
                if (oldCase.Department_Id.HasValue)
                {
                    deptIds = new int[] { oldCase.Department_Id.Value };
                }

                var workTimeCalc = workTimeCalcFactory.Build(oldCase.RegTime, oldCase.FinishingDate.Value, deptIds);
                leadTime = workTimeCalc.CalculateWorkTime(
                    oldCase.RegTime,
                    oldCase.FinishingDate.Value.ToUniversalTime(),
                    oldCase.Department_Id) - oldCase.ExternalTime;

                oldCase.LeadTime = leadTime;

                // ActionLeadTime Calc
                if (oldCase != null && oldCase.Id > 0)
                {
                    workTimeCalcFactory = new WorkTimeCalculatorFactory(
                        ManualDependencyResolver.Get<IHolidayService>(),
                        customer.WorkingDayStart,
                        customer.WorkingDayEnd,
                        timeZone);
                    deptIds = null;
                    if (oldCase.Department_Id.HasValue)
                    {
                        deptIds = new int[] { oldCase.Department_Id.Value };
                    }

                    workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, oldCase.FinishingDate.Value, deptIds, customerTimeOffset);
                    actionLeadTime = workTimeCalc.CalculateWorkTime(
                        oldCase.ChangeTime,
                        oldCase.FinishingDate.Value.ToUniversalTime(),
                        oldCase.Department_Id, customerTimeOffset) - actionExternalTime;
                }
            }

            var ei = new CaseExtraInfo()
            {
                CreatedByApp = CreatedByApplications.Helpdesk5,
                LeadTimeForNow = leadTime,
                ActionLeadTime = actionLeadTime,
                ActionExternalTime = actionExternalTime
            };
            //End

            //var ei = new CaseExtraInfo() {CreatedByApp = CreatedByApplications.Helpdesk5, LeadTimeForNow = 0, ActionLeadTime = 0, ActionExternalTime = 0 };
            int caseHistoryId = _caseService.SaveCase(oldCase, caseLog, SessionFacade.CurrentUser.Id, User.Identity.Name, ei, out errors);
            caseLog.CaseHistoryId = caseHistoryId;

            //find files for old log
            var logFiles = _logFileService.GetLogFileNamesByLogId(caseLog.OldLog_Id.Value, true);

            caseLog.Id = _logService.SaveLog(caseLog, logFiles.Count, out errors);

            byte[] fileContent;

            var newLogFiles = new List<CaseLogFileDto>();
            var exFiles = new List<LogExistingFileModel>();
            foreach (var file in logFiles)
            {
                if (file.IsExistLogFile)
                {
                    var logNoteFile = new LogExistingFileModel
                    {
                        Name = file.Name,
                        CaseId = caseLog.CaseId,
                        IsExistCaseFile = file.IsExistCaseFile,
                        IsExistLogFile = file.IsExistLogFile,
                        LogId = caseLog.Id,
                        LogType = file.LogType
                    };
                    exFiles.Add(logNoteFile);
                }
                else
                {
                    if (file.IsExistCaseFile)
                    {
                        var logNoteFile = new LogExistingFileModel
                        {
                            Name = file.Name,
                            CaseId = caseLog.CaseId,
                            IsExistCaseFile = file.IsExistCaseFile,
                            IsExistLogFile = file.IsExistLogFile,
                            LogType = file.LogType
                        };
                        exFiles.Add(logNoteFile);
                    }
                    else
                    {
                        var model = _logFileService.GetFileContentByIdAndFileName(caseLog.OldLog_Id.Value, basePath, file.Name, file.LogType);
                        fileContent = model.Content;
                        var logNoteFile = new CaseLogFileDto(fileContent, basePath, file.Name, DateTime.UtcNow, caseLog.Id, _workContext.User.UserId, file.LogType, null);
                        newLogFiles.Add(logNoteFile);
                    }
                }
            }

            var logPaths = new List<KeyValuePair<CaseLogFileDto, string>>();
            _logFileService.AddFiles(newLogFiles, logPaths, exFiles, caseLog.Id);

            if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
            {
                var userId = currentLoggedInUser.Id;
                foreach (var path in logPaths)
                {
                    _fileViewLogService.Log(oldCase.Id, userId, path.Key.FileName, path.Value, FileViewLogFileSource.Helpdesk, FileViewLogOperation.Add);
                }
            }

            // send emails
            _caseService.SendCaseEmail(oldCase.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase, caseLog, null, currentLoggedInUser);
        }
        #endregion

        #region --Get Data--

        /// <summary>
        /// The edit mode.
        /// </summary>
        /// <param name="m">
        /// The m.
        /// </param>
        /// <param name="topic">
        /// The topic.
        /// </param>
        /// <param name="departmensForUser">
        /// The departments for user.
        /// </param>
        /// <param name="accessToWorkinggroups">
        /// The access to working groups.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        /// 
        private AccessMode EditMode(CaseInputViewModel m,
                                          string topic,
                                          IList<Department> departmensForUser,
                                          List<CustomerWorkingGroupForUser> accessToWorkinggroups,
                                          bool temporaryHasAccessToWG = false)
        {
            var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();

            if (m == null)
            {
                return AccessMode.NoAccess;
            }

            if (SessionFacade.CurrentUser == null)
            {
                return AccessMode.NoAccess;
            }

#pragma warning disable 0618
            if (m.case_ == null)
            {
                return AccessMode.NoAccess;
            }
#pragma warning restore 0618

            if (departmensForUser != null)
            {
                var accessToDepartments = departmensForUser.Select(d => d.Id).ToList();
                if (SessionFacade.CurrentUser.UserGroupId < 3)
                {
#pragma warning disable 0618
                    if (accessToDepartments.Count > 0 && m.case_.Department_Id.HasValue)
                    {
                        if (!accessToDepartments.Contains(m.case_.Department_Id.Value))
                        {
                            return AccessMode.NoAccess;
                        }
                    }
#pragma warning restore 0618
                }
            }

            // In new case shouldn't check
            /*Updated in this way:*/
            /*If user does not have access to WG, if last action was "Save", user can see the Case in readonly mode 
             * there is no ticket. (Per knows more info)
             */

#pragma warning disable 0618
            if (accessToWorkinggroups != null && m.case_.Id != 0)
            {
                if (SessionFacade.CurrentUser.UserGroupId < 3)
                {
                    if (accessToWorkinggroups.Count > 0 && m.case_.WorkingGroup_Id.HasValue)
                    {
                        var wg = accessToWorkinggroups.FirstOrDefault(w => w.WorkingGroup_Id == m.case_.WorkingGroup_Id.Value);
                        if (wg == null && (gs != null && gs.LockCaseToWorkingGroup == 1))
                        {
                            return temporaryHasAccessToWG ? AccessMode.ReadOnly : AccessMode.NoAccess;
                        }

                        if (wg != null && wg.RoleToUWG == 1)
                        {
                            return AccessMode.ReadOnly;
                        }
                    }
                }
            }
#pragma warning restore 0618

#pragma warning disable 0618
            if (m.case_.FinishingDate.HasValue)
            {
                return AccessMode.ReadOnly;
            }
#pragma warning restore 0618

            if (m.CaseLock != null && m.CaseLock.IsLocked)
            {
                return AccessMode.ReadOnly;
            }

            if (topic == ModuleName.Log
                && SessionFacade.CurrentUser.UserGroupId == (int)BusinessData.Enums.Admin.Users.UserGroup.Administrator
                && SessionFacade.CurrentUser.Id != m.CaseLog.UserId)
            {
                return AccessMode.ReadOnly;
            }

            return AccessMode.FullAccess;
        }

        private string GetIdsFromSearchResult(IList<CaseSearchResult> cases)
        {
            if (cases == null)
                return string.Empty;

            return string.Join(",", cases.Select(c => c.Id));
        }

        private List<string> GetInactiveFieldsValue(CaseMasterDataFieldsModel fields)
        {
            var ret = new List<string>();

            if (fields == null || fields.CustomerId <= 0)
            {
                throw new ArgumentException("@Customer is not valid!");
            }

            if (fields.RegionId.HasValue)
            {
                var region = _regionService.GetRegion(fields.RegionId.Value);
                if (region != null && region.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Region_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.CaseTypeId.HasValue)
            {
                var caseType = _caseTypeService.GetCaseType(fields.CaseTypeId.Value);
                if (caseType != null && caseType.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.ProductAreaId.HasValue)
            {
                var productArea = _productAreaService.GetProductArea(fields.ProductAreaId.Value);
                if (productArea != null && productArea.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.CategoryId.HasValue)
            {
                var category = _categoryService.GetCategory(fields.CategoryId.Value, fields.CustomerId);
                if (category != null && category.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Category_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.SystemId.HasValue)
            {
                var system = _systemService.GetSystem(fields.SystemId.Value);
                if (system != null && system.Status == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.System_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.SupplierId.HasValue)
            {
                var supplier = _supplierService.GetSupplier(fields.SupplierId.Value);
                if (supplier != null && supplier.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Supplier_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.PriorityId.HasValue)
            {
                var priority = _priorityService.GetPriority(fields.PriorityId.Value);
                if (priority != null && priority.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Priority_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.StatusId.HasValue)
            {
                var status = _statusService.GetStatus(fields.StatusId.Value);
                if (status != null && status.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Status_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.SubStatusId.HasValue)
            {
                var subStatus = _stateSecondaryService.GetStateSecondary(fields.SubStatusId.Value);
                if (subStatus != null && subStatus.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.WorkingGroupId.HasValue)
            {
                var workingGroup = _workingGroupService.GetWorkingGroup(fields.WorkingGroupId.Value);
                if (workingGroup != null && workingGroup.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            return ret;
        }

        private string GetDepartmentsFrom(string departments_OrganizationUnits)
        {
            string ret = string.Empty;
            var ids = departments_OrganizationUnits.Split(',');
            var depIds = new List<int>();
            if (ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (!string.IsNullOrEmpty(ids[i].Trim()))
                    {
                        var id = int.Parse(ids[i].Trim());
                        if (id > 0 || id == ObjectExtensions.notAssignedDepartment().Id)
                        {
                            depIds.Add(id);
                        }
                    }
                }
            }
            if (depIds.Any())
                ret = string.Join(",", depIds);
            return ret;
        }

        private string GetOrganizationUnitsFrom(string departments_OrganizationUnits)
        {
            string ret = string.Empty;
            var ids = departments_OrganizationUnits.Split(',');
            var ouIds = new List<int>();
            if (ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (!string.IsNullOrEmpty(ids[i].Trim()))
                    {
                        var id = int.Parse(ids[i].Trim());
                        if (id < 0 && id != ObjectExtensions.notAssignedDepartment().Id)
                        {
                            ouIds.Add(-id);
                        }
                    }
                }
            }
            if (ouIds.Any())
                ret = string.Join(",", ouIds);
            return ret;
        }

        private CaseRemainingTimeTable GetRemainigTimeById(RemainingTimes remainigTimeId)
        {
            var ret = new CaseRemainingTimeTable();

            switch (remainigTimeId)
            {
                case RemainingTimes.Delayed:
                    ret = new CaseRemainingTimeTable((int)remainigTimeId, null, 5, true);
                    break;

                case RemainingTimes.OneHour:
                    ret = new CaseRemainingTimeTable(1, null, 5, true);
                    break;

                case RemainingTimes.TwoHours:
                    ret = new CaseRemainingTimeTable(2, null, 5, true);
                    break;

                case RemainingTimes.FourHours:
                    ret = new CaseRemainingTimeTable(2, 4, 5, true);
                    break;

                case RemainingTimes.EightHours:
                    ret = new CaseRemainingTimeTable(4, 8, 5, true);
                    break;

                case RemainingTimes.OneDay:
                    ret = new CaseRemainingTimeTable(1, null, 5, false);
                    break;

                case RemainingTimes.TwoDays:
                    ret = new CaseRemainingTimeTable(2, null, 5, false);
                    break;

                case RemainingTimes.ThreeDays:
                    ret = new CaseRemainingTimeTable(3, null, 5, false);
                    break;

                case RemainingTimes.FourDays:
                    ret = new CaseRemainingTimeTable(4, null, 5, false);
                    break;

                case RemainingTimes.FiveDays:
                    ret = new CaseRemainingTimeTable(5, null, 5, false);
                    break;

                case RemainingTimes.MaxDays:
                    ret = new CaseRemainingTimeTable((int)remainigTimeId, null, 5, false);
                    break;
                default:
                    ret = null;
                    break;
            }

            return ret;

        }

        #endregion

        #region --Initiating--

        private CaseSearchFilterData CreateCaseSearchFilterData(int cusId, UserOverview userOverview, DHDomain.CustomerUser cu, CaseSearchModel sm)
        {
            var userId = userOverview.Id;
            var fd = new CaseSearchFilterData
            {
                customerUserSetting = cu,
                customerSetting = GetCustomerSettings(cusId),
                filterCustomerId = cusId
            };

            //Customers
            var customers = _masterDataService.GetCustomers(userId);
            List<ItemOverview> customersList = new List<ItemOverview>();
            foreach (var customer in customers)
            {
                customersList.Add(new ItemOverview(customer.Name, customer.Id.ToString(), customer.Active));
            }
            fd.filterCustomersCustom = customersList;

            //region
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseRegionFilter))
            {
                var regions = _regionService.GetRegions(cusId);
                regions.Insert(0, ObjectExtensions.notAssignedRegion());
                fd.filterRegion = regions;
            }

            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseDepartmentFilter))
            {
                var departments = _departmentService.GetDepartmentsByUserPermissions(userId, cusId, false);

                if (!departments.Any())
                {
                    departments =
                        _departmentService.GetDepartments(cusId)
                            .ToList();
                }

                if (fd.customerSetting != null && fd.customerSetting.ShowOUsOnDepartmentFilter != 0)
                    departments = AddOrganizationUnitsToDepartments(departments);

                departments.Insert(0, ObjectExtensions.notAssignedDepartment());

                fd.filterDepartment = departments;
            }

            //ärendetyp
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCaseTypeFilter))
            {
                fd.filterCaseType = _caseTypeService.GetCaseTypesOverviewWithChildren(cusId, true);
            }

            //working group
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseWorkingGroupFilter))
            {
                var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
                const bool isTakeOnlyActive = true;
                if (gs.LockCaseToWorkingGroup == 0)
                {
                    var wkgrs = _workingGroupService.GetAllWorkingGroupsForCustomer(cusId);
                    wkgrs.Insert(0, ObjectExtensions.notAssignedWorkingGroup());
                    fd.filterWorkingGroup = wkgrs;
                }
                else
                {
                    if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
                    {
                        var wkgrs = _workingGroupService.GetWorkingGroups(cusId, userId, isTakeOnlyActive, true);
                        wkgrs.Insert(0, ObjectExtensions.notAssignedWorkingGroup());
                        fd.filterWorkingGroup = wkgrs;
                    }
                    else
                    {
                        var wkgrs = _workingGroupService.GetWorkingGroups(cusId, isTakeOnlyActive);
                        wkgrs.Insert(0, ObjectExtensions.notAssignedWorkingGroup());
                        fd.filterWorkingGroup = wkgrs;
                    }

                    //var isAdmin = SessionFacade.CurrentUser.IsAdministrator();
                    //if (isAdmin)                    
                    //    fd.filterWorkingGroup = _workingGroupService.GetWorkingGroups(cusId, isTakeOnlyActive);                           
                    //else
                    //    fd.filterWorkingGroup = _workingGroupService.GetWorkingGroups(cusId, userId, isTakeOnlyActive);                                     
                }
            }

            //produktonmråde
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseProductAreaFilter))
            {
                fd.filterProductArea = _productAreaService.GetProductAreasOverviewWithChildren(cusId, isActiveOnly: true);
            }

            //kategori                        
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCategoryFilter))
            {
                //const bool isTakeOnlyActive = true;
                fd.filterCategory =
                    _categoryService.GetParentCategoriesWithChildren(cusId, true)
                        .OrderBy(c => Translation.GetMasterDataTranslation(c.Name))
                        .ToList();
            }


            //fd.filterCategory = _categoryService.GetActiveCategories(cusId);
            //prio
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CasePriorityFilter))
            {
                var priorities = _priorityService.GetPriorities(cusId);
                priorities.Insert(0, ObjectExtensions.notAssignedPriority());
                fd.filterPriority = priorities;

            }
            //status
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStatusFilter))
            {
                var status = _statusService.GetStatuses(cusId);
                status.Insert(0, ObjectExtensions.notAssignedStatus());
                fd.filterStatus = status;
            }
            //understatus
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStateSecondaryFilter))
            {
                var stateSecondaries = _stateSecondaryService.GetStateSecondaries(cusId);
                stateSecondaries.Insert(0, ObjectExtensions.notAssignedStateSecondary());
                fd.filterStateSecondary = stateSecondaries;
            }

            fd.filterCaseProgress = ObjectExtensions.GetFilterForCases(SessionFacade.CurrentUser.FollowUpPermission, cusId);
            fd.CaseRegistrationDateStartFilter = fd.customerUserSetting.CaseRegistrationDateStartFilter;
            fd.CaseRegistrationDateEndFilter = fd.customerUserSetting.CaseRegistrationDateEndFilter;
            fd.CaseWatchDateStartFilter = fd.customerUserSetting.CaseWatchDateStartFilter;
            fd.CaseWatchDateEndFilter = fd.customerUserSetting.CaseWatchDateEndFilter;
            fd.CaseClosingDateStartFilter = fd.customerUserSetting.CaseClosingDateStartFilter;
            fd.CaseClosingDateEndFilter = fd.customerUserSetting.CaseClosingDateEndFilter;
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseClosingReasonFilter))
            {
                fd.ClosingReasons = _finishingCauseService.GetFinishingCausesWithChilds(cusId);
            }

            fd.caseSearchFilter = sm.CaseSearchFilter;
            fd.CaseClosingDateEndFilter = sm.CaseSearchFilter.CaseClosingDateEndFilter;
            fd.CaseClosingDateStartFilter = sm.CaseSearchFilter.CaseClosingDateStartFilter;
            fd.CaseRegistrationDateEndFilter = sm.CaseSearchFilter.CaseRegistrationDateEndFilter;
            fd.CaseRegistrationDateStartFilter = sm.CaseSearchFilter.CaseRegistrationDateStartFilter;
            fd.CaseWatchDateEndFilter = sm.CaseSearchFilter.CaseWatchDateEndFilter;
            fd.CaseWatchDateStartFilter = sm.CaseSearchFilter.CaseWatchDateStartFilter;
            fd.CaseInitiatorFilter = sm.CaseSearchFilter.Initiator;
            fd.InitiatorSearchScope = sm.CaseSearchFilter.InitiatorSearchScope;
            fd.SearchInMyCasesOnly = sm.CaseSearchFilter.SearchInMyCasesOnly;

            //användare
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseUserFilter))
            {
                fd.RegisteredByUserList =
                    _userService.GetUserOnCases(cusId, true).MapToSelectList(fd.customerSetting);

                if (!string.IsNullOrEmpty(fd.caseSearchFilter.User))
                {
                    fd.lstfilterUser = fd.caseSearchFilter.User.Split(',').Select(int.Parse).ToArray();
                }
            }

            //ansvarig
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseResponsibleFilter))
            {
                fd.ResponsibleUserList =
                    _userService.GetAvailablePerformersOrUserId(cusId).Cast<IUserCommon>().MapToSelectList(fd.customerSetting);

                if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserResponsible))
                {
                    fd.lstfilterResponsible = fd.caseSearchFilter.UserResponsible.Split(',').Select(int.Parse).ToArray();
                }
            }

            var performers = _userService.GetAvailablePerformersOrUserId(cusId);

            //performers
            performers.Insert(0, ObjectExtensions.notAssignedPerformer());
            fd.AvailablePerformersList = performers.MapToCustomSelectList(fd.caseSearchFilter.UserPerformer, fd.customerSetting);
            if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserPerformer))
            {
                fd.lstfilterPerformer = fd.caseSearchFilter.UserPerformer.Split(',').Select(int.Parse).ToArray();
            }

            //Tid kvar 
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseRemainingTimeFilter))
                fd.filterCaseRemainingTime = GetRemainigTimeList(cusId);


            return fd;
        }

        #endregion

        #region --Othres--

        private IList<Department> AddOrganizationUnitsToDepartments(IList<Department> departments)
        {
            if (departments.Any())
            {
                var allOUsNeeded = _ouService.GetAllOUs()
                                                  .Where(o => o.Department_Id.HasValue)
                                                  .Where(o => departments.Select(d => d.Id).ToList().Contains(o.Department_Id.Value))
                                                  .ToList();
                var dep_Ou = new List<Department>();
                foreach (var dep in departments)
                {
                    var currentOUsForDep = allOUsNeeded.Where(o => o.Department_Id == dep.Id).ToList();
                    foreach (var o in currentOUsForDep)
                    {
                        /*Attention: As we make combination of Department and OU list in a one filter in case overview, 
                        we use -OU.id (Id with minus sign) to detect it is OU.Id (Not Department.Id) and 
                        then for fetching data we convert Negative to positive, Also they are storing Negative in the Session*/
                        var newDep = new Department()
                        {
                            Id = -o.Id,
                            DepartmentName = dep.DepartmentName + " - " + (o.Parent_OU_Id.HasValue ? o.Parent.Name + " - " + o.Name : o.Name),
                            IsActive = 1
                        };
                        dep_Ou.Add(newDep);
                    }
                }

                foreach (var newDep_OU in dep_Ou.ToList())
                    departments.Add(newDep_OU);

                departments = departments.OrderBy(d => d.DepartmentName).ToList();
            }

            return departments;
        }

        #endregion

        #region --Resolvers--

        private void ResolveParentPathesForFilter(CaseSearchFilter f)
        {
            // hämta parent path för casetype
            if (f.CaseType > 0)
            {
                var c = _caseTypeService.GetCaseType(f.CaseType);
                if (c != null)
                {
                    f.ParantPath_CaseType = c.getCaseTypeParentPath();
                }
            }

            if (!string.IsNullOrWhiteSpace(f.ProductArea))
            {
                if (f.ProductArea != "0")
                {
                    var p =
                        _productAreaService.GetProductArea(
                            f.ProductArea.ToInt());
                    if (p != null)
                    {
                        f.ParantPath_ProductArea = string.Join(
                            " - ",
                            _productAreaService.GetParentPath(p.Id, SessionFacade.CurrentCustomer.Id));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(f.Category))
            {
                if (f.Category != "0")
                {
                    var c =
                        _categoryService.GetCategory(
                            f.Category.ToInt(), SessionFacade.CurrentCustomer.Id);
                    if (c != null)
                    {
                        f.ParantPath_Category = string.Join(
                            " - ",
                            _categoryService.GetParentPath(c.Id, SessionFacade.CurrentCustomer.Id));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(f.CaseClosingReasonFilter))
            {
                if (f.CaseClosingReasonFilter != "0")
                {
                    var fc =
                        _finishingCauseService.GetFinishingCause(
                            f.CaseClosingReasonFilter.ToInt());
                    if (fc != null)
                    {
                        f.ParentPathClosingReason = fc.GetFinishingCauseParentPath();
                    }
                }
            }
        }

        #endregion

        #region --Get Models--

        /// <summary>
        /// The get case input view model.
        /// </summary>
        private CaseInputViewModel GetCaseInputViewModel(
            int userId,
            int customerId,
            int caseId,
            CaseLockModel caseLocked,
            IList<CaseFieldSetting> customerFieldSettings,
            string redirectFrom = "",
            string backUrl = null,
            int? templateId = null,
            int? copyFromCaseId = null,
            bool updateState = true,
            int? templateistrue = 0,
            int? parentCaseId = null,
            string activeTab = "")
        {
            var m = new CaseInputViewModel();
            SessionFacade.IsCaseDataChanged = false;
            m.BackUrl = backUrl;
            m.CanGetRelatedCases = SessionFacade.CurrentUser.IsAdministrator();
            m.CurrentUserRole = SessionFacade.CurrentUser.UserGroupId;
            m.CurrentUserName = SessionFacade.CurrentUserIdentity.UserId;
            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            var acccessToGroups = _userService.GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, customerId);
            var deps = _departmentService.GetDepartmentsByUserPermissions(userId, customerId);

            var whiteList = _globalSettingService.GetFileUploadWhiteList();
            m.HasFileUploadWhiteList = whiteList != null;
            m.FileUploadWhiteList = whiteList;

            var currentUser = UsersMapper.MapToUser(SessionFacade.CurrentUser);

            var isCreateNewCase = caseId == 0;
            m.CaseLock = caseLocked;

            m.CaseUnlockAccess =
                _userPermissionsChecker.UserHasPermission(currentUser, UserPermission.CaseUnlockPermission);

            m.MailTemplates = _mailTemplateService.GetCustomMailTemplatesList(customerId).ToList();

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            m.UserTimeZone = userTimeZone;

            var userHasInvoicePermission = _userPermissionsChecker.UserHasPermission(currentUser, UserPermission.InvoicePermission);
            var userHasInventoryViewPermission = _userPermissionsChecker.UserHasPermission(currentUser, UserPermission.InventoryViewPermission);

            // Establish current solution and set split option if available
            CaseSolution caseTemplate = null;
            if (templateId.HasValue)
            {
                caseTemplate = _caseSolutionService.GetCaseSolution(templateId.Value);
                m.CurrentCaseSolution = caseTemplate;
                m.CaseTemplateSplitToCaseSolutionID = m.CurrentCaseSolution.SplitToCaseSolution_Id;

                var caseTemplateSettings =
                    _caseSolutionSettingService.GetCaseSolutionSettingOverviews(templateId.Value);

                if (caseTemplateSettings.Any())
                {
                    m.CaseSolutionSettingModels = CaseSolutionSettingModel.CreateModel(caseTemplateSettings);
                }
            }

            if (!isCreateNewCase)
            {
                var markCaseAsRead = string.IsNullOrWhiteSpace(redirectFrom);
#pragma warning disable 0618
                m.case_ = _caseService.GetCaseById(caseId); //todo: check if case has been requested before and can be reused!
#pragma warning restore 0618

#pragma warning disable 0618
                if (m.CurrentCaseSolution == null && m.case_.CurrentCaseSolution_Id.HasValue)
                {
                    m.CurrentCaseSolution = _caseSolutionService.GetCaseSolution(m.case_.CurrentCaseSolution_Id.Value);
                    m.CaseTemplateSplitToCaseSolutionID = m.CurrentCaseSolution.SplitToCaseSolution_Id;
                }
#pragma warning restore 0618


                //TODO: update code to use CAseHistorues instead of m.case.CaseHistory
                m.CaseHistories = _caseService.GetCaseHistories(caseId);

                m.IsFollowUp = _caseFollowUpService.IsCaseFollowUp(SessionFacade.CurrentUser.Id, caseId);


#pragma warning disable 0618
                var editMode = EditMode(m, ModuleName.Cases, deps, acccessToGroups);
                if (m.case_.Unread != 0 && updateState && editMode == AccessMode.FullAccess)
                    _caseService.MarkAsRead(caseId);
#pragma warning restore 0618

#pragma warning disable 0618
                //todo: review
                customerId = customerId == 0 ? m.case_.Customer_Id : customerId;
                //SessionFacade.CurrentCaseLanguageId = m.case_.RegLanguage_Id;
#pragma warning restore 0618

#pragma warning disable 0618
                var userLocal_ChangeTime = TimeZoneInfo.ConvertTimeFromUtc(m.case_.ChangeTime, userTimeZone);
#pragma warning restore 0618
                m.ChangeTime = userLocal_ChangeTime;
                m.ExternalInvoices = GetExternalInvoices(caseId);
                var caseFolowerUsers = _caseExtraFollowersService.GetCaseExtraFollowers(caseId);
                m.MapToFollowerUsers(caseFolowerUsers);
            }


            m.CaseInternalLogAccess = _userPermissionsChecker.UserHasPermission(currentUser, UserPermission.CaseInternalLogPermission);

            var customerUserSetting = _customerUserService.GetCustomerUserSettings(customerId, userId);
            if (customerUserSetting == null)
                throw new ArgumentException(string.Format("No customer settings for this customer '{0}' and user '{1}'", customerId, userId));

#pragma warning disable 0618
            var case_ = m.case_;
#pragma warning restore 0618
            var customer = _customerService.GetCustomer(customerId);
            var customerSetting = GetCustomerSettings(customerId);
            var outputFormatter = new OutputFormatter(customerSetting.IsUserFirstLastNameRepresentation == 1, userTimeZone);
            m.OutFormatter = outputFormatter;
            m.customerUserSetting = customerUserSetting;

            //todo: first
            m.caseFieldSettings = customerFieldSettings;
            m.CaseFieldSettingWithLangauges = _caseFieldSettingService.GetAllCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);

            //todo: performance - query
            m.CaseSectionModels = _caseSectionService.GetCaseSections(customerId, SessionFacade.CurrentLanguageId);

            m.DepartmentFilterFormat = customerSetting.DepartmentFilterFormat;
            m.ParantPath_CaseType = ParentPathDefaultValue;
            m.ParantPath_ProductArea = ParentPathDefaultValue;
            m.ParantPath_Category = ParentPathDefaultValue;
            m.ParantPath_OU = ParentPathDefaultValue;
            m.MinWorkingTime = customerSetting.MinRegWorkingTime;
            m.WhiteFilesList = _globalSettingService.GetFileUploadWhiteList();
            m.MaxFileSize = 36700160;
            m.CaseFilesModel = new CaseFilesModel();
            m.CaseFileNames = GetCaseFileNames(caseId);
            m.LogFilesModel = null; //not used on case page
            m.LogFileNames = m.CaseFileNames;

            m.EnableTwoAttachments = customerFieldSettings.getCaseSettingsValue(TranslationCaseFields.tblLog_Filename_Internal.ToString())?.IsActive ?? false;
            m.ActiveTab = activeTab;

            var virtualDirPath = _masterDataService.GetVirtualDirectoryPath(customerId);
            m.CaseFilesUrlBuilder = new CaseFilesUrlBuilder(virtualDirPath, RequestExtension.GetAbsoluteUrl());

            // Computer user categories
            var computerUserCategories = _computerService.GetComputerUserCategoriesByCustomerID(customerId, true);
            m.ComputerUserCategories = computerUserCategories.Where(o => !o.IsEmpty).ToList();
            m.EmptyComputerCategoryName = computerUserCategories.FirstOrDefault(o => o.IsEmpty)?.Name;

            #region User Search Category

            var initiatorFieldSettings
                = customerFieldSettings.getCaseSettingsValue(TranslationCaseFields.UserSearchCategory_Id.ToString());

            if (initiatorFieldSettings.IsActive)
            {
                var defaultCategoryId = 0;

                //set default value only for new case
                if (Int32.TryParse(initiatorFieldSettings.DefaultValue, out defaultCategoryId))
                {
                    var category = GetComputerUserCategoryByID(defaultCategoryId);
                    if (category != null)
                        m.InitiatorComputerUserCategory = category;
                }
            }

            #endregion

            #region IsAbout - User Search Category

            var regFieldSettings =
                customerFieldSettings.getCaseSettingsValue(TranslationCaseFields.IsAbout_UserSearchCategory_Id.ToString());

            if (regFieldSettings.IsActive)
            {
                var defaultCategoryId = ComputerUserCategory.EmptyCategoryId;

                //set default value only for new case
                if (Int32.TryParse(regFieldSettings.DefaultValue, out defaultCategoryId))
                {
                    var category = GetComputerUserCategoryByID(defaultCategoryId);
                    if (category != null)
                        m.RegardingComputerUserCategory = category;
                }
            }

            #endregion

            if (isCreateNewCase)
            {
                #region New case model initialization actions

                var userName = SessionFacade.CurrentUserIdentity?.GetUserIdWithDomain();
                if (copyFromCaseId.HasValue)
                {
#pragma warning disable 0618
                    m.case_ = _caseService.Copy(
                        copyFromCaseId.Value,
                        userId,
                        SessionFacade.CurrentLanguageId,
                        Request.GetIpAddress(),
                        CaseRegistrationSource.Administrator,
                        userName);

                    m.MapToFollowerUsers(m.case_.CaseFollowers);
#pragma warning restore 0618
                }
                else if (parentCaseId.HasValue)
                {
#pragma warning disable 0618
                    ParentCaseInfo parentCaseInfo;
                    m.case_ = _caseService.InitChildCaseFromCase(
                       parentCaseId.Value,
                       userId,
                       Request.GetIpAddress(),
                       CaseRegistrationSource.Administrator,
                       userName,
                       out parentCaseInfo);
#pragma warning restore 0618

                    m.ParentCaseInfo = parentCaseInfo.MapBusinessToWebModel(outputFormatter);
                }
                else
                {
#pragma warning disable 0618
                    m.case_ = _caseService.InitCase(
                        customerId,
                        userId,
                        SessionFacade.CurrentLanguageId,
                        Request.GetIpAddress(),
                        CaseRegistrationSource.Administrator,
                        customerSetting,
                        userName);
#pragma warning restore 0618

                    //m.case_ = _caseService.InitCase(
                    //        customerId,
                    //        userId,
                    //        customer.Language_Id,
                    //        Request.GetIpAddress(),
                    //        CaseRegistrationSource.Administrator,
                    //        customerSetting,
                    //        windowsUser);
                }

                //todo: Move to caseService.Initcase() -> _customerRepository.GetCustomerDefaults ?
                var defaultStateSecondary = _stateSecondaryService.GetDefaultOverview(customerId);
                if (defaultStateSecondary != null)
                {
#pragma warning disable 0618
                    m.case_.StateSecondary_Id = int.Parse(defaultStateSecondary.Value);
#pragma warning restore 0618
                }

                // todo: Set default search category based on case section settings
                // set visibility

                #endregion
            }
            else
            {
                #region Existing case model initialization actions

                m.Logs = _logService.GetCaseLogOverviews(caseId, m.CaseInternalLogAccess, m.EnableTwoAttachments && m.CaseInternalLogAccess);

                var useVd = !string.IsNullOrEmpty(virtualDirPath);

                var canDelete = (SessionFacade.CurrentUser.DeleteAttachedFilePermission == 1);
                m.SavedFiles = canDelete ? string.Empty : m.CaseFileNames;

                var caseFiles = _caseFileService.GetCaseFiles(caseId, canDelete).OrderBy(x => x.CreatedDate);

                m.CaseFilesModel = new CaseFilesModel(caseId.ToString(), caseFiles.ToArray(), m.SavedFiles, useVd);

#pragma warning disable 0618
                if (m.case_.User_Id.HasValue)
                {
                    m.RegByUser = _userService.GetUser(m.case_.User_Id.Value);
                }
#pragma warning restore 0618

                if (m.Logs != null)
                {
                    var finishingCauses = new List<FinishingCauseInfo>();
                    if (case_.FinishingDate == null)
                    {
                        finishingCauses = _finishingCauseService.GetFinishingCauseInfos(customerId).Where(x => x.Merged == false).ToList();
                    }
                    else
                    {
                        finishingCauses = _finishingCauseService.GetAllFinishingCauseInfos(customerId).Where(x => x.Merged == false).ToList();
                    }
                    var lastLog = m.Logs.FirstOrDefault(); //todo: check if its correct - order
                    if (lastLog != null)
                    {
                        m.FinishingCause = CommonHelper.GetFinishingCauseFullPath(finishingCauses.ToArray(), lastLog.FinishingType);
                    }
                }

                var childCases = _caseService.GetChildCasesFor(caseId);
                var mergedCases = _caseService.GetMergedCasesFor(caseId);


                m.ChildCaseViewModel = new ChildCaseViewModel
                {
                    Formatter = outputFormatter,
                    ChildCaseList = childCases,
                    MergedChildList = mergedCases
                };

                m.ClosedChildCasesCount = childCases.Count(it => it.ClosingDate != null);
                m.ParentCaseInfo = _caseService.GetParentInfo(caseId).MapBusinessToWebModel(outputFormatter);
                m.MergedParentInfo = _caseService.GetMergedParentInfo(caseId).MapBusinessToWebModel(outputFormatter);
                //This means it's a child if not null (not merged)
                if (m.ParentCaseInfo != null)
                {
                    m.IndependentChild = m.ParentCaseInfo.IsChildIndependent;
                }

                #endregion
            }

            var isRelatedCase = caseId > 0 && _caseService.IsRelated(caseId);
            m.IsRelatedCase = isRelatedCase;

            var customerCaseSolutions =
                _caseSolutionService.GetCustomerCaseSolutionsOverview(customerId, userId);
            var langId = SessionFacade.CurrentLanguageId;
            //TODO: reuse case solutions!
            m.CaseTemplateButtons =
                customerCaseSolutions.Where(c => c.ShowInsideCase != 0 && c.ConnectedButton.HasValue && c.ConnectedButton > 0) // ConnectedButton = 0 reserved for workflow steps 
                    .Select(c => new CaseTemplateButton()
                    {
                        CaseTemplateId = c.CaseSolutionId,
                        CaseTemplateName = c.Name,
                        ButtonNumber = c.ConnectedButton.Value
                    })
                    .OrderBy(c => c.ButtonNumber)
                    .ToList();
            foreach (var button in m.CaseTemplateButtons)
            {
                if (_caseSolutionService.GetCaseSolutionTranslation(button.CaseTemplateId, langId) != null)
                {
                    button.CaseTemplateName = _caseSolutionService.GetCaseSolutionTranslation(button.CaseTemplateId, langId).CaseSolutionName;
                }
            }
            var workflowCaseSolutionIds =
                customerCaseSolutions.Where(x => x.ConnectedButton == 0 && x.Status > 0)
                    .Select(x => x.CaseSolutionId)
                    .ToList();

#pragma warning disable 0618
            if (m.case_.FinishingDate == null)
            {
                m.WorkflowSteps = _caseSolutionService.GetWorkflowSteps(customerId,
                  m.case_,
                  workflowCaseSolutionIds,
                  isRelatedCase,
                  SessionFacade.CurrentUser,
                  ApplicationType.Helpdesk,
                  templateId,
                  langId);
            }
#pragma warning restore 0618


            m.CaseMailSetting = new CaseMailSetting(
                customer.NewCaseEmailList,
                customer.HelpdeskEmail,
                RequestExtension.GetAbsoluteUrl(),
                customerSetting.DontConnectUserToWorkingGroup);

            m.CaseMailSetting.DontSendMailToNotifier = !customer.CommunicateWithNotifier.ToBool();

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.CaseType_Id))
            {
                const bool takeOnlyActive = true;
                m.caseTypes = _caseTypeService.GetCaseTypesOverviewWithChildren(customerId, takeOnlyActive).OrderBy(c => Translation.GetMasterDataTranslation(c.Name)).ToList();
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Category_Id))
            {
                m.categories = _categoryService.GetParentCategoriesWithChildren(customerId, true);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Impact_Id))
            {
                m.impacts = _impactService.GetImpacts(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Priority_Id))
            {
                m.priorities = _priorityService.GetPriorities(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.ProductArea_Id))
            {
#pragma warning disable 0618
                m.productAreas =
                    _productAreaService.GetTopProductAreasForUserOnCase(customerId, m.case_.ProductArea_Id, SessionFacade.CurrentUser)
                        .OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList();
#pragma warning restore 0618
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Region_Id))
            {
                m.regions = _regionService.GetRegions(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Status_Id))
            {
                m.statuses = _statusService.GetStatuses(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.StateSecondary_Id))
            {
                m.stateSecondaries = _stateSecondaryService.GetStateSecondaries(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Supplier_Id))
            {
                m.suppliers = _supplierService.GetSuppliers(customerId);
                m.countries = _countryService.GetCountries(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.System_Id))
            {
#pragma warning disable 0618
                m.systems = _systemService.GetSystems(customerId, true, m.case_.System_Id);
#pragma warning restore 0618
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Urgency_Id))
            {
                m.urgencies = _urgencyService.GetUrgencies(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.CausingPart))
            {
#pragma warning disable 0618
                var causingParts = GetCausingPartsModel(customerId, m.case_.CausingPartId);
                m.causingParts = causingParts;
#pragma warning restore 0618
                //#1
                //m.causingParts = _causingPartService.GetCausingParts(customerId);
            }

            // "Workging group" field
            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.WorkingGroup_Id))
            {
                var IsTakeOnlyActive = isCreateNewCase;
                m.workingGroups = _workingGroupService.GetAllWorkingGroupsForCustomer(customerId, IsTakeOnlyActive);
            }

            if (isCreateNewCase && m.workingGroups != null && m.workingGroups.Count > 0)
            {
                var defWorkingGroup = m.workingGroups.Where(it => it.IsDefault == 1).FirstOrDefault();
                if (defWorkingGroup != null)
                {
#pragma warning disable 0618
                    m.case_.WorkingGroup_Id = defWorkingGroup.Id;
#pragma warning restore 0618
                }
            }

            // Set working group and performerId from the case type working group if any for New case only
#pragma warning disable 0618
            if (isCreateNewCase && m.case_.CaseType_Id > 0)
            {
                var caseType = _caseTypeService.GetCaseType(m.case_.CaseType_Id);
                if (caseType != null)
                {
                    if (caseType.WorkingGroup_Id.HasValue)
                        m.case_.WorkingGroup_Id = caseType.WorkingGroup_Id;

                    if (caseType.User_Id.HasValue)
                        m.case_.Performer_User_Id = caseType.User_Id.Value;
                }
            }
#pragma warning restore 0618

#pragma warning disable 0618
            //product area should override working group set by case type before
            if (isCreateNewCase && m.case_.ProductArea_Id.HasValue)
            {
                var productArea = _productAreaService.GetProductArea(m.case_.ProductArea_Id.Value);
                if (productArea != null)
                {
                    if (productArea.WorkingGroup_Id.HasValue)
                    {
                        m.case_.WorkingGroup_Id = productArea.WorkingGroup_Id.Value;
                    }

                    if (productArea.Priority_Id.HasValue)
                    {
                        m.case_.Priority_Id = productArea.Priority_Id.Value;
                    }
                }
            }
#pragma warning restore 0618

            // "RegistrationSourceCustomer" field
            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.RegistrationSourceCustomer))
            {
                var customerSources =
                    _registrationSourceCustomerService.GetCustomersActiveRegistrationSources(customerId).ToArray();

#pragma warning disable 0618
                if (m.case_.RegistrationSourceCustomer_Id.HasValue)
                {
                    m.CustomerRegistrationSourceId = m.case_.RegistrationSourceCustomer_Id.Value;
                    m.SelectedCustomerRegistrationSource = m.case_.RegistrationSourceCustomer.SourceName;
                }
                else
                {
                    var defaultSource =
                        customerSources.Where(it => it.SystemCode == (int)CaseRegistrationSource.Administrator).FirstOrDefault();

                    if (isCreateNewCase && defaultSource != null)
                    {
                        m.CustomerRegistrationSourceId = defaultSource.Id;
                        m.SelectedCustomerRegistrationSource = defaultSource.SourceName;
                    }
                }
#pragma warning restore 0618

                m.CustomerRegistrationSources.AddRange(
                    customerSources.Select(
                        it => new SelectListItem()
                        {
                            Text = Translation.GetCoreTextTranslation(it.SourceName),
                            Value = it.Id.ToString(),
                            Selected = it.Id == m.CustomerRegistrationSourceId
                        }));
            }

            if (customerSetting.ModuleProject == 1)
            {
                m.projects = _projectService.GetCustomerProjects(customerId);
            }

            if (customerSetting.ModuleChangeManagement == 1)
            {
                m.changes = _changeService.GetChanges(customerId);
            }

            m.finishingCauses = _finishingCauseService.GetFinishingCausesWithChilds(customerId).Where(x => x.Merged == false).ToList();
            m.problems = _problemService.GetCustomerProblems(customerId, false);
            m.currencies = _currencyService.GetCurrencies();

            m.projects = _projectService.GetCustomerProjects(customerId);
            m.departments = deps.Any() ? deps :
                GetCustomerDepartments(customerId)
                .Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
                .ToList();

            m.standardTexts = _standardTextService.GetStandardTexts(customerId);
            m.Languages = _languageService.GetActiveLanguages();

#pragma warning disable 0618
            var responsibleUsersList = _userService.GetAvailablePerformersOrUserId(customerId, m.case_.CaseResponsibleUser_Id);
#pragma warning restore 0618

            var performersListForSearch = _userService.GetAvailablePerformersOrUserId(customerId, null, true);
            m.PerformersToSearch = createPerformerforSearchList(customerId, performersListForSearch);

            m.FollowersModel =
                m.SendToDialogModel =
                    _sendToDialogModelFactory.CreateNewSendToDialogModel(
                        customerId, responsibleUsersList.ToList(), customerSetting, _emailGroupService, _workingGroupService, _emailService);

            m.CaseLog = _logService.InitCaseLog(SessionFacade.CurrentUser.Id, string.Empty);
#pragma warning disable 0618
            m.CaseKey = m.case_.Id == 0 ? m.case_.CaseGUID.ToString() : m.case_.Id.ToString(global::System.Globalization.CultureInfo.InvariantCulture);
#pragma warning restore 0618
            m.LogKey = m.CaseLog.LogGuid.ToString();
            m.CurrentCaseLanguageId = SessionFacade.CurrentCaseLanguageId;

#pragma warning disable 0618
            if (m.case_.Supplier_Id > 0 && m.suppliers != null)
            {
                var sup = m.suppliers.FirstOrDefault(x => x.Id == m.case_.Supplier_Id.GetValueOrDefault());
                m.CountryId = sup?.Country_Id.GetValueOrDefault();
            }
#pragma warning restore 0618

            if (caseTemplate != null)
            {
                #region New case initialize

                if (isCreateNewCase)
                {
                    if (caseTemplate.CaseType_Id.HasValue)
                    {
#pragma warning disable 0618
                        m.case_.CaseType_Id = caseTemplate.CaseType_Id.Value;
#pragma warning restore 0618
                    }

                    if (caseTemplate.SetCurrentUserAsPerformer.ToBool())
                    {
#pragma warning disable 0618
                        m.case_.Performer_User_Id = SessionFacade.CurrentUser.Id;
#pragma warning restore 0618
                    }
                    else
                    {
#pragma warning disable 0618
                        m.case_.Performer_User_Id = caseTemplate.PerformerUser_Id;
#pragma warning restore 0618
                    }

                    if (caseTemplate.Category_Id != null)
                    {
#pragma warning disable 0618
                        m.case_.Category_Id = caseTemplate.Category_Id.Value;
#pragma warning restore 0618
                    }

                    if (caseTemplate.CausingPartId.HasValue)
                    {
#pragma warning disable 0618
                        m.case_.CausingPartId = caseTemplate.CausingPartId.Value;
#pragma warning restore 0618
                        if (m.causingParts != null)
                        {
                            var templateCausingPart = m.causingParts.Where(c => c.Value == caseTemplate.CausingPartId.Value.ToString()).SingleOrDefault();
                            if (templateCausingPart != null)
                                templateCausingPart.Selected = true;
                        }
                    }

                    if (caseTemplate.UpdateNotifierInformation.HasValue)
                    {
                        m.UpdateNotifierInformation = caseTemplate.UpdateNotifierInformation.Value.ToBool();
                    }

                    if (caseTemplate.AddFollowersBtn.HasValue)
                    {
                        m.AddFollowersBtn = caseTemplate.AddFollowersBtn.Value;
                    }

                    if (caseTemplate.Supplier_Id != null)
#pragma warning disable 0618
                        m.case_.Supplier_Id = caseTemplate.Supplier_Id.Value;
#pragma warning restore 0618

                    var isCopy = parentCaseId.HasValue;

                    if (!string.IsNullOrEmpty(caseTemplate.ReportedBy))
#pragma warning disable 0618
                        m.case_.ReportedBy = caseTemplate.ReportedBy;
#pragma warning restore 0618

                    if (caseTemplate.Department_Id != null)
#pragma warning disable 0618
                        m.case_.Department_Id = caseTemplate.Department_Id;
#pragma warning restore 0618

                    m.CaseMailSetting.DontSendMailToNotifier = caseTemplate.NoMailToNotifier.ToBool();

                    if (caseTemplate.ProductArea_Id != null)
#pragma warning disable 0618
                        m.case_.ProductArea_Id = caseTemplate.ProductArea_Id;
#pragma warning restore 0618

                    if (caseTemplate.ProductArea_Id.HasValue)
#pragma warning disable 0618
                        m.case_.ProductArea = _productAreaService.GetProductArea(caseTemplate.ProductArea_Id.Value);
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.Caption))
#pragma warning disable 0618
                        m.case_.Caption = caseTemplate.Caption;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.Description))
#pragma warning disable 0618
                        m.case_.Description = caseTemplate.Description;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.Miscellaneous))
#pragma warning disable 0618
                        m.case_.Miscellaneous = caseTemplate.Miscellaneous;
#pragma warning restore 0618

                    // Set WORKING GROUP from Case Template:
                    if (caseTemplate.SetCurrentUsersWorkingGroup == 1)
                    {
                        var userDefaultWGId = _userService.GetUserDefaultWorkingGroupId(SessionFacade.CurrentUser.Id, customer.Id);
#pragma warning disable 0618
                        m.case_.WorkingGroup_Id = userDefaultWGId;
#pragma warning restore 0618
                    }
                    else if (caseTemplate.CaseWorkingGroup_Id.HasValue)
                    {
#pragma warning disable 0618
                        m.case_.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id.Value;
#pragma warning restore 0618
                    }

                    if (caseTemplate.Priority_Id != null)
#pragma warning disable 0618
                        m.case_.Priority_Id = caseTemplate.Priority_Id;
#pragma warning restore 0618

                    /*Disabled maybe we need this in the future*/
                    // 
                    //if (caseTemplate.Priority_Id.HasValue)
                    //    m.case_.Priority_Id = caseTemplate.Priority_Id;
                    //else
                    //{
                    //    if (m.case_.ProductArea_Id.HasValue && m.case_.ProductArea != null)
                    //        m.case_.Priority_Id = m.case_.ProductArea.Priority_Id;
                    //}

                    if (caseTemplate.Project_Id.HasValue)
#pragma warning disable 0618
                        m.case_.Project_Id = caseTemplate.Project_Id;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.Text_External))
                    {
                        m.CaseLog.TextExternal = caseTemplate.Text_External;
                        m.CaseLog.SendMailAboutCaseToNotifier = true;
                    }

                    if (!string.IsNullOrEmpty(caseTemplate.Text_Internal))
                        m.CaseLog.TextInternal = caseTemplate.Text_Internal;

                    if (caseTemplate.FinishingCause_Id.HasValue)
                        m.CaseLog.FinishingType = caseTemplate.FinishingCause_Id;

                    if (m.CaseLog.FinishingType.HasValue)
                        m.CaseLog.FinishingDate = DateTime.UtcNow;

                    if (!string.IsNullOrEmpty(caseTemplate.PersonsName))
#pragma warning disable 0618
                        m.case_.PersonsName = caseTemplate.PersonsName;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.PersonsEmail))
#pragma warning disable 0618
                        m.case_.PersonsEmail = caseTemplate.PersonsEmail;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.PersonsPhone))
#pragma warning disable 0618
                        m.case_.PersonsPhone = caseTemplate.PersonsPhone;
#pragma warning restore 0618

                    if (caseTemplate.Region_Id.HasValue)
#pragma warning disable 0618
                        m.case_.Region_Id = caseTemplate.Region_Id;
#pragma warning restore 0618

                    if (caseTemplate.OU_Id.HasValue)
#pragma warning disable 0618
                        m.case_.OU_Id = caseTemplate.OU_Id;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.Place))
#pragma warning disable 0618
                        m.case_.Place = caseTemplate.Place;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.UserCode))
#pragma warning disable 0618
                        m.case_.UserCode = caseTemplate.UserCode;
#pragma warning restore 0618

                    if (caseTemplate.Urgency_Id.HasValue)
#pragma warning disable 0618
                        m.case_.Urgency_Id = caseTemplate.Urgency_Id;
#pragma warning restore 0618

                    if (caseTemplate.Impact_Id.HasValue)
#pragma warning disable 0618
                        m.case_.Impact_Id = caseTemplate.Impact_Id;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryNumber))
#pragma warning disable 0618
                        m.case_.InvoiceNumber = caseTemplate.InvoiceNumber;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.ReferenceNumber))
#pragma warning disable 0618
                        m.case_.ReferenceNumber = caseTemplate.ReferenceNumber;
#pragma warning restore 0618

                    if (caseTemplate.Status_Id.HasValue)
#pragma warning disable 0618
                        m.case_.Status_Id = caseTemplate.Status_Id;
#pragma warning restore 0618

                    if (caseTemplate.StateSecondary_Id.HasValue)
#pragma warning disable 0618
                        m.case_.StateSecondary_Id = caseTemplate.StateSecondary_Id;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.VerifiedDescription))
#pragma warning disable 0618
                        m.case_.VerifiedDescription = caseTemplate.VerifiedDescription;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.SolutionRate))
#pragma warning disable 0618
                        m.case_.SolutionRate = caseTemplate.SolutionRate;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryNumber))
#pragma warning disable 0618
                        m.case_.InventoryNumber = caseTemplate.InventoryNumber;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryType))
#pragma warning disable 0618
                        m.case_.InventoryType = caseTemplate.InventoryType;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryLocation))
#pragma warning disable 0618
                        m.case_.InventoryLocation = caseTemplate.InventoryLocation;
#pragma warning restore 0618

                    if (caseTemplate.System_Id.HasValue)
#pragma warning disable 0618
                        m.case_.System_Id = caseTemplate.System_Id;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.Currency))
#pragma warning disable 0618
                        m.case_.Currency = caseTemplate.Currency;
#pragma warning restore 0618

                    if (caseTemplate.Cost != 0)
#pragma warning disable 0618
                        m.case_.Cost = caseTemplate.Cost;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryNumber))
#pragma warning disable 0618
                        m.case_.OtherCost = caseTemplate.OtherCost;
#pragma warning restore 0618

                    if (caseTemplate.AgreedDate.HasValue)
#pragma warning disable 0618
                        m.case_.AgreedDate = caseTemplate.AgreedDate;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.Available))
#pragma warning disable 0618
                        m.case_.Available = caseTemplate.Available;
#pragma warning restore 0618

                    if (caseTemplate.ContactBeforeAction != 0)
#pragma warning disable 0618
                        m.case_.ContactBeforeAction = caseTemplate.ContactBeforeAction;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.CostCentre))
#pragma warning disable 0618
                        m.case_.CostCentre = caseTemplate.CostCentre;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.PersonsCellPhone))
#pragma warning disable 0618
                        m.case_.PersonsCellphone = caseTemplate.PersonsCellPhone;
#pragma warning restore 0618

#pragma warning disable 0618
                    if (m.case_.IsAbout == null)
                        m.case_.IsAbout = new CaseIsAboutEntity();

                    m.case_.IsAbout.Id = 0;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_ReportedBy))
#pragma warning disable 0618
                        m.case_.IsAbout.ReportedBy = caseTemplate.IsAbout_ReportedBy;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_PersonsName))
#pragma warning disable 0618
                        m.case_.IsAbout.Person_Name = caseTemplate.IsAbout_PersonsName;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_PersonsEmail))
#pragma warning disable 0618
                        m.case_.IsAbout.Person_Email = caseTemplate.IsAbout_PersonsEmail;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_PersonsPhone))
#pragma warning disable 0618
                        m.case_.IsAbout.Person_Phone = caseTemplate.IsAbout_PersonsPhone;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_PersonsCellPhone))
#pragma warning disable 0618
                        m.case_.IsAbout.Person_Cellphone = caseTemplate.IsAbout_PersonsCellPhone;
#pragma warning restore 0618

                    if (caseTemplate.IsAbout_Region_Id.HasValue)
#pragma warning disable 0618
                        m.case_.IsAbout.Region_Id = caseTemplate.IsAbout_Region_Id;
#pragma warning restore 0618

                    if (caseTemplate.IsAbout_Department_Id.HasValue)
#pragma warning disable 0618
                        m.case_.IsAbout.Department_Id = caseTemplate.IsAbout_Department_Id;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.Available))
#pragma warning disable 0618
                        m.case_.IsAbout.OU_Id = caseTemplate.IsAbout_OU_Id;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_CostCentre))
#pragma warning disable 0618
                        m.case_.IsAbout.CostCentre = caseTemplate.IsAbout_CostCentre;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_Place))
#pragma warning disable 0618
                        m.case_.IsAbout.Place = caseTemplate.IsAbout_Place;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.UserCode))
#pragma warning disable 0618
                        m.case_.IsAbout.UserCode = caseTemplate.UserCode;
#pragma warning restore 0618

                    if (caseTemplate.RegistrationSource.HasValue)
                    {
                        m.CustomerRegistrationSourceId = caseTemplate.RegistrationSource.Value;
                        var RegistrationSource = _registrationSourceCustomerService.GetRegistrationSouceCustomer(caseTemplate.RegistrationSource.Value);
                        m.SelectedCustomerRegistrationSource = RegistrationSource.SourceName;
                    }

                    // "watch date" 
                    if (caseTemplate.WatchDate.HasValue)
                    {
#pragma warning disable 0618
                        m.case_.WatchDate = caseTemplate.WatchDate;
#pragma warning restore 0618
                    }
                    else
                    {
#pragma warning disable 0618
                        if (m.case_.Department_Id.HasValue && m.case_.Priority_Id.HasValue)
                        {
                            var dept = _departmentService.GetDepartment(m.case_.Department_Id.Value);
                            var priority =
                                m.priorities.Where(it => it.Id == m.case_.Priority_Id && it.IsActive == 1).FirstOrDefault();
                            if (dept != null && dept.WatchDateCalendar_Id.HasValue && priority != null && priority.SolutionTime == 0)
                            {
                                m.case_.WatchDate =
                                    _watchDateCalendarService.GetClosestDateTo(dept.WatchDateCalendar_Id.Value, DateTime.UtcNow);
                            }
                        }
#pragma warning restore 0618
                    }

                    if (caseTemplate.Project_Id.HasValue)
#pragma warning disable 0618
                        m.case_.Project_Id = caseTemplate.Project_Id;
#pragma warning restore 0618

                    if (caseTemplate.Problem_Id.HasValue)
#pragma warning disable 0618
                        m.case_.Problem_Id = caseTemplate.Problem_Id;
#pragma warning restore 0618

                    if (caseTemplate.Change_Id.HasValue)
#pragma warning disable 0618
                        m.case_.Change_Id = caseTemplate.Change_Id;
#pragma warning restore 0618

                    if (caseTemplate.FinishingDate.HasValue)
#pragma warning disable 0618
                        m.case_.FinishingDate = caseTemplate.FinishingDate;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.FinishingDescription))
#pragma warning disable 0618
                        m.case_.FinishingDescription = caseTemplate.FinishingDescription;
#pragma warning restore 0618

                    if (caseTemplate.PlanDate.HasValue)
#pragma warning disable 0618
                        m.case_.PlanDate = caseTemplate.PlanDate;
#pragma warning restore 0618

                    if (!string.IsNullOrEmpty(caseTemplate.Name))
                        m.CaseTemplateName = caseTemplate.Name;

                    if (caseTemplate.SMS != 0)
#pragma warning disable 0618
                        m.case_.SMS = caseTemplate.SMS;
#pragma warning restore 0618

                    if (caseTemplate.Verified != 0)
#pragma warning disable 0618
                        m.case_.Verified = caseTemplate.Verified;
#pragma warning restore 0618

                    // This is used for hide fields(which are not in casetemplate) in new case input
                    m.templateistrue = templateistrue;

                    var finishingCauses = _finishingCauseService.GetFinishingCauseInfos(customerId);
                    m.FinishingCause = CommonHelper.GetFinishingCauseFullPath(finishingCauses.ToArray(), caseTemplate.FinishingCause_Id);

                    //save case original working group 
#pragma warning disable 0618
                    int? caseWorkingGroupId = m.case_.WorkingGroup_Id;
#pragma warning restore 0618

                    //set working group and performer from CaseType if they have not been set before
                    if (caseTemplate.CaseType_Id.HasValue)
                    {
                        var caseType = _caseTypeService.GetCaseType(caseTemplate.CaseType_Id.Value);
                        if (caseType != null)
                        {
                            if (caseWorkingGroupId == null && caseType.WorkingGroup_Id.HasValue)
#pragma warning disable 0618
                                m.case_.WorkingGroup_Id = caseType.WorkingGroup_Id;
#pragma warning restore 0618

#pragma warning disable 0618
                            if (m.case_.Performer_User_Id == null && caseType.User_Id.HasValue)
                                m.case_.Performer_User_Id = caseType.User_Id.Value;
#pragma warning restore 0618
                        }
                    }

                    //set working group and priority from CaseType if they have not been set before
                    if (caseTemplate.ProductArea_Id.HasValue)
                    {
                        var productArea = _productAreaService.GetProductArea(caseTemplate.ProductArea_Id.Value);
                        if (productArea != null)
                        {
                            if (caseWorkingGroupId == null && productArea.WorkingGroup_Id.HasValue)
#pragma warning disable 0618
                                m.case_.WorkingGroup_Id = productArea.WorkingGroup_Id;
#pragma warning restore 0618

#pragma warning disable 0618
                            if (m.case_.Priority_Id == null && productArea.Priority_Id.HasValue)
                                m.case_.Priority_Id = productArea.Priority_Id.Value;
#pragma warning restore 0618
                        }
                    }
                }

                #endregion

                //todo: check if value from template should be set on NewCase only but value from db used for existing case! 
                #region User Search Categories

                //set default values only if active = true
                if (initiatorFieldSettings.IsActive && caseTemplate.UserSearchCategory_Id.HasValue)
                {
                    m.InitiatorComputerUserCategory =
                        GetComputerUserCategoryByID(caseTemplate.UserSearchCategory_Id.Value);
                }

                //set default values only if active = true
                if (regFieldSettings.IsActive && caseTemplate.IsAbout_UserSearchCategory_Id.HasValue)
                {
                    m.RegardingComputerUserCategory =
                        GetComputerUserCategoryByID(caseTemplate.IsAbout_UserSearchCategory_Id.Value);
                }

                #endregion
            }

#pragma warning disable 0618
            if (m.case_.ReportedBy != null)
            {
                var reportedByUser = _computerService.GetComputerUserByUserID(m.case_.ReportedBy);
                if (reportedByUser != null && reportedByUser.ComputerUsersCategoryID.HasValue)
                {
                    m.InitiatorComputerUserCategory = _computerService.GetComputerUserCategoryByID(reportedByUser.ComputerUsersCategoryID.Value);
                    if (m.InitiatorComputerUserCategory != null)
                    {
                        m.InitiatorReadOnly = m.InitiatorComputerUserCategory.IsReadOnly;
                    }
                }
            }
#pragma warning restore 0618

#pragma warning disable 0618
            if (m.case_.IsAbout != null && m.case_.IsAbout.ReportedBy != null)
            {
                var reportedByUser = _computerService.GetComputerUserByUserID(m.case_.IsAbout.ReportedBy);
                if (reportedByUser != null && reportedByUser.ComputerUsersCategoryID.HasValue)
                {
                    m.RegardingComputerUserCategory = _computerService.GetComputerUserCategoryByID(reportedByUser.ComputerUsersCategoryID.Value);
                    if (m.RegardingComputerUserCategory != null)
                    {
                        m.RegardingReadOnly = m.RegardingComputerUserCategory.IsReadOnly;
                    }
                }
            }
#pragma warning restore 0618

            BusinessData.Models.User.CustomerUserInfo admUser = null;
#pragma warning disable 0618
            if (m.case_.Performer_User_Id.HasValue)
            {
                admUser = _userService.GetUserInfo(m.case_.Performer_User_Id.Value);
            }
#pragma warning restore 0618

            var performersList = responsibleUsersList;
#pragma warning disable 0618
            if (customerSetting.DontConnectUserToWorkingGroup == 0 && m.case_.WorkingGroup_Id > 0)
            {
                performersList = _userService.GetAvailablePerformersForWorkingGroup(customerId, m.case_.WorkingGroup_Id);
            }
#pragma warning restore 0618

            if (admUser != null && !performersList.Any(u => u.Id == admUser.Id))
            {
                performersList.Insert(0, admUser);
            }

            var temporaryUserHasAccessToWG = redirectFrom.ToLower() == "save";
            m.EditMode = EditMode(m, ModuleName.Cases, deps, acccessToGroups, temporaryUserHasAccessToWG);

            if (m.EditMode == AccessMode.FullAccess)
                _logFileService.ClearExistingAttachedFiles(caseId);

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.OU_Id))
            {
                //m.ous = _ouService.GetOUs(customerId);
#pragma warning disable 0618
                m.ous = _organizationService.GetOUs(m.case_.Department_Id).ToList();
#pragma warning restore 0618
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.IsAbout_OU_Id))
            {
#pragma warning disable 0618
                if (m.case_.IsAbout != null)
                    m.isaboutous = _organizationService.GetOUs(m.case_.IsAbout.Department_Id).ToList();
                else
                    m.isaboutous = null;
#pragma warning restore 0618
            }

            // hämta parent path för casetype
#pragma warning disable 0618
            if (m.case_.CaseType_Id > 0)
            {
                var caseType = _caseTypeService.GetCaseType(m.case_.CaseType_Id);
                if (caseType != null)
                {
                    caseType = Translation.TranslateCaseType(caseType);
                    m.ParantPath_CaseType = caseType.getCaseTypeParentPath();
                }
            }
#pragma warning restore 0618

            // hämta parent path för productArea 
            m.ProductAreaHasChild = 0;
#pragma warning disable 0618
            if (m.case_.ProductArea_Id.HasValue)
            {
                var p = _productAreaService.GetProductArea(m.case_.ProductArea_Id.GetValueOrDefault());
                if (p != null)
                {
                    var names =
                        _productAreaService.GetParentPath(p.Id, customerId).Select(name => Translation.GetMasterDataTranslation(name));
                    m.ParantPath_ProductArea = string.Join(" - ", names);
                    if (p.SubProductAreas != null && p.SubProductAreas.Where(s => s.IsActive != 0).ToList().Count > 0)
                    {
                        m.ProductAreaHasChild = 1;
                    }
                }
            }
#pragma warning restore 0618

            //Check if FinishingCases has childs
            m.FinishingCauseHasChild = 0;

            // hämta parent path för Category
            m.CategoryHasChild = 0;
#pragma warning disable 0618
            if (m.case_.Category_Id.HasValue)
            {
                var c = _categoryService.GetCategory(m.case_.Category_Id.GetValueOrDefault(), customerId);
                if (c != null)
                {
                    if (m.CaseTemplateButtons != null)
                    {
                        var names =
                            _categoryService.GetParentPath(c.Id, customerId).Select(name => Translation.GetMasterDataTranslation(name));
                        m.ParantPath_Category = string.Join(" - ", names);
                        if (c.SubCategories != null && c.SubCategories.Where(s => s.IsActive != 0).ToList().Count > 0)
                        {
                            m.CategoryHasChild = 1;
                        }
                    }
                }
            }
#pragma warning restore 0618

            // check department info
            m.ShowInvoiceFields = 0;
#pragma warning disable 0618
            if (m.case_.Department_Id > 0 && m.case_.Department_Id.HasValue)
            {
                var d = _departmentService.GetDepartment(m.case_.Department_Id.Value);
                if (d != null)
                {
                    m.ShowInvoiceFields = d.Charge;
                    m.ShowExternalInvoiceFields = d.ShowInvoice;
                    m.TimeRequired = d.ChargeMandatory.ToBool();
                }
            }
#pragma warning restore 0618

            // check state secondary info
            if (string.IsNullOrEmpty(m.CaseLog.TextExternal))
            {
                m.CaseLog.SendMailAboutCaseToNotifier = false;
            }

            m.Disable_SendMailAboutCaseToNotifier = false;
#pragma warning disable 0618
            if (m.case_.StateSecondary_Id > 0)
            {
                if (m.case_.StateSecondary != null)
                {
                    m.Disable_SendMailAboutCaseToNotifier = m.case_.StateSecondary.NoMailToNotifier == 1;
                    m.CaseLog.SendMailAboutCaseToNotifier = false;
                    //if (m.case_.StateSecondary.NoMailToNotifier == 1)
                    //    m.CaseLog.SendMailAboutCaseToNotifier = false;
                    //else
                    //    m.CaseLog.SendMailAboutCaseToNotifier = true;
                }

                if (m.CaseLog != null)
                {
                    if (m.CaseLog.TextExternal != null)
                        m.CaseLog.SendMailAboutCaseToNotifier = true;
                }
            }
#pragma warning restore 0618

            if (isCreateNewCase)
            {
#pragma warning disable 0618
                m.case_.DefaultOwnerWG_Id = null;
                if (m.case_.User_Id.HasValue && m.case_.User_Id != 0)
                {
                    // http://redmine.fastdev.se/issues/10997
                    /*var curUser = _userService.GetUser(m.case_.User_Id);                        

                    if (curUser.Default_WorkingGroup_Id != null)
                        m.case_.DefaultOwnerWG_Id = curUser.Default_WorkingGroup_Id;*/

                    var userDefaultWorkingGroupId = _userService.GetUserDefaultWorkingGroupId(m.case_.User_Id.Value, m.case_.Customer_Id);
                    if (userDefaultWorkingGroupId.HasValue)
                    {
                        m.case_.DefaultOwnerWG_Id = userDefaultWorkingGroupId;
                    }
                }
#pragma warning restore 0618
            }
            else
            {
#pragma warning disable 0618
                if (m.case_.DefaultOwnerWG_Id.HasValue && m.case_.DefaultOwnerWG_Id.Value > 0)
                {
                    m.CaseOwnerDefaultWorkingGroup = _workingGroupService.GetWorkingGroup(m.case_.DefaultOwnerWG_Id.Value);
                }
#pragma warning restore 0618
            }

            // TODO: Should mix CustomerSettings & Setting                 
            m.CustomerSettings = _workContext.Customer.Settings;
            m.Setting = customerSetting;

            // "Responsible"
#pragma warning disable 0618
            m.ResponsibleUser_Id = m.case_.CaseResponsibleUser_Id ?? 0;
#pragma warning restore 0618
            const bool isAddEmpty = true;
            m.ResponsibleUsersAvailable = responsibleUsersList.MapToSelectList(m.Setting, isAddEmpty);

            // "Administrator" (Performer)
#pragma warning disable 0618
            m.Performer_Id = m.case_.Performer_User_Id ?? 0;
#pragma warning restore 0618

            m.Performers =
                performersList.Where(it => it.IsActive == 1 && (it.Performer == 1 || it.Id == m.Performer_Id)).MapToSelectList(m.Setting, isAddEmpty);

#pragma warning disable 0618
            m.DynamicCase = _caseService.GetDynamicCase(m.case_.Id);
#pragma warning restore 0618
            if (m.DynamicCase != null)
            {
                var l = m.Languages.Where(x => x.Id == SessionFacade.CurrentLanguageId).FirstOrDefault();

#pragma warning disable 0618
                //ex: unitedkingdom/Hiring/edit/[CaseId]/?UserId=[UserId]&language=[Language]
                m.DynamicCase.FormPath = m.DynamicCase.FormPath
                    .Replace("[CaseId]", m.case_.Id.ToString())
                    .Replace("[UserId]", HttpUtility.UrlEncode(SessionFacade.CurrentUser.UserId.ToString()))
                    .Replace("[ApplicationType]", "HD5")
                    .Replace("[Language]", l.LanguageId);
#pragma warning restore 0618
            }

#pragma warning disable 0618
            var caseSolutionId = (m.case_.CaseSolution_Id != null)
                ? m.case_.CaseSolution_Id.Value
                : templateId ?? 0;
#pragma warning restore 0618

            m.HasExtendedComputerUsers =
                _caseSolutionService.CheckIfExtendedFormExistForSolutionsInCategories(customerId, m.ComputerUserCategories.Select(c => c.Id).ToList());

            #region Extended Case

            try
            {
                string extendedCasePathMask = _globalSettingService.GetGlobalSettings().FirstOrDefault().ExtendedCasePath;

                if (!string.IsNullOrEmpty(extendedCasePathMask))
                {
                    int userRole = 0;

                    //#58691
                    //If user is Admin - send in another Number 
                    //3 = Kundadministratör
                    //4 = Administratör
                    if (SessionFacade.CurrentUser.UserGroupId >= 3)
                    {
                        userRole = 99;
                    }
                    else
                    {
                        //Take the highest workinggroupId with "Admin" access (UserRole)
                        int userWorkingGroupId = 0;
                        var userWorkingGroup =
                            _workingGroupService.GetWorkingGroupsAdmin(customerId, SessionFacade.CurrentUser.Id)
                                .OrderByDescending(x => x.WorkingGroupId)
                                .FirstOrDefault();

                        if (userWorkingGroup != null)
                        {
                            userWorkingGroupId = userWorkingGroup.WorkingGroupId;
                        }
                        userRole = userWorkingGroupId;
                    }

                    //CHECK HOW TO HANDLE WHEN FROM EMAIL
                    //At the moment we are only fetching 1 extended case since it is only programmed that way in editPage.js
#pragma warning disable 0618
                    var stateSecondaryId = m?.case_?.StateSecondary_Id ?? 0;
#pragma warning restore 0618

                    m.ExtendedCases =
                        GetExtendedCaseFormModel(customerId, caseId, caseSolutionId, stateSecondaryId, extendedCasePathMask,
                            SessionFacade.CurrentLanguageId, userRole, SessionFacade.CurrentUser.UserGUID.ToString());

                    m.ContainsExtendedCase = m.ExtendedCases != null && m.ExtendedCases.Any();

                    //for hidden
                    if (m.ContainsExtendedCase)
                    {
                        m.ExtendedCaseGuid = m.ExtendedCases.FirstOrDefault().ExtendedCaseGuid;
                    }
                }
            }
            catch (Exception)
            {
                //TODO:
                throw;
            }

            #endregion //Extended Case

            #region Extended sections

            /*try
            {*/
            {
                string extendedCasePath = _globalSettingService.GetGlobalSettings().FirstOrDefault().ExtendedCasePath;

                if (!string.IsNullOrEmpty(extendedCasePath))
                {
                    int userRole = 0;

                    //#58691
                    //If user is Admin - send in another Number 
                    //3 = Kundadministratör
                    //4 = Administratör
                    if (SessionFacade.CurrentUser.UserGroupId >= 3)
                    {
                        userRole = 99;

                    }
                    else
                    {
                        //Take the highest workinggroupId with "Admin" access (UserRole)
                        var userWorkingGroup =
                            _workingGroupService.GetWorkingGroupsAdmin(customerId, SessionFacade.CurrentUser.Id)
                                .OrderByDescending(x => x.WorkingGroupId)
                                .FirstOrDefault();

                        int userWorkingGroupId = 0;

                        if (userWorkingGroup != null)
                        {
                            userWorkingGroupId = userWorkingGroup.WorkingGroupId;
                        }
                        userRole = userWorkingGroupId;
                    }

#pragma warning disable 0618
                    // Load if existing
                    if (m.case_.Id != 0)
                    {
                        m.ExtendedCaseSections = GetExtendedCaseSectionsModel(m.case_, customerId, extendedCasePath, SessionFacade.CurrentUser.UserGUID.ToString(), SessionFacade.CurrentLanguageId, userRole);
                    }
                    else
                    {
                        m.ExtendedCaseSections = new Dictionary<CaseSectionType, ExtendedCaseFormForCaseModel>();
                    }
#pragma warning restore 0618

                    //var sections = (Dictionary <CaseSectionType, ExtendedCaseFormModel>)_caseService.GetExtendedCaseSectionModels(customerId, caseId);
                    //m.ExtendedCaseSections = Enum
                    //	.GetValues(typeof(CaseSectionType))
                    //	.Cast<CaseSectionType>()
                    //	.Select(o => new
                    //	{
                    //		CaseSection = o,
                    //		Form = _caseService.GetExtendedCaseSectionForm(caseSolutionId, customerId, caseId, SessionFacade.CurrentLanguageId, SessionFacade.CurrentUser.UserGUID.ToString(), (m.case_ != null && m.case_.StateSecondary != null ? m.case_.StateSecondary.StateSecondaryId : 0), (m.case_ != null && m.case_.Workinggroup != null ? m.case_.Workinggroup.WorkingGroupId : 0), extendedCasePath, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserId, ApplicationType.Helpdesk, userRole)
                    //	})
                    //(Dictionary<CaseSectionType, ExtendedCaseSection>)_caseService.GetExtendedCaseSectionForm(caseSolutionId, customerId, caseId, SessionFacade.CurrentLanguageId, SessionFacade.CurrentUser.UserGUID.ToString(), (m.case_ != null && m.case_.StateSecondary != null ? m.case_.StateSecondary.StateSecondaryId : 0), (m.case_ != null && m.case_.Workinggroup != null ? m.case_.Workinggroup.WorkingGroupId : 0), extendedCasePath, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserId, ApplicationType.Helpdesk, userRole);


                    m.ContainsExtendedCase = m.ExtendedCases != null && m.ExtendedCases.Any();

                    //for hidden
                    if (m.ContainsExtendedCase)
                    {
                        m.ExtendedCaseGuid = m.ExtendedCases.FirstOrDefault().ExtendedCaseGuid;
                    }

                    //m.ExtendedCaseSections = m.CurrentCaseSolution.CaseSectionsExtendedCaseForm
                    //	.ToDictionary(o => (CaseSectionType)o.CaseSection.SectionType,
                    //		o => new ExtendedCaseSection
                    //		{
                    //			ExtendedCaseFormID = o.ExtendedCaseFormID,
                    //			CaseSolutionID = o.CaseSolutionID
                    //		});

                }
                else
                {
                    m.ExtendedCaseSections = new Dictionary<CaseSectionType, ExtendedCaseFormForCaseModel>();
                }
            }
            /*}
            catch (Exception)
            {
                //TODO:
                throw;
            }*/


            #endregion //Extended sections

            #region CaseDocuments

            //OM case har extendedcase, hämta värden 


#pragma warning disable 0618
            m.CaseDocuments = _caseDocumentService.GetCaseDocuments(customerId, m.case_, SessionFacade.CurrentUser, ApplicationType.Helpdesk);
#pragma warning restore 0618

            #endregion

            if (case_ != null)
            {
                m.MapCaseToCaseInputViewModel(case_, userTimeZone);
            }

            m.CaseTemplateTreeButton = GetCaseTemplateTreeModel(customerId, userId, CaseSolutionLocationShow.InsideTheCase, SessionFacade.CurrentLanguageId);
            m.CasePrintView = new ReportModel(false);
            m.UserHasInvoicePermission = userHasInvoicePermission;
            m.UserHasInventoryViewPermission = userHasInventoryViewPermission;
#pragma warning disable 0618
            m.IsCaseReopened = m.case_.CaseHistories != null && m.case_.CaseHistories.Where(ch => ch.FinishingDate.HasValue).Any();
#pragma warning restore 0618

            m.StatusBar = isCreateNewCase ? new Dictionary<string, string>() : GetStatusBar(m);

            return m;
        }

        public ComputerUserCategory GetComputerUserCategoryByID(int categoryID)
        {
            if (categoryID == ComputerUserCategory.EmptyCategoryId)
            {
                return ComputerUserCategory.CreateEmptyCategory();
            }

            var category = _computerService.GetComputerUserCategoryByID(categoryID);
            if (category.IsEmpty)
            {
                category.ID = ComputerUserCategory.EmptyCategoryId;
            }

            return category;
        }

        private List<ExternalInvoiceModel> GetExternalInvoices(int caseId)
        {
            return _externalInvoiceService.GetExternalInvoices(caseId).Select(x => new ExternalInvoiceModel
            {
                Id = x.Id,
                Name = x.InvoiceNumber,
                Amount = x.InvoicePrice,
                Charge = x.Charge,
                InvoiceRow = x.InvoiceRow == null ? null : new InvoiceRowViewModel { Status = x.InvoiceRow.Status }
            }).ToList();
        }

        private IList<ExtendedCaseFormForCaseModel> GetExtendedCaseFormModel(int customerId, int caseId, int caseSolutionId, int stateSecondaryId,
            string extendedCasePathMask, int languageId, int userRole, string userGuid)
        {
            var res = new List<ExtendedCaseFormForCaseModel>();

            var extendedCaseFormData =
                _caseService.GetCaseExtendedCaseForm(caseSolutionId, customerId, caseId, SessionFacade.CurrentUser.UserGUID.ToString(), stateSecondaryId);

            if (extendedCaseFormData == null)
                return res;

            var extendedCasePath = ExtendedCaseUrlBuilder.BuildExtendedCaseUrl(extendedCasePathMask, new ExtededCaseUrlParams
            {
                caseStatus = extendedCaseFormData.StateSecondaryId,
                userRole = userRole,
                userGuid = userGuid,
                customerId = customerId,
            });

            var model = new ExtendedCaseFormForCaseModel
            {
                CaseId = caseId,
                Id = extendedCaseFormData.ExtendedCaseFormId,
                ExtendedCaseGuid = extendedCaseFormData.ExtendedCaseGuid,
                Name = extendedCaseFormData.ExtendedCaseFormName,
                Path = extendedCasePath,
                LanguageId = languageId,
                //CaseStatus = caseStateSecondaryId, //majid: Set by querystring at the moment
                //UserRole = caseWorkingGroupId      //majid: Set by querystring at the moment
            };

            return new List<ExtendedCaseFormForCaseModel>() { model };
        }

        private Dictionary<CaseSectionType, ExtendedCaseFormForCaseModel> GetExtendedCaseSectionsModel(Case case_, int customerId, string extendedCasePathMask,
            string userGuid, int languageId, int userWorkingGroupId)
        {
            var caseId = case_.Id;
            var caseStateSecondaryId = case_?.StateSecondary?.StateSecondaryId ?? 0;
            var caseWorkingGroupId = case_?.Workinggroup?.WorkingGroupId ?? 0;

            var extendedCaseFormModels = new List<ExtendedCaseFormForCaseModel>();
            var exCaseFormsList = _caseService.GetExtendedCaseSectionForms(caseId, customerId);
            foreach (var ecCaseForm in exCaseFormsList)
            {
                var extendedCasePath = ExtendedCaseUrlBuilder.BuildExtendedCaseUrl(extendedCasePathMask, new ExtededCaseUrlParams
                {
                    formId = ecCaseForm.ExtendedCaseFormId,
                    caseStatus = ecCaseForm.StateSecondaryId,
                    userRole = userWorkingGroupId, //majid: NOTE, this is from now on userWorkingGroupId. 
                    userGuid = userGuid,
                    customerId = customerId,
                    autoLoad = true
                });

                var exendedForm = new ExtendedCaseFormForCaseModel
                {
                    CaseId = caseId,
                    SectionType = (CaseSectionType)ecCaseForm.SectionType,
                    Id = ecCaseForm.ExtendedCaseFormId,
                    ExtendedCaseGuid = ecCaseForm.ExtendedCaseGuid,
                    Path = extendedCasePath,
                    LanguageId = languageId,
                    //CaseStatus = caseStateSecondaryId,/Sent by querystring at the moment 
                    //UserRole = caseWorkingGroupId,/Sent by querystring at the moment
                    Name = ecCaseForm.ExtendedCaseFormName ?? string.Empty,
                    //UserGuid   //todo: check
                };

                extendedCaseFormModels.Add(exendedForm);
            }

            var res = new Dictionary<CaseSectionType, ExtendedCaseFormForCaseModel>();
            foreach (var sectionType in Enum.GetValues(typeof(CaseSectionType)).Cast<CaseSectionType>())
            {
                var extendedCaseSectionForm = extendedCaseFormModels.SingleOrDefault(x => x.SectionType == sectionType);
                if (extendedCaseSectionForm != null)
                    res.Add(sectionType, extendedCaseSectionForm);
            }

            return res;
        }

        // TODO: Refactor to better position
        public class ExtendedCaseSection
        {
            public int CaseSolutionID { get; set; }
            public int ExtendedCaseFormID { get; set; }
        }

#pragma warning disable 0618
        private Dictionary<string, string> GetStatusBar(CaseInputViewModel model)
        {
            var values = new Dictionary<string, string>();
            var templateText = "<strong>{0}</strong>&nbsp;{1}&nbsp;|&nbsp;";
            var settings = model.caseFieldSettings.Where(m => m.ShowStatusBar).Select(m => m.Name).ToList();
            var defaultValue = "";
            var dateFormat = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            foreach (var setting in settings)
            {
                var template = "";
                var value = "";
                if (setting == GlobalEnums.TranslationCaseFields.ReportedBy.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.CostCentre.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.Place.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.UserCode.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.InventoryLocation.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.CaseNumber.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.RegTime.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.ChangeTime.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.InventoryNumber.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.Caption.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.Available.ToString() ||
                   setting == GlobalEnums.TranslationCaseFields.SolutionRate.ToString())
                {
                    var property = model.case_.GetType().GetProperty(setting);
                    value = defaultValue;
                    if (property != null)
                    {
                        var propValue = property.GetValue(model.case_, null);
                        if (propValue != null)
                            value = propValue.ToString();
                    }

                }
                else if (setting == GlobalEnums.TranslationCaseFields.Persons_Name.ToString())
                {
                    value = model.case_.PersonsName;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString())
                {
                    value = model.case_.PersonsEmail;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Persons_Phone.ToString())
                {
                    value = model.case_.PersonsPhone;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString())
                {
                    value = model.case_.PersonsCellphone;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Region_Id.ToString())
                {
                    value = model.case_.Region != null ? model.case_.Region.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Department_Id.ToString())
                {
                    value = model.case_.Department != null ? model.case_.Department.DepartmentName : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.OU_Id.ToString())
                {
                    value = model.case_.Ou != null ? model.case_.Ou.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString())
                {
                    value = model.case_.IsAbout != null ? model.case_.IsAbout.ReportedBy : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString())
                {
                    value = model.case_.IsAbout != null ? model.case_.IsAbout.Person_Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString())
                {
                    value = model.case_.IsAbout != null ? model.case_.IsAbout.Person_Email : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString())
                {
                    value = model.case_.IsAbout != null ? model.case_.IsAbout.Person_Phone : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString())
                {
                    value = model.case_.IsAbout != null ? model.case_.IsAbout.Person_Cellphone : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString())
                {
                    var region = model.regions.FirstOrDefault(m => m.Id == model.case_.IsAbout.Region_Id);
                    value = region != null ? region.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString())
                {
                    var department = model.departments.FirstOrDefault(m => m.Id == model.case_.IsAbout.Department_Id);
                    value = department != null ? department.DepartmentName : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString())
                {
                    var ou = model.ous.FirstOrDefault(m => m.Id == model.case_.IsAbout.OU_Id);
                    value = ou != null ? ou.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString())
                {
                    value = model.case_.IsAbout != null ? model.case_.IsAbout.CostCentre : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString())
                {
                    value = model.case_.IsAbout != null ? model.case_.IsAbout.Place : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString())
                {
                    value = model.case_.IsAbout != null ? model.case_.IsAbout.UserCode : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString())
                {
                    value = model.case_.InventoryType;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString())
                {
                    var regSource = model.case_.RegistrationSourceCustomer_Id.HasValue ?
                        model.CustomerRegistrationSources.FirstOrDefault(m => m.Value == model.case_.RegistrationSourceCustomer_Id.Value.ToString()) :
                        null;
                    value = regSource != null ? regSource.Text : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.User_Id.ToString())
                {
                    value = defaultValue;
                    if (model.RegByUser != null)
                    {
                        value = model.RegByUser.SurName;
                        if (model.CaseOwnerDefaultWorkingGroup != null)
                            value += string.Format(" {0}", model.CaseOwnerDefaultWorkingGroup.WorkingGroupName);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(model.case_.RegUserName))
                            value = model.case_.RegUserName;
                        if (!string.IsNullOrWhiteSpace(model.case_.RegUserId))
                            value += string.Format(" {0}", model.case_.RegUserId);
                    }
                }
                else if (setting == GlobalEnums.TranslationCaseFields.CaseType_Id.ToString())
                {
                    var caseType = model.caseTypes.FirstOrDefault(m => m.Id == model.case_.CaseType_Id);

                    if (caseType == null)
                    {
                        var caseTypes = model.caseTypes;
                        var maxDepth = 10;
                        for (var i = 0; i < maxDepth; i++)
                        {
                            caseType = caseTypes
                                .Where(m => m.SubCaseTypes != null && m.SubCaseTypes.Any())
                                .SelectMany(m => m.SubCaseTypes)
                                .FirstOrDefault(m => m.Id == model.case_.CaseType_Id); ;
                            if (caseType != null) break;
                            caseTypes = caseTypes.Where(m => m.SubCaseTypes != null && m.SubCaseTypes.Any())
                                .SelectMany(m => m.SubCaseTypes).ToList();
                        }

                    }
                    value = caseType != null ? caseType.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString())
                {
                    var prodArea = model.productAreas.FirstOrDefault(m => m.Id == model.case_.ProductArea_Id);
                    if (prodArea == null)
                    {
                        var productAreas = model.productAreas;
                        var maxDepth = 10;
                        for (var i = 0; i < maxDepth; i++)
                        {
                            prodArea = productAreas
                                .Where(m => m.SubProductAreas != null && m.SubProductAreas.Any())
                                .SelectMany(m => m.SubProductAreas)
                                .FirstOrDefault(m => m.Id == model.case_.ProductArea_Id); ;

                            if (prodArea != null)
                                break;

                            productAreas =
                                productAreas.Where(m => m.SubProductAreas != null && m.SubProductAreas.Any())
                                    .SelectMany(m => m.SubProductAreas)
                                    .ToList();
                        }
                    }
                    value = prodArea != null ? prodArea.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.System_Id.ToString())
                {
                    var system = model.systems.FirstOrDefault(m => m.Id == model.case_.System_Id);
                    value = system != null ? system.SystemName : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Urgency_Id.ToString())
                {
                    var urgency = model.urgencies.FirstOrDefault(m => m.Id == model.case_.Urgency_Id);
                    value = urgency != null ? urgency.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Impact_Id.ToString())
                {
                    var impact = model.impacts.FirstOrDefault(m => m.Id == model.case_.Impact_Id);
                    value = impact != null ? impact.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Category_Id.ToString())
                {
                    var cat = model.categories.FirstOrDefault(m => m.Id == model.case_.Category_Id);
                    value = cat != null ? cat.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Supplier_Id.ToString())
                {
                    var sup = model.suppliers.FirstOrDefault(m => m.Id == model.case_.Supplier_Id);
                    value = sup != null ? sup.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.AgreedDate.ToString())
                {
                    value = model.case_.AgreedDate.HasValue ? model.case_.AgreedDate.Value.ToString(dateFormat) : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Cost.ToString())
                {
                    value = string.Format("{0}/{1} {2}", model.case_.Cost, model.case_.OtherCost, model.case_.Currency);
                }
                else if (setting == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString())
                {
                    var wg = model.workingGroups.FirstOrDefault(m => m.Id == model.case_.WorkingGroup_Id);
                    value = wg != null ? wg.WorkingGroupName : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString())
                {
                    var resp = model.case_.CaseResponsibleUser_Id.HasValue
                        ? model.ResponsibleUsersAvailable.FirstOrDefault(m =>
                            m.Value == model.case_.CaseResponsibleUser_Id.Value.ToString())
                        : null;
                    value = resp != null ? resp.Text : defaultValue;
                }

                else if (setting == GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString())
                {
                    var perfomer = model.case_.Performer_User_Id.HasValue ?
                        model.Performers.FirstOrDefault(m => m.Value == model.case_.Performer_User_Id.Value.ToString()) :
                        null;
                    value = perfomer != null ? perfomer.Text : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Priority_Id.ToString())
                {
                    var priority = model.priorities.FirstOrDefault(m => m.Id == model.case_.Priority_Id);
                    value = priority != null ? priority.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Status_Id.ToString())
                {
                    var status = model.statuses.FirstOrDefault(m => m.Id == model.case_.Status_Id);
                    value = status != null ? status.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString())
                {
                    var status = model.stateSecondaries.FirstOrDefault(m => m.Id == model.case_.StateSecondary_Id);
                    value = status != null ? status.Name : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.PlanDate.ToString())
                {
                    value = model.case_.PlanDate.HasValue ? model.case_.PlanDate.Value.ToString(dateFormat) : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.WatchDate.ToString())
                {
                    value = model.case_.WatchDate.HasValue ? model.case_.WatchDate.Value.ToString(dateFormat) : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.Verified.ToString())
                {
                    value = Translation.GetCoreTextTranslation(model.case_.Verified == 1 ? "Ja" : "Nej");
                }
                else if (setting == GlobalEnums.TranslationCaseFields.CausingPart.ToString())
                {
                    var causing = model.case_.CausingPartId.HasValue ?
                        model.causingParts.FirstOrDefault(m => m.Value == model.case_.CausingPartId.Value.ToString()) :
                        null;
                    value = causing != null ? causing.Text : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.FinishingDate.ToString())
                {
                    value = model.case_.FinishingDate.HasValue ? model.case_.FinishingDate.Value.ToString(dateFormat) : defaultValue;
                }
                else if (setting == GlobalEnums.TranslationCaseFields.ClosingReason.ToString())
                {
                    value = model.FinishingCause;
                }
                template = string.Format(templateText, Translation.GetForCase(setting, model.CustomerSettings.CustomerId), value);
                values.Add(setting, template);
            }

            return values;
        }
#pragma warning restore 0618

        private CaseSearchResultModel GetUnfilteredCases(
                   string sortBy,
                   string sortByAsc,
                   int? relatedCasesCaseId = null,
                   string relatedCasesUserId = null,
                   int[] caseIds = null)
        {
            var searchResult = new CaseSearchResultModel
            {
                GridSettings =
                    _caseOverviewSettingsService.GetSettings(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id),
                caseSettings = _caseSettingService.GetCaseSettingsWithUser(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId)
            };

            var search = this.InitEmptySearchModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            search.Search.SortBy = sortBy ?? string.Empty;
            search.Search.Ascending = sortByAsc.ToBool();
            search.CaseSearchFilter.CaseProgress = CaseProgressFilter.None;

            var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;

            CaseRemainingTimeData remainingTime;
            CaseAggregateData aggregateData;
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id).ToArray();

            var currentUserId = SessionFacade.CurrentUser.Id;
            var customerId = SessionFacade.CurrentCustomer.Id;
            var customerSettings = _customerUserService.GetCustomerUserSettings(customerId, currentUserId);

            searchResult.cases = _caseSearchService.Search(
                search.CaseSearchFilter,
                searchResult.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                customerSettings.RestrictedCasePermission,
                search.Search,
                _workContext.Customer.WorkingDayStart,
                _workContext.Customer.WorkingDayEnd,
                TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId),
                ApplicationTypes.Helpdesk,
                showRemainingTime,
                out remainingTime,
                out aggregateData,
                relatedCasesCaseId,
                relatedCasesUserId,
                caseIds).Items;

            searchResult.cases = CommonHelper.TreeTranslate(searchResult.cases, SessionFacade.CurrentCustomer.Id, _productAreaService);
            search.Search.IdsForLastSearch = this.GetIdsFromSearchResult(searchResult.cases);
            searchResult.BackUrl = Url.Action("RelatedCasesFull", "Cases", new { area = string.Empty, caseId = relatedCasesCaseId, userId = relatedCasesUserId });

            return searchResult;
        }

        private RelatedCasesFullViewModel GetRelatedCasesViewModel(
                                            string sortBy,
                                            string sortByAsc,
                                            int caseId,
                                            string userId)
        {
            var cases = this.GetUnfilteredCases(sortBy, sortByAsc, caseId, userId);
            var model = _caseModelFactory.GetRelatedCasesFullModel(cases, userId, caseId, sortBy, sortByAsc.ToBool());

            return model;
        }

        private CaseSearchModel InitCaseSearchModel(int customerId, int userId)
        {
            DHDomain.ISearch s = new DHDomain.Search();
            var f = new CaseSearchFilter();
            var m = new CaseSearchModel();
            var cu = _customerUserService.GetCustomerUserSettings(customerId, userId);
            if (cu == null)
            {
                throw new Exception(string.Format("Customers settings is empty or not valid for customer id {0}", customerId));
            }

            f.CustomerId = customerId;
            f.UserId = userId;
            f.CaseType = cu.CaseCaseTypeFilter.ReturnCustomerUserValue().ToInt();
            f.Category = cu.CaseCategoryFilter.ReturnCustomerUserValue();
            f.Priority = cu.CasePriorityFilter.ReturnCustomerUserValue();
            f.ProductArea = cu.CaseProductAreaFilter.ReturnCustomerUserValue();
            f.Region = cu.CaseRegionFilter.ReturnCustomerUserValue();
            f.Department = cu.CaseDepartmentFilter.ReturnCustomerUserValue();
            f.StateSecondary = cu.CaseStateSecondaryFilter.ReturnCustomerUserValue();
            f.Status = cu.CaseStatusFilter.ReturnCustomerUserValue();
            f.User = cu.CaseUserFilter.ReturnCustomerUserValue();
            f.UserPerformer = cu.CasePerformerFilter.ReturnCustomerUserValue();
            f.UserResponsible = cu.CaseResponsibleFilter.ReturnCustomerUserValue();
            f.WorkingGroup = cu.CaseWorkingGroupFilter.ReturnCustomerUserValue();
            f.CaseProgress = CaseSearchFilter.InProgressCases;
            f.CaseRegistrationDateStartFilter = cu.CaseRegistrationDateStartFilter;
            f.CaseRegistrationDateEndFilter = cu.CaseRegistrationDateEndFilter;
            f.CaseWatchDateStartFilter = cu.CaseWatchDateStartFilter;
            f.CaseWatchDateEndFilter = cu.CaseWatchDateEndFilter;
            f.CaseClosingDateStartFilter = cu.CaseClosingDateStartFilter;
            f.CaseClosingDateEndFilter = cu.CaseClosingDateEndFilter;
            f.CaseClosingReasonFilter = cu.CaseClosingReasonFilter.ReturnCustomerUserValue();
            f.CaseRemainingTime = cu.CaseRemainingTimeFilter.ReturnCustomerUserValue();
            f.PageInfo = new PageInfo();
            this.ResolveParentPathesForFilter(f);

            s.SortBy = "CaseNumber";
            s.Ascending = true;

            return new CaseSearchModel() { CaseSearchFilter = f, Search = s };
        }

        private CaseSearchModel InitEmptySearchModel(int customerId, int userId)
        {
            var s = new Search();
            var f = new CaseSearchFilter();

            f.CustomerId = customerId;
            f.UserId = userId;
            f.CaseType = 0;
            f.Category = null;
            f.Priority = null;
            f.ProductArea = null;
            f.Region = null;
            f.StateSecondary = null;
            f.Status = null;
            f.User = null;
            f.UserPerformer = null;
            f.UserResponsible = null;
            f.WorkingGroup = null;
            f.CaseProgress = CaseSearchFilter.InProgressCases;
            f.CaseRegistrationDateStartFilter = null;
            f.CaseRegistrationDateEndFilter = null;
            f.CaseWatchDateStartFilter = null;
            f.CaseWatchDateEndFilter = null;
            f.CaseClosingDateStartFilter = null;
            f.CaseClosingDateEndFilter = null;
            f.CaseClosingReasonFilter = null;
            f.PageInfo = new PageInfo();
            ResolveParentPathesForFilter(f);

            s.SortBy = "CaseNumber";
            s.Ascending = true;

            return new CaseSearchModel() { CaseSearchFilter = f, Search = s };
        }

        private List<MyFavoriteFilterJSModel> GetMyFavorites(int customerId, int userId)
        {
            var ret = new List<MyFavoriteFilterJSModel>();
            var favorites = _caseService.GetMyFavoritesWithFields(customerId, userId);
            if (favorites.Any())
            {
                foreach (var favorite in favorites)
                    ret.Add(new MyFavoriteFilterJSModel(favorite.Id, favorite.Name, favorite.Fields));
            }
            return ret.OrderBy(f => f.Name).ToList();
        }

        private List<SelectListItem> GetRemainigTimeList(int customerId)
        {
            var curCustomer = _customerService.GetCustomer(customerId);
            var dayWorkingTime = curCustomer.WorkingDayEnd - curCustomer.WorkingDayStart;
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Akuta ärenden", Enums.TranslationSource.TextTranslation),
                Value = RemainingTimes.Delayed.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<1 h",
                Value = RemainingTimes.OneHour.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<2 h",
                Value = RemainingTimes.TwoHours.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<4 h",
                Value = RemainingTimes.FourHours.ToStr(),
                Selected = false
            });

            if (dayWorkingTime > 8)
            {
                li.Add(new SelectListItem()
                {
                    Text = Translation.Get("Återstående åtgärdstid") + ", " + "<8 h",
                    Value = RemainingTimes.EightHours.ToStr(),
                    Selected = false
                });
            }

            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<1 d",
                Value = RemainingTimes.OneDay.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<2 d",
                Value = RemainingTimes.TwoDays.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<3 d",
                Value = RemainingTimes.ThreeDays.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<4 d",
                Value = RemainingTimes.FourDays.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + "<5 d",
                Value = RemainingTimes.FiveDays.ToStr(),
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Återstående åtgärdstid") + ", " + ">=5 d",
                Value = RemainingTimes.MaxDays.ToStr(),
                Selected = false
            });

            return li;
        }


        private List<ItemOverview> GetMaxRowsFilter()
        {
            var ret = new List<ItemOverview>();
            ret.Add(new ItemOverview("10", "10"));
            ret.Add(new ItemOverview("20", "20"));
            ret.Add(new ItemOverview("30", "30"));
            ret.Add(new ItemOverview("40", "40"));
            ret.Add(new ItemOverview("50", "50"));
            ret.Add(new ItemOverview("100", "100"));
            ret.Add(new ItemOverview("200", "200"));
            return ret;
        }

        private CaseLockModel GetCaseLockModel(int caseId, int userId, bool isNeedLock = true, string activeTab = "")
        {
            var caseLock = _caseLockService.GetCaseLockOverviewByCaseId(caseId);
            var globalSettings = _globalSettingService.GetGlobalSettings().FirstOrDefault();

            return GetCaseLockModel(caseLock, caseId, userId, globalSettings, isNeedLock, activeTab);
        }

        private CaseLockModel GetCaseLockModel(ICaseLockOverview caseLock, int caseId, int userId, GlobalSetting globalSettings, bool isNeedLock = true, string activeTab = "")
        {
            CaseLockModel caseLockModel = null;

            var caseIsLocked = true;
            var extendedSec = (globalSettings != null && globalSettings.CaseLockExtendTime > 0 ? globalSettings.CaseLockExtendTime : this.DefaultExtendCaseLockTime);
            var timerInterval = (globalSettings != null ? globalSettings.CaseLockTimer : 0);
            var bufferTime = (globalSettings != null && globalSettings.CaseLockBufferTime > 0 ? globalSettings.CaseLockBufferTime : this.DefaultCaseLockBufferTime);
            var caseLockGUID = string.Empty;
            var nowTime = DateTime.Now;

            if (caseLock == null)
            {
                // Case is not locked 
                caseIsLocked = false;
            }
            else
            {
                if ((caseLock.ExtendedTime.AddSeconds(bufferTime) < nowTime) ||
                    (caseLock.ExtendedTime.AddSeconds(bufferTime) >= nowTime &&
                     caseLock.UserId == userId &&
                     caseLock.BrowserSession == Session.SessionID))
                {
                    // Unlock case because user left the Case in anormal way (Close browser/reset computer)
                    // Unlock case because it was open by current user last time / recently
                    _caseLockService.UnlockCaseByCaseId(caseId);
                    caseIsLocked = false;
                }
            }

            if (!caseIsLocked)
            {
                // Lock Case if it's not locked
                var now = DateTime.Now;
                var extendedLockTime = now.AddSeconds(extendedSec);
                var newLockGUID = Guid.NewGuid();

                var newCaseLock = new CaseLock(caseId, userId, newLockGUID, Session.SessionID, now, extendedLockTime, activeTab);
                if (isNeedLock)
                    _caseLockService.LockCase(newCaseLock);

                caseLockModel = newCaseLock.MapToViewModel(caseIsLocked, extendedSec, timerInterval, activeTab);
            }
            else
            {
                caseLockModel = caseLock.MapToViewModel(caseIsLocked, extendedSec, timerInterval, activeTab);
            }

            return caseLockModel;
        }

        private CaseLockModel GetCaseLockOverviewModel(ICaseLockOverview caseLock, int caseId, GlobalSetting globalSettings)
        {
            CaseLockModel caseLockModel = null;

            var caseIsLocked = true;
            var extendedSec = (globalSettings != null && globalSettings.CaseLockExtendTime > 0 ? globalSettings.CaseLockExtendTime : this.DefaultExtendCaseLockTime);
            var timerInterval = (globalSettings != null ? globalSettings.CaseLockTimer : 0);
            var bufferTime = (globalSettings != null && globalSettings.CaseLockBufferTime > 0 ? globalSettings.CaseLockBufferTime : this.DefaultCaseLockBufferTime);
            var caseLockGUID = string.Empty;
            var nowTime = DateTime.Now;

            if (caseLock == null)
            {
                // Case is not locked 
                caseIsLocked = false;
            }
            else
            {
                //Check if case is still locked
                var caseLockExtendedTime = caseLock.ExtendedTime.AddSeconds(bufferTime);
                if (caseLockExtendedTime >= nowTime)
                {
                    caseIsLocked = true;
                }
                else
                {
                    caseIsLocked = false;
                }
            }

            caseLockModel = caseLock.MapToViewModel(caseIsLocked, extendedSec, timerInterval);

            return caseLockModel;
        }


        private CaseStatisticsViewModel GetCaseStatisticsModel(CaseAggregateData aggregateData, int caseCount,
                                                               string expandedGroup, List<string> fields)
        {
            if (aggregateData == null)
                return null;

            var customerId = SessionFacade.CurrentCustomer.Id;
            var ret = new CaseStatisticsViewModel();
            ret.ExpandedGroup = expandedGroup;
            ret.CaseData.AttributeId = "Case";
            ret.CaseData.AttributeName = Translation.Get("Ärenden");
            ret.CaseData.AttributeValue = caseCount.ToString();
            int sumValues;

            #region Status

            if (fields.Contains(GlobalEnums.TranslationCaseFields.Status_Id.ToString()))
            {
                sumValues = 0;
                var statusChildren = new List<DataAttributeGroup>();
                var allStatus = _statusService.GetStatuses(customerId);
                foreach (var status in aggregateData.Status)
                {
                    var name = allStatus.Where(s => s.Id == status.Key).Select(s => s.Name).FirstOrDefault() ?? string.Empty;
                    statusChildren.Add(new DataAttributeGroup(status.Key.ToString(), name, status.Value.ToString()));
                    sumValues += status.Value;
                }

                var statusGroup = new DataAttributeGroup(
                                                    "Status",
                                                    Translation.Get(GlobalEnums.TranslationCaseFields.Status_Id.ToString(),
                                                                    Enums.TranslationSource.CaseTranslation, customerId),
                                                    sumValues.ToString(),
                                                    statusChildren.OrderBy(s => s.AttributeName).ToList());

                ret.CaseData.AddChild(statusGroup);

            }
            #endregion

            #region SubStatus
            if (fields.Contains(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()))
            {
                sumValues = 0;
                var subStatusChildren = new List<DataAttributeGroup>();
                var allSubStatus = _stateSecondaryService.GetStateSecondaries(customerId);
                foreach (var subStatus in aggregateData.SubStatus)
                {
                    var name = allSubStatus.Where(s => s.Id == subStatus.Key).Select(s => s.Name).FirstOrDefault() ?? string.Empty;
                    subStatusChildren.Add(new DataAttributeGroup(subStatus.Key.ToString(), Translation.Get(name), subStatus.Value.ToString()));
                    sumValues += subStatus.Value;
                }

                var subStatusGroup = new DataAttributeGroup(
                                                    "SubStatus",
                                                    Translation.Get(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(),
                                                                    Enums.TranslationSource.CaseTranslation, customerId),
                                                    sumValues.ToString(),
                                                    subStatusChildren.OrderBy(ss => ss.AttributeName).ToList());

                ret.CaseData.AddChild(subStatusGroup);
            }
            #endregion

            return ret;
        }

        private CaseSettingModel GetCaseSettingModel(
            int customerId,
            int userId,
            int gridId = GridSettingsService.CASE_OVERVIEW_GRID_ID,
            IList<CaseFieldSetting> customerCaseFieldSettings = null)
        {
            var ret = new CaseSettingModel();
            const bool IsTakeOnlyActive = true;
            ret.CustomerId = customerId;
            ret.UserId = userId;

            var userCaseSettings = _customerUserService.GetUserCaseSettings(customerId, userId);

            var regions = _regionService.GetRegions(customerId);
            ret.RegionCheck = userCaseSettings.Region != string.Empty;
            regions.Insert(0, ObjectExtensions.notAssignedRegion());
            ret.Regions = regions;
            ret.SelectedRegion = userCaseSettings.Region;

            var customerSettings = GetCustomerSettings(customerId);

            var departments = _departmentService.GetDepartmentsByUserPermissions(userId, customerId, IsTakeOnlyActive);
            if (!departments.Any())
            {
                departments =
                    GetCustomerDepartments(customerId).Where(d =>
                        d.Region_Id == null ||
                        IsTakeOnlyActive == false ||
                        (IsTakeOnlyActive && d.Region != null && d.Region.IsActive != 0)).ToList();
            }

            //var departments = _departmentService.GetDepartments(customerId, ActivationStatus.All);
            ret.IsDepartmentChecked = userCaseSettings.Departments != string.Empty;

            if (customerSettings != null && customerSettings.ShowOUsOnDepartmentFilter != 0)
                departments = AddOrganizationUnitsToDepartments(departments);

            departments.Insert(0, ObjectExtensions.notAssignedDepartment());
            ret.Departments = departments;

            ret.SelectedDepartments = userCaseSettings.Departments;

            ret.RegisteredByCheck = userCaseSettings.RegisteredBy != string.Empty;
            ret.RegisteredByUserList = _userService.GetUserOnCases(customerId, IsTakeOnlyActive).MapToSelectList(customerSettings);
            if (!string.IsNullOrEmpty(userCaseSettings.RegisteredBy))
            {
                ret.lstRegisterBy = userCaseSettings.RegisteredBy.Split(',').Select(int.Parse).ToArray();
            }

            ret.CaseTypeCheck = userCaseSettings.CaseType != string.Empty;
            ret.CaseTypes = _caseTypeService.GetCaseTypesOverviewWithChildren(customerId, IsTakeOnlyActive).ToList();
            ret.CaseTypePath = "--";
            int caseType;
            int.TryParse(userCaseSettings.CaseType, out caseType);
            ret.CaseTypeId = caseType;
            if (caseType > 0)
            {
                var ct = _caseTypeService.GetCaseType(ret.CaseTypeId);
                if (ct != null)
                {
                    ret.CaseTypePath = ct.getCaseTypeParentPath();
                }
            }

            ret.ProductAreas = _productAreaService.GetProductAreasOverviewWithChildren(customerId);

            ret.ProductAreaPath = "--";

            int pa;
            int.TryParse(userCaseSettings.ProductArea, out pa);
            ret.ProductAreaId = pa;

            if (pa > 0)
            {
                var p = _productAreaService.GetProductArea(ret.ProductAreaId);
                if (p != null)
                    ret.ProductAreaPath = string.Join(" - ", _productAreaService.GetParentPath(p.Id, customerId));
            }
            ret.ProductAreaCheck = (userCaseSettings.ProductArea != string.Empty);

            //var userWorkingGroup = _userService.GetUserWorkingGroups().Where(u => u.User_Id == userId).Select(x => x.WorkingGroup_Id);

            //var isAdmin = SessionFacade.CurrentUser.IsAdministrator();
            //if (isAdmin)
            if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
            {
                var workingGroups = _workingGroupService.GetWorkingGroups(customerId, userId, true, true).ToList();
                ret.WorkingGroups = workingGroups;
            }
            else
            {
                var workingGroups = _workingGroupService.GetWorkingGroups(customerId, true).ToList();
                ret.WorkingGroups = workingGroups;
            }
            ret.WorkingGroups.Insert(0, ObjectExtensions.notAssignedWorkingGroup());


            //.Where(w => userWorkingGroup.Contains(w.Id))
            ret.WorkingGroupCheck = (userCaseSettings.WorkingGroup != string.Empty);
            //ret.WorkingGroups = workingGroups;
            ret.SelectedWorkingGroup = userCaseSettings.WorkingGroup;

            ret.ResponsibleCheck = userCaseSettings.Responsible;

            ret.AdministratorCheck = true;

            var performers = _userService.GetAvailablePerformersOrUserId(customerId);
            performers.Insert(0, ObjectExtensions.notAssignedPerformer());
            ret.AvailablePerformersList = performers.MapToSelectList(customerSettings);
            if (!string.IsNullOrEmpty(userCaseSettings.Administrators))
            {
                ret.lstAdministrator = userCaseSettings.Administrators.Split(',').Select(int.Parse).ToArray();
            }

            var priorities = _priorityService.GetPriorities(customerId).OrderBy(p => p.Code).ToList();
            ret.PriorityCheck = (userCaseSettings.Priority != string.Empty);
            priorities.Insert(0, ObjectExtensions.notAssignedPriority());
            ret.Priorities = priorities;
            ret.SelectedPriority = userCaseSettings.Priority;

            ret.Categories =
                _categoryService.GetParentCategoriesWithChildren(customerId, true).OrderBy(c => Translation.GetMasterDataTranslation(c.Name)).ToList();

            ret.CategoryPath = "--";

            int ca;
            int.TryParse(userCaseSettings.Category, out ca);
            ret.CategoryId = ca;

            if (ca > 0)
            {
                var c = _categoryService.GetCategory(ret.CategoryId, customerId);
                if (c != null)
                    ret.CategoryPath = string.Join(" - ", _categoryService.GetParentPath(c.Id, customerId));
            }
            ret.CategoryCheck = (userCaseSettings.Category != string.Empty);

            //var categories = _categoryService.GetActiveCategories(customerId).OrderBy(c => c.Name).ToList();
            //ret.CategoryCheck = (userCaseSettings.Category != string.Empty);
            //ret.Categories = categories;
            //ret.SelectedCategory = userCaseSettings.Category;

            var states = _statusService.GetStatuses(customerId).OrderBy(s => s.Name).ToList();
            ret.StateCheck = (userCaseSettings.State != string.Empty);
            states.Insert(0, ObjectExtensions.notAssignedStatus());
            ret.States = states;
            ret.SelectedState = userCaseSettings.State;

            var subStates = _stateSecondaryService.GetStateSecondaries(customerId).OrderBy(s => s.Name).ToList();
            ret.SubStateCheck = (userCaseSettings.SubState != string.Empty);
            subStates.Insert(0, ObjectExtensions.notAssignedStateSecondary());
            ret.SubStates = subStates;
            ret.SelectedSubState = userCaseSettings.SubState;

            var caseRemainingTime = this.GetRemainigTimeList(customerId);
            ret.CaseRemainingTimeChecked = userCaseSettings.CaseRemainingTime != string.Empty;
            ret.filterCaseRemainingTime = caseRemainingTime;
            ret.SelectedCaseRemainingTime = userCaseSettings.CaseRemainingTime;

            ret.CaseRegistrationDateStartFilter = userCaseSettings.CaseRegistrationDateStartFilter;
            ret.CaseRegistrationDateEndFilter = userCaseSettings.CaseRegistrationDateEndFilter;
            ret.CaseWatchDateStartFilter = userCaseSettings.CaseWatchDateStartFilter;
            ret.CaseWatchDateEndFilter = userCaseSettings.CaseWatchDateEndFilter;
            ret.CaseClosingDateStartFilter = userCaseSettings.CaseClosingDateStartFilter;
            ret.CaseClosingDateEndFilter = userCaseSettings.CaseClosingDateEndFilter;
            ret.CaseRegistrationDateFilterShow = userCaseSettings.CaseRegistrationDateFilterShow;
            ret.CaseWatchDateFilterShow = userCaseSettings.CaseWatchDateFilterShow;
            ret.CaseClosingDateFilterShow = userCaseSettings.CaseClosingDateFilterShow;

            /// "Inititator" field
            ret.CaseInitiatorFilterShow = userCaseSettings.CaseInitiatorFilterShow;

            ret.ClosingReasonPath = "--";
            int closingReason;
            int.TryParse(userCaseSettings.CaseClosingReasonFilter, out closingReason);
            ret.ClosingReasonId = closingReason;
            if (closingReason > 0)
            {
                var fc = _finishingCauseService.GetFinishingCause(ret.ClosingReasonId);
                if (fc != null)
                {
                    ret.ClosingReasonPath = fc.GetFinishingCauseParentPath();
                }
            }

            ret.ClosingReasonCheck = userCaseSettings.CaseClosingReasonFilter != string.Empty;
            ret.ClosingReasons = _finishingCauseService.GetFinishingCausesWithChilds(customerId);

            ret.ColumnSettingModel =
                _caseOverviewSettingsService.GetSettings(
                    customerId,
                    SessionFacade.CurrentUser.UserGroupId,
                    userId,
                    gridId,
                    customerCaseFieldSettings);

            return ret;
        }

        private List<SelectListItem> GetCausingPartsModel(int customerId, int? curCausingPartId)
        {
            var allActiveCausinParts = _causingPartService.GetActiveParentCausingParts(customerId, curCausingPartId);
            var ret = new List<SelectListItem>();

            var parentRet = new List<SelectListItem>();
            var childrenRet = new List<SelectListItem>();

            foreach (var causingPart in allActiveCausinParts)
            {
                var curName = string.Empty;
                if (causingPart.Parent != null && curCausingPartId.HasValue && causingPart.Id == curCausingPartId.Value)
                {
                    curName = string.Format("{0} - {1}", Translation.Get(causingPart.Parent.Name, Enums.TranslationSource.TextTranslation, customerId),
                                                         Translation.Get(causingPart.Name, Enums.TranslationSource.TextTranslation, customerId));

                    childrenRet.Add(new SelectListItem() { Value = causingPart.Id.ToString(), Text = curName, Selected = true });
                }
                else
                {
                    if (causingPart.Children.Any())
                    {
                        foreach (var child in causingPart.Children)
                        {
                            if (child.IsActive)
                            {
                                curName = string.Format("{0} - {1}", Translation.Get(causingPart.Name, Enums.TranslationSource.TextTranslation, customerId),
                                                                     Translation.Get(child.Name, Enums.TranslationSource.TextTranslation, customerId));

                                var isSelected = (child.Id == curCausingPartId);
                                childrenRet.Add(new SelectListItem() { Value = child.Id.ToString(), Text = curName, Selected = isSelected });
                            }
                        }
                    }
                    else
                    {
                        curName = Translation.Get(causingPart.Name, Enums.TranslationSource.TextTranslation, customerId);
                        var isSelected = (causingPart.Id == curCausingPartId);
                        parentRet.Add(new SelectListItem() { Value = causingPart.Id.ToString(), Text = curName, Selected = isSelected });
                    }
                }
            }

            ret = parentRet.OrderBy(p => p.Text).Union(childrenRet.OrderBy(c => c.Text)).ToList();

            return ret.GroupBy(r => r.Value).Select(g => g.First()).ToList();
        }

        private List<SelectListItem> GetProductAreasModel(int customerId, int? curProductAreaId)
        {
            var allProductAreas = _productAreaService.GetTopProductAreasWithChilds(customerId, false);
            var ret = new List<SelectListItem>();

            var parentRet = new List<SelectListItem>();
            var childrenRet = new List<SelectListItem>();

            var curName = string.Empty;

            foreach (var productArea in allProductAreas)
            {
                curName = productArea.ResolveFullName();
                parentRet.Add(new SelectListItem()
                {
                    Value = productArea.Id.ToString(),
                    Text = curName,
                    Selected = (productArea.Id == curProductAreaId),
                    Disabled = productArea.IsActive == 0
                });
                parentRet.AddRange(GetProductAreaChild(productArea, curProductAreaId, productArea.IsActive == 0));
            }

            ret = parentRet.OrderBy(p => p.Text).ToList();
            return ret;

        }

        private List<SelectListItem> GetProductAreaChild(ProductArea productArea, int? curProductAreaId, bool parentIsDisabled)
        {
            var ret = new List<SelectListItem>();
            if (productArea.SubProductAreas.Any())
            {
                var curName = string.Empty;
                foreach (var child in productArea.SubProductAreas)
                {
                    curName = child.ResolveFullName();
                    ret.Add(new SelectListItem()
                    {
                        Value = child.Id.ToString(),
                        Text = curName,
                        Selected = (child.Id == curProductAreaId),
                        Disabled = (parentIsDisabled || child.IsActive == 0)
                    });
                    ret.AddRange(GetProductAreaChild(child, curProductAreaId, child.IsActive == 0));
                }
            }

            return ret;

        }

        private CaseRuleModel GetCaseRuleModel(int customerId, CaseRuleMode ruleType,
                                               CaseCurrentDataModel currentData,
                                               List<CaseSolutionSettingModel> templateSettingModel,
                                               Setting customerSettings,
                                               List<CaseFieldSetting> caseFieldSettings)
        {
            _caseRuleFactory.Initialize(_regionService, _departmentService, _ouService,
                                        _registrationSourceCustomerService, _caseTypeService, _productAreaService,
                                        _systemService, _urgencyService, _impactService, _categoryService,
                                        _supplierService, _workingGroupService, _userService,
                                        _priorityService, _statusService, _stateSecondaryService,
                                        _projectService, _problemService, _causingPartService,
                                        _changeService, _finishingCauseService, _watchDateCalendarService,
                                        _computerService);

            var model = _caseRuleFactory.GetCaseRuleModel(customerId, ruleType, currentData, customerSettings, caseFieldSettings, new List<CaseSolutionSettingModel>());

            return model;
        }

        private CaseFieldStatusType GetFieldStatusType(TranslationCaseFields caseField, List<CaseFieldSetting> caseFieldSettings,
                                                       CaseSolutionFields templateField, List<CaseSolutionSettingModel> templateFieldSettings)
        {
            var mode = CaseSolutionModes.DisplayField;
            var checkedTemplateSetting = false;
            var templateSettingMode = CaseSolutionModes.Hide;
            var caseSettingMode = CaseSolutionModes.Hide;

            // Check template behavior if exists
            if (templateFieldSettings != null && templateFieldSettings.Any())
            {
                var templateSetting = templateFieldSettings.Where(s => s.CaseSolutionField == templateField).SingleOrDefault();
                if (templateSetting != null)
                {
                    checkedTemplateSetting = true;
                    templateSettingMode = templateSetting.CaseSolutionMode;
                }
            }

            // Check Customer Case-Field Settings behavior 
            var caseFieldSetting = caseFieldSettings.getCaseFieldSettingId(caseField.ToString());
            if (caseFieldSettings.IsFieldLocked(caseField))
            {
                caseSettingMode = CaseSolutionModes.ReadOnly;
            }
            else if (caseFieldSettings.IsFieldRequiredOrVisible(caseField))
            {
                caseSettingMode = CaseSolutionModes.DisplayField;
            }

            if (checkedTemplateSetting)
            {
                if (caseSettingMode == CaseSolutionModes.Hide)
                    mode = CaseSolutionModes.Hide;
                else
                {
                    if (templateSettingMode == CaseSolutionModes.DisplayField && caseSettingMode == CaseSolutionModes.ReadOnly)
                        mode = CaseSolutionModes.ReadOnly;
                    else
                        mode = caseSettingMode;
                }
            }
            else
            {
                mode = caseSettingMode;
            }

            switch (mode)
            {
                case CaseSolutionModes.DisplayField:
                    return CaseFieldStatusType.Editable;

                case CaseSolutionModes.Hide:
                    return CaseFieldStatusType.Hidden;

                case CaseSolutionModes.ReadOnly:
                    return CaseFieldStatusType.Readonly;
            }

            return CaseFieldStatusType.Editable;
        }
        private List<CasePerformersSearch> createPerformerforSearchList(int customerId, IList<BusinessData.Models.User.CustomerUserInfo> performers)
        {
            var performersToSearch = new List<CasePerformersSearch>();
            var customerWg = _workingGroupService.GetAllWorkingGroupsForCustomer(customerId);
            var foundWg = false;
            foreach (var user in performers)
            {
                foundWg = false;
                foreach (var uwg in user.WorkingGroups)
                {
                    if (uwg.IsMemberOfGroup)
                    {
                        var wg = customerWg.FirstOrDefault(w => w.Id == uwg.WorkingGroupId && w.IsActive != 0);
                        if (wg != null)
                        {
                            var newRecord = new CasePerformersSearch
                              (
                                  user.Id,
                                  user.FirstName,
                                  user.SurName,
                                  wg.WorkingGroupName,
                                  wg.Id,
                                  user.Email
                              );
                            performersToSearch.Add(newRecord);
                            foundWg = true;
                        }
                    }
                }
                if (!foundWg)
                {
                    var newRecord = new CasePerformersSearch
                               (
                                   user.Id,
                                   user.FirstName,
                                   user.SurName,
                                   null,
                                   0,
                                   user.Email
                               );
                    performersToSearch.Add(newRecord);
                }
            }
            return performersToSearch
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ThenBy(p => p.WorkingGroupName)
                .ToList();
        }
        #endregion

        #region --General--

        private SimpleToken GetSimpleToken(string userName, string password)
        {
            try
            {
                var baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.ApplicationPath.TrimEnd('/'));
                var webApiService = new WebApiService(baseUrl);
                var token = AsyncHelper.RunSync(() => webApiService.GetAccessToken(userName, password));

                if (token != null)
                {
                    var encriptionKey = ConfigurationManager.AppSettings.AllKeys.Contains(AppSettingsKey.EncryptionKey) ?
                                        ConfigurationManager.AppSettings[AppSettingsKey.EncryptionKey].ToString() : string.Empty;

                    token.access_token = AESCryptoProvider.Encrypt256(token.access_token, encriptionKey);
                    token.refresh_token = AESCryptoProvider.Encrypt256(token.refresh_token, encriptionKey);
                }

                return token;
            }
            catch
            {
                return null;
            }
        }

        private string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        private RouteValueDictionary ExtractRouteInfo(string fullUrl)
        {
            var questionMarkIndex = fullUrl.IndexOf('?');
            string queryString = null;
            string url = fullUrl;
            var qsObject = new NameValueCollection();
            if (questionMarkIndex != -1)
            {
                url = fullUrl.Substring(0, questionMarkIndex);
                queryString = fullUrl.Substring(questionMarkIndex + 1);
                qsObject = HttpUtility.ParseQueryString(queryString);
            }

            var request = new HttpRequest(null, url, queryString);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var res = new RouteValueDictionary
            {
                {"action", "index"},
                {"controller", "cases"},
            };
            if (routeData != null && routeData.Values != null)
            {
                if (routeData.Values.ContainsKey("action") && routeData.Values.ContainsKey("controller"))
                {
                    res["action"] = routeData.Values["action"];
                    res["controller"] = routeData.Values["controller"];
                }

                if (routeData.Values.ContainsKey("area"))
                {
                    res["area"] = routeData.Values["area"];
                }

                if (routeData.Values.ContainsKey("id") && !String.IsNullOrEmpty(routeData.Values["id"].ToString()))
                {
                    res["id"] = routeData.Values["id"];
                }
            }

            foreach (var key in qsObject.AllKeys)
            {
                res.Add(key, qsObject[key]);
            }

            return res;
        }

        private RouteValueDictionary ExtractCurrentRouteInfo()
        {
            var fullUrl = Request.Url.AbsoluteUri;
            return this.ExtractRouteInfo(fullUrl);
        }

        private RouteValueDictionary ExtractPreviousRouteInfo()
        {
            var fullUrl = Request.UrlReferrer.ToString();
            return this.ExtractRouteInfo(fullUrl);
        }

        private void AddViewDataValues()
        {
            ViewData["Callback"] = "SendToDialogCaseCallback";
            ViewData["Id"] = "divSendToDialogCase";
        }

        private string GetLinkWithHash(string hash, object routeObject, string action = null, string controller = null)
        {
            var prevInfo = this.ExtractCurrentRouteInfo();
            if (string.IsNullOrEmpty(action))
            {
                action = prevInfo["action"].ToString();
            }

            if (string.IsNullOrEmpty(controller))
            {
                controller = prevInfo["controller"].ToString();
            }

            var url = UrlHelper.GenerateUrl(
                null,
                action,
                controller,
                null,
                null,
                hash,
                new RouteValueDictionary(routeObject),
                Url.RouteCollection,
                Url.RequestContext,
                false);
            return url;
        }

        #endregion

        #endregion

        #region GetCustomerDepartments

        private readonly IDictionary<int, IList<Department>> _customerDepartments = new Dictionary<int, IList<Department>>();
        private AdvancedSearchBehavior _advancedSearchBehavior;

        private IList<Department> GetCustomerDepartments(int customerId)
        {
            if (!_customerDepartments.ContainsKey(customerId))
            {
                var departments = _departmentService.GetDepartments(customerId);
                _customerDepartments.Add(customerId, departments);
            }

            return _customerDepartments[customerId];
        }

        private JsonResult GetInvoiceTime(int departmentId, int? ouId = null)
        {
            var d = _departmentService.GetDepartment(departmentId);

            var isDebitRequired =
                d.ChargeMandatory.ToBool() &&
                (d.InvoiceChargeType == InvoiceChargeType.PerDepartment
                    ? d.Charge.ToBool()
                    : _departmentService.CheckIfOUsRequireDebit(departmentId, ouId));

            return Json(new
            {
                ShowInvoice = d.ShowInvoice.ToBool(),
                ChargeMandatory = isDebitRequired,
                Charge = d.Charge,
                ShowInvoiceTime = d.ShowInvoiceTime,
                ShowInvoiceOvertime = d.ShowInvoiceOvertime,
                ShowInvoiceMaterial = d.ShowInvoiceMaterial,
                ShowInvoicePrice = d.ShowInvoicePrice
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion


#if DEBUG

        #region Test Actions 

        [HttpGet]
        public JsonResult GenerateCaseLogFiles(int caseId, int count)
        {
            var customerId = _workContext.Customer.CustomerId;
            var basePath = _masterDataService.GetFilePath(customerId);

            var dummyContent = GenerateDummyFileContent(1);

            var @case = _caseService.GetCaseById(caseId);
            var lastLogNote = @case.Logs.OrderByDescending(l => l.Id).FirstOrDefault();

            if (lastLogNote == null)
                return Json(new { Success = false, Error = "Case doesn't have log notes." }, JsonRequestBehavior.AllowGet);

            try
            {
                foreach (var pos in Enumerable.Range(0, count))
                {
                    var fileName = $"file_{Guid.NewGuid()}";
                    var logFile = new CaseLogFileDto(dummyContent, basePath, fileName, DateTime.UtcNow, lastLogNote.Id, _workContext.User?.UserId, LogFileType.External, null);
                    var path = "";
                    _logFileService.AddFile(logFile, ref path);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GenerateCaseFiles(int caseId, int count)
        {
            var customerId = _workContext.Customer.CustomerId;
            var basePath = _masterDataService.GetFilePath(customerId);

            var dummyContent = GenerateDummyFileContent(1);

            try
            {
                foreach (var pos in Enumerable.Range(0, count))
                {
                    var fileName = $"file_{Guid.NewGuid()}";
                    var caseFile = new CaseFileDto(dummyContent, basePath, fileName, DateTime.UtcNow, caseId, _workContext.User?.UserId);

                    // TODO: log? Is this used?
                    string path = "";
                    _caseFileService.AddFile(caseFile, ref path);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        private byte[] GenerateDummyFileContent(int sizeInMb)
        {
            byte[] data = new byte[sizeInMb * 1024];
            Random rng = new Random();
            rng.NextBytes(data);
            return data;
        }

        #endregion
#endif
    }
}

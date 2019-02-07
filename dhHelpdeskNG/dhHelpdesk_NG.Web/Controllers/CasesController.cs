using DH.Helpdesk.BusinessData.Models.Paging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DH.Helpdesk.BusinessData.Models.ExternalInvoice;
using DH.Helpdesk.BusinessData.Models.Logs;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Common.Enums.Settings;
using DH.Helpdesk.Common.Exceptions;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Web.Areas.Inventory.Models;
using DH.Helpdesk.Web.Models.Invoice;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Domain.Interfaces;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Services.BusinessLogic.Cases;
using DH.Helpdesk.Services.BusinessLogic.Mappers.Grid;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.Web.Common.Models.Case;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.Web.Infrastructure.Logger;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
using DH.Helpdesk.Web.Models.Case;


namespace DH.Helpdesk.Web.Controllers
{
    using DH.Helpdesk.BusinessData.Enums.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.BusinessData.Models.FinishingCause;
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
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions;
    using DH.Helpdesk.Web.Infrastructure.Grid;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.CaseLockMappers;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice;
    using DH.Helpdesk.Web.Infrastructure.Mvc;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Case.ChildCase;
    using DH.Helpdesk.Web.Models.Case.Input;
    using DH.Helpdesk.Web.Models.Case.Output;
    using DH.Helpdesk.Web.Models.CaseLock;
    using DH.Helpdesk.Web.Models.Shared;
    using DH.Helpdesk.Common.Extensions.DateTime;

    using Org.BouncyCastle.Bcpg;

    using DHDomain = DH.Helpdesk.Domain;
    using ParentCaseInfo = DH.Helpdesk.BusinessData.Models.Case.ChidCase.ParentCaseInfo;
    using DH.Helpdesk.Web.Enums;
    using Microsoft.Reporting.WebForms;
    using DH.Helpdesk.BusinessData.Models.ReportService;
    using DH.Helpdesk.Services.Services.Reports;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
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
        private readonly int _defaultMaxRows;
        private readonly int _defaultCaseLockBufferTime;
        private readonly int _defaultExtendCaseLockTime;
        private readonly IWatchDateCalendarService _watchDateCalendarServcie;
        private readonly IReportServiceService _reportServiceService;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly IExternalInvoiceService _externalInvoiceService;
        private readonly ICaseExtraFollowersService _caseExtraFollowersService;
        private readonly ICaseRuleFactory _caseRuleFactory;
        private readonly IOrderService _orderService;
        private readonly IOrderAccountService _orderAccountService;
        private readonly ICaseSectionService _caseSectionService;
        private readonly ICaseDocumentService _caseDocumentService;
        private readonly IExtendedCaseService _extendedCaseService;
        private readonly ISendToDialogModelFactory _sendToDialogModelFactory;

        #endregion

        #region ***Constructor***

        public CasesController(
            DH.Helpdesk.Services.BusinessLogic.Cases.ICaseProcessor caseProcessor,
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
            IWatchDateCalendarService watchDateCalendarServcie,
            ICaseInvoiceSettingsService CaseInvoiceSettingsService,
            ICausingPartService causingPartService,
            IInvoiceArticlesModelFactory invoiceArticlesModelFactory,
            IReportServiceService reportServiceService,
            IUserPermissionsChecker userPermissionsChecker,
            IExternalInvoiceService externalInvoiceService,
            ICaseExtraFollowersService caseExtraFollowersService,
            ICaseRuleFactory caseRuleFactory,
            IOrderService orderService,
            IOrderAccountService orderAccountService,
            ICaseDocumentService caseDocumentService,
            ICaseSectionService caseSectionService,
            IExtendedCaseService extendedCaseService,
            ISendToDialogModelFactory sendToDialogModelFactory)
            : base(masterDataService)
        {
            this._caseProcessor = caseProcessor;
            this._masterDataService = masterDataService;
            this._caseService = caseService;
            this._caseSearchService = caseSearchService;
            this._caseFieldSettingService = caseFieldSettingService;
            this._caseFileService = caseFileService;
            this._caseSettingService = caseSettingService;
            this._caseTypeService = caseTypeService;
                 _caseFollowUpService = caseFollowUpService;
            this._categoryService = categoryService;
            this._computerService = computerService;
            this._countryService = countryService;
            this._currencyService = currencyService;
            this._customerService = customerService;
            this._customerUserService = customerUserService;
            this._departmentService = departmentService;
            this._finishingCauseService = finishingCauseService;
            this._impactService = impactService;
            this._ouService = ouService;
            this._problemService = problemService;
            this._priorityService = priorityService;
            this._productAreaService = productAreaService;
            this._regionService = regionService;
            this._settingService = settingService;
            this._stateSecondaryService = stateSecondaryService;
            this._statusService = statusService;
            this._standardTextService = standardTextService;
            this._supplierService = supplierService;
            this._systemService = systemService;
            this._urgencyService = urgencyService;
            this._userService = userService;
            this._workingGroupService = workingGroupService;
            this._projectService = projectService;
            this._changeService = changeService;
            this._logService = logService;
            this._logFileService = logFileService;
            this._userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
            this._caseSolutionService = caseSolutionService;
            this._emailGroupService = emailGroupService;
            this._emailService = emailService;
            this._languageService = languageService;
            this._globalSettingService = globalSettingService;
            this._workContext = workContext;
            this._caseNotifierModelFactory = caseNotifierModelFactory;
            this._notifierService = notifierService;
            this._invoiceArticleService = invoiceArticleService;
            this._caseSolutionSettingService = caseSolutionSettingService;
            this._caseModelFactory = caseModelFactory;
            this._caseOverviewSettingsService = caseOverviewSettingsService;
            this._gridSettingsService = gridSettingsService;
            this._organizationService = organizationService;
            this._orgJsonService = orgJsonService;
            this._registrationSourceCustomerService = registrationSourceCustomerService;
            this._caseLockService = caseLockService;
            this._watchDateCalendarServcie = watchDateCalendarServcie;
            this._mailTemplateService = mailTemplateService;
            this._defaultMaxRows = 10;
            this._defaultCaseLockBufferTime = 30; // Second
            this._defaultExtendCaseLockTime = 60; // Second
            this._causingPartService = causingPartService;
            this._reportServiceService = reportServiceService;
            this._invoiceArticlesModelFactory = invoiceArticlesModelFactory;
            this._userPermissionsChecker = userPermissionsChecker;
            this._externalInvoiceService = externalInvoiceService;
            this._caseExtraFollowersService = caseExtraFollowersService;
            this._caseRuleFactory = caseRuleFactory;
            this._orderService = orderService;
            this._orderAccountService = orderAccountService;
            this._caseDocumentService = caseDocumentService;
            this._caseSectionService = caseSectionService;
            this._extendedCaseService = extendedCaseService;
            this._sendToDialogModelFactory = sendToDialogModelFactory;
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
                SessionFacade.CurrentCustomer = this._customerService.GetCustomer(customerId.Value);
                SessionFacade.CaseOverviewGridSettings = null;
                if (!keepAdvancedSearch.HasValue || !keepAdvancedSearch.Value)
                    SessionFacade.CurrentAdvancedSearch = null;
            }
            else
            {
                if (SessionFacade.CurrentCustomer == null)
                {
                    SessionFacade.CurrentCustomer = this._customerService.GetCustomer(SessionFacade.CurrentUser.CustomerId);
                    SessionFacade.CaseOverviewGridSettings = null;
                    if (!keepAdvancedSearch.HasValue || !keepAdvancedSearch.Value)
                        SessionFacade.CurrentAdvancedSearch = null;
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
                    this._gridSettingsService.GetForCustomerUserGrid(
                        customerId,
                        SessionFacade.CurrentUser.UserGroupId,
                        userId,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);
            }

            if (SessionFacade.CurrentCaseSearch == null)
                SessionFacade.CurrentCaseSearch = this.InitCaseSearchModel(customerId, userId);

            var m = new JsonCaseIndexViewModel();

            var customerUser = this._customerUserService.GetCustomerUserSettings(customerId, userId);
            m.CaseSearchFilterData = this.CreateCaseSearchFilterData(customerId, SessionFacade.CurrentUser, customerUser, SessionFacade.CurrentCaseSearch);
            m.CaseTemplateTreeButton = this.GetCaseTemplateTreeModel(customerId, userId, CaseSolutionLocationShow.OnCaseOverview);
            this._caseSettingService.GetCaseSettingsWithUser(customerId, userId, SessionFacade.CurrentUser.UserGroupId);

            m.CaseSetting = this.GetCaseSettingModel(customerId, userId);
            m.CaseSearchFilterData.IsAboutEnabled = m.CaseSetting.ColumnSettingModel.CaseFieldSettings.GetIsAboutEnabled();

            var user = this._userService.GetUser(userId);

            SessionFacade.CaseOverviewGridSettings.pageOptions.pageIndex =
                SessionFacade.CurrentCaseSearch.CaseSearchFilter.PageInfo.PageNumber;


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
                                                    }
            };

            return this.View("Index", m);
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
                SessionFacade.CaseOverviewGridSettings = f.IsConnectToParent
                    ? _gridSettingsService.GetForCustomerUserGrid(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id,
                        GridSettingsService.CASE_CONNECTPARENT_GRID_ID)
                    : _gridSettingsService.GetForCustomerUserGrid(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id,
                        GridSettingsService.CASE_OVERVIEW_GRID_ID);
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

            var searchResult = _caseSearchService.Search(
                f,
                m.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.RestrictedCasePermission,
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
                    remainingView = RenderPartialViewToString("CaseRemainingTime", this._caseModelFactory.GetCaseRemainingTimeModel(remainingTimeData, this._workContext));
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
            });
        }

        private IList<Dictionary<string, object>> BuildSearchResultData(IList<CaseSearchResult> caseSearchResults, GridSettingsModel gridSettings, OutputFormatter outputFormatter)
        {
            var data = new List<Dictionary<string, object>>();
            var ids = caseSearchResults.Select(o => o.Id).ToArray();
            var globalSettings = this._globalSettingService.GetGlobalSettings().FirstOrDefault();

            var casesLocks = _caseLockService.GetLockedCasesToOverView(ids, globalSettings, this._defaultCaseLockBufferTime).ToList();
        
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
                    {"isClosed", searchRow.IsUrgent},
                    {"isParent", searchRow.IsParent},
                    {"ParentId", searchRow.ParentId}
                };

                var caseLock = casesLocks.Where(x => x.CaseId == caseId).FirstOrDefault();

                //this specific case is locked
                if (caseLock != null)
                {
                        jsRow.Add("isCaseLocked", true);
                        jsRow.Add("caseLockedIconTitle", $"{caseLock.User.FirstName} {caseLock.User.LastName} ({caseLock.User.UserId})");
                        jsRow.Add("caseLockedIconUrl", $"/Content/icons/{CaseIcon.Locked.CaseIconSrc()}");
                }

                foreach (var col in gridSettings.columnDefs)
                {
                    var searchCol = searchRow.Columns.FirstOrDefault(it => it.Key == col.name);
                    jsRow.Add(col.name, searchCol != null ? outputFormatter.FormatField(searchCol) : string.Empty);
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

            return Json(this._caseService.SaveFavorite(favoriteBM));
        }

        [HttpPost]
        public JsonResult DeleteFavorite(int favoriteId)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
                return Json("Please logout and login again!");

            return Json(this._caseService.DeleteFavorite(favoriteId));
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
                this._caseLockService.UnlockCaseByGUID(new Guid(lockGUID));
            return Json("Success");
        }

        public JsonResult UnLockCaseByCaseId(int caseId)
        {
            this._caseLockService.UnlockCaseByCaseId(caseId);
            return Json("Success");
        }

        public JsonResult IsCaseAvailable(int caseId, DateTime caseChangedTime, string lockGuid)
        {
            var caseLock = this._caseLockService.GetCaseLockOverviewByCaseId(caseId);


            if (caseLock != null && 
                caseLock.LockGUID == new Guid(lockGuid) && 
                caseLock.ExtendedTime >= DateTime.Now)
            {
                // Case still is locked by me
                return Json(true);
            }
            else if (caseLock == null || 
                     (caseLock != null && !(caseLock.ExtendedTime >= DateTime.Now)))
            {
                //case is not locked by me or is not locked at all 
                var curCase = _caseService.GetCaseById(caseId);
                if (curCase != null && curCase.ChangeTime.RoundTick() == caseChangedTime.RoundTick())
                    return Json(true);//case is not updated yet by any other
                else
                    return Json(false);
            }
            else
                return Json(false);
        }


   

        public JsonResult ReExtendCaseLock(string lockGuid, int extendValue)
        {
            return Json(this._caseLockService.ReExtendLockCase(new Guid(lockGuid), extendValue));
        }

        #endregion

        #region --Case Save/Delete--

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult New(CaseEditInput m, int? templateId)
        {
            int caseId = this.Save(m);
            CheckTemplateParameters(templateId, caseId);
            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = m.updateNotifierInformation, activeTab = m.ActiveTab });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectResult NewAndClose(CaseEditInput m, int? templateId, string BackUrl)
        {
            var newChild = false;

            if (m.case_.Id == 0 && m.ParentId != null)
                newChild = true;

            m.ActiveTab = "";

            int caseId = this.Save(m);
            this.CheckTemplateParameters(templateId, caseId);
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
                url = this.GetLinkWithHash(string.Empty, new { customerId = m.case_.Customer_Id }, "Index");
            }

            return this.Redirect(url);
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult NewAndAddCase(CaseEditInput m, int? templateId)
        {
            int caseId = this.Save(m);
            CheckTemplateParameters(templateId, caseId);
            return this.RedirectToAction("new", "cases", new { customerId = m.case_.Customer_Id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult Edit(CaseEditInput m)
        {
            // Save current case
            int caseId = this.Save(m);

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



                    //var splitInput = this.GetCaseInputViewModel(
                    //	userId,
                    //	customerId,
                    //	0,
                    //	caseLockModel,
                    //	customerCaseFieldSettings,
                    //	string.Empty,
                    //	null,
                    //	 m.SplitToCaseSolution_Id.Value,
                    //	null,
                    //	false,
                    //	0, 
                    //	caseId);

                    //var editModel = new CaseEditInput()
                    //{
                    //	CaseSolution_Id = m.SplitToCaseSolution_Id.Value,
                    //	caseFieldSettings = new List<CaseFieldSetting>() ,
                    //	caseLock = new CaseLock(),
                    //	caseLog = splitInput.CaseLog,
                    //	caseMailSetting = splitInput.CaseMailSetting,
                    //	CurrentCaseSolution_Id = m.SplitToCaseSolution_Id.Value,
                    //	case_ = splitInput.case_,
                    //	IndependentChild = true,
                    //	SplitToCaseSolution_Id = splitInput.CaseTemplateSplitToCaseSolutionID,
                    //	ParentId = splitInput.ParentCaseInfo.ParentId
                    //};
                    var identity = global::System.Security.Principal.WindowsIdentity.GetCurrent();
                    var windowsUser = identity != null ? identity.Name : null;
                    var child = this._caseService.Copy(
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

            return this.RedirectToAction("edit", "cases", new { id = caseId, redirectFrom = "save", uni = m.updateNotifierInformation, updateState = false, activeTab = m.ActiveTab });
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectResult EditAndClose(CaseEditInput m, string BackUrl)
        {
            m.ActiveTab = "";
            this.Save(m);
            return string.IsNullOrEmpty(BackUrl) ? this.Redirect(Url.Action("index", "cases", new { customerId = m.case_.Customer_Id })) : this.Redirect(BackUrl);
        }

        [HttpPost]
        [ValidateInput(false)]
        public RedirectToRouteResult EditAndAddCase(CaseEditInput m)
        {
            this.Save(m);
            return this.RedirectToAction("new", "cases", new { customerId = m.case_.Customer_Id });
        }

        [HttpPost]
        public RedirectResult DeleteCase(
            int caseId,
            int customerId,
            int? parentCaseId,
            string backUrl
            )
        {
            var basePath = this._masterDataService.GetFilePath(customerId);
            var caseGuid = this._caseService.Delete(caseId, basePath, parentCaseId);
            this._userTemporaryFilesStorage.ResetCacheForObject(caseGuid.ToString());
            if (parentCaseId.HasValue)
            {
                var parentCase = this._caseService.GetCaseById(parentCaseId.Value);
                if (parentCase.Performer_User_Id == SessionFacade.CurrentUser.Id)
                {
                    var url = this.GetLinkWithHash(ChildCasesHashTab, new { id = parentCaseId.Value }, "Edit");
                    return this.Redirect(url);
                }
            }
            return string.IsNullOrEmpty(backUrl) ? this.Redirect(Url.Action("index", "cases", new { customerId = customerId })) : this.Redirect(backUrl);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int id, int caseId)
        {
            var currentuser = this._userService.GetUser(SessionFacade.CurrentUser.Id);
            string err = "";

            var tmpLog = this._logService.GetLogById(id);
            var logFiles = this._logFileService.FindFileNamesByLogId(id);

            var logFileStr = string.Empty;
            if (logFiles.Any())
            {
                if (currentuser.DeleteAttachedFilePermission != 0)
                {
                    logFileStr = string.Format("{0}{1}", StringTags.LogFile, string.Join(StringTags.Seperator, logFiles.ToArray()));
                }
                else
                {
                    err = Translation.GetCoreTextTranslation("Du kan inte ta bort noteringen, eftersom du saknar behörighet att ta bort bifogade filer") + ".";
                    TempData["PreventError"] = err;
                    return this.RedirectToAction("editlog", "cases", new { id = id, customerId = SessionFacade.CurrentCustomer.Id });
                }
            }

            var c = this._caseService.GetCaseById(caseId);
            var basePath = _masterDataService.GetFilePath(c.Customer_Id);
            var logGuid = this._logService.Delete(id, basePath);
            this._userTemporaryFilesStorage.ResetCacheForObject(logGuid.ToString());

            IDictionary<string, string> errors;
            string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            var logStr = string.Format("{0}{1}{2}{3}{4}{5}",
                                        StringTags.Delete,
                                        StringTags.ExternalLog,
                                        tmpLog.TextExternal,
                                        StringTags.InternalLog,
                                        tmpLog.TextInternal,
                                        logFileStr);

            var extraField = new ExtraFieldCaseHistory { CaseLog = logStr };
            this._caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, CreatedByApplications.Helpdesk5, out errors, string.Empty, extraField);

            return this.RedirectToAction("edit", "cases", new { id = caseId });

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
                        var caseInvoices = this._invoiceArticleService.GetCaseInvoicesWithTimeZone(m.case_.Id, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                        var invoiceArticles = this._invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
                        m.InvoiceModel = new CaseInvoiceModel(customerId.Value, m.case_.Id, invoiceArticles, "", m.CaseKey, m.LogKey);
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

            if (SessionFacade.CurrentUser != null)
            {
                /* Used for Extended Case */
                TempData["Case_Id"] = id;

                var userId = SessionFacade.CurrentUser.Id;

                var caseLockViewModel = GetCaseLockModel(id, userId, true, activeTab);
                
                //todo: check if GetCaseById can be used in model!
                int customerId = moveToCustomerId.HasValue ? moveToCustomerId.Value : _caseService.GetCaseCustomerId(id);

                var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);
                m = this.GetCaseInputViewModel(userId, customerId, id, caseLockViewModel, caseFieldSettings, redirectFrom, backUrl, null, null, updateState);

                m.ActiveTab = (!string.IsNullOrEmpty(caseLockViewModel.ActiveTab) ? caseLockViewModel.ActiveTab : activeTab);
                m.ActiveTab = (m.ActiveTab == "") ? GetActiveTab(m.case_.CaseSolution_Id, id) : activeTab; //Fallback to casesolution

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
                var currentCase = this._caseService.GetCaseById(id);
                if (currentCustomerId != currentCase.Customer_Id)
                    this.InitFilter(currentCase.Customer_Id, false, CasesCustomFilter.None, false, true);

                //Get order
                m.OrderId = _orderService.GetOrder(id)?.Id;

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
                    m.MovedFromCustomerId = m.case_.Customer_Id;
                    m.case_.Customer_Id = moveToCustomerId.Value;
                    m.case_.CaseType_Id = 0;
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
                    var caseInvoices = this._invoiceArticleService.GetCaseInvoicesWithTimeZone(id, TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    var invoiceArticles = this._invoiceArticlesModelFactory.CreateCaseInvoiceArticlesModel(caseInvoices);
                    m.InvoiceModel = new CaseInvoiceModel(m.case_.Customer_Id, m.case_.Id, invoiceArticles, "", m.CaseKey, m.LogKey);
                }

                m.CustomerSettings = this._workContext.Customer.Settings; //current customer settings
                m.CustomerSettings.ModuleCaseInvoice = Convert.ToBoolean(caseCustomerSettings.ModuleCaseInvoice); // TODO FIX

                #region ConnectToParentModel

                m.ConnectToParentModel = new JsonCaseIndexViewModel();
                if (!m.IsItChildCase())
                {
                    var customerUser = this._customerUserService.GetCustomerUserSettings(currentCustomerId, userId);
                    m.ConnectToParentModel.CaseSearchFilterData = this.CreateCaseSearchFilterData(currentCustomerId, SessionFacade.CurrentUser, customerUser, this.InitCaseSearchModel(customerId, userId));

                    //todo:
                    m.ConnectToParentModel.CaseSetting = this.GetCaseSettingModel(currentCustomerId, userId, GridSettingsService.CASE_CONNECTPARENT_GRID_ID, caseFieldSettings);

                    var gridSettings = this._gridSettingsService.GetForCustomerUserGrid(
                        customerId,
                        SessionFacade.CurrentUser.UserGroupId,
                        userId,
                        GridSettingsService.CASE_CONNECTPARENT_GRID_ID);
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
                        }
                    };
                }

                #endregion

            }
            AddViewDataValues();

            // Positive: Send Mail to...
            m.CaseMailSetting.DontSendMailToNotifier = m.CaseMailSetting.DontSendMailToNotifier == false;
            m.IsReturnToCase = retToCase;

            return this.View(m);
        }

        public ActionResult EditLog(int id, int customerId, bool newLog = false, bool editLog = false, bool isCaseReopened = false)
        {
            CaseInputViewModel m = null;

            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                var cu = this._customerUserService.GetCustomerUserSettings(customerId, userId);
                var cs = GetCustomerSettings(customerId);
                var customer = this._customerService.GetCustomer(customerId);
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);

                if (cu != null)
                {
                    m = new CaseInputViewModel();
                    m.CaseLog = this._logService.GetLogById(id);
                    m.LogKey = m.CaseLog.Id.ToString();
                    m.customerUserSetting = cu;
                    m.caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);
                    m.CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);
                    m.CaseSectionModels = _caseSectionService.GetCaseSections(customerId, SessionFacade.CurrentLanguageId);
                    m.finishingCauses = this._finishingCauseService.GetFinishingCausesWithChilds(customerId);
                    m.case_ = this._caseService.GetCaseById(m.CaseLog.CaseId);
                    m.IsCaseReopened = isCaseReopened;
                    var useVD = !string.IsNullOrEmpty(this._masterDataService.GetVirtualDirectoryPath(customerId));
                    var logFiles = _logFileService.GetLogFileNamesByLogId(id);
                    m.LogFilesModel = new FilesModel(id.ToString(), logFiles, useVD);
                    const bool isAddEmpty = true;

                    var responsibleUsersAvailable = this._userService.GetAvailablePerformersOrUserId(customerId, m.case_.CaseResponsibleUser_Id);
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
                    if (m.case_.Department_Id > 0 && m.case_.Department_Id.HasValue)
                    {
                        var d = this._departmentService.GetDepartment(m.case_.Department_Id.Value);
                        if (d != null)
                        {
                            m.ShowInvoiceFields = d.Charge;
                            m.ShowExternalInvoiceFields = d.ShowInvoice;
                            m.TimeRequired = d.ChargeMandatory.ToBool();
                        }
                    }

                    m.CaseLog.SendMailAboutCaseToNotifier = !string.IsNullOrEmpty(m.CaseLog.TextExternal);// customer.CommunicateWithNotifier.ToBool();

                    // check state secondary info
                    m.Disable_SendMailAboutCaseToNotifier = false;
                    if (m.case_.StateSecondary_Id > 0)
                        if (m.case_.StateSecondary != null)
                        {
                            m.Disable_SendMailAboutCaseToNotifier = m.case_.StateSecondary.NoMailToNotifier == 1;
                            if (m.case_.StateSecondary.NoMailToNotifier == 1)
                                m.CaseLog.SendMailAboutCaseToNotifier = false;
                            else
                                m.CaseLog.SendMailAboutCaseToNotifier = true;
                        }

                    m.stateSecondaries = this._stateSecondaryService.GetStateSecondaries(customerId);

                    var acccessToGroups = this._userService.GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, customerId);
                    var deps = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId);

                    // TODO: Should mix CustomerSettings & Setting 
                    m.CustomerSettings = this._workContext.Customer.Settings;
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

                    m.CaseInternalLogAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseInternalLogPermission);
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
            }else
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
                WorkingGroupName = _case.Workinggroup?.WorkingGroupName
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
            var result = this._computerService.SearchComputerUsers(customerId, query, categoryID);

            var ComputerUserSearchRestriction = GetCustomerSettings(customerId).ComputerUserSearchRestriction;
            if (ComputerUserSearchRestriction == 1)
            {
                var departmentIds = this._departmentService.GetDepartmentsByUserPermissions(SessionFacade.CurrentUser.Id, customerId).Select(x => x.Id).ToList();
                //user has no departments checked == access to all departments. TODO: change getdepartmentsbyuserpermissions to actually reflect the "none selected"
                if (departmentIds.Count == 0)
                {
                    departmentIds = this._departmentService.GetDepartments(customerId).Select(x => x.Id).ToList();
                }

                result = this._computerService.SearchComputerUsersByDepartments(customerId, query, departmentIds, categoryID);
            }

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

            var extendedCasePathMask = this._globalSettingService.GetGlobalSettings().FirstOrDefault().ExtendedCasePath;
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
                var userWorkingGroup = this._workingGroupService.GetWorkingGroupsAdmin(customerID, currentUserID)
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

            var models = _caseSearchService.GetUserEmailsForCaseSend(SessionFacade.CurrentCustomer.Id, query, searchScope);
            return Json(new { searchKey = searchKey, result = models });
        }

        [HttpPost]
        public JsonResult Get_User(int id)
        {
            var cu = this._computerService.GetComputerUser(id);
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
            var result = this._computerService.SearchPcNumber(customerId, query);
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
            var result = this._computerService.SearchPcNumberByUserId(customerId, userId);          
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

            var list = this._orgJsonService.GetActiveDepartmentForUserByRegion(id, SessionFacade.CurrentUser.Id, customerId, departmentFilterFormat);
            return this.Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeWorkingGroupFilterUser(int? id, int customerId)
        {
            IList<BusinessData.Models.User.CustomerUserInfo> performersList;
            var customerSettings = GetCustomerSettings(customerId);
            if (customerSettings.DontConnectUserToWorkingGroup == 0 && id > 0)
            {
                performersList = this._userService.GetAvailablePerformersForWorkingGroup(customerId, id);
            }
            else
            {
                performersList = this._userService.GetAvailablePerformersOrUserId(customerId);
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
            var dept = this._departmentService.GetDepartment(departmentId);
            DateTime? res = null;
            if (dept != null && dept.WatchDateCalendar_Id.HasValue)
            {
                res = this._watchDateCalendarServcie.GetClosestDateTo(dept.WatchDateCalendar_Id.Value, DateTime.UtcNow);
            }

            return this.Json(new { result = "success", data = res }, JsonRequestBehavior.AllowGet);
        }

        public int ChangeWorkingGroupSetStateSecondary(int? id)
        {
            int ret = 0;
            if (id.HasValue)
            {
                DHDomain.WorkingGroupEntity w = this._workingGroupService.GetWorkingGroup(id.Value);
                ret = w != null ? w.StateSecondary_Id.HasValue ? w.StateSecondary_Id.Value : 0 : 0;
            }
            return ret;
        }

        public JsonResult GetDepartmentInvoiceParameters(int? departmentId, int? ouId)
        {
            return departmentId.HasValue? GetInvoiceTime(departmentId.Value, ouId) : null;
        }

        public JsonResult ChangeDepartment(int? id, int customerId)
        {
            var list = this._orgJsonService.GetActiveOUForDepartmentAsIdName(id, customerId);
            return this.Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeCountry(int? id, int customerId, int departmentFilterFormat)
        {
            var list = id.HasValue ? this._supplierService.GetSuppliersByCountry(customerId, id.GetValueOrDefault()).Select(x => new { id = x.Id, name = x.Name }) : this._supplierService.GetSuppliers(customerId).Where(x => x.IsActive == 1).Select(x => new { id = x.Id, name = x.Name });
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
                else if(p.LogText != null)
                {
                    ret = p.LogText;
                }else
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
                ret = this._priorityService.GetPriorityIdByImpactAndUrgency(impactId.Value, urgencyId.Value);
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
            var productArea = this._productAreaService.GetProductArea(pId);
            if (productArea != null && productArea.SubProductAreas != null &&
                productArea.SubProductAreas.Where(p => p.IsActive != 0).ToList().Count > 0)
                res = "true";

            return Json(res);
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

        #region --Files--

        [HttpGet]
        public ActionResult Files(string id, string savedFiles)
        {
            //var files = this._caseFileService.GetCaseFiles(int.Parse(id));
            var files = GuidHelper.IsGuid(id)
                                ? this._userTemporaryFilesStorage.FindFileNamesAndDates(id, ModuleName.Cases)
                                : this._caseFileService.FindFileNamesAndDatesByCaseId(int.Parse(id));

            var cfs = MakeCaseFileModel(files, savedFiles);
            var customerId = 0;
            if (!GuidHelper.IsGuid(id))
            {
                customerId = this._caseService.GetCaseById(int.Parse(id)).Customer_Id;
            }

            //
            bool UseVD = false;
            if (customerId != 0 && !string.IsNullOrEmpty(this._masterDataService.GetVirtualDirectoryPath(customerId)))
            {
                UseVD = true;
            }

            var model = new CaseFilesModel(id, cfs.ToArray(), savedFiles, UseVD);
            return this.PartialView("_CaseFiles", model);
        }


        [HttpGet]
        public ActionResult LogFiles(string id, int? caseId = null)
        {
            var files = GuidHelper.IsGuid(id)
                                ? this._userTemporaryFilesStorage.FindFileNames(id, ModuleName.Log)
                                : this._logFileService.FindFileNamesByLogId(int.Parse(id));

            var existingFiles = new List<LogFileModel>();
            if (caseId.HasValue && caseId.Value > 0)
            {
                var exFiles = _logFileService.GetExistingFileNamesByCaseId(caseId.Value);
                existingFiles = exFiles.Select(x => new LogFileModel
                {
                    Name = x.Name,
                    IsExistCaseFile = x.IsExistCaseFile,
                    IsExistLogFile = x.IsExistLogFile,
                    ObjId = x.LogId ?? x.CaseId
                }).ToList();
            }
            existingFiles.AddRange(files.Select(x => new LogFileModel
            {
                Name = x
            }));

            var customerId = 0;

            if (!GuidHelper.IsGuid(id))
            {
                var logCaseId = this._logService.GetLogById(int.Parse(id)).CaseId;
                customerId = this._caseService.GetCaseById(logCaseId).Customer_Id;
            }
            bool UseVD = false;
            if (customerId != 0 && !string.IsNullOrEmpty(this._masterDataService.GetVirtualDirectoryPath(customerId)))
            {
                UseVD = true;
            }

            var model = new FilesModel(id, existingFiles, UseVD);
            return this.PartialView("_CaseLogFiles", model);
        }

        [HttpPost]
        public void UploadCaseFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            int caseId = 0;
            if (GuidHelper.IsGuid(id))
            {
                if (_userTemporaryFilesStorage.FileExists(name, id, ModuleName.Cases))
                {
                    name = DateTime.Now.ToString() + '-' + name;
                }
                _userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Cases);
            }
            else if (Int32.TryParse(id, out caseId))
            {
                if (_caseFileService.FileExists(int.Parse(id), name))
                {
                    name = DateTime.Now.ToString() + '_' + name;
                }
                
                var customerId = _caseService.GetCaseCustomerId(caseId);
                var basePath = _masterDataService.GetFilePath(customerId);

                var caseFileDto = new CaseFileDto(
                                uploadedData,
                                basePath,
                                name,
                                DateTime.Now,
                                int.Parse(id),
                                _workContext.User.UserId);
                _caseFileService.AddFile(caseFileDto);
            }
        }

        [HttpPost]
        public void UploadLogFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                if (this._userTemporaryFilesStorage.FileExists(name, id, ModuleName.Log))
                {
                    //return;
                    //this.userTemporaryFilesStorage.DeleteFile(name, id, ModuleName.Log); 
                    //throw new HttpException((int)HttpStatusCode.Conflict, null); because it take a long time.
                }
                this._userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Log);
            }
        }

        [HttpPost]
        public void DeleteCaseFile(string id, string fileName)
        {
            if (GuidHelper.IsGuid(id))
            {
                _userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Cases);
            }
            else
            {
                var c = _caseService.GetCaseById(int.Parse(id));
                var basePath = _masterDataService.GetFilePath(c.Customer_Id);

                _caseFileService.DeleteByCaseIdAndFileName(int.Parse(id), basePath, fileName.Trim());
                _invoiceArticleService.DeleteFileByCaseId(int.Parse(id), fileName.Trim());

                IDictionary<string, string> errors;
                string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                var extraField = new ExtraFieldCaseHistory { CaseFile = StringTags.Delete + fileName.Trim() };
                _caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser, CreatedByApplications.Helpdesk5, out errors, string.Empty, extraField);
            }
        }

        [HttpPost]
        public void DeleteLogFile(string id, string fileName, int? fileId = null)
        {
            if ( fileId.HasValue)
            {
                if (fileId == 0)
                {
                    if (GuidHelper.IsGuid(id))
                    {
                        this._userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Log);
                    }
                }
                else
                    _logFileService.DeleteByFileIdAndFileName(fileId.Value, fileName.Trim());
            }
            else
            {
                if (GuidHelper.IsGuid(id))
                    this._userTemporaryFilesStorage.DeleteFile(fileName.Trim(), id, ModuleName.Log);
                else
                {
                    var log = this._logService.GetLogById(int.Parse(id));
                    DHDomain.Case c = null;
                    var basePath = string.Empty;
                    if (log != null)
                    {
                        c = this._caseService.GetCaseById(log.CaseId);
                        if (c != null)
                            basePath = _masterDataService.GetFilePath(c.Customer_Id);
                    }

                    this._logFileService.DeleteByLogIdAndFileName(int.Parse(id), basePath, fileName.Trim());

                    IDictionary<string, string> errors;
                    string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    if (c != null)
                    {
                        var extraField = new ExtraFieldCaseHistory {LogFile = StringTags.Delete + fileName.Trim()};
                        this._caseService.SaveCaseHistory(c, SessionFacade.CurrentUser.Id, adUser,
                            CreatedByApplications.Helpdesk5, out errors, string.Empty, extraField);
                    }
                }
            }
        }

        [HttpGet]
        public UnicodeFileContentResult DownloadFile(string id, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(id))
                fileContent = this._userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Cases);
            else
            {
                var c = this._caseService.GetCaseById(int.Parse(id));
                var basePath = string.Empty;
                if (c != null)
                    basePath = _masterDataService.GetFilePath(c.Customer_Id);

                fileContent = this._caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
            }

            return new UnicodeFileContentResult(fileContent, fileName);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)] // do not replace with [HttpGet, HttpHead]
        public ActionResult DownloadLogFile(string id, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(id))
                fileContent = this._userTemporaryFilesStorage.GetFileContent(fileName, id, ModuleName.Log);
            else
            {
                var l = this._logService.GetLogById(int.Parse(id));
                var basePath = string.Empty;
                if (l != null)
                {
                    var c = this._caseService.GetCaseById(l.CaseId);
                    if (c != null)
                        basePath = _masterDataService.GetFilePath(c.Customer_Id);
                }

                try
                {
                    fileContent = this._logFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
                }
                catch (FileNotFoundException e)
                {
                    return HttpNotFound("File not found");
                }
            }

            return new UnicodeFileContentResult(fileContent, fileName);
        }

        private string GetCaseFileNames(int id)
        {
            var files = _caseFileService.FindFileNamesByCaseId(id);
            return string.Join("|", files);
        }

        [HttpPost]
        public ActionResult AttachExistingFile(List<CaseAttachedExFileModel> files, int caseId)
        {
            if (files != null && files.Any())
            {
                var allFiles = files.Select(x => new LogExistingFileModel
                {
                    Name = x.FileName,
                    CaseId = x.CaseId,
                    IsExistLogFile = !x.IsCaseFile,
                    IsExistCaseFile = x.IsCaseFile,
                    LogId = x.LogId
                });
                var success = _logFileService.SaveAttachedExistingLogFiles(allFiles, caseId);

                return Json(new {success});
            }
            return Json(new { success = true }); ;
        }

        [HttpGet]
        public ActionResult GetCaseFilesJS(int caseId)
        {
            var files = _caseFileService.FindFileNamesAndDatesByCaseId(caseId);
            var cfs = MakeCaseFileModel(files, string.Empty);
            var customerId = 0;
            customerId = _caseService.GetCaseById(caseId).Customer_Id;

            bool UseVD = false;
            if (customerId != 0 && !string.IsNullOrEmpty(_masterDataService.GetVirtualDirectoryPath(customerId)))
            {
                UseVD = true;
            }

            var model = new CaseFilesModel(caseId.ToString(), cfs.ToArray(), string.Empty, UseVD);
            return PartialView("_CaseFiles", model);
        }
        
        [HttpGet]
        public ActionResult GetCaseInputModelForLog(int caseId)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;
            var caseLockModel = new CaseLockModel();
            var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);
            var model = this.GetCaseInputViewModel(userId, customerId, caseId, caseLockModel, caseFieldSettings);
            return PartialView("_CaseLog", model);
        }

        [HttpGet]
        public ActionResult GetCaseInputModelForHistory(int caseId)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var userId = SessionFacade.CurrentUser.Id;
            var caseLockModel = new CaseLockModel();
            var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);
            var model = this.GetCaseInputViewModel(userId, customerId, caseId, caseLockModel, caseFieldSettings);
            return PartialView("_CaseHistory", model);
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

            this._customerUserService.UpdateUserCaseSetting(newCaseSetting);
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

            this._gridSettingsService.SaveCaseoviewSettings(
                inputSettings.MapToGridSettingsModel(),
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserGroupId);
            SessionFacade.CaseOverviewGridSettings = null;
        }

        #endregion

        #region --Parent Child Case--

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

            var parentCaseID = caseEditInput.case_.Id;
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

            var parentCaseID = caseEditInput.case_.Id;


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
            var newCase = this._caseService.InitChildCaseFromCase(
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

            //var caseParam = new NewCaseParams
            //{
            //	customerId = SessionFacade.CurrentCustomer.Id,
            //	templateId = m.CaseTemplateSplitToCaseSolutionID,
            //	copyFromCaseId = parentCaseID,
            //	caseLanguageId = SessionFacade.CurrentCaseLanguageId
            //};

            //m.NewModeParams = caseParam;
            //m.IndependentChild = true;

            //AddViewDataValues();

            //// TODO: Why invert? 
            //// Positive: Send Mail to...
            //if (m.CaseMailSetting.DontSendMailToNotifier == false)
            //	m.CaseMailSetting.DontSendMailToNotifier = true;
            //else
            //	m.CaseMailSetting.DontSendMailToNotifier = false;

            

            //return this.View("New", m);
        }

        public ActionResult ConnectToParentCase(int id, int parentCaseId)
        {
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            if (SessionFacade.CurrentUser.CreateCasePermission != 1 || SessionFacade.CurrentUser.CreateSubCasePermission != 1)
            {
                return new RedirectResult("~/Error/Forbidden");
            }

            _caseService.AddParentCase(id, parentCaseId);


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
            var relatedCases = this._caseService.GetCaseRelatedCases(
                                                caseId,
                                                this._workContext.Customer.CustomerId,
                                                userId,
                                                SessionFacade.CurrentUser);

            var model = this._caseModelFactory.GetRelatedCasesModel(relatedCases, this._workContext.Customer.CustomerId, userId);
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

            var model = this._caseModelFactory.GetRelatedCasesFullModel(
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
            var count = this._caseService.GetCaseRelatedCasesCount(
                                                caseId,
                                                this._workContext.Customer.CustomerId,
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
        public JsonResult CalculateSLA(int caseId, int userId, DateTime simulateTime)
        {
            var utcNow = simulateTime;
            var case_ = this._caseService.GetCaseById(caseId);
            if (case_ == null || case_.Id < 1)
                return Json("There is no case info for case id:" + caseId.ToString());

            var user = this._userService.GetUser(userId);
            if (user == null || user.Id < 1)
                return Json("User number is not exist:" + userId.ToString());

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

            var curCustomer = _customerService.GetCustomer(case_.Customer_Id);
            var caseRegTime = DateTime.SpecifyKind(case_.RegTime, DateTimeKind.Utc);

            bool edit = true;
            // get case as it was before edit           
            var curCase = this._caseService.GetCaseHistoryByCaseId(caseId).Where(ch => ch.CreatedDate < simulateTime)
                                                                              .OrderByDescending(x => x.CreatedDate)
                                                                              .FirstOrDefault();
            var oldCase = this._caseService.GetCaseHistoryByCaseId(caseId).Where(ch => ch.CreatedDate < curCase.CreatedDate)
                                                                            .OrderByDescending(x => x.CreatedDate)
                                                                            .FirstOrDefault();
            var externalTime = 0;
            if (edit)
            {
                #region Editing existing case
                //oldCase = this._caseService.GetDetachedCaseById(case_.Id);


                if (curCase == null)
                    return Json("There is no history about case");

                var oldCase_StateSecondary_Id = oldCase.StateSecondary_Id;
                if (oldCase_StateSecondary_Id.HasValue)
                {
                    var caseSubState = this._stateSecondaryService.GetStateSecondary(oldCase_StateSecondary_Id.Value);

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
        }

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

        public JsonResult MoveCaseToExternalCustomer(int caseId, int customerId)
        {
            var userId = this._workContext.User.UserId;
            try
            {
                _caseProcessor.MoveCaseToExternalCustomer(caseId, userId, customerId);
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
 
            return Json(new { Success = true, Location = "/Cases/" });
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
            var userId = this._workContext.User.UserId;

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

        private CaseTemplateTreeModel GetCaseTemplateTreeModel(int customerId, int userId, CaseSolutionLocationShow location)
        {
            var model = new CaseTemplateTreeModel
            {
                CustomerId = customerId,
                CaseTemplateCategoryTree =
                    _caseSolutionService.GetCaseSolutionCategoryTree(customerId, userId, location)
                        .Where(c => c.CaseTemplates == null || (c.CaseTemplates != null && c.CaseTemplates.Any())).ToList()
            };
            
            return model;
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
            var case_ = m.case_;
            var caseLog = m.caseLog;
            var caseMailSetting = m.caseMailSetting;
            var updateNotifierInformation = m.updateNotifierInformation;
            m.caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(case_.Customer_Id);
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
                parentCase = this._caseService.GetCaseById(m.ParentId.Value);
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
            if (caseMailSetting.DontSendMailToNotifier == false)
                caseMailSetting.DontSendMailToNotifier = true;
            else
                caseMailSetting.DontSendMailToNotifier = false;

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
            var customerfieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(case_.Customer_Id);

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString() &&
                                                  fs.ShowOnStartPage == 0).Any())
            {
                case_.PersonsEmail = string.Empty;
            }

            DHDomain.Case oldCase = new DHDomain.Case();
            if (edit)
            {
                #region Editing existing case
                oldCase = this._caseService.GetDetachedCaseById(case_.Id);
                var cu = this._customerUserService.GetCustomerUserSettings(case_.Customer_Id, SessionFacade.CurrentUser.Id);
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
                    var caseSubState = this._stateSecondaryService.GetStateSecondary(oldCase.StateSecondary_Id.Value);

                    // calculating time spent in "inactive" state since last changing every save
                    if (caseSubState.IncludeInCaseStatistics == 0)
                    {
                        var workTimeCalcFactory =
                            new WorkTimeCalculatorFactory(
                                ManualDependencyResolver.Get<IHolidayService>(),
                                curCustomer.WorkingDayStart,
                                curCustomer.WorkingDayEnd,
                                TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                        int[] deptIds = null;
                        if (case_.Department_Id.HasValue)
                        {
                            deptIds = new int[] { case_.Department_Id.Value };
                        }

                        var workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, utcNow, deptIds);
                        case_.ExternalTime = workTimeCalc.CalculateWorkTime(
                            oldCase.ChangeTime,
                            utcNow,
                            oldCase.Department_Id) + oldCase.ExternalTime;


                        workTimeCalcFactory =
                        new WorkTimeCalculatorFactory(
                            ManualDependencyResolver.Get<IHolidayService>(),
                            curCustomer.WorkingDayStart,
                            curCustomer.WorkingDayEnd,
                            TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));

                        deptIds = null;
                        if (case_.Department_Id.HasValue)
                        {
                            deptIds = new int[] { case_.Department_Id.Value };
                        }

                        workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, utcNow, deptIds, customerTimeOffset);
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

            case_.LatestSLACountDate = CalculateLatestSLACountDate(oldCase.StateSecondary_Id, case_.StateSecondary_Id, oldCase.LatestSLACountDate);

            var leadTime = 0;
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
                        caseLog.FinishingDate = DateTime.SpecifyKind(caseLog.FinishingDate.Value, DateTimeKind.Local).ToUniversalTime();
                    }
                }

                case_.FinishingDate = DatesHelper.Max(case_.RegTime, caseLog.FinishingDate.Value);

                var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                    ManualDependencyResolver.Get<IHolidayService>(),
                    curCustomer.WorkingDayStart,
                    curCustomer.WorkingDayEnd,
                    TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                int[] deptIds = null;
                if (case_.Department_Id.HasValue)
                {
                    deptIds = new int[] { case_.Department_Id.Value };
                }

                var workTimeCalc = workTimeCalcFactory.Build(case_.RegTime, case_.FinishingDate.Value, deptIds);
                leadTime = workTimeCalc.CalculateWorkTime(
                    case_.RegTime,
                    case_.FinishingDate.Value.ToUniversalTime(),
                    case_.Department_Id) - case_.ExternalTime;

                case_.LeadTime = leadTime;

                // ActionLeadTime Calc
                if (oldCase != null && oldCase.Id > 0)
                {
                    workTimeCalcFactory = new WorkTimeCalculatorFactory(
                        ManualDependencyResolver.Get<IHolidayService>(),
                        curCustomer.WorkingDayStart,
                        curCustomer.WorkingDayEnd,
                        TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    deptIds = null;
                    if (oldCase.Department_Id.HasValue)
                    {
                        deptIds = new int[] { oldCase.Department_Id.Value };
                    }

                    workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, case_.FinishingDate.Value, deptIds, customerTimeOffset);
                    actionLeadTime = workTimeCalc.CalculateWorkTime(
                        oldCase.ChangeTime,
                        case_.FinishingDate.Value.ToUniversalTime(),
                        oldCase.Department_Id, customerTimeOffset) - actionExternalTime;
                }
            }
            else
            {
                var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                    ManualDependencyResolver.Get<IHolidayService>(),
                    curCustomer.WorkingDayStart,
                    curCustomer.WorkingDayEnd,
                    TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                int[] deptIds = null;
                if (case_.Department_Id.HasValue)
                {
                    deptIds = new int[] { case_.Department_Id.Value };
                }

                var workTimeCalc = workTimeCalcFactory.Build(case_.RegTime, utcNow, deptIds);
                leadTime = workTimeCalc.CalculateWorkTime(
                    case_.RegTime,
                    utcNow.ToUniversalTime(),
                    case_.Department_Id) - case_.ExternalTime;


                // ActionLeadTime Calc                
                if (oldCase != null && oldCase.Id > 0)
                {
                    workTimeCalcFactory = new WorkTimeCalculatorFactory(
                        ManualDependencyResolver.Get<IHolidayService>(),
                        curCustomer.WorkingDayStart,
                        curCustomer.WorkingDayEnd,
                        TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    deptIds = null;
                    if (oldCase.Department_Id.HasValue)
                    {
                        deptIds = new int[] { oldCase.Department_Id.Value };
                    }

                    workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, utcNow, deptIds, customerTimeOffset);
                    actionLeadTime = workTimeCalc.CalculateWorkTime(
                        oldCase.ChangeTime,
                        utcNow.ToUniversalTime(),
                        oldCase.Department_Id, customerTimeOffset) - actionExternalTime;
                }
            }

            var childCasesIds = this._caseService.GetChildCasesFor(case_.Id).Where(it => !it.ClosingDate.HasValue).Select(it => it.Id).ToArray();

            var ei = new CaseExtraInfo()
            {
                CreatedByApp = CreatedByApplications.Helpdesk5,
                LeadTimeForNow = leadTime,
                ActionLeadTime = actionLeadTime,
                ActionExternalTime = actionExternalTime
            };

            // save case and case history
            int caseHistoryId = this._caseService.SaveCase(
                        case_,
                        caseLog,
                        SessionFacade.CurrentUser.Id,
                        this.User.Identity.Name,
                        ei,
                        out errors,
                        parentCase, 
                        m.FollowerUsers);

            if (isItChildCase)
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
                
                var caseNotifier = this._caseNotifierModelFactory.Create(
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

                this._notifierService.UpdateCaseNotifier(caseNotifier);
            }

            // save log
            var temporaryLogFiles = this._userTemporaryFilesStorage.FindFiles(caseLog.LogGuid.ToString(), ModuleName.Log);
            var temporaryExLogFiles = _logFileService.GetExistingFileNamesByCaseId(case_.Id);
            var logFileCount = temporaryLogFiles.Count + temporaryExLogFiles.Count;
            caseLog.CaseId = case_.Id;
            caseLog.CaseHistoryId = caseHistoryId;

            if (!edit)
            {
                /* Create Relationship between Case & ExtendedCase*/
                if (m.ExtendedCaseGuid != Guid.Empty)
                {
                    var exData = _caseService.GetExtendedCaseData(m.ExtendedCaseGuid);
                    _caseService.CreateExtendedCaseRelationship(case_.Id, exData.Id);
                }
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
            }
            else // If edit existing case
            {
                if (m.ExtendedInitiatorGUID.HasValue)
                {
                    var exData = _caseService.GetExtendedCaseData(m.ExtendedInitiatorGUID.Value);
                    _caseService.CheckAndUpdateExtendedCaseSectionData(exData.Id, m.case_.Id, m.case_.Customer_Id, CaseSectionType.Initiator);
                }
                else
                {
                    _caseService.RemoveAllExtendedCaseSectionData(case_.Id, m.case_.Customer_Id, CaseSectionType.Initiator);

                }

                if (m.ExtendedRegardingGUID.HasValue)
                {
                    var exData = _caseService.GetExtendedCaseData(m.ExtendedRegardingGUID.Value);
                    _caseService.CheckAndUpdateExtendedCaseSectionData(exData.Id, m.case_.Id, m.case_.Customer_Id, CaseSectionType.Regarding);
                }
                else
                {
                    _caseService.RemoveAllExtendedCaseSectionData(case_.Id, m.case_.Customer_Id, CaseSectionType.Regarding);

                }
            }

            /* #58573 Check that user have access to write to InternalLogNote */
            bool caseInternalLogAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseInternalLogPermission);

            if (!caseInternalLogAccess)
            {
                m.caseLog.TextInternal = null;
            }


            var orginalInternalLog = caseLog.TextInternal;

            if (caseLog.SendLogToParentChildLog.HasValue && caseLog.SendLogToParentChildLog.Value
                && !string.IsNullOrEmpty(caseLog.TextInternal) && childCasesIds.Length > 0)
            {
                caseLog.TextInternal = string.Format(
                    "[{0}]: {1}",
                    Translation.Get(CaseLog.ParentChildCasesMarker),
                    caseLog.TextInternal);
            }

            if (caseLog.SendLogToParentChildLog.HasValue && caseLog.SendLogToParentChildLog.Value && parentCase != null)
            {
                caseLog.TextInternal = string.Format(
                   "[{0}]: {1}",
                   Translation.Get(CaseLog.ChildParentCasesMarker),
                   caseLog.TextInternal);
            }

            caseLog.Id = this._logService.SaveLog(caseLog, logFileCount, out errors);
            caseLog.TextInternal = orginalInternalLog;

            if (caseLog != null && caseLog.SendLogToParentChildLog.HasValue && caseLog.SendLogToParentChildLog.Value
                && !string.IsNullOrEmpty(caseLog.TextInternal))
            {
                if (parentCase != null)
                {
                    var parentCaseLog = new CaseLog
                    {
                        CaseId = parentCase.Id,
                        UserId = caseLog.UserId,
                        TextInternal = string.Format("[{0} #{1}]: {2}", Translation.Get(CaseLog.ChildCaseMarker), case_.CaseNumber, caseLog.TextInternal)
                    };
                    this.UpdateCaseLogForCase(parentCase, parentCaseLog);
                    
                }

                if (childCasesIds != null && childCasesIds.Length > 0)
                {
                    caseLog.TextInternal = string.Format("[{0}]: {1}", Translation.Get(CaseLog.ParentCaseMarker), caseLog.TextInternal);
                    this._logService.AddParentCaseLogToChildCases(childCasesIds, caseLog);
                }
            }

            var basePath = _masterDataService.GetFilePath(case_.Customer_Id);

            // save case files
            if (!edit)
            {
                var temporaryFiles = this._userTemporaryFilesStorage.FindFiles(case_.CaseGUID.ToString(), ModuleName.Cases);
                var newCaseFiles = temporaryFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, case_.Id, this._workContext.User.UserId)).ToList();
                _caseFileService.AddFiles(newCaseFiles);
            }

            // save log files
            var newLogFiles = temporaryLogFiles.Select(f => new CaseFileDto(f.Content, basePath, f.Name, DateTime.UtcNow, caseLog.Id, this._workContext.User.UserId)).ToList();
            this._logFileService.AddFiles(newLogFiles, temporaryExLogFiles, caseLog.Id);
            var allLogFiles = temporaryExLogFiles.Select(x => new CaseFileDto(basePath, x.Name, x.IsExistCaseFile ? Convert.ToInt32(case_.CaseNumber) : x.LogId.Value, x.IsExistCaseFile)).ToList();
            allLogFiles.AddRange(newLogFiles);

            if (movedFromCustomerId.HasValue)
            {
                var fromBasePath = _masterDataService.GetFilePath(movedFromCustomerId.Value);
                if (!fromBasePath.Equals(basePath, StringComparison.CurrentCultureIgnoreCase))
                {
                    this._caseFileService.MoveCaseFiles(case_.CaseNumber.ToString(), fromBasePath, basePath);
                    this._logFileService.MoveLogFiles(case_.Id, fromBasePath, basePath);
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
            var currentLoggedInUser = this._userService.GetUser(SessionFacade.CurrentUser.Id);

            // send emails
            this._caseService.SendCaseEmail(case_.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase, caseLog, allLogFiles, currentLoggedInUser);

            var actions = this._caseService.CheckBusinessRules(BREventType.OnSaveCase, case_, oldCase);
            if (actions.Any())
                this._caseService.ExecuteBusinessActions(actions, case_, caseLog, userTimeZone, caseHistoryId, basePath, SessionFacade.CurrentLanguageId,
                                                          caseMailSetting, allLogFiles);
            //Unlock Case            
            if (m.caseLock != null && !string.IsNullOrEmpty(m.caseLock.LockGUID))
                this._caseLockService.UnlockCaseByGUID(new Guid(m.caseLock.LockGUID));

            // delete temp folders                
            this._userTemporaryFilesStorage.ResetCacheForObject(case_.CaseGUID.ToString());
            this._userTemporaryFilesStorage.ResetCacheForObject(caseLog.LogGuid.ToString());

            return case_.Id;
        }

        private DateTime? CalculateLatestSLACountDate(int? oldSubStateId, int? newSubStateId, DateTime? oldSLADate)
        {
            DateTime? ret = null;
            /* -1: Blank | 0: Non-Counting | 1: Counting */
            var oldSubStateMode = -1;
            var newSubStateMode = -1;

            if (oldSubStateId.HasValue)
            {
                var oldSubStatus = _stateSecondaryService.GetStateSecondary(oldSubStateId.Value);
                if (oldSubStatus != null)
                    oldSubStateMode = oldSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (newSubStateId.HasValue)
            {
                var newSubStatus = _stateSecondaryService.GetStateSecondary(newSubStateId.Value);
                if (newSubStatus != null)
                    newSubStateMode = newSubStatus.IncludeInCaseStatistics == 0 ? 0 : 1;
            }

            if (oldSubStateMode == -1 && newSubStateMode == -1)
                ret = null;
            else if (oldSubStateMode == -1 && newSubStateMode == 1)
                ret = null;
            else if (oldSubStateMode == 0 && newSubStateMode == 1)
                ret = null;
            else if (oldSubStateMode == 0 && newSubStateMode == -1)
                ret = null;
            else if (oldSubStateMode == -1 && newSubStateMode == 0)
                ret = DateTime.UtcNow;
            else if (oldSubStateMode == 1 && newSubStateMode == 0)
                ret = DateTime.UtcNow;
            else if (oldSubStateMode == 1 && newSubStateMode == -1)
                ret = oldSLADate;
            else if (oldSubStateMode == 1 && newSubStateMode == 1)
                ret = oldSLADate;
            else if (oldSubStateMode == 0 && newSubStateMode == 0)
                ret = oldSLADate;

            return ret;
        }

        private void UpdateCaseLogForCase(Case @case, CaseLog caseLog)
        {
            if (@case == null || caseLog == null)
            {
                throw new ArgumentException("@case or/and caseLog is null");
            }

            IDictionary<string, string> errors;

            var c = this._caseService.GetCaseById(caseLog.CaseId);

            this._logService.AddChildCaseLogToParentCase(c.Id, caseLog);

            // save case and case history
            var ei = new CaseExtraInfo() { CreatedByApp = CreatedByApplications.Helpdesk5, LeadTimeForNow = 0, ActionLeadTime = 0, ActionExternalTime = 0 };
            int caseHistoryId = this._caseService.SaveCase(c, caseLog, SessionFacade.CurrentUser.Id, this.User.Identity.Name, ei, out errors);
            caseLog.CaseHistoryId = caseHistoryId;
        }

        private void NewCaselog(Case @case, CaseLog caseLog)
        {
            if (caseLog == null)
            {
                throw new ArgumentException("caseLog is null");
            }

            IDictionary<string, string> errors;

            var oldCase = this._caseService.GetCaseById(caseLog.CaseId);

            var customer = this._customerService.GetCustomer(oldCase.Customer_Id);
            var customerSetting = GetCustomerSettings(oldCase.Customer_Id);
            var basePath = _masterDataService.GetFilePath(oldCase.Customer_Id);
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            var currentLoggedInUser = this._userService.GetUser(SessionFacade.CurrentUser.Id);

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
                var caseSubState = this._stateSecondaryService.GetStateSecondary(oldCase.StateSecondary_Id.Value);

                // calculating time spent in "inactive" state since last changing every save
                if (caseSubState.IncludeInCaseStatistics == 0)
                {
                    var workTimeCalcFactory =
                        new WorkTimeCalculatorFactory(
                            ManualDependencyResolver.Get<IHolidayService>(),
                            customer.WorkingDayStart,
                            customer.WorkingDayEnd,
                            TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
                    int[] deptIds = null;
                    if (@case.Department_Id.HasValue)
                    {
                        deptIds = new int[] { @case.Department_Id.Value };
                    }

                    var workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, DateTime.UtcNow, deptIds);
                    @case.ExternalTime = workTimeCalc.CalculateWorkTime(
                        oldCase.ChangeTime,
                        DateTime.UtcNow,
                        oldCase.Department_Id) + oldCase.ExternalTime;


                    workTimeCalcFactory =
                    new WorkTimeCalculatorFactory(
                        ManualDependencyResolver.Get<IHolidayService>(),
                        customer.WorkingDayStart,
                        customer.WorkingDayEnd,
                        TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));

                    deptIds = null;
                    if (@case.Department_Id.HasValue)
                    {
                        deptIds = new int[] { @case.Department_Id.Value };
                    }

                    workTimeCalc = workTimeCalcFactory.Build(oldCase.ChangeTime, DateTime.UtcNow, deptIds, customerTimeOffset);
                    actionExternalTime = workTimeCalc.CalculateWorkTime(
                        oldCase.ChangeTime,
                        DateTime.UtcNow,
                        oldCase.Department_Id, customerTimeOffset);

                }
            }

            oldCase.LatestSLACountDate = CalculateLatestSLACountDate(oldCase.StateSecondary_Id, @case.StateSecondary_Id, oldCase.LatestSLACountDate);

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

                var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                    ManualDependencyResolver.Get<IHolidayService>(),
                    customer.WorkingDayStart,
                    customer.WorkingDayEnd,
                    TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
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
                        TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId));
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
            int caseHistoryId = this._caseService.SaveCase(oldCase, caseLog, SessionFacade.CurrentUser.Id, this.User.Identity.Name, ei, out errors);
            caseLog.CaseHistoryId = caseHistoryId;

            //find files for old log
            var logFiles = this._logFileService.GetLogFileNamesByLogId(caseLog.OldLog_Id.Value);

            caseLog.Id = this._logService.SaveLog(caseLog, logFiles.Count, out errors);

            byte[] fileContent;

            var newCaseFiles = new List<CaseFileDto>();
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
                        LogId = caseLog.Id
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
                            IsExistLogFile = file.IsExistLogFile
                        };
                        exFiles.Add(logNoteFile);
                    }
                    else
                    {
                        fileContent = this._logFileService.GetFileContentByIdAndFileName(caseLog.OldLog_Id.Value, basePath, file.Name);
                        var logNoteFile = new CaseFileDto(fileContent, basePath, file.Name, DateTime.UtcNow, caseLog.Id, this._workContext.User.UserId);
                        newCaseFiles.Add(logNoteFile);
                    }
                }
            }
            _logFileService.AddFiles(newCaseFiles, exFiles, caseLog.Id);

            // send emails
            this._caseService.SendCaseEmail(oldCase.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, oldCase, caseLog, null, currentLoggedInUser);
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
            var gs = this._globalSettingService.GetGlobalSettings().FirstOrDefault();

            if (m == null)
            {
                return AccessMode.NoAccess;
            }

            if (SessionFacade.CurrentUser == null)
            {
                return AccessMode.NoAccess;
            }

            if (m.case_ == null)
            {
                return AccessMode.NoAccess;
            }

            if (departmensForUser != null)
            {
                var accessToDepartments = departmensForUser.Select(d => d.Id).ToList();
                if (SessionFacade.CurrentUser.UserGroupId < 3)
                {
                    if (accessToDepartments.Count > 0 && m.case_.Department_Id.HasValue)
                    {
                        if (!accessToDepartments.Contains(m.case_.Department_Id.Value))
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

            if (accessToWorkinggroups != null && m.case_.Id != 0)
            {
                if (SessionFacade.CurrentUser.UserGroupId < 3)
                {
                    if (accessToWorkinggroups.Count > 0 && m.case_.WorkingGroup_Id.HasValue)
                    {
                        var wg = accessToWorkinggroups.FirstOrDefault(w => w.WorkingGroup_Id == m.case_.WorkingGroup_Id.Value);
                        if (wg == null && (gs != null && gs.LockCaseToWorkingGroup == 1))
                        {
                            return temporaryHasAccessToWG? AccessMode.ReadOnly : AccessMode.NoAccess;
                        }

                        if (wg != null && wg.RoleToUWG == 1)
                        {                            
                            return AccessMode.ReadOnly;
                        }
                    }
                }
            }

            if (m.case_.FinishingDate.HasValue)
            {
                return AccessMode.ReadOnly;
            }

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
                var region = this._regionService.GetRegion(fields.RegionId.Value);
                if (region != null && region.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Region_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.CaseTypeId.HasValue)
            {
                var caseType = this._caseTypeService.GetCaseType(fields.CaseTypeId.Value);
                if (caseType != null && caseType.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.ProductAreaId.HasValue)
            {
                var productArea = this._productAreaService.GetProductArea(fields.ProductAreaId.Value);
                if (productArea != null && productArea.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.CategoryId.HasValue)
            {
                var category = this._categoryService.GetCategory(fields.CategoryId.Value, fields.CustomerId);
                if (category != null && category.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Category_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.SupplierId.HasValue)
            {
                var supplier = this._supplierService.GetSupplier(fields.SupplierId.Value);
                if (supplier != null && supplier.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Supplier_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.PriorityId.HasValue)
            {
                var priority = this._priorityService.GetPriority(fields.PriorityId.Value);
                if (priority != null && priority.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Priority_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.StatusId.HasValue)
            {
                var status = this._statusService.GetStatus(fields.StatusId.Value);
                if (status != null && status.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.Status_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.SubStatusId.HasValue)
            {
                var subStatus = this._stateSecondaryService.GetStateSecondary(fields.SubStatusId.Value);
                if (subStatus != null && subStatus.IsActive == 0)
                    ret.Add(string.Format("[{0}]", Translation.Get(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(),
                                            Enums.TranslationSource.CaseTranslation,
                                            fields.CustomerId)));
            }
            if (fields.WorkingGroupId.HasValue)
            {
                var workingGroup = this._workingGroupService.GetWorkingGroup(fields.WorkingGroupId.Value);
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
                    if (!string.IsNullOrEmpty(ids[i].Trim()) && int.Parse(ids[i].Trim()) > 0)
                        depIds.Add(int.Parse(ids[i]));
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
                    if (!string.IsNullOrEmpty(ids[i].Trim()) && int.Parse(ids[i].Trim()) < 0)
                        ouIds.Add(-int.Parse(ids[i]));
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

            //region
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseRegionFilter))
                fd.filterRegion = this._regionService.GetRegions(cusId);

            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseDepartmentFilter))
            {
                const bool IsTakeOnlyActive = false;

                fd.filterDepartment =
                    _departmentService.GetDepartmentsByUserPermissions(userId, cusId, IsTakeOnlyActive);

                if (!fd.filterDepartment.Any())
                {
                    fd.filterDepartment =
                        this._departmentService.GetDepartments(cusId)
                            .Where(
                                d =>
                                d.Region_Id == null
                                || IsTakeOnlyActive == false
                                || (IsTakeOnlyActive && d.Region != null && d.Region.IsActive != 0))
                            .ToList();
                }

                if (fd.customerSetting != null && fd.customerSetting.ShowOUsOnDepartmentFilter != 0)
                    fd.filterDepartment = AddOrganizationUnitsToDepartments(fd.filterDepartment);
            }

            //ärendetyp
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCaseTypeFilter))
            {
                const bool IsTakeOnlyActive = true;
                fd.filterCaseType = this._caseTypeService.GetCaseTypesOverviewWithChildren(cusId, IsTakeOnlyActive);
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
                    //    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, isTakeOnlyActive);                           
                    //else
                    //    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, userId, isTakeOnlyActive);                                     
                }
            }

            //produktonmråde
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseProductAreaFilter))
            {
                fd.filterProductArea = this._productAreaService.GetProductAreasOverviewWithChildren(cusId, isActiveOnly:true);
            }

            //kategori                        
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseCategoryFilter))
            {
                //const bool isTakeOnlyActive = true;
                fd.filterCategory =
                    this._categoryService.GetParentCategoriesWithChildren(cusId, true)
                        .OrderBy(c => Translation.GetMasterDataTranslation(c.Name))
                        .ToList();
            }


            //fd.filterCategory = this._categoryService.GetActiveCategories(cusId);
            //prio
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CasePriorityFilter))
                fd.filterPriority = this._priorityService.GetPriorities(cusId);
            //status
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStatusFilter))
                fd.filterStatus = this._statusService.GetStatuses(cusId);
            //understatus
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseStateSecondaryFilter))
                fd.filterStateSecondary = this._stateSecondaryService.GetStateSecondaries(cusId);

            fd.filterCaseProgress = ObjectExtensions.GetFilterForCases(SessionFacade.CurrentUser.FollowUpPermission, cusId);
            fd.CaseRegistrationDateStartFilter = fd.customerUserSetting.CaseRegistrationDateStartFilter;
            fd.CaseRegistrationDateEndFilter = fd.customerUserSetting.CaseRegistrationDateEndFilter;
            fd.CaseWatchDateStartFilter = fd.customerUserSetting.CaseWatchDateStartFilter;
            fd.CaseWatchDateEndFilter = fd.customerUserSetting.CaseWatchDateEndFilter;
            fd.CaseClosingDateStartFilter = fd.customerUserSetting.CaseClosingDateStartFilter;
            fd.CaseClosingDateEndFilter = fd.customerUserSetting.CaseClosingDateEndFilter;
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseClosingReasonFilter))
            {
                fd.ClosingReasons = this._finishingCauseService.GetFinishingCausesWithChilds(cusId);
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
                const bool IsTakeOnlyActive = true;
                fd.RegisteredByUserList = 
                    this._userService.GetUserOnCases(cusId, IsTakeOnlyActive).MapToSelectList(fd.customerSetting);

                if (!string.IsNullOrEmpty(fd.caseSearchFilter.User))
                {
                    fd.lstfilterUser = fd.caseSearchFilter.User.Split(',').Select(int.Parse).ToArray();
                }
            }

            //ansvarig
            if (!string.IsNullOrWhiteSpace(fd.customerUserSetting.CaseResponsibleFilter))
            {
                fd.ResponsibleUserList = 
                    this._userService.GetAvailablePerformersOrUserId(cusId).Cast<IUserCommon>().MapToSelectList(fd.customerSetting);

                if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserResponsible))
                {
                    fd.lstfilterResponsible = fd.caseSearchFilter.UserResponsible.Split(',').Select(int.Parse).ToArray();
                }
            }

            var performers = this._userService.GetAvailablePerformersOrUserId(cusId);

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
                var allOUsNeeded = this._ouService.GetAllOUs()
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
                var c = this._caseTypeService.GetCaseType(f.CaseType);
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
                        this._productAreaService.GetProductArea(
                            f.ProductArea.ToInt());
                    if (p != null)
                    {
                        f.ParantPath_ProductArea = string.Join(
                            " - ",
                            this._productAreaService.GetParentPath(p.Id, SessionFacade.CurrentCustomer.Id));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(f.Category))
            {
                if (f.Category != "0")
                {
                    var c =
                        this._categoryService.GetCategory(
                            f.Category.ToInt(), SessionFacade.CurrentCustomer.Id);
                    if (c != null)
                    {
                        f.ParantPath_Category = string.Join(
                            " - ",
                            this._categoryService.GetParentPath(c.Id, SessionFacade.CurrentCustomer.Id));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(f.CaseClosingReasonFilter))
            {
                if (f.CaseClosingReasonFilter != "0")
                {
                    var fc =
                        this._finishingCauseService.GetFinishingCause(
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
            SessionFacade.CurrentCaseLanguageId = SessionFacade.CurrentLanguageId;
            var acccessToGroups = this._userService.GetWorkinggroupsForUserAndCustomer(SessionFacade.CurrentUser.Id, customerId);
            var deps = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId);

            var isCreateNewCase = caseId == 0;
            m.CaseLock = caseLocked;

            m.CaseUnlockAccess = 
                _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseUnlockPermission);

            m.MailTemplates = _mailTemplateService.GetCustomMailTemplatesList(customerId).ToList();

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);

            var userHasInvoicePermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InvoicePermission);
			var userHasInventoryViewPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryViewPermission);

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
                m.case_ = this._caseService.GetCaseById(caseId); //todo: check if case has been requested before and can be reused!

                if (m.CurrentCaseSolution == null && m.case_.CurrentCaseSolution_Id.HasValue)
                {
                    m.CurrentCaseSolution = _caseSolutionService.GetCaseSolution(m.case_.CurrentCaseSolution_Id.Value);
                    m.CaseTemplateSplitToCaseSolutionID = m.CurrentCaseSolution.SplitToCaseSolution_Id;
                }


                //TODO: update code to use CAseHistorues instead of m.case.CaseHistory
                m.CaseHistories = _caseService.GetCaseHistories(caseId);

                m.IsFollowUp = _caseFollowUpService.IsCaseFollowUp(SessionFacade.CurrentUser.Id, caseId);


                var editMode = this.EditMode(m, ModuleName.Cases, deps, acccessToGroups);
                if (m.case_.Unread != 0 && updateState && editMode == AccessMode.FullAccess)
                    this._caseService.MarkAsRead(caseId);
                
                //todo: review
                customerId = customerId == 0 ? m.case_.Customer_Id : customerId;
                //SessionFacade.CurrentCaseLanguageId = m.case_.RegLanguage_Id;

                var userLocal_ChangeTime = TimeZoneInfo.ConvertTimeFromUtc(m.case_.ChangeTime, userTimeZone);
                m.ChangeTime = userLocal_ChangeTime;
                m.ExternalInvoices = GetExternalInvoices(caseId);
                var caseFolowerUsers = _caseExtraFollowersService.GetCaseExtraFollowers(caseId);
                m.MapToFollowerUsers(caseFolowerUsers);
            }
            
            
            m.CaseInternalLogAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CaseInternalLogPermission);

            var customerUserSetting = this._customerUserService.GetCustomerUserSettings(customerId, userId);
            if (customerUserSetting == null)
            {
                throw new ArgumentException(string.Format("No customer settings for this customer '{0}' and user '{1}'", customerId, userId));
            }

            var case_ = m.case_;
            var customer = this._customerService.GetCustomer(customerId);
            var customerSetting = GetCustomerSettings(customerId);
            var outputFormatter = new OutputFormatter(customerSetting.IsUserFirstLastNameRepresentation == 1, userTimeZone);
            m.OutFormatter = outputFormatter;
            m.customerUserSetting = customerUserSetting;

            //todo: first
            m.caseFieldSettings = customerFieldSettings;
            m.CaseFieldSettingWithLangauges = this._caseFieldSettingService.GetAllCaseFieldSettingsWithLanguages(customerId, SessionFacade.CurrentLanguageId);
            
            //todo: performance - query
            m.CaseSectionModels = _caseSectionService.GetCaseSections(customerId, SessionFacade.CurrentLanguageId);

            m.DepartmentFilterFormat = customerSetting.DepartmentFilterFormat;
            m.ParantPath_CaseType = ParentPathDefaultValue;
            m.ParantPath_ProductArea = ParentPathDefaultValue;
            m.ParantPath_Category = ParentPathDefaultValue;
            m.ParantPath_OU = ParentPathDefaultValue;
            m.MinWorkingTime = customerSetting.MinRegWorkingTime;
            m.CaseFilesModel = new CaseFilesModel();
            m.LogFilesModel = new FilesModel();
            m.CaseFileNames = GetCaseFileNames(caseId);
            m.LogFileNames = m.CaseFileNames;

            m.ActiveTab = activeTab;

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
                var defaultCategoryId = 0;

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
                    m.case_ = this._caseService.Copy(
                        copyFromCaseId.Value,
                        userId,
                        SessionFacade.CurrentLanguageId,
                        this.Request.GetIpAddress(),
                        CaseRegistrationSource.Administrator,
                        userName);

                    m.MapToFollowerUsers(m.case_.CaseFollowers);
                }
                else if (parentCaseId.HasValue)
                {
                    ParentCaseInfo parentCaseInfo;
                    m.case_ = this._caseService.InitChildCaseFromCase(
                       parentCaseId.Value,
                       userId,
                       this.Request.GetIpAddress(),
                       CaseRegistrationSource.Administrator,
                       userName,
                       out parentCaseInfo);

                    m.ParentCaseInfo = parentCaseInfo.MapBusinessToWebModel(outputFormatter);
                }
                else
                {
                    m.case_ = this._caseService.InitCase(
                        customerId,
                        userId,
                        SessionFacade.CurrentLanguageId,
                        this.Request.GetIpAddress(),
                        CaseRegistrationSource.Administrator,
                        customerSetting,
                        userName);

                    //m.case_ = this._caseService.InitCase(
                    //        customerId,
                    //        userId,
                    //        customer.Language_Id,
                    //        this.Request.GetIpAddress(),
                    //        CaseRegistrationSource.Administrator,
                    //        customerSetting,
                    //        windowsUser);
                }

                //todo: Move to caseService.Initcase() -> _customerRepository.GetCustomerDefaults ?
                var defaultStateSecondary = this._stateSecondaryService.GetDefaultOverview(customerId);
                if (defaultStateSecondary != null)
                {
                    m.case_.StateSecondary_Id = int.Parse(defaultStateSecondary.Value);
                }

                // todo: Set default search category based on case section settings
                // set visibility
                #endregion
            }
            else
            {
                #region Existing case model initialization actions
                m.Logs = this._logService.GetCaseLogOverviews(caseId);
                _finishingCauseService.GetFinishingCausesWithChilds(customerId);

                var useVd = !string.IsNullOrEmpty(this._masterDataService.GetVirtualDirectoryPath(customerId));
                
                var canDelete = (SessionFacade.CurrentUser.DeleteAttachedFilePermission == 1);
                m.SavedFiles = canDelete ? string.Empty : m.CaseFileNames;

                var caseFiles = _caseFileService.GetCaseFiles(caseId, canDelete).OrderBy(x => x.CreatedDate);

                m.CaseFilesModel = new CaseFilesModel(caseId.ToString(), caseFiles.ToArray(), m.SavedFiles, useVd);

                m.CaseAttachedExFiles = 
                    caseFiles.Select(x => new CaseAttachedExFileModel
                                            {
                                                Id = x.Id,
                                                FileName = x.FileName,
                                                IsCaseFile = true,
                                                CaseId = caseId
                                            }).ToList();

                var exLogFiles = _logFileService.GetLogFilesByCaseId(caseId).Select(x => new CaseAttachedExFileModel
                {
                    Id = x.Id,
                    FileName = x.Name,
                    LogId = x.ObjId
                }).ToList();
                m.CaseAttachedExFiles.AddRange(exLogFiles);

                if (m.case_.User_Id.HasValue)
                {
                    m.RegByUser = this._userService.GetUser(m.case_.User_Id.Value);
                }

                if (m.Logs != null)
                {
                    var finishingCauses = this._finishingCauseService.GetFinishingCauseInfos(customerId);
                    var lastLog = m.Logs.FirstOrDefault(); //todo: check if its correct - order
                    if (lastLog != null)
                    {
                        m.FinishingCause = CommonHelper.GetFinishingCauseFullPath(finishingCauses.ToArray(), lastLog.FinishingType);
                    }
                }

                var childCases = this._caseService.GetChildCasesFor(caseId);
                m.ChildCaseViewModel = new ChildCaseViewModel
                {
                    Formatter = outputFormatter,
                    ChildCaseList = childCases
                };
                m.ClosedChildCasesCount = childCases.Count(it => it.ClosingDate != null && !it.Indepandent);
                m.ParentCaseInfo = this._caseService.GetParentInfo(caseId).MapBusinessToWebModel(outputFormatter);
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

            //TODO: reuse case solutions!
            m.CaseTemplateButtons =
                customerCaseSolutions.Where(c => c.ShowInsideCase != 0 && c.ConnectedButton.HasValue && c.ConnectedButton > 0)
                    .Select(c => new CaseTemplateButton()
                    {
                        CaseTemplateId = c.CaseSolutionId,
                        CaseTemplateName = c.Name,
                        ButtonNumber = c.ConnectedButton.Value
                    })
                    .OrderBy(c => c.ButtonNumber)
                    .ToList();

            m.WorkflowSteps =
                _caseSolutionService.GetWorkflowSteps(customerId, m.case_, customerCaseSolutions, isRelatedCase,
                    SessionFacade.CurrentUser, ApplicationType.Helpdesk, templateId);

            m.CaseMailSetting = new CaseMailSetting(
                customer.NewCaseEmailList,
                customer.HelpdeskEmail,
                RequestExtension.GetAbsoluteUrl(),
                customerSetting.DontConnectUserToWorkingGroup);

            m.CaseMailSetting.DontSendMailToNotifier = !customer.CommunicateWithNotifier.ToBool();

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.CaseType_Id))
            {
                const bool takeOnlyActive = true;
                m.caseTypes = this._caseTypeService.GetCaseTypesOverviewWithChildren(customerId, takeOnlyActive).OrderBy(c => Translation.GetMasterDataTranslation(c.Name)).ToList();
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Category_Id))
            {
                m.categories = this._categoryService.GetParentCategoriesWithChildren(customerId, true);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Impact_Id))
            {
                m.impacts = this._impactService.GetImpacts(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Priority_Id))
            {
                m.priorities = this._priorityService.GetPriorities(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.ProductArea_Id))
            {
                m.productAreas = 
                    this._productAreaService.GetTopProductAreasForUserOnCase(customerId, m.case_.ProductArea_Id, SessionFacade.CurrentUser)
                        .OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList();
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Region_Id))
            {
                m.regions = this._regionService.GetRegions(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Status_Id))
            {
                m.statuses = this._statusService.GetStatuses(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.StateSecondary_Id))
            {
                m.stateSecondaries = this._stateSecondaryService.GetStateSecondaries(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Supplier_Id))
            {
                m.suppliers = this._supplierService.GetSuppliers(customerId);
                m.countries = this._countryService.GetCountries(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.System_Id))
            {
                m.systems = this._systemService.GetSystems(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.Urgency_Id))
            {
                m.urgencies = this._urgencyService.GetUrgencies(customerId);
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.CausingPart))
            {
                var causingParts = GetCausingPartsModel(customerId, m.case_.CausingPartId);
                m.causingParts = causingParts;
                //#1
                //m.causingParts = this._causingPartService.GetCausingParts(customerId);
            }

            // "Workging group" field
            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.WorkingGroup_Id))
            {
                var IsTakeOnlyActive = isCreateNewCase;
                m.workingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, IsTakeOnlyActive);
            }

            if (isCreateNewCase && m.workingGroups != null && m.workingGroups.Count > 0)
            {
                var defWorkingGroup = m.workingGroups.Where(it => it.IsDefault == 1).FirstOrDefault();
                if (defWorkingGroup != null)
                {
                    m.case_.WorkingGroup_Id = defWorkingGroup.Id;
                }
            }

            // Set working group and performerId from the case type working group if any for New case only
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

            // "RegistrationSourceCustomer" field
            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.RegistrationSourceCustomer))
            {
                var customerSources =
                    this._registrationSourceCustomerService.GetCustomersActiveRegistrationSources(customerId).ToArray();

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
                m.projects = this._projectService.GetCustomerProjects(customerId);
            }

            if (customerSetting.ModuleChangeManagement == 1)
            {
                m.changes = this._changeService.GetChanges(customerId);
            }

            m.finishingCauses = this._finishingCauseService.GetFinishingCausesWithChilds(customerId);
            m.problems = this._problemService.GetCustomerProblems(customerId, false);
            m.currencies = this._currencyService.GetCurrencies();
            
            m.projects = this._projectService.GetCustomerProjects(customerId);
            m.departments = deps.Any() ? deps :
                GetCustomerDepartments(customerId)
                .Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
                .ToList();

            m.standardTexts = this._standardTextService.GetStandardTexts(customerId);
            m.Languages = this._languageService.GetActiveLanguages();

            var responsibleUsersList = this._userService.GetAvailablePerformersOrUserId(customerId, m.case_.CaseResponsibleUser_Id);
            m.FollowersModel = m.SendToDialogModel =_sendToDialogModelFactory.CreateNewSendToDialogModel(customerId, responsibleUsersList.ToList(), customerSetting, _emailGroupService, _workingGroupService, _emailService);

            m.CaseLog = this._logService.InitCaseLog(SessionFacade.CurrentUser.Id, string.Empty);
            m.CaseKey = m.case_.Id == 0 ? m.case_.CaseGUID.ToString() : m.case_.Id.ToString(global::System.Globalization.CultureInfo.InvariantCulture);
            m.LogKey = m.CaseLog.LogGuid.ToString();

            if (m.case_.Supplier_Id > 0 && m.suppliers != null)
            {
                var sup = m.suppliers.FirstOrDefault(x => x.Id == m.case_.Supplier_Id.GetValueOrDefault());
                m.CountryId = sup?.Country_Id.GetValueOrDefault();
            }
            
            if (caseTemplate != null) 
            {
                #region New case initialize

                if (isCreateNewCase)
                {
                    if (caseTemplate.CaseType_Id.HasValue)
                    {
                        m.case_.CaseType_Id = caseTemplate.CaseType_Id.Value;
                    }

                    if (caseTemplate.SetCurrentUserAsPerformer.ToBool())
                    {
                        m.case_.Performer_User_Id = SessionFacade.CurrentUser.Id;
                    }
                    else
                    {
                        m.case_.Performer_User_Id = caseTemplate.PerformerUser_Id;
                    }

                    if (caseTemplate.Category_Id != null)
                    {
                        m.case_.Category_Id = caseTemplate.Category_Id.Value;
                    }

                    if (caseTemplate.CausingPartId.HasValue)
                    {
                        m.case_.CausingPartId = caseTemplate.CausingPartId.Value;
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
                        m.case_.Supplier_Id = caseTemplate.Supplier_Id.Value;

                    var isCopy = parentCaseId.HasValue;

                    if (!string.IsNullOrEmpty(caseTemplate.ReportedBy))
                        m.case_.ReportedBy = caseTemplate.ReportedBy;

                    if (caseTemplate.Department_Id != null)
                        m.case_.Department_Id = caseTemplate.Department_Id;

                    m.CaseMailSetting.DontSendMailToNotifier = caseTemplate.NoMailToNotifier.ToBool();

                    if (caseTemplate.ProductArea_Id != null)
                        m.case_.ProductArea_Id = caseTemplate.ProductArea_Id;

                    if (caseTemplate.ProductArea_Id.HasValue)
                        m.case_.ProductArea = _productAreaService.GetProductArea(caseTemplate.ProductArea_Id.Value);

                    if (!string.IsNullOrEmpty(caseTemplate.Caption))
                        m.case_.Caption = caseTemplate.Caption;

                    if (!string.IsNullOrEmpty(caseTemplate.Description))
                        m.case_.Description = caseTemplate.Description;

                    if (!string.IsNullOrEmpty(caseTemplate.Miscellaneous))
                        m.case_.Miscellaneous = caseTemplate.Miscellaneous;

                    // Set WORKING GROUP from Case Template:
                    if (caseTemplate.SetCurrentUsersWorkingGroup == 1)
                    {
                        var userDefaultWGId = _userService.GetUserDefaultWorkingGroupId(SessionFacade.CurrentUser.Id, customer.Id);
                        m.case_.WorkingGroup_Id = userDefaultWGId;
                    }
                    else if (caseTemplate.CaseWorkingGroup_Id.HasValue)
                    {
                        m.case_.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id.Value;
                    }
                    
                    if (caseTemplate.Priority_Id != null)
                        m.case_.Priority_Id = caseTemplate.Priority_Id;

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
                        m.case_.Project_Id = caseTemplate.Project_Id;

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
                        m.case_.PersonsName = caseTemplate.PersonsName;

                    if (!string.IsNullOrEmpty(caseTemplate.PersonsEmail))
                        m.case_.PersonsEmail = caseTemplate.PersonsEmail;

                    if (!string.IsNullOrEmpty(caseTemplate.PersonsPhone))
                        m.case_.PersonsPhone = caseTemplate.PersonsPhone;

                    if (caseTemplate.Region_Id.HasValue)
                        m.case_.Region_Id = caseTemplate.Region_Id;

                    if (caseTemplate.OU_Id.HasValue)
                        m.case_.OU_Id = caseTemplate.OU_Id;

                    if (!string.IsNullOrEmpty(caseTemplate.Place))
                        m.case_.Place = caseTemplate.Place;

                    if (!string.IsNullOrEmpty(caseTemplate.UserCode))
                        m.case_.UserCode = caseTemplate.UserCode;

                    if (caseTemplate.Urgency_Id.HasValue)
                        m.case_.Urgency_Id = caseTemplate.Urgency_Id;

                    if (caseTemplate.Impact_Id.HasValue)
                        m.case_.Impact_Id = caseTemplate.Impact_Id;

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryNumber))
                        m.case_.InvoiceNumber = caseTemplate.InvoiceNumber;

                        if (!string.IsNullOrEmpty(caseTemplate.ReferenceNumber))
                            m.case_.ReferenceNumber = caseTemplate.ReferenceNumber;

                    if (caseTemplate.Status_Id.HasValue)
                        m.case_.Status_Id = caseTemplate.Status_Id;

                    if (caseTemplate.StateSecondary_Id.HasValue)
                        m.case_.StateSecondary_Id = caseTemplate.StateSecondary_Id;

                    // TODO: JWE What is "Verfied"?
                    //m.case_.Verified = caseTemplate.Verified;

                    if (!string.IsNullOrEmpty(caseTemplate.VerifiedDescription))
                        m.case_.VerifiedDescription = caseTemplate.VerifiedDescription;

                    if (!string.IsNullOrEmpty(caseTemplate.SolutionRate))
                        m.case_.SolutionRate = caseTemplate.SolutionRate;

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryNumber))
                        m.case_.InventoryNumber = caseTemplate.InventoryNumber;

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryType))
                        m.case_.InventoryType = caseTemplate.InventoryType;

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryLocation))
                        m.case_.InventoryLocation = caseTemplate.InventoryLocation;

                    if (caseTemplate.System_Id.HasValue)
                        m.case_.System_Id = caseTemplate.System_Id;

                    if (!string.IsNullOrEmpty(caseTemplate.Currency))
                        m.case_.Currency = caseTemplate.Currency;

                    if (caseTemplate.Cost != 0)
                        m.case_.Cost = caseTemplate.Cost;

                    if (!string.IsNullOrEmpty(caseTemplate.InventoryNumber))
                        m.case_.OtherCost = caseTemplate.OtherCost;

                    if (caseTemplate.AgreedDate.HasValue)
                        m.case_.AgreedDate = caseTemplate.AgreedDate;

                    if (!string.IsNullOrEmpty(caseTemplate.Available))
                        m.case_.Available = caseTemplate.Available;

                    if (caseTemplate.ContactBeforeAction != 0)
                        m.case_.ContactBeforeAction = caseTemplate.ContactBeforeAction;

                    if (!string.IsNullOrEmpty(caseTemplate.CostCentre))
                        m.case_.CostCentre = caseTemplate.CostCentre;

                    if (!string.IsNullOrEmpty(caseTemplate.PersonsCellPhone))
                        m.case_.PersonsCellphone = caseTemplate.PersonsCellPhone;

                    if (m.case_.IsAbout == null)
                        m.case_.IsAbout = new CaseIsAboutEntity();

                    m.case_.IsAbout.Id = 0;

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_ReportedBy))
                        m.case_.IsAbout.ReportedBy = caseTemplate.IsAbout_ReportedBy;

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_PersonsName))
                        m.case_.IsAbout.Person_Name = caseTemplate.IsAbout_PersonsName;

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_PersonsEmail))
                        m.case_.IsAbout.Person_Email = caseTemplate.IsAbout_PersonsEmail;

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_PersonsPhone))
                        m.case_.IsAbout.Person_Phone = caseTemplate.IsAbout_PersonsPhone;

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_PersonsCellPhone))
                        m.case_.IsAbout.Person_Cellphone = caseTemplate.IsAbout_PersonsCellPhone;

                    if (caseTemplate.IsAbout_Region_Id.HasValue)
                        m.case_.IsAbout.Region_Id = caseTemplate.IsAbout_Region_Id;

                    if (caseTemplate.IsAbout_Department_Id.HasValue)
                        m.case_.IsAbout.Department_Id = caseTemplate.IsAbout_Department_Id;

                    if (!string.IsNullOrEmpty(caseTemplate.Available))
                        m.case_.IsAbout.OU_Id = caseTemplate.IsAbout_OU_Id;

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_CostCentre))
                        m.case_.IsAbout.CostCentre = caseTemplate.IsAbout_CostCentre;

                    if (!string.IsNullOrEmpty(caseTemplate.IsAbout_Place))
                        m.case_.IsAbout.Place = caseTemplate.IsAbout_Place;

                    if (!string.IsNullOrEmpty(caseTemplate.UserCode))
                        m.case_.IsAbout.UserCode = caseTemplate.UserCode;

                    if (caseTemplate.RegistrationSource.HasValue)
                    {
                        m.CustomerRegistrationSourceId = caseTemplate.RegistrationSource.Value;
                        var RegistrationSource = this._registrationSourceCustomerService.GetRegistrationSouceCustomer(caseTemplate.RegistrationSource.Value);
                        m.SelectedCustomerRegistrationSource = RegistrationSource.SourceName;
                    }

                    // "watch date" 
                    if (caseTemplate.WatchDate.HasValue)
                    {
                        m.case_.WatchDate = caseTemplate.WatchDate;
                    }
                    else
                    {
                        if (m.case_.Department_Id.HasValue && m.case_.Priority_Id.HasValue)
                        {
                            var dept = _departmentService.GetDepartment(m.case_.Department_Id.Value);
                            var priority =
                                m.priorities.Where(it => it.Id == m.case_.Priority_Id && it.IsActive == 1).FirstOrDefault();
                            if (dept != null && dept.WatchDateCalendar_Id.HasValue && priority != null && priority.SolutionTime == 0)
                            {
                                m.case_.WatchDate =
                                    _watchDateCalendarServcie.GetClosestDateTo(dept.WatchDateCalendar_Id.Value, DateTime.UtcNow);
                            }
                        }
                    }

                    if (caseTemplate.Project_Id.HasValue)
                        m.case_.Project_Id = caseTemplate.Project_Id;

                    if (caseTemplate.Problem_Id.HasValue)
                        m.case_.Problem_Id = caseTemplate.Problem_Id;

                    if (caseTemplate.Change_Id.HasValue)
                        m.case_.Change_Id = caseTemplate.Change_Id;

                    if (caseTemplate.FinishingDate.HasValue)
                        m.case_.FinishingDate = caseTemplate.FinishingDate;

                    if (!string.IsNullOrEmpty(caseTemplate.FinishingDescription))
                        m.case_.FinishingDescription = caseTemplate.FinishingDescription;

                    if (caseTemplate.PlanDate.HasValue)
                        m.case_.PlanDate = caseTemplate.PlanDate;

                    if (!string.IsNullOrEmpty(caseTemplate.Name))
                        m.CaseTemplateName = caseTemplate.Name;

                    if (caseTemplate.SMS != 0)
                        m.case_.SMS = caseTemplate.SMS;

                    if (caseTemplate.Verified != 0)
                        m.case_.Verified = caseTemplate.Verified;

                    // This is used for hide fields(which are not in casetemplate) in new case input
                    m.templateistrue = templateistrue;

                    var finishingCauses = _finishingCauseService.GetFinishingCauseInfos(customerId);
                    m.FinishingCause = CommonHelper.GetFinishingCauseFullPath(finishingCauses.ToArray(), caseTemplate.FinishingCause_Id);

                    //save case original working group 
                    int? caseWorkingGroupId = m.case_.WorkingGroup_Id;

                    //set working group and performer from CaseType if they have not been set before
                    if (caseTemplate.CaseType_Id.HasValue)
                    {
                        var caseType = _caseTypeService.GetCaseType(caseTemplate.CaseType_Id.Value);
                        if (caseType != null)
                        {
                            if (caseWorkingGroupId == null && caseType.WorkingGroup_Id.HasValue)
                                m.case_.WorkingGroup_Id = caseType.WorkingGroup_Id;

                            if (m.case_.Performer_User_Id == null && caseType.User_Id.HasValue)
                                m.case_.Performer_User_Id = caseType.User_Id.Value;
                        }
                    }

                    //set working group and priority from CaseType if they have not been set before
                    if (caseTemplate.ProductArea_Id.HasValue)
                    {
                        var productArea = _productAreaService.GetProductArea(caseTemplate.ProductArea_Id.Value);
                        if (productArea != null)
                        {
                            if (caseWorkingGroupId == null && productArea.WorkingGroup_Id.HasValue)
                                m.case_.WorkingGroup_Id = productArea.WorkingGroup_Id;

                            if (m.case_.Priority_Id == null && productArea.Priority_Id.HasValue)
                                m.case_.Priority_Id = productArea.Priority_Id.Value;
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

            if (m.case_.ReportedBy != null)
            {
                var reportedByUser = this._computerService.GetComputerUserByUserID(m.case_.ReportedBy);
                if (reportedByUser != null && reportedByUser.ComputerUsersCategoryID.HasValue)
                {
                    m.InitiatorComputerUserCategory = _computerService.GetComputerUserCategoryByID(reportedByUser.ComputerUsersCategoryID.Value);
                    if (m.InitiatorComputerUserCategory != null)
                    {
                        m.InitiatorReadOnly = m.InitiatorComputerUserCategory.IsReadOnly;
                    }
                }
            }

            if (m.case_.IsAbout != null && m.case_.IsAbout.ReportedBy != null)
            {
                var reportedByUser = this._computerService.GetComputerUserByUserID(m.case_.IsAbout.ReportedBy);
                if (reportedByUser != null && reportedByUser.ComputerUsersCategoryID.HasValue)
                {
                    m.RegardingComputerUserCategory = _computerService.GetComputerUserCategoryByID(reportedByUser.ComputerUsersCategoryID.Value);
                    if (m.RegardingComputerUserCategory != null)
                    {
                        m.RegardingReadOnly = m.RegardingComputerUserCategory.IsReadOnly;
                    }
                }
            }

            BusinessData.Models.User.CustomerUserInfo admUser = null;
            if (m.case_.Performer_User_Id.HasValue)
            {
                admUser = this._userService.GetUserInfo(m.case_.Performer_User_Id.Value);
            }

            var performersList = responsibleUsersList;
            if (customerSetting.DontConnectUserToWorkingGroup == 0 && m.case_.WorkingGroup_Id > 0)
            {
                performersList = this._userService.GetAvailablePerformersForWorkingGroup(customerId, m.case_.WorkingGroup_Id);
            }

            if (admUser != null && !performersList.Any(u => u.Id == admUser.Id) )
            {
                performersList.Insert(0, admUser);
            }

            var temporaryUserHasAccessToWG = redirectFrom.ToLower() == "save";
            m.EditMode = EditMode(m, ModuleName.Cases, deps, acccessToGroups, temporaryUserHasAccessToWG);
            if (m.EditMode == AccessMode.FullAccess)
                _logFileService.ClearExistingAttachedFiles(caseId);

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.OU_Id))
            {
                //m.ous = this._ouService.GetOUs(customerId);
                m.ous = this._organizationService.GetOUs(m.case_.Department_Id).ToList();
            }

            if (m.caseFieldSettings.ShowOnPage(TranslationCaseFields.IsAbout_OU_Id))
            {
                if (m.case_.IsAbout != null)
                    m.isaboutous = this._organizationService.GetOUs(m.case_.IsAbout.Department_Id).ToList();
                else
                    m.isaboutous = null;
            }
            
            // hämta parent path för casetype
            if (m.case_.CaseType_Id > 0)
            {
                var caseType = _caseTypeService.GetCaseType(m.case_.CaseType_Id);
                if (caseType != null)
                {
                    caseType = Translation.TranslateCaseType(caseType);
                    m.ParantPath_CaseType = caseType.getCaseTypeParentPath();
                }
            }

            // hämta parent path för productArea 
            m.ProductAreaHasChild = 0;
            if (m.case_.ProductArea_Id.HasValue)
            {
                var p = this._productAreaService.GetProductArea(m.case_.ProductArea_Id.GetValueOrDefault());
                if (p != null)
                {
                    var names =
                        this._productAreaService.GetParentPath(p.Id, customerId).Select(name => Translation.GetMasterDataTranslation(name));
                    m.ParantPath_ProductArea = string.Join(" - ", names);
                    if (p.SubProductAreas != null && p.SubProductAreas.Where(s => s.IsActive != 0).ToList().Count > 0)
                    {
                        m.ProductAreaHasChild = 1;
                    }
                }
            }

            // hämta parent path för Category
            m.CategoryHasChild = 0;
            if (m.case_.Category_Id.HasValue)
            {
                var c = this._categoryService.GetCategory(m.case_.Category_Id.GetValueOrDefault(), customerId);
                if (c != null)
                {
                    if (m.CaseTemplateButtons != null)
                    {
                        var names =
                            this._categoryService.GetParentPath(c.Id, customerId).Select(name => Translation.GetMasterDataTranslation(name));
                        m.ParantPath_Category = string.Join(" - ", names);
                        if (c.SubCategories != null && c.SubCategories.Where(s => s.IsActive != 0).ToList().Count > 0)
                        {
                            m.CategoryHasChild = 1;
                        }
                    }
                }
            }

            // check department info
            m.ShowInvoiceFields = 0;
            if (m.case_.Department_Id > 0 && m.case_.Department_Id.HasValue)
            {
                var d = this._departmentService.GetDepartment(m.case_.Department_Id.Value);
                if (d != null)
                {
                    m.ShowInvoiceFields = d.Charge;
                    m.ShowExternalInvoiceFields = d.ShowInvoice;
                    m.TimeRequired = d.ChargeMandatory.ToBool();
                }
            }

            // check state secondary info
            if (string.IsNullOrEmpty(m.CaseLog.TextExternal))
            {
                m.CaseLog.SendMailAboutCaseToNotifier = false;
            }

            m.Disable_SendMailAboutCaseToNotifier = false;
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
            
            if (isCreateNewCase)
            {
                m.case_.DefaultOwnerWG_Id = null;
                if (m.case_.User_Id.HasValue && m.case_.User_Id != 0)
                {
                    // http://redmine.fastdev.se/issues/10997
                    /*var curUser = _userService.GetUser(m.case_.User_Id);                        

                    if (curUser.Default_WorkingGroup_Id != null)
                        m.case_.DefaultOwnerWG_Id = curUser.Default_WorkingGroup_Id;*/

                    var userDefaultWorkingGroupId = this._userService.GetUserDefaultWorkingGroupId(m.case_.User_Id.Value, m.case_.Customer_Id);
                    if (userDefaultWorkingGroupId.HasValue)
                    {
                        m.case_.DefaultOwnerWG_Id = userDefaultWorkingGroupId;
                    }
                }
            }
            else
            {
                if (m.case_.DefaultOwnerWG_Id.HasValue && m.case_.DefaultOwnerWG_Id.Value > 0)
                {
                    m.CaseOwnerDefaultWorkingGroup = this._workingGroupService.GetWorkingGroup(m.case_.DefaultOwnerWG_Id.Value);
                }
            }

            // TODO: Should mix CustomerSettings & Setting                 
            m.CustomerSettings = this._workContext.Customer.Settings;
            m.Setting = customerSetting;

            // "Responsible"
            m.ResponsibleUser_Id = m.case_.CaseResponsibleUser_Id ?? 0;
            const bool isAddEmpty = true;
            m.ResponsibleUsersAvailable = responsibleUsersList.MapToSelectList(m.Setting, isAddEmpty);

            // "Administrator" (Performer)
            m.Performer_Id = m.case_.Performer_User_Id ?? 0;
            m.Performers = 
                performersList.Where(it => it.IsActive == 1 && (it.Performer == 1 || it.Id == m.Performer_Id)).MapToSelectList(m.Setting, isAddEmpty);

            m.DynamicCase = this._caseService.GetDynamicCase(m.case_.Id);
            if (m.DynamicCase != null)
            {
                var l = m.Languages.Where(x => x.Id == SessionFacade.CurrentLanguageId).FirstOrDefault();

                //ex: unitedkingdom/Hiring/edit/[CaseId]/?UserId=[UserId]&language=[Language]
                m.DynamicCase.FormPath = m.DynamicCase.FormPath
                    .Replace("[CaseId]", m.case_.Id.ToString())
                    .Replace("[UserId]", HttpUtility.UrlEncode(SessionFacade.CurrentUser.UserId.ToString()))
                    .Replace("[ApplicationType]", "HD5")
                    .Replace("[Language]", l.LanguageId);
            }

            var caseSolutionId = (m.case_.CaseSolution_Id != null)
                ? m.case_.CaseSolution_Id.Value
                : templateId ?? 0;

            m.HasExtendedComputerUsers =
                _caseSolutionService.CheckIfExtendedFormExistForSolutionsInCategories(customerId, m.ComputerUserCategories.Select(c => c.Id).ToList());

            #region Extended Case

            try
            {
                string extendedCasePathMask = this._globalSettingService.GetGlobalSettings().FirstOrDefault().ExtendedCasePath;
           
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
                            this._workingGroupService.GetWorkingGroupsAdmin(customerId, SessionFacade.CurrentUser.Id)
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
                    var stateSecondaryId = m?.case_?.StateSecondary_Id ?? 0;
                    
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
                string extendedCasePath = this._globalSettingService.GetGlobalSettings().FirstOrDefault().ExtendedCasePath;

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
                            this._workingGroupService.GetWorkingGroupsAdmin(customerId, SessionFacade.CurrentUser.Id)
                                .OrderByDescending(x => x.WorkingGroupId)
                                .FirstOrDefault();

                        int userWorkingGroupId = 0;

                        if (userWorkingGroup != null)
                        {
                            userWorkingGroupId = userWorkingGroup.WorkingGroupId;
                        }
                        userRole = userWorkingGroupId;
                    }

                    // Load if existing
                    if (m.case_.Id != 0)
                    {
                        m.ExtendedCaseSections = GetExtendedCaseSectionsModel(m.case_, customerId, extendedCasePath, SessionFacade.CurrentUser.UserGUID.ToString(), SessionFacade.CurrentLanguageId, userRole);
                    }
                    else
                    {
                        m.ExtendedCaseSections = new Dictionary<CaseSectionType, ExtendedCaseFormModel>();
                    }

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
                    m.ExtendedCaseSections = new Dictionary<CaseSectionType, ExtendedCaseFormModel>();
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


            m.CaseDocuments = _caseDocumentService.GetCaseDocuments(customerId, m.case_, SessionFacade.CurrentUser, ApplicationType.Helpdesk);

            #endregion

            if (case_ != null)
            {
                m.MapCaseToCaseInputViewModel(case_, userTimeZone);
            }

            m.CaseTemplateTreeButton = GetCaseTemplateTreeModel(customerId, userId, CaseSolutionLocationShow.InsideTheCase);
            m.CasePrintView = new ReportModel(false);
            m.UserHasInvoicePermission = userHasInvoicePermission;
			m.UserHasInventoryViewPermission = userHasInventoryViewPermission;
			m.IsCaseReopened = m.case_.CaseHistories != null && m.case_.CaseHistories.Where(ch => ch.FinishingDate.HasValue).Any();

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

        public bool CheckIfFieldVisible(IList<CaseFieldSetting> caseFieldSettings, TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            var isVisible =
                CaseSolutionSettingModel.IsFieldAlwaysVisible(caseTemplateFieldName) ||
                caseFieldSettings.IsFieldRequiredOrVisible(caseFieldName);

            return isVisible;
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

        private IList<ExtendedCaseFormModel> GetExtendedCaseFormModel(int customerId, int caseId, int caseSolutionId, int stateSecondaryId,
            string extendedCasePathMask, int languageId, int userRole, string userGuid)
        {
            var res = new List<ExtendedCaseFormModel>();

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
            
            var model = new ExtendedCaseFormModel
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

            return new List<ExtendedCaseFormModel>() { model };
        }

        private Dictionary<CaseSectionType, ExtendedCaseFormModel> GetExtendedCaseSectionsModel(Case case_,int customerId, string extendedCasePathMask,
            string userGuid, int languageId, int userWorkingGroupId)
        {
            var caseId = case_.Id;
            var caseStateSecondaryId = case_?.StateSecondary?.StateSecondaryId ?? 0;
            var caseWorkingGroupId = case_?.Workinggroup?.WorkingGroupId ?? 0;

            var extendedCaseFormModels = new List<ExtendedCaseFormModel>();
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

                var exendedForm = new ExtendedCaseFormModel
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

            var res = new Dictionary<CaseSectionType, ExtendedCaseFormModel>();
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
                if(setting == GlobalEnums.TranslationCaseFields.ReportedBy.ToString() ||
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

                } else if (setting == GlobalEnums.TranslationCaseFields.Persons_Name.ToString())
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
                    this._caseOverviewSettingsService.GetSettings(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id),
                caseSettings = this._caseSettingService.GetCaseSettingsWithUser(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId)
            };
            var search = this.InitEmptySearchModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            search.Search.SortBy = sortBy ?? string.Empty;
            search.Search.Ascending = sortByAsc.ToBool();
            search.CaseSearchFilter.CaseProgress = CaseProgressFilter.None;
            var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;
            CaseRemainingTimeData remainingTime;
            CaseAggregateData aggregateData;
            var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id).ToArray();
            searchResult.cases = _caseSearchService.Search(
                search.CaseSearchFilter,
                searchResult.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.RestrictedCasePermission,
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
            var model = this._caseModelFactory.GetRelatedCasesFullModel(cases, userId, caseId, sortBy, sortByAsc.ToBool());

            return model;
        }

        private CaseSearchModel InitCaseSearchModel(int customerId, int userId)
        {
            DHDomain.ISearch s = new DHDomain.Search();
            var f = new CaseSearchFilter();
            var m = new CaseSearchModel();
            var cu = this._customerUserService.GetCustomerUserSettings(customerId, userId);
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
            var favorites = this._caseService.GetMyFavorites(customerId, userId);
            if (favorites.Any())
            {
                foreach (var favorite in favorites)
                    ret.Add(new MyFavoriteFilterJSModel(favorite.Id, favorite.Name, favorite.Fields));
            }
            return ret.OrderBy(f => f.Name).ToList();
        }

        private List<SelectListItem> GetRemainigTimeList(int customerId)
        {
            var curCustomer = this._customerService.GetCustomer(customerId);
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

        private IList<CaseFileModel> MakeCaseFileModel(IList<CaseFileDate> files, string savedFiles)
        {
            var res = new List<CaseFileModel>();
            int i = 0;

            var savedFileList = string.IsNullOrEmpty(savedFiles) ? null : savedFiles.Split('|').ToList();

            foreach (var f in files)
            {
                i++;
                var canDelete = !(savedFileList != null && savedFileList.Contains(f.FileName));
                var cf = new CaseFileModel(i, i, f.FileName, f.FileDate, SessionFacade.CurrentUser.FirstName + " " + SessionFacade.CurrentUser.SurName, canDelete);
                res.Add(cf);
            }

            return res;
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
            var extendedSec = (globalSettings != null && globalSettings.CaseLockExtendTime > 0 ? globalSettings.CaseLockExtendTime : this._defaultExtendCaseLockTime);
            var timerInterval = (globalSettings != null ? globalSettings.CaseLockTimer : 0);
            var bufferTime = (globalSettings != null && globalSettings.CaseLockBufferTime > 0 ? globalSettings.CaseLockBufferTime : this._defaultCaseLockBufferTime);
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
                    this._caseLockService.LockCase(newCaseLock);

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
            var extendedSec = (globalSettings != null && globalSettings.CaseLockExtendTime > 0 ? globalSettings.CaseLockExtendTime : this._defaultExtendCaseLockTime);
            var timerInterval = (globalSettings != null ? globalSettings.CaseLockTimer : 0);
            var bufferTime = (globalSettings != null && globalSettings.CaseLockBufferTime > 0 ? globalSettings.CaseLockBufferTime : this._defaultCaseLockBufferTime);
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
                var allStatus = this._statusService.GetStatuses(customerId);
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
                var allSubStatus = this._stateSecondaryService.GetStateSecondaries(customerId);
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

        private CaseSettingModel GetCaseSettingModel(int customerId, int userId,
            int gridId = GridSettingsService.CASE_OVERVIEW_GRID_ID,
            IList<CaseFieldSetting> customerCaseFieldSettings = null)
        {
            var ret = new CaseSettingModel();
            const bool IsTakeOnlyActive = true;
            ret.CustomerId = customerId;
            ret.UserId = userId;

            var userCaseSettings = _customerUserService.GetUserCaseSettings(customerId, userId);

            var regions = this._regionService.GetRegions(customerId);
            ret.RegionCheck = userCaseSettings.Region != string.Empty;
            ret.Regions = regions;
            ret.SelectedRegion = userCaseSettings.Region;

            var customerSettings = GetCustomerSettings(customerId);

            var departments = this._departmentService.GetDepartmentsByUserPermissions(userId, customerId, IsTakeOnlyActive);
            if (!departments.Any())
            {
                departments =
                    GetCustomerDepartments(customerId)
                        .Where(
                            d =>
                            d.Region_Id == null
                            || IsTakeOnlyActive == false
                            || (IsTakeOnlyActive && d.Region != null && d.Region.IsActive != 0))
                        .ToList();
            }

            //var departments = this._departmentService.GetDepartments(customerId, ActivationStatus.All);
            ret.IsDepartmentChecked = userCaseSettings.Departments != string.Empty;

            if (customerSettings != null && customerSettings.ShowOUsOnDepartmentFilter != 0)
                ret.Departments = AddOrganizationUnitsToDepartments(departments);
            else
                ret.Departments = departments;

            ret.SelectedDepartments = userCaseSettings.Departments;



            ret.RegisteredByCheck = userCaseSettings.RegisteredBy != string.Empty;
            ret.RegisteredByUserList = this._userService.GetUserOnCases(customerId, IsTakeOnlyActive).MapToSelectList(customerSettings);
            if (!string.IsNullOrEmpty(userCaseSettings.RegisteredBy))
            {
                ret.lstRegisterBy = userCaseSettings.RegisteredBy.Split(',').Select(int.Parse).ToArray();
            }

            ret.CaseTypeCheck = userCaseSettings.CaseType != string.Empty;
            ret.CaseTypes = this._caseTypeService.GetCaseTypesOverviewWithChildren(customerId, IsTakeOnlyActive).ToList();
            ret.CaseTypePath = "--";
            int caseType;
            int.TryParse(userCaseSettings.CaseType, out caseType);
            ret.CaseTypeId = caseType;
            if (caseType > 0)
            {
                var ct = this._caseTypeService.GetCaseType(ret.CaseTypeId);
                if (ct != null)
                {
                    ret.CaseTypePath = ct.getCaseTypeParentPath();
                }
            }

            ret.ProductAreas = this._productAreaService.GetProductAreasOverviewWithChildren(customerId);

            ret.ProductAreaPath = "--";

            int pa;
            int.TryParse(userCaseSettings.ProductArea, out pa);
            ret.ProductAreaId = pa;

            if (pa > 0)
            {
                var p = this._productAreaService.GetProductArea(ret.ProductAreaId);
                if (p != null)
                    ret.ProductAreaPath = string.Join(" - ", this._productAreaService.GetParentPath(p.Id, customerId));
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

            var performers = this._userService.GetAvailablePerformersOrUserId(customerId);
            performers.Insert(0, ObjectExtensions.notAssignedPerformer());
            ret.AvailablePerformersList = performers.MapToSelectList(customerSettings);
            if (!string.IsNullOrEmpty(userCaseSettings.Administrators))
            {
                ret.lstAdministrator = userCaseSettings.Administrators.Split(',').Select(int.Parse).ToArray();
            }

            var priorities = _priorityService.GetPriorities(customerId).OrderBy(p => p.Code).ToList();
            ret.PriorityCheck = (userCaseSettings.Priority != string.Empty);
            ret.Priorities = priorities;
            ret.SelectedPriority = userCaseSettings.Priority;

            ret.Categories = 
                this._categoryService.GetParentCategoriesWithChildren(customerId, true).OrderBy(c => Translation.GetMasterDataTranslation(c.Name)).ToList();

            ret.CategoryPath= "--";

            int ca;
            int.TryParse(userCaseSettings.Category, out ca);
            ret.CategoryId = ca;

            if (ca > 0)
            {
                var c = this._categoryService.GetCategory(ret.CategoryId, customerId);
                if (c != null)
                    ret.CategoryPath = string.Join(" - ", this._categoryService.GetParentPath(c.Id, customerId));
            }
            ret.CategoryCheck = (userCaseSettings.Category != string.Empty);

            //var categories = _categoryService.GetActiveCategories(customerId).OrderBy(c => c.Name).ToList();
            //ret.CategoryCheck = (userCaseSettings.Category != string.Empty);
            //ret.Categories = categories;
            //ret.SelectedCategory = userCaseSettings.Category;

            var states = _statusService.GetStatuses(customerId).OrderBy(s => s.Name).ToList();
            ret.StateCheck = (userCaseSettings.State != string.Empty);
            ret.States = states;
            ret.SelectedState = userCaseSettings.State;

            var subStates = _stateSecondaryService.GetStateSecondaries(customerId).OrderBy(s => s.Name).ToList();
            ret.SubStateCheck = (userCaseSettings.SubState != string.Empty);
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
                var fc = this._finishingCauseService.GetFinishingCause(ret.ClosingReasonId);
                if (fc != null)
                {
                    ret.ClosingReasonPath = fc.GetFinishingCauseParentPath();
                }
            }

            ret.ClosingReasonCheck = userCaseSettings.CaseClosingReasonFilter != string.Empty;
            ret.ClosingReasons = this._finishingCauseService.GetFinishingCausesWithChilds(customerId);

            ret.ColumnSettingModel =
                this._caseOverviewSettingsService.GetSettings(
                    customerId,
                    SessionFacade.CurrentUser.UserGroupId, userId,
                    gridId,
                    customerCaseFieldSettings);

            return ret;
        }

        private List<SelectListItem> GetCausingPartsModel(int customerId, int? curCausingPartId)
        {
            var allActiveCausinParts = this._causingPartService.GetActiveParentCausingParts(customerId, curCausingPartId);
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
            var allProductAreas = this._productAreaService.GetTopProductAreasForUser(customerId, SessionFacade.CurrentUser, false);
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
                                        _changeService, _finishingCauseService, _watchDateCalendarServcie, 
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
                    var logFile = new CaseFileDto(dummyContent, basePath, fileName, DateTime.UtcNow, lastLogNote.Id, this._workContext.User?.UserId);
                    this._logFileService.AddFile(logFile);
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
                    var caseFile = new CaseFileDto(dummyContent, basePath, fileName, DateTime.UtcNow, caseId, this._workContext.User?.UserId);
                    _caseFileService.AddFile(caseFile);
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

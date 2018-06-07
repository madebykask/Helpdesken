using System.Web.Mvc;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Services.Services.Feedback;
using DH.Helpdesk.Services.Services.Invoice;
using DH.Helpdesk.Web.Infrastructure.Authentication;
using DH.Helpdesk.Web.Infrastructure.Utilities;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;

namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.Services.BusinessLogic.Accounts;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Services.Services.Concrete.Licenses;
    using DH.Helpdesk.Services.Services.Concrete.Orders;
    using DH.Helpdesk.Services.Services.Concrete.Reports;
    using DH.Helpdesk.Services.Services.Concrete.Users;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Services.Services.Orders;
    using DH.Helpdesk.Services.Services.Reports;
    using DH.Helpdesk.Services.Services.Users;

    using Ninject.Modules;
    using Services.Services.EmployeeService;
    using Services.Services.EmployeeService.Concrete;
    using Services.Services.ExtendedCase;
    using Services.Services.UniversalCase;
    using Services.Services.WebApi;

    public sealed class ServicesModule : NinjectModule
    {
        #region Public Methods and Operators

        public override void Load()
        {
            this.Bind<IChangeService>().To<ChangeService>();
            this.Bind<IFaqService>().To<FaqService>();
            this.Bind<ILinkService>().To<LinkService>();
            this.Bind<INotifierService>().To<NotifierService>();
            this.Bind<IProblemService>().To<ProblemService>();
            this.Bind<IProjectService>().To<ProjectService>();

            this.Bind<IChangeCategoryService>().To<ChangeCategoryService>();
            this.Bind<IChangeGroupService>().To<ChangeGroupService>();
            this.Bind<IChangeImplementationStatusService>().To<ChangeImplementationStatusService>();
            this.Bind<IChangeObjectService>().To<ChangeObjectService>();
            this.Bind<IChangePriorityService>().To<ChangePriorityService>();
            this.Bind<IChangeStatusService>().To<ChangeStatusService>();
            this.Bind<IQestionnaireService>().To<QuestionnaireService>();
            this.Bind<IFeedbackService>().To<FeedbackService>();
            this.Bind<IQestionnaireQuestionService>().To<QuestionnaireQuestionService>();
            this.Bind<IQestionnaireQuestionOptionService>().To<QuestionnaireQuestionOptionService>();
            this.Bind<ICircularService>().To<CircularService>();
            this.Bind<IInventoryService>().To<InventoryService>();
            this.Bind<IAccountService>().To<AccountService>();
            this.Bind<IAccountActivityService>().To<AccountActivityService>();
            this.Bind<IAccountFieldSettingsService>().To<AccountFieldSettingsService>();
            this.Bind<IBuildingService>().To<BuildingService>();
            this.Bind<IBulletinBoardService>().To<BulletinBoardService>();
            this.Bind<ICalendarService>().To<CalendarService>();
            this.Bind<ICaseFieldSettingService>().To<CaseFieldSettingService>();
            this.Bind<ICaseService>().To<CaseService>();
            this.Bind<ICaseSettingsService>().To<CaseSettingsService>();
            this.Bind<ICaseSolutionService>().To<CaseSolutionService>();
            this.Bind<ICaseFileService>().To<CaseFileService>();
            this.Bind<ICaseTypeService>().To<CaseTypeService>();
            this.Bind<ICaseFollowUpService>().To<CaseFollowUpService>();
            this.Bind<ICaseSearchService>().To<CaseSearchService>();
            this.Bind<ICaseSectionService>().To<CaseSectionService>();
            this.Bind<ICategoryService>().To<CategoryService>();
            this.Bind<IChecklistActionService>().To<ChecklistActionService>();
            this.Bind<ICheckListServiceService>().To<CheckListServiceService>();
            this.Bind<IComputerService>().To<ComputerService>();
            this.Bind<IContractCategoryService>().To<ContractCategoryService>();
            this.Bind<IContractService>().To<ContractService>();
            this.Bind<ICountryService>().To<CountryService>();
            this.Bind<ICustomerService>().To<CustomerService>();
            this.Bind<ICurrencyService>().To<CurrencyService>();
            this.Bind<ICustomerUserService>().To<CustomerUserService>();
            this.Bind<IDailyReportService>().To<DailyReportService>();
            this.Bind<IDepartmentService>().To<DepartmentService>();
            this.Bind<IDivisionService>().To<DivisionService>();
            this.Bind<IDocumentService>().To<DocumentService>();
            this.Bind<IDomainService>().To<DomainService>();
            this.Bind<IEmailGroupService>().To<EmailGroupService>();
            this.Bind<IFormService>().To<FormService>();
            this.Bind<IFinishingCauseService>().To<FinishingCauseService>();
            this.Bind<IFloorService>().To<FloorService>();
            this.Bind<IGlobalSettingService>().To<GlobalSettingService>();
            this.Bind<IHolidayService>().To<HolidayService>();
            this.Bind<IImpactService>().To<ImpactService>();
            this.Bind<IInfoService>().To<InfoService>();
            this.Bind<ILanguageService>().To<LanguageService>();
            this.Bind<ILogFileService>().To<LogFileService>();
            this.Bind<ILogService>().To<LogService>();
            this.Bind<IMailTemplateService>().To<MailTemplateService>();
            this.Bind<IMasterDataService>().To<MasterDataService>();
            this.Bind<IOperationLogCategoryService>().To<OperationLogCategoryService>();
            this.Bind<IOperationLogEmailLogService>().To<OperationLogEmailLogService>();
            this.Bind<IOperationObjectService>().To<OperationObjectService>();
            this.Bind<IOrderService>().To<OrderService>();
            this.Bind<IOperationLogService>().To<OperationLogService>();
            this.Bind<IOrderStateService>().To<OrderStateService>();
            this.Bind<IOrderTypeService>().To<OrderTypeService>();
            this.Bind<IOUService>().To<OUService>();
            this.Bind<IPriorityService>().To<PriorityService>();
            this.Bind<IProblemLogService>().To<ProblemLogService>();
            this.Bind<IProductAreaService>().To<ProductAreaService>();
            this.Bind<IProgramService>().To<ProgramService>();
            this.Bind<IRegionService>().To<RegionService>();
            this.Bind<IRoomService>().To<RoomService>();
            this.Bind<ISettingService>().To<SettingService>();
            this.Bind<IStandardTextService>().To<StandardTextService>();
            this.Bind<IStateSecondaryService>().To<StateSecondaryService>();
            this.Bind<IStatusService>().To<StatusService>();
            this.Bind<IStatisticsService>().To<StatisticsService>();
            this.Bind<ISupplierService>().To<SupplierService>();
            this.Bind<ISystemService>().To<SystemService>();
            this.Bind<ITemplateService>().To<TemplateService>();
            this.Bind<ITextTranslationService>().To<TextTranslationService>();
            this.Bind<IUrgencyService>().To<UrgencyService>();
            this.Bind<IUserService>().To<UserService>();
            this.Bind<IWatchDateCalendarService>().To<WatchDateCalendarService>();
            this.Bind<IWorkingGroupService>().To<WorkingGroupService>();
            this.Bind<ICausingPartService>().To<CausingPartService>();
            this.Bind<IEmailService>().To<EmailService>();
            this.Bind<IFeedbackTemplateService>().To<FeedbackTemplateService>();
            this.Bind<IComputerModulesService>().To<ComputerModulesService>();
            this.Bind<IInventorySettingsService>().To<InventorySettingsService>();
            this.Bind<IPlaceService>().To<PlaceService>();
            this.Bind<IOrganizationService>().To<OrganizationService>();
            this.Bind<IInvoiceArticleService>().To<InvoiceArticleService>();
            this.Bind<ICaseSolutionSettingService>().To<CaseSolutionSettingService>();
            this.Bind<ICaseInvoiceSettingsService>().To<CaseInvoiceSettingsService>();
            this.Bind<ICheckListsService>().To<CheckListsService>();
            this.Bind<IUsersPasswordHistoryService>().To<UsersPasswordHistoryService>();
            this.Bind<IRegistrationSourceCustomerService>().To<RegistrationSourceCustomerService>();
            this.Bind<ICaseLockService>().To<CaseLockService>();
            this.Bind<ICaseSolutionConditionService>().To<CaseSolutionConditionService>();

            // Liceneses module services
            this.Bind<IProductsService>().To<ProductsService>();
            this.Bind<ILicensesService>().To<LicensesService>();
            this.Bind<IVendorsService>().To<VendorsService>();
            this.Bind<IManufacturersService>().To<ManufacturersService>();
            this.Bind<IApplicationsService>().To<ApplicationsService>();
            this.Bind<IComputersService>().To<ComputersService>();
            this.Bind<IMailTemplateServiceNew>().To<MailTemplateServiceNew>();

            // Orders module services
            this.Bind<IOrdersService>().To<OrdersService>();
            this.Bind<IOrderFieldSettingsService>().To<OrderFieldSettingsService>();
            this.Bind<IModulesService>().To<ModulesService>();

            this.Bind<IOrderAccountService>().To<OrderAccountService>();
            this.Bind<IOrderAccountSettingsService>().To<OrderAccountSettingsService>();

            this.Bind<IOrderAccountProxyService>().To<OrderAccountProxyService>();
            this.Bind<IOrderAccountSettingsProxyService>().To<OrderAccountSettingsProxyService>();

            this.Bind<IOrderAccountDefaultSettingsCreator>().To<OrderAccountDefaultSettingsCreator>();
            this.Bind<ICaseDocumentService>().To<CaseDocumentService>();

            this.Bind<IConditionService>().To<ConditionService>();
            this.Bind<IGDPROperationsService, IGDPRDataPrivacyAccessService, IGDPRFavoritesService>().To<GDPRService>();
            this.Bind<IGDPRTasksService>().To<GDPRTasksService>();

            // Survey service
            this.Bind<ISurveyService>().To<SurveyService>();

            // Reports module
            this.Bind<IReportService>().To<ReportService>();
            this.Bind<IReportServiceService>().To<ReportServiceService>();

            this.Bind<ILogProgramService>().To<LogProgramService>();

            this.Bind<IBusinessRuleService>().To<BusinessRuleService>();

            //Invoice
            Bind<IExternalInvoiceService>().To<ExternalInvoiceService>();
            Bind<IInvoiceService>().To<InvoiceService>();

            Bind<ICaseExtraFollowersService>().To<CaseExtraFollowersService>();
            Bind<IUniversalCaseService>().To<UniversalCaseService>();
            Bind<IExtendedCaseService>().To<ExtendedCaseService>();
            Bind<IMetaDataService>().To<MetaDataService>();
            Bind<IEmployeeService>().To<EmployeeService>();
            Bind<IWebApiService>().To<WebApiService>();

            Bind<ICaseDiagnosticService>().To<CaseDiagnosticService>().InRequestScope();
        }

        #endregion
    }
}
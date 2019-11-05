using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Services.Services.CaseStatistic;
using DH.Helpdesk.Services.Services.Feedback;
using DH.Helpdesk.Services.Services.Invoice;
using DH.Helpdesk.Web.Infrastructure.Utilities;
using Ninject.Web.Common;

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
            Bind<IChangeService>().To<ChangeService>();
            Bind<IFaqService>().To<FaqService>();
            Bind<ILinkService>().To<LinkService>();
            Bind<INotifierService>().To<NotifierService>();
            Bind<IProblemService>().To<ProblemService>();
            Bind<IProjectService>().To<ProjectService>();

            Bind<IChangeCategoryService>().To<ChangeCategoryService>();
            Bind<IChangeGroupService>().To<ChangeGroupService>();
            Bind<IChangeImplementationStatusService>().To<ChangeImplementationStatusService>();
            Bind<IChangeObjectService>().To<ChangeObjectService>();
            Bind<IChangePriorityService>().To<ChangePriorityService>();
            Bind<IChangeStatusService>().To<ChangeStatusService>();
            Bind<IQestionnaireService>().To<QuestionnaireService>();
            Bind<IFeedbackService>().To<FeedbackService>();
            Bind<IQestionnaireQuestionService>().To<QuestionnaireQuestionService>();
            Bind<IQestionnaireQuestionOptionService>().To<QuestionnaireQuestionOptionService>();
            Bind<ICircularService>().To<CircularService>();
            Bind<IInventoryService>().To<InventoryService>();
            Bind<IAccountService>().To<AccountService>();
            Bind<IAccountActivityService>().To<AccountActivityService>();
            Bind<IAccountFieldSettingsService>().To<AccountFieldSettingsService>();
            Bind<IBuildingService>().To<BuildingService>();
            Bind<IBulletinBoardService>().To<BulletinBoardService>();
            Bind<ICalendarService>().To<CalendarService>();
            Bind<ICaseFieldSettingService>().To<CaseFieldSettingService>();
            Bind<ICaseService>().To<CaseService>();
            Bind<ICaseSettingsService>().To<CaseSettingsService>();
            Bind<ICaseSolutionService>().To<CaseSolutionService>();
            Bind<ICaseFileService>().To<CaseFileService>();
            Bind<ICaseTypeService>().To<CaseTypeService>();
            Bind<ICaseFollowUpService>().To<CaseFollowUpService>();
            Bind<ICaseSearchService>().To<CaseSearchService>();
            Bind<ICaseSectionService>().To<CaseSectionService>();
            Bind<ICategoryService>().To<CategoryService>();
            Bind<IChecklistActionService>().To<ChecklistActionService>();
            Bind<ICheckListServiceService>().To<CheckListServiceService>();
            Bind<IComputerService>().To<ComputerService>();
            Bind<IContractCategoryService>().To<ContractCategoryService>();
            Bind<IContractService>().To<ContractService>();
            Bind<ICountryService>().To<CountryService>();
            Bind<ICustomerService>().To<CustomerService>();
            Bind<ICurrencyService>().To<CurrencyService>();
            Bind<ICustomerUserService>().To<CustomerUserService>();
            Bind<IDailyReportService>().To<DailyReportService>();
            Bind<IDepartmentService>().To<DepartmentService>();
            Bind<IDivisionService>().To<DivisionService>();
            Bind<IDocumentService>().To<DocumentService>();
            Bind<IDomainService>().To<DomainService>();
            Bind<IEmailGroupService>().To<EmailGroupService>();
            Bind<IFormService>().To<FormService>();
            Bind<IFinishingCauseService>().To<FinishingCauseService>();
            Bind<IFloorService>().To<FloorService>();
            Bind<IGlobalSettingService>().To<GlobalSettingService>();
            Bind<IHolidayService>().To<HolidayService>();
            Bind<IImpactService>().To<ImpactService>();
            Bind<IInfoService>().To<InfoService>();
            Bind<ILanguageService>().To<LanguageService>();
            Bind<ILogFileService>().To<LogFileService>();
            Bind<ILogService>().To<LogService>();
            Bind<IMailTemplateService>().To<MailTemplateService>();
            Bind<IMasterDataService>().To<MasterDataService>();
            Bind<IOperationLogCategoryService>().To<OperationLogCategoryService>();
            Bind<IOperationLogEmailLogService>().To<OperationLogEmailLogService>();
            Bind<IOperationObjectService>().To<OperationObjectService>();
            //Bind<IOrderService>().To<OrderService>();
            Bind<IOperationLogService>().To<OperationLogService>();
            Bind<IOrderStateService>().To<OrderStateService>();
            Bind<IOrderTypeService>().To<OrderTypeService>();
            Bind<IOUService>().To<OUService>();
            Bind<IPriorityService>().To<PriorityService>();
            Bind<IProblemLogService>().To<ProblemLogService>();
            Bind<IProductAreaService>().To<ProductAreaService>();
            Bind<IProgramService>().To<ProgramService>();
            Bind<IRegionService>().To<RegionService>();
            Bind<IRoomService>().To<RoomService>();
            Bind<ISettingService>().To<SettingService>();
            Bind<IStandardTextService>().To<StandardTextService>();
            Bind<IStateSecondaryService>().To<StateSecondaryService>();
            Bind<IStatusService>().To<StatusService>();
            Bind<IStatisticsService>().To<StatisticsService>();
            Bind<ISupplierService>().To<SupplierService>();
            Bind<ISystemService>().To<SystemService>();
            Bind<ITemplateService>().To<TemplateService>();
            Bind<ITextTranslationService>().To<TextTranslationService>();
            Bind<IUrgencyService>().To<UrgencyService>();
            Bind<IUserService>().To<UserService>();
            Bind<IUserEmailsSearchService>().To<UserEmailsSearchService>();
            Bind<IWatchDateCalendarService>().To<WatchDateCalendarService>();
            Bind<IWorkingGroupService>().To<WorkingGroupService>();
            Bind<ICausingPartService>().To<CausingPartService>();
            Bind<IEmailService>().To<EmailService>();
            Bind<IFeedbackTemplateService>().To<FeedbackTemplateService>();
            Bind<IComputerModulesService>().To<ComputerModulesService>();
            Bind<IInventorySettingsService>().To<InventorySettingsService>();
            Bind<IPlaceService>().To<PlaceService>();
            Bind<IOrganizationService>().To<OrganizationService>();
			Bind<IOrganizationJsonService>().To<OrganizationJsonService>();
			Bind<IInvoiceArticleService>().To<InvoiceArticleService>();
            Bind<ICaseSolutionSettingService>().To<CaseSolutionSettingService>();
            Bind<ICaseInvoiceSettingsService>().To<CaseInvoiceSettingsService>();
            Bind<ICheckListsService>().To<CheckListsService>();
            Bind<IUsersPasswordHistoryService>().To<UsersPasswordHistoryService>();
            Bind<IRegistrationSourceCustomerService>().To<RegistrationSourceCustomerService>();
            Bind<ICaseLockService>().To<CaseLockService>();
            Bind<ICaseSolutionConditionService>().To<CaseSolutionConditionService>();
            Bind<ISettingsLogic>().To<SettingsLogic>();

            // Liceneses module services
            Bind<IProductsService>().To<ProductsService>();
            Bind<ILicensesService>().To<LicensesService>();
            Bind<IVendorsService>().To<VendorsService>();
            Bind<IManufacturersService>().To<ManufacturersService>();
            Bind<IApplicationsService>().To<ApplicationsService>();
            Bind<IComputersService>().To<ComputersService>();
            Bind<IMailTemplateServiceNew>().To<MailTemplateServiceNew>();

            // Orders module services
            Bind<IOrdersService>().To<OrdersService>();
            Bind<IOrderFieldSettingsService>().To<OrderFieldSettingsService>();
            Bind<IModulesService>().To<ModulesService>();

            Bind<IOrderAccountService>().To<OrderAccountService>();
            Bind<IOrderAccountSettingsService>().To<OrderAccountSettingsService>();

            Bind<IOrderAccountProxyService>().To<OrderAccountProxyService>();
            Bind<IOrderAccountSettingsProxyService>().To<OrderAccountSettingsProxyService>();

            Bind<IOrderAccountDefaultSettingsCreator>().To<OrderAccountDefaultSettingsCreator>();
            Bind<ICaseDocumentService>().To<CaseDocumentService>();

            Bind<IConditionService>().To<ConditionService>();
            Bind<IGDPROperationsService, IGDPRDataPrivacyAccessService, IGDPRFavoritesService>().To<GDPRService>();
            Bind<IGDPRTasksService>().To<GDPRTasksService>();

            // Survey service
            Bind<ISurveyService>().To<SurveyService>();

            // Reports module
            Bind<IReportService>().To<ReportService>();
            Bind<IReportServiceService>().To<ReportServiceService>();

            Bind<ILogProgramService>().To<LogProgramService>();

            Bind<IBusinessRuleService>().To<BusinessRuleService>();

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
            Bind<ICaseStatisticService>().To<CaseStatisticService>().InRequestScope();
            Bind<IMail2TicketService>().To<Mail2TicketService>().InRequestScope();

            // Feature toggle
            Bind<IFeatureToggleService>().To<FeatureToggleService>();

			Bind<IFileViewLogService>().To<FileViewLogService>();

		}

        #endregion
    }
}
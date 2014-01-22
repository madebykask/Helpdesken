[assembly: WebActivator.PreApplicationStartMethod(typeof(dhHelpdesk_NG.Web.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(dhHelpdesk_NG.Web.App_Start.NinjectMVC3), "Stop")]

namespace dhHelpdesk_NG.Web.App_Start
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Data.Repositories.Changes.Concrete;
    using dhHelpdesk_NG.Data.Repositories.Concrete;
    using dhHelpdesk_NG.Data.Repositories.Faq;
    using dhHelpdesk_NG.Data.Repositories.Faq.Concrete;
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete;
    using dhHelpdesk_NG.Data.Repositories.Problem;
    using dhHelpdesk_NG.Data.Repositories.Problem.Concrete;
    using dhHelpdesk_NG.Data.Repositories.Projects;
    using dhHelpdesk_NG.Data.Repositories.Projects.Concrete;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Service.Changes;
    using dhHelpdesk_NG.Service.Changes.Concrete;
    using dhHelpdesk_NG.Service.Concrete;
    using dhHelpdesk_NG.Web.NinjectModules;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Mvc;

    using ModelFactoriesModule = dhHelpdesk_NG.Web.NinjectModules.Faq.ModelFactoriesModule;
    using ToolsModule = dhHelpdesk_NG.Web.NinjectModules.ToolsModule;

    public static class NinjectMVC3
    {
        #region Static Fields

        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        #endregion

        #region Public Methods and Operators

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        #endregion

        #region Methods

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(
                new NinjectModules.Changes.ChangesModule(),
                new NinjectModules.Faq.ModelFactoriesModule(),
                new NinjectModules.Notifiers.ConvertersModule(),
                new NinjectModules.Notifiers.ModelFactoriesModule(),
                new NinjectModules.Notifiers.ToolsModule(),
                new NinjectModules.Changes.DtoFactoriesModule(),
                new NinjectModules.Changes.ModelFactoriesModule(),
                new NinjectModules.Problems.ProblemModule(),
                new RepositoriesModule(),
                new ServicesModule(),
                new ToolsModule());

            RegisterServices(kernel);
            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            // Data Infrastructure
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();

            kernel.Bind<IAccountRepository>().To<AccountRepository>();
            kernel.Bind<IAccountActivityGroupRepository>().To<AccountActivityGroupRepository>();
            kernel.Bind<IAccountActivityRepository>().To<AccountActivityRepository>();
            kernel.Bind<IAccountEMailLogRepository>().To<AccountEMailLogRepository>();
            kernel.Bind<IAccountFieldSettingsRepository>().To<AccountFieldSettingsRepository>();
            kernel.Bind<IAccountTypeRepository>().To<AccountTypeRepository>();
            kernel.Bind<IApplicationRepository>().To<ApplicationRepository>();
            kernel.Bind<IBuildingRepository>().To<BuildingRepository>();
            kernel.Bind<IBulletinBoardRepository>().To<BulletinBoardRepository>();
            kernel.Bind<ICalendarRepository>().To<CalendarRepository>();
            kernel.Bind<ICaseRepository>().To<CaseRepository>();
            kernel.Bind<ICaseFieldSettingLanguageRepository>().To<CaseFieldSettingLanguageRepository>();
            kernel.Bind<ICaseFieldSettingRepository>().To<CaseFieldSettingRepository>();
            kernel.Bind<ICaseFileRepository>().To<CaseFileRepository>();
            kernel.Bind<ICaseHistoryRepository>().To<CaseHistoryRepository>();
            kernel.Bind<ICaseInvoiceRowRepository>().To<CaseInvoiceRowRepository>();
            kernel.Bind<ICaseQuestionCategoryRepository>().To<CaseQuestionCategoryRepository>();
            kernel.Bind<ICaseQuestionHeaderRepository>().To<CaseQuestionHeaderRepository>();
            kernel.Bind<ICaseQuestionRepository>().To<CaseQuestionRepository>();
            kernel.Bind<ICaseSearchRepository>().To<CaseSearchRepository>();
            kernel.Bind<ICaseSettingRepository>().To<CaseSettingRepository>();
            kernel.Bind<ICaseSolutionCategoryRepository>().To<CaseSolutionCategoryRepository>();
            kernel.Bind<ICaseSolutionRepository>().To<CaseSolutionRepository>();
            kernel.Bind<ICaseSolutionScheduleRepository>().To<CaseSolutionScheduleRepository>();
            kernel.Bind<ICaseTypeRepository>().To<CaseTypeRepository>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<IChangeCategoryRepository>().To<ChangeCategoryRepository>();
            kernel.Bind<IChangeEMailLogRepository>().To<ChangeEMailLogRepository>();
            kernel.Bind<IChangeFileRepository>().To<ChangeFileRepository>();
            kernel.Bind<IChangeGroupRepository>().To<ChangeGroupRepository>();
            kernel.Bind<IChangeImplementationStatusRepository>().To<ChangeImplementationStatusRepository>();
            kernel.Bind<IChangeLogRepository>().To<ChangeLogRepository>();
            kernel.Bind<IChangeObjectRepository>().To<ChangeObjectRepository>();
            kernel.Bind<IChangePriorityRepository>().To<ChangePriorityRepository>();
            kernel.Bind<IChangeRepository>().To<ChangeRepository>();
            kernel.Bind<IChangeStatusRepository>().To<ChangeStatusRepository>();
            kernel.Bind<IChecklistActionRepository>().To<ChecklistActionRepository>();
            kernel.Bind<IChecklistRepository>().To<ChecklistRepository>();
            kernel.Bind<IChecklistRowRepository>().To<ChecklistRowRepository>();
            kernel.Bind<IChecklistServiceRepository>().To<ChecklistServiceRepository>();
            kernel.Bind<IComputerFieldSettingsRepository>().To<ComputerFieldSettingsRepository>();
            kernel.Bind<IComputerHistoryRepository>().To<ComputerHistoryRepository>();
            kernel.Bind<IComputerLogRepository>().To<ComputerLogRepository>();
            kernel.Bind<IComputerModelRepository>().To<ComputerModelRepository>();
            kernel.Bind<IComputerRepository>().To<ComputerRepository>();
            kernel.Bind<IComputerTypeRepository>().To<ComputerTypeRepository>();
            kernel.Bind<IComputerUserCustomerUserGroupRepository>().To<ComputerUserCustomerUserGroupRepository>();
            kernel.Bind<IComputerUserLogRepository>().To<ComputerUserLogRepository>();
            kernel.Bind<IComputerUsersBlackListRepository>().To<ComputerUsersBlackListRepository>();
            kernel.Bind<IContractCategoryRepository>().To<ContractCategoryRepository>();
            kernel.Bind<IContractFieldSettingsRepository>().To<ContractFieldSettingsRepository>();
            kernel.Bind<IContractFileRepository>().To<ContractFileRepository>();
            kernel.Bind<IContractHistoryRepository>().To<ContractHistoryRepository>();
            kernel.Bind<IContractLogRepository>().To<ContractLogRepository>();
            kernel.Bind<IContractRepository>().To<ContractRepository>();
            kernel.Bind<ICountryRepository>().To<CountryRepository>();
            kernel.Bind<ICurrencyRepository>().To<CurrencyRepository>();
            kernel.Bind<ICustomerRepository>().To<CustomerRepository>();
            kernel.Bind<ICustomerUserRepository>().To<CustomerUserRepository>();
            kernel.Bind<IDailyReportRepository>().To<DailyReportRepository>();
            kernel.Bind<IDailyReportSubjectRepository>().To<DailyReportSubjectRepository>();
            kernel.Bind<IDepartmentRepository>().To<DepartmentRepository>();
            kernel.Bind<IDepartmentUserRepository>().To<DepartmentUserRepository>();
            kernel.Bind<IDivisionRepository>().To<DivisionRepository>();
            kernel.Bind<IDocumentationRepository>().To<DocumentationRepository>();
            kernel.Bind<IDocumentCategoryRepository>().To<DocumentCategoryRepository>();
            kernel.Bind<IDocumentRepository>().To<DocumentRepository>();
            kernel.Bind<IDomainRepository>().To<DomainRepository>();
            kernel.Bind<IEMailGroupRepository>().To<EMailGroupRepository>();
            kernel.Bind<IEmailLogRepository>().To<EmailLogRepository>();
            kernel.Bind<IEmploymentTypeRepository>().To<EmploymentTypeRepository>();
            kernel.Bind<IFaqCategoryLanguageRepository>().To<FaqCategoryLanguageRepository>();
            kernel.Bind<IFaqCategoryRepository>().To<FaqCategoryRepository>();
            kernel.Bind<IFaqFileRepository>().To<FaqFileRepository>();
            kernel.Bind<IFAQLanguageRepository>().To<FAQLanguageRepository>();
            kernel.Bind<IFaqRepository>().To<FaqRepository>();
            kernel.Bind<IFinishingCauseCategoryRepository>().To<FinishingCauseCategoryRepository>();
            kernel.Bind<IFinishingCauseRepository>().To<FinishingCauseRepository>();
            kernel.Bind<IFloorRepository>().To<FloorRepository>();
            kernel.Bind<IFormFieldRepository>().To<FormFieldRepository>();
            kernel.Bind<IFormRepository>().To<FormRepository>();
            kernel.Bind<IGlobalSettingRepository>().To<GlobalSettingRepository>();
            kernel.Bind<IHolidayRepository>().To<HolidayRepository>();
            kernel.Bind<IHolidayHeaderRepository>().To<HolidayHeaderRepository>();
            kernel.Bind<IImpactRepository>().To<ImpactRepository>();
            kernel.Bind<IInfoTextRepository>().To<InfoTextRepository>();
            kernel.Bind<IInventoryRepository>().To<InventoryRepository>();
            kernel.Bind<IInventoryTypePropertyRepository>().To<InventoryTypePropertyRepository>();
            kernel.Bind<IInventoryTypePropertyValueRepository>().To<InventoryTypePropertyValueRepository>();
            kernel.Bind<IInventoryTypeRepository>().To<InventoryTypeRepository>();
            kernel.Bind<IInvoiceHeaderRepository>().To<InvoiceHeaderRepository>();
            kernel.Bind<IInvoiceRowRepository>().To<InvoiceRowRepository>();
            kernel.Bind<ILanguageRepository>().To<LanguageRepository>();
            kernel.Bind<ILicenseFileRepository>().To<LicenseFileRepository>();
            kernel.Bind<ILicenseRepository>().To<LicenseRepository>();
            kernel.Bind<ILinkRepository>().To<LinkRepository>();
            kernel.Bind<ILocalAdminRepository>().To<LocalAdminRepository>();
            kernel.Bind<ILogFileRepository>().To<LogFileRepository>();
            kernel.Bind<ILogicalDriveRepository>().To<LogicalDriveRepository>();
            kernel.Bind<ILogProgramRepository>().To<LogProgramRepository>();
            kernel.Bind<ILogRepository>().To<LogRepository>();
            kernel.Bind<ILogSyncRepository>().To<LogSyncRepository>();
            kernel.Bind<IMailTemplateIdentifierRepository>().To<MailTemplateIdentifierRepository>();
            kernel.Bind<IMailTemplateRepository>().To<MailTemplateRepository>();
            kernel.Bind<IMailTemplateLanguageRepository>().To<MailTemplateLanguageRepository>();
            kernel.Bind<IManufacturerRepository>().To<ManufacturerRepository>();
            kernel.Bind<INICRepository>().To<NICRepository>();
            kernel.Bind<IOperatingSystemRepository>().To<OperatingSystemRepository>();
            kernel.Bind<IOperationLogCategoryRepository>().To<OperationLogCategoryRepository>();
            kernel.Bind<IOperationLogEMailLogRepository>().To<OperationLogEMailLogRepository>();
            kernel.Bind<IOperationLogRepository>().To<OperationLogRepository>();
            kernel.Bind<IOperationObjectRepository>().To<OperationObjectRepository>();
            kernel.Bind<IOrderEMailLogRepository>().To<OrderEMailLogRepository>();
            kernel.Bind<IOrderFieldSettingsRepository>().To<OrderFieldSettingsRepository>();
            kernel.Bind<IOrderLogRepository>().To<OrderLogRepository>();
            kernel.Bind<IOrderRepository>().To<OrderRepository>();
            kernel.Bind<IOrderStateRepository>().To<OrderStateRepository>();
            kernel.Bind<IOrderTypeRepository>().To<OrderTypeRepository>();
            kernel.Bind<IOULanguageRepository>().To<OULanguageRepository>();
            kernel.Bind<IOrganizationUnitRepository>().To<OrganizationUnitRepository>();
            kernel.Bind<IPermissionLanguageRepository>().To<PermissionLanguageRepository>();
            kernel.Bind<IPermissionRepository>().To<PermissionRepository>();
            kernel.Bind<IPrinterFieldSettingsRepository>().To<PrinterFieldSettingsRepository>();
            kernel.Bind<IPrinterRepository>().To<PrinterRepository>();
            kernel.Bind<IPriorityImpactUrgencyRepository>().To<PriorityImpactUrgencyRepository>();
            kernel.Bind<IPriorityLanguageRepository>().To<PriorityLanguageRepository>();
            kernel.Bind<IPriorityRepository>().To<PriorityRepository>();
            kernel.Bind<IProblemEMailLogRepository>().To<ProblemEMailLogRepository>();
            kernel.Bind<IProblemLogRepository>().To<ProblemLogRepository>();
            kernel.Bind<IProblemRepository>().To<ProblemRepository>();
            kernel.Bind<IProcessorRepository>().To<ProcessorRepository>();
            kernel.Bind<IProductAreaQuestionRepository>().To<ProductAreaQuestionRepository>();
            kernel.Bind<IProductAreaQuestionVersionRepository>().To<ProductAreaQuestionVersionRepository>();
            kernel.Bind<IProductAreaRepository>().To<ProductAreaRepository>();
            kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<IProgramRepository>().To<ProgramRepository>();
            kernel.Bind<IProjectCollaboratorRepository>().To<ProjectCollaboratorRepository>();
            kernel.Bind<IProjectFileRepository>().To<ProjectFileRepository>();
            kernel.Bind<IProjectLogRepository>().To<ProjectLogRepository>();
            kernel.Bind<IProjectRepository>().To<ProjectRepository>();
            kernel.Bind<IProjectScheduleRepository>().To<ProjectScheduleRepository>();
            kernel.Bind<IQuestionCategoryRepository>().To<QuestionCategoryRepository>();
            kernel.Bind<IQuestionGroupRepository>().To<QuestionGroupRepository>();
            kernel.Bind<IQuestionnaireCircularPartRepository>().To<QuestionnaireCircularPartRepository>();
            kernel.Bind<IQuestionnaireCircularRepository>().To<QuestionnaireCircularRepository>();
            kernel.Bind<IQuestionnaireLanguageRepository>().To<QuestionnaireLanguageRepository>();
            kernel.Bind<IQuestionnaireQuesLangRepository>().To<QuestionnaireQuesLangRepository>();
            kernel.Bind<IQuestionnaireQuesOpLangRepository>().To<QuestionnaireQuesOpLangRepository>();
            kernel.Bind<IQuestionnaireQuestionOptionRepository>().To<QuestionnaireQuestionOptionRepository>();
            kernel.Bind<IQuestionnaireQuestionRepository>().To<QuestionnaireQuestionRepository>();
            kernel.Bind<IQuestionnaireQuestionResultRepository>().To<QuestionnaireQuestionResultRepository>();
            kernel.Bind<IQuestionnaireResultRepository>().To<QuestionnaireResultRepository>();
            kernel.Bind<IQuestionnaireRepository>().To<QuestionnaireRepository>();
            kernel.Bind<IQuestionRegistrationRepository>().To<QuestionRegistrationRepository>();
            kernel.Bind<IQuestionRepository>().To<QuestionRepository>();
            kernel.Bind<IRAMRepository>().To<RAMRepository>();
            kernel.Bind<IRegionLanguageRepository>().To<RegionLanguageRepository>();
            kernel.Bind<IRegionRepository>().To<RegionRepository>();
            kernel.Bind<IReportCustomerRepository>().To<ReportCustomerRepository>();
            kernel.Bind<IReportRepository>().To<ReportRepository>();
            kernel.Bind<IRoomRepository>().To<RoomRepository>();
            kernel.Bind<IServerFieldSettingsRepository>().To<ServerFieldSettingsRepository>();
            kernel.Bind<IServerLogicalDriveRepository>().To<ServerLogicalDriveRepository>();
            kernel.Bind<IServerRepository>().To<ServerRepository>();
            kernel.Bind<IServerSoftwareRepository>().To<ServerSoftwareRepository>();
            kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<ISoftwareRepository>().To<SoftwareRepository>();
            kernel.Bind<IStandardTextRepository>().To<StandardTextRepository>();
            kernel.Bind<IStateSecondaryRepository>().To<StateSecondaryRepository>();
            kernel.Bind<IStatusRepository>().To<StatusRepository>();
            kernel.Bind<ISupplierRepository>().To<SupplierRepository>();
            kernel.Bind<ISystemRepository>().To<SystemRepository>();
            kernel.Bind<ITextTranslationRepository>().To<TextTranslationRepository>();
            kernel.Bind<ITextRepository>().To<TextRepository>();
            kernel.Bind<ITimeRegistrationRepository>().To<TimeRegistrationRepository>();
            kernel.Bind<ITimeTypeRepository>().To<TimeTypeRepository>();
            kernel.Bind<IUrgencyLanguageRepository>().To<UrgencyLanguageRepository>();
            kernel.Bind<IUrgencyRepository>().To<UrgencyRepository>();
            kernel.Bind<IUserGroupRepository>().To<UserGroupRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IUserRoleRepository>().To<UserRoleRepository>();
            kernel.Bind<IUsersPasswordHistoryRepository>().To<UsersPasswordHistoryRepository>();
            kernel.Bind<IUserWorkingGroupRepository>().To<UserWorkingGroupRepository>();
            kernel.Bind<IVendorRepository>().To<VendorRepository>();
            kernel.Bind<IWatchDateCalendarRepository>().To<WatchDateCalendarRepository>();
            kernel.Bind<IWatchDateCalendarValueRepository>().To<WatchDateCalendarValueRepository>();
            kernel.Bind<IWorkingGroupRepository>().To<WorkingGroupRepository>();



            #region Services

            kernel.Bind<IAccountService>().To<AccountService>();
            kernel.Bind<IAccountActivityService>().To<AccountActivityService>();
            kernel.Bind<IBuildingService>().To<BuildingService>();
            kernel.Bind<IBulletinBoardService>().To<BulletinBoardService>();
            kernel.Bind<ICalendarService>().To<CalendarService>();
            kernel.Bind<ICaseFieldSettingService>().To<CaseFieldSettingService>();
            kernel.Bind<ICaseService>().To<CaseService>();
            kernel.Bind<ICaseSettingsService>().To<CaseSettingsService>();
            kernel.Bind<ICaseSolutionService>().To<CaseSolutionService>();
            kernel.Bind<ICaseFileService>().To<CaseFileService>();
            kernel.Bind<ICaseTypeService>().To<CaseTypeService>();
            kernel.Bind<ICaseSearchService>().To<CaseSearchService>();
            kernel.Bind<ICategoryService>().To<CategoryService>();
            kernel.Bind<IChangeGroupService>().To<ChangeGroupService>();
            kernel.Bind<IChangeImplementationStatusService>().To<ChangeImplementationStatusService>();
            kernel.Bind<IChangeObjectService>().To<ChangeObjectService>();
            kernel.Bind<IChangePriorityService>().To<ChangePriorityService>();
            kernel.Bind<IChangeStatusService>().To<ChangeStatusService>();
            kernel.Bind<IChangeService>().To<ChangeService>();
            kernel.Bind<IChecklistActionService>().To<ChecklistActionService>();
            kernel.Bind<IChecklistServiceService>().To<ChecklistServiceService>();
            kernel.Bind<IComputerService>().To<ComputerService>();
            kernel.Bind<IContractCategoryService>().To<ContractCategoryService>();
            kernel.Bind<ICountryService>().To<CountryService>();
            kernel.Bind<ICustomerService>().To<CustomerService>();
            kernel.Bind<ICurrencyService>().To<CurrencyService>();
            kernel.Bind<ICustomerUserService>().To<CustomerUserService>();
            kernel.Bind<IDailyReportService>().To<DailyReportService>();
            kernel.Bind<IDepartmentService>().To<DepartmentService>();
            kernel.Bind<IDivisionService>().To<DivisionService>();
            kernel.Bind<IDocumentService>().To<DocumentService>();
            kernel.Bind<IDomainService>().To<DomainService>();
            kernel.Bind<IEmailService>().To<EmailService>();
            kernel.Bind<IFormService>().To<FormService>();
            kernel.Bind<IFinishingCauseService>().To<FinishingCauseService>();
            kernel.Bind<IFloorService>().To<FloorService>();
            kernel.Bind<IGlobalSettingService>().To<GlobalSettingService>();
            kernel.Bind<IHolidayService>().To<HolidayService>();
            kernel.Bind<IImpactService>().To<ImpactService>();
            kernel.Bind<IInfoService>().To<InfoService>();
            kernel.Bind<ILanguageService>().To<LanguageService>();
            kernel.Bind<ILinkService>().To<LinkService>();
            kernel.Bind<ILogService>().To<LogService>();
            kernel.Bind<IMailTemplateService>().To<MailTemplateService>();
            kernel.Bind<IMasterDataService>().To<MasterDataService>();
            kernel.Bind<IOperationLogCategoryService>().To<OperationLogCategoryService>();
            kernel.Bind<IOperationObjectService>().To<OperationObjectService>();
            kernel.Bind<IOrderService>().To<OrderService>();
            kernel.Bind<IOperationLogService>().To<OperationLogService>();
            kernel.Bind<IOrderStateService>().To<OrderStateService>();
            kernel.Bind<IOrderTypeService>().To<OrderTypeService>();
            kernel.Bind<IOUService>().To<OUService>();
            kernel.Bind<IPriorityService>().To<PriorityService>();
            kernel.Bind<IProblemService>().To<ProblemService>();
            kernel.Bind<IProblemLogService>().To<ProblemLogService>();
            kernel.Bind<IProductAreaService>().To<ProductAreaService>();
            kernel.Bind<IProgramService>().To<ProgramService>();
            kernel.Bind<IProjectService>().To<ProjectService>();
            kernel.Bind<IRegionService>().To<RegionService>();
            kernel.Bind<IRoomService>().To<RoomService>();
            kernel.Bind<ISettingService>().To<SettingService>();
            kernel.Bind<IStandardTextService>().To<StandardTextService>();
            kernel.Bind<IStateSecondaryService>().To<StateSecondaryService>();
            kernel.Bind<IStatusService>().To<StatusService>();
            kernel.Bind<ISupplierService>().To<SupplierService>();
            kernel.Bind<ISystemService>().To<SystemService>();
            kernel.Bind<ITemplateService>().To<TemplateService>();
            kernel.Bind<ITextTranslationService>().To<TextTranslationService>();
            kernel.Bind<IUrgencyService>().To<UrgencyService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IWatchDateCalendarService>().To<WatchDateCalendarService>();
            kernel.Bind<IWorkingGroupService>().To<WorkingGroupService>();
            kernel.Bind<IChangeCategoryService>().To<ChangeCategoryService>();

            #endregion

            // caching
            kernel.Bind<ICacheProvider>().To<CacheProvider>();
        }

        #endregion
    }
}
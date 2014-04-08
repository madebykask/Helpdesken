[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DH.Helpdesk.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DH.Helpdesk.Web.App_Start.NinjectWebCommon), "Stop")]

namespace DH.Helpdesk.Web.App_Start
{
    using System;
    using System.Web;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Computers.Concrete;
    using DH.Helpdesk.Dal.Repositories.Concrete;
    using DH.Helpdesk.Dal.Repositories.Faq;
    using DH.Helpdesk.Dal.Repositories.Faq.Concrete;
    using DH.Helpdesk.Dal.Repositories.Inventory;
    using DH.Helpdesk.Dal.Repositories.Inventory.Concrete;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Dal.Repositories.MailTemplates.Concrete;
    using DH.Helpdesk.Dal.Repositories.Printers;
    using DH.Helpdesk.Dal.Repositories.Printers.Concrete;
    using DH.Helpdesk.Dal.Repositories.Problem;
    using DH.Helpdesk.Dal.Repositories.Problem.Concrete;
    using DH.Helpdesk.Dal.Repositories.Servers;
    using DH.Helpdesk.Dal.Repositories.Servers.Concrete;
    using DH.Helpdesk.Dal.Repositories.Users;
    using DH.Helpdesk.Dal.Repositories.Users.Concrete;
    using DH.Helpdesk.Dal.Repositories.WorkstationModules;
    using DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Localization;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.NinjectModules.Common;
    using DH.Helpdesk.Web.NinjectModules.Modules;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(
               new ChangesModule(),
               new FaqModule(),
               new NotifiersModule(),
               new ProblemModule(),
               new RepositoriesModule(),
               new ServicesModule(),
               new ProjectModule(),
               new ToolsModule(),
               new LinkModule(),
               new WorkContextModule(),
               new UserModule());

            ManualDependencyResolver.SetKernel(kernel);

            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
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
            kernel.Bind<IChangeGroupService>().To<ChangeGroupService>();
            kernel.Bind<IChangeImplementationStatusService>().To<ChangeImplementationStatusService>();
            kernel.Bind<IChangeObjectService>().To<ChangeObjectService>();
            kernel.Bind<IChangePriorityService>().To<ChangePriorityService>();
            kernel.Bind<IChangeStatusService>().To<ChangeStatusService>();
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
            kernel.Bind<IEmailGroupRepository>().To<EmailGroupRepository>();
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
            kernel.Bind<IFormFieldValueRepository>().To<FormFieldValueRepository>();
            kernel.Bind<IFormRepository>().To<FormRepository>();
            kernel.Bind<IGlobalSettingRepository>().To<GlobalSettingRepository>();
            kernel.Bind<IHolidayRepository>().To<HolidayRepository>();
            kernel.Bind<IHolidayHeaderRepository>().To<HolidayHeaderRepository>();
            kernel.Bind<IImpactRepository>().To<ImpactRepository>();
            kernel.Bind<IInfoTextRepository>().To<InfoTextRepository>();
            kernel.Bind<IInventoryRepository>().To<InventoryRepository>();
            kernel.Bind<IInventoryTypeRepository>().To<InventoryTypeRepository>();
            kernel.Bind<IInvoiceHeaderRepository>().To<InvoiceHeaderRepository>();
            kernel.Bind<IInvoiceRowRepository>().To<InvoiceRowRepository>();
            kernel.Bind<ILanguageRepository>().To<LanguageRepository>();
            kernel.Bind<ILicenseFileRepository>().To<LicenseFileRepository>();
            kernel.Bind<ILicenseRepository>().To<LicenseRepository>();
            kernel.Bind<ILinkRepository>().To<LinkRepository>();
            kernel.Bind<ILinkGroupRepository>().To<LinkGroupRepository>();
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
            kernel.Bind<IQuestionCategoryRepository>().To<QuestionCategoryRepository>();
            kernel.Bind<IQuestionGroupRepository>().To<QuestionGroupRepository>();
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
            kernel.Bind<IModuleRepository>().To<ModuleRepository>();
            kernel.Bind<IUserModuleRepository>().To<UserModuleRepository>();

            #region Services

            kernel.Bind<IAccountService>().To<AccountService>();
            kernel.Bind<IAccountActivityService>().To<AccountActivityService>();
            kernel.Bind<IAccountFieldSettingsService>().To<AccountFieldSettingsService>();
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
            kernel.Bind<IEmailGroupService>().To<EmailGroupService>();
            kernel.Bind<IFormService>().To<FormService>();
            kernel.Bind<IFinishingCauseService>().To<FinishingCauseService>();
            kernel.Bind<IFloorService>().To<FloorService>();
            kernel.Bind<IGlobalSettingService>().To<GlobalSettingService>();
            kernel.Bind<IHolidayService>().To<HolidayService>();
            kernel.Bind<IImpactService>().To<ImpactService>();
            kernel.Bind<IInfoService>().To<InfoService>();
            kernel.Bind<ILanguageService>().To<LanguageService>();
            kernel.Bind<ILinkService>().To<LinkService>();
            kernel.Bind<ILogFileService>().To<LogFileService>();
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
            kernel.Bind<IRegionService>().To<RegionService>();
            kernel.Bind<IRoomService>().To<RoomService>();
            kernel.Bind<ISettingService>().To<SettingService>();
            kernel.Bind<IStandardTextService>().To<StandardTextService>();
            kernel.Bind<IStateSecondaryService>().To<StateSecondaryService>();
            kernel.Bind<IStatusService>().To<StatusService>();
            kernel.Bind<IStatisticsService>().To<StatisticsService>();
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
    }
}

using DH.Helpdesk.Dal.Repositories.Inventory;
using DH.Helpdesk.Dal.Repositories.Inventory.Concrete;
using DH.Helpdesk.Dal.Repositories.Printers;
using DH.Helpdesk.Dal.Repositories.Printers.Concrete;
using DH.Helpdesk.Dal.Repositories.Servers;
using DH.Helpdesk.Dal.Repositories.Servers.Concrete;

using DH.Helpdesk.SelfService;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.Services.Services.Authentication;
using DH.Helpdesk.Services.Services.Feedback;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Services.Services.CaseStatistic;
using Ninject.Web.Common.WebHost;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace DH.Helpdesk.SelfService
{
    using System;
    using System.Web;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.ActionSetting;
    using DH.Helpdesk.Dal.Repositories.ActionSetting.Concrete;
    using DH.Helpdesk.Dal.Repositories.Computers;
    using DH.Helpdesk.Dal.Repositories.Computers.Concrete;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Dal.Repositories.MailTemplates.Concrete;
    using DH.Helpdesk.Dal.Repositories.Notifiers;
    using DH.Helpdesk.Dal.Repositories.Notifiers.Concrete;
    using DH.Helpdesk.Dal.Repositories.Problem;
    using DH.Helpdesk.Dal.Repositories.Projects;
    using DH.Helpdesk.Dal.Repositories.Projects.Concrete;
    using DH.Helpdesk.Dal.Repositories.Users;
    using DH.Helpdesk.Dal.Repositories.Users.Concrete;
    using DH.Helpdesk.Dal.Repositories.WorkstationModules;
    using DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete;
    using DH.Helpdesk.Dal.Repositories.ADFS;
    using DH.Helpdesk.Dal.Repositories.ADFS.Concrete;
    using DH.Helpdesk.SelfService.NinjectModules.Modules;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Infrastructure.Concrete;
    using DH.Helpdesk.Services.Services;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using DH.Helpdesk.Dal.Infrastructure.Concrete;
    using DH.Helpdesk.Dal.Repositories.Concrete;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.SelfService.Infrastructure.Tools;
    using DH.Helpdesk.SelfService.Infrastructure.Tools.Concrete;
    using DH.Helpdesk.Dal.Repositories.Problem.Concrete;
    using DH.Helpdesk.Dal.Repositories.Invoice;
    using DH.Helpdesk.Dal.Repositories.Invoice.Concrete;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Dal.Repositories.Cases.Concrete;
    using DH.Helpdesk.Dal.Repositories.BusinessRules;
    using DH.Helpdesk.Dal.Repositories.BusinessRules.Concrete;
    using Dal.Repositories.Faq;
    using Dal.Repositories.Faq.Concrete;
    using Dal.Repositories.Questionnaire;
    using Dal.Repositories.Questionnaire.Concrete;
    using Services.Services.ExtendedCase;
    using Services.Services.UniversalCase;
    using Dal.Repositories.MetaData;
    using Dal.Repositories.MetaData.Concrete;
    using Services.Services.EmployeeService;
    using Services.Services.EmployeeService.Concrete;
    using Services.Services.WebApi;
    using Dal.Repositories.Condition;
    using Dal.Repositories.Condition.Concrete;
  

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(
                new LoggerModule(),
                new WorkContextModule(),  
                new UserModule(), 
                new ProblemModule(),
                new CommonModule(), 
                new EmailModule(), 
                new NotifiersModule(), 
                new ToolsModule(),
                new OrdersModule(), 
                new InventoryModule());

            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            ManualDependencyResolver.SetKernel(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Data Infrastructure
#pragma warning disable 0618
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
#pragma warning restore 0618
            kernel.Bind<IFilesStorage>().To<FilesStorage>().InRequestScope();
            kernel.Bind<IUserTemporaryFilesStorageFactory>().To<UserTemporaryFilesStorageFactory>().InRequestScope();
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();
            kernel.Bind<IEmailSendingSettingsProvider>().To<EmailSendingSettingsProvider>().InRequestScope();

            kernel.Bind<IFederatedAuthenticationSettings>().To<FederatedAuthenticationSettings>();

            kernel.Bind<IFederatedAuthenticationService>()
                .ToMethod(ctx => new FederatedAuthenticationService(ctx.Kernel.Get<ILoggerService>(Log4NetLoggerService.LogType.Session)))
                .InRequestScope();

            kernel.Bind<ISelfServiceConfigurationService>().To<SelfServiceConfigurationService>().InSingletonScope();

            // Repositories
            kernel.Bind<ICaseSolutionLanguageRepository>().To<CaseSolutionLanguageRepository>();
            kernel.Bind<ICaseSolutionCategoryLanguageRepository>().To<CaseSolutionCategoryLanguageRepository>();
            kernel.Bind<ICircularRepository>().To<CircularRepository>();
            kernel.Bind<IQuestionnaireQuestionOptionRepository>().To<QuestionnaireQuestionOptionRepository>();
            kernel.Bind<IQuestionnaireQuestionRepository>().To<QuestionnaireQuestionRepository>();
            kernel.Bind<IQuestionnaireRepository>().To<QuestionnaireRepository>();

            kernel.Bind<ICustomerRepository>().To<CustomerRepository>();
            kernel.Bind<ITextRepository>().To<TextRepository>();
            kernel.Bind<ILanguageRepository>().To<LanguageRepository>();
            kernel.Bind<IReportCustomerRepository>().To<ReportCustomerRepository>();
            kernel.Bind<ICaseFieldSettingRepository>().To<CaseFieldSettingRepository>();
            kernel.Bind<ICaseFieldSettingLanguageRepository>().To<CaseFieldSettingLanguageRepository>();
            kernel.Bind<IReportRepository>().To<ReportRepository>();
            kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<IContractCategoryRepository>().To<ContractCategoryRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<ICaseRepository>().To<CaseRepository>();
            kernel.Bind<ICaseFileRepository>().To<CaseFileRepository>();                                                          
            kernel.Bind<ICaseHistoryRepository>().To<CaseHistoryRepository>();                                                          
            kernel.Bind<IEmailLogRepository>().To<EmailLogRepository>();                                                          
            kernel.Bind<ILogRepository>().To<LogRepository>();                                                          
            kernel.Bind<ILogFileRepository>().To<LogFileRepository>();                                                          
            kernel.Bind<IFormFieldValueRepository>().To<FormFieldValueRepository>();
            kernel.Bind<IRegionRepository>().To<RegionRepository>();
            kernel.Bind<ICaseTypeRepository>().To<CaseTypeRepository>();
            kernel.Bind<ISupplierRepository>().To<SupplierRepository>();
            kernel.Bind<IPriorityRepository>().To<PriorityRepository>();
            kernel.Bind<IPriorityLanguageRepository>().To<PriorityLanguageRepository>();
            kernel.Bind<IStatusRepository>().To<StatusRepository>();
            kernel.Bind<IUserWorkingGroupRepository>().To<UserWorkingGroupRepository>();
            kernel.Bind<IProductAreaRepository>().To<ProductAreaRepository>();
            kernel.Bind<IMailTemplateRepository>().To<MailTemplateRepository>();
            kernel.Bind<IWorkingGroupRepository>().To<WorkingGroupRepository>();
            kernel.Bind<IMailTemplateLanguageRepository>().To<MailTemplateLanguageRepository>();
            kernel.Bind<IMailTemplateIdentifierRepository>().To<MailTemplateIdentifierRepository>();
            kernel.Bind<ICaseSettingRepository>().To<CaseSettingRepository>();
            kernel.Bind<IUserGroupRepository>().To<UserGroupRepository>();
            kernel.Bind<IInfoTextRepository>().To<InfoTextRepository>();
            kernel.Bind<IPriorityImpactUrgencyRepository>().To<PriorityImpactUrgencyRepository>();
            kernel.Bind<IDepartmentRepository>().To<DepartmentRepository>();
            kernel.Bind<ISystemRepository>().To<SystemRepository>();
            kernel.Bind<IOperatingSystemRepository>().To<OperatingSystemRepository>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<ICurrencyRepository>().To<CurrencyRepository>();
            kernel.Bind<ICountryRepository>().To<CountryRepository>();
            kernel.Bind<INotifierFieldSettingRepository>().To<NotifierFieldSettingRepository>();
            kernel.Bind<INotifierGroupRepository>().To<NotifierGroupRepository>();
            kernel.Bind<INotifierRepository>().To<NotifierRepository>();
            kernel.Bind<IComputerUsersBlackListRepository>().To<ComputerUsersBlackListRepository>();
            kernel.Bind<IComputerUsersRepository>().To<ComputerUsersRepository>();
            kernel.Bind<IComputerRepository>().To<ComputerRepository>();
            kernel.Bind<IComputerStatusRepository>().To<ComputerStatusRepository>();
            kernel.Bind<IOrganizationUnitRepository>().To<OrganizationUnitRepository>();
            kernel.Bind<IAccountActivityRepository>().To<AccountActivityRepository>();
            kernel.Bind<ICustomerUserRepository>().To<CustomerUserRepository>();
            kernel.Bind<IOrderTypeRepository>().To<OrderTypeRepository>();
            kernel.Bind<IUserRoleRepository>().To<UserRoleRepository>();
            kernel.Bind<IDepartmentUserRepository>().To<DepartmentUserRepository>();
            kernel.Bind<ILogProgramRepository>().To<LogProgramRepository>();
            kernel.Bind<IModuleRepository>().To<ModuleRepository>();
            kernel.Bind<IUserModuleRepository>().To<UserModuleRepository>();
            kernel.Bind<ICaseSearchRepository>().To<CaseSearchRepository>();
            kernel.Bind<IGlobalSettingRepository>().To<GlobalSettingRepository>();
            kernel.Bind<IProblemLogRepository>().To<ProblemLogRepository>();
            kernel.Bind<IProblemEMailLogRepository>().To<ProblemEMailLogRepository>();
            kernel.Bind<IProblemRepository>().To<ProblemRepository>();
            kernel.Bind<IImpactRepository>().To<ImpactRepository>();            
            kernel.Bind<IFinishingCauseRepository>().To<FinishingCauseRepository>();
            kernel.Bind<IFinishingCauseCategoryRepository>().To<FinishingCauseCategoryRepository>();
            kernel.Bind<IStateSecondaryRepository>().To<StateSecondaryRepository>();
            kernel.Bind<ICaseSolutionRepository>().To<CaseSolutionRepository>();
            kernel.Bind<ICaseSolutionCategoryRepository>().To<CaseSolutionCategoryRepository>();
            kernel.Bind<ICaseSolutionScheduleRepository>().To<CaseSolutionScheduleRepository>();
            kernel.Bind<INotifierFieldSettingLanguageRepository>().To<NotifierFieldSettingLanguageRepository>();
            kernel.Bind<IActionSettingRepository>().To<ActionSettingRepository>();
            kernel.Bind<IInvoiceArticleUnitRepository>().To<InvoiceArticleUnitRepository>();
            kernel.Bind<IInvoiceArticleRepository>().To<InvoiceArticleRepository>();
            kernel.Bind<ICaseInvoiceArticleRepository>().To<CaseInvoiceArticleRepository>();            
            kernel.Bind<IBulletinBoardRepository>().To<BulletinBoardRepository>();
            kernel.Bind<ICaseSolutionSettingRepository>().To<CaseSolutionSettingRepository>();
            kernel.Bind<IDocumentRepository>().To<DocumentRepository>();
            kernel.Bind<IDocumentCategoryRepository>().To<DocumentCategoryRepository>();
            kernel.Bind<IFormRepository>().To<FormRepository>();
            kernel.Bind<IADFSRepository>().To<ADFSRepository>();
            kernel.Bind<IHolidayRepository>().To<HolidayRepository>();
            kernel.Bind<IHolidayHeaderRepository>().To<HolidayHeaderRepository>();
            kernel.Bind<IDomainRepository>().To<DomainRepository>();
            kernel.Bind<ILinkRepository>().To<LinkRepository>();
            kernel.Bind<ICaseLockRepository>().To<CaseLockRepository>();
            kernel.Bind<ICalendarRepository>().To<CalendarRepository>();
            kernel.Bind<ILinkGroupRepository>().To<LinkGroupRepository>();
            kernel.Bind<IUsersPasswordHistoryRepository>().To<UsersPasswordHistoryRepository>();
            kernel.Bind<ICaseFilterFavoriteRepository>().To<CaseFilterFavoriteRepository>();
            kernel.Bind<ICaseInvoiceSettingsRepository>().To<CaseInvoiceSettingsRepository>();
            kernel.Bind<IProjectCollaboratorRepository>().To<ProjectCollaboratorRepository>();
            kernel.Bind<IProjectFileRepository>().To<ProjectFileRepository>();
            kernel.Bind<IProjectLogRepository>().To<ProjectLogRepository>();
            kernel.Bind<IProjectRepository>().To<ProjectRepository>();
            kernel.Bind<IProjectScheduleRepository>().To<ProjectScheduleRepository>();
            kernel.Bind<IReportFavoriteRepository>().To<ReportFavoriteRepository>();
            kernel.Bind<IMail2TicketRepository>().To<Mail2TicketRepository>();
            kernel.Bind<IBusinessRuleRepository>().To<BusinessRuleRepository>();
            kernel.Bind<IEmailGroupRepository>().To<EmailGroupRepository>();
            kernel.Bind<IEmailGroupEmailRepository>().To<EmailGroupEmailRepository>();
            kernel.Bind<IFaqRepository>().To<FaqRepository>();
            kernel.Bind<IFaqFileRepository>().To<FaqFileRepository>();
            kernel.Bind<IQuestionRepository>().To<QuestionRepository>();
            kernel.Bind<IFaqCategoryRepository>().To<FaqCategoryRepository>();
            kernel.Bind<IFaqCategoryLanguageRepository>().To<FaqCategoryLanguageRepository>();
            kernel.Bind<IUrgencyRepository>().To<UrgencyRepository>();
            kernel.Bind<ICaseExtraFollowersRepository>().To<CaseExtraFollowersRepository>();
            kernel.Bind<IUserEmailRepository>().To<UserEmailRepository>();
            kernel.Bind<IOrderEMailLogRepository>().To<OrderEMailLogRepository>();
            kernel.Bind<IOrderFieldSettingsRepository>().To<OrderFieldSettingsRepository>();
            kernel.Bind<IOrderLogRepository>().To<OrderLogRepository>();
            kernel.Bind<IOrderRepository>().To<OrderRepository>();
            kernel.Bind<IContractRepository>().To<ContractRepository>();
            kernel.Bind<IOrderStateRepository>().To<OrderStateRepository>();
            kernel.Bind<IExtendedCaseFormRepository>().To<ExtendedCaseFormRepository>();
            kernel.Bind<IExtendedCaseDataRepository>().To<ExtendedCaseDataRepository>();
			kernel.Bind<ITextTranslationRepository>().To<TextTranslationRepository>();
            kernel.Bind<ITextTypeRepository>().To<TextTypeRepository>();
            kernel.Bind<IExtendedCaseValueRepository>().To<ExtendedCaseValueRepository>();
            kernel.Bind<IApplicationRepository>().To<ApplicationRepository>();
            kernel.Bind<IEmailLogAttemptRepository>().To<EmailLogAttemptRepository>();
            kernel.Bind<IWatchDateCalendarValueRepository>().To<WatchDateCalendarValueRepository>();
            kernel.Bind<IWatchDateCalendarRepository>().To<WatchDateCalendarRepository>();
            kernel.Bind<IMetaDataRepository>().To<MetaDataRepository>();            
            kernel.Bind<IEntityInfoRepository>().To<EntityInfoRepository>();
            kernel.Bind<ICaseFollowUpRepository>().To<CaseFollowUpRepository>();
            kernel.Bind<IConditionRepository>().To<ConditionRepository>();

            kernel.Bind<IInventoryTypeRepository>().To<InventoryTypeRepository>();
            kernel.Bind<IInventoryTypeStandardSettingsRepository>().To<InventoryTypeStandardSettingsRepository>();
            kernel.Bind<IServerRepository>().To<ServerRepository>();
            kernel.Bind<IPrinterRepository>().To<PrinterRepository>();
            kernel.Bind<IInventoryRepository>().To<InventoryRepository>();
            kernel.Bind<IInventoryTypePropertyValueRepository>().To<InventoryTypePropertyValueRepository>();
            kernel.Bind<IComputerLogRepository>().To<ComputerLogRepository>();
            kernel.Bind<IComputerInventoryRepository>().To<ComputerInventoryRepository>();
            kernel.Bind<IOperationLogRepository>().To<OperationLogRepository>();
            kernel.Bind<IInventoryTypeGroupRepository>().To<InventoryTypeGroupRepository>();
            kernel.Bind<IInventoryFieldSettingsRepository>().To<InventoryFieldSettingsRepository>();
            kernel.Bind<IInventoryDynamicFieldSettingsRepository>().To<InventoryDynamicFieldSettingsRepository>();
            kernel.Bind<IComputerFieldSettingsRepository>().To<ComputerFieldSettingsRepository>();
            kernel.Bind<IComputerHistoryRepository>().To<ComputerHistoryRepository>();
            kernel.Bind<ILogicalDriveRepository>().To<LogicalDriveRepository>();
            kernel.Bind<ISoftwareRepository>().To<SoftwareRepository>();
            kernel.Bind<IServerFieldSettingsRepository>().To<ServerFieldSettingsRepository>();
            kernel.Bind<IOperationObjectRepository>().To<OperationObjectRepository>();
            kernel.Bind<IOperationLogEMailLogRepository>().To<OperationLogEMailLogRepository>();
            kernel.Bind<IServerLogicalDriveRepository>().To<ServerLogicalDriveRepository>();
            kernel.Bind<IServerSoftwareRepository>().To<ServerSoftwareRepository>();
            kernel.Bind<IPrinterFieldSettingsRepository>().To<PrinterFieldSettingsRepository>();
            kernel.Bind<ICaseSectionsRepository>().To<CaseSectionsRepository>();
            kernel.Bind<IComputerUserCategoryRepository>().To<ComputerUserCategoryRepository>();
            kernel.Bind<IFeatureToggleRepository>().To<FeatureToggleRepository>();
			kernel.Bind<IFileViewLogRepository>().To<FileViewLogRepository>();

			// Service             
			kernel.Bind<IMasterDataService>().To<MasterDataService>();            
            kernel.Bind<ISettingService>().To<SettingService>();
            kernel.Bind<ICaseService>().To<CaseService>();
            kernel.Bind<ILogService>().To<LogService>();
            kernel.Bind<ICustomerService>().To<CustomerService>();            
            kernel.Bind<ICaseFieldSettingService>().To<CaseFieldSettingService>();            
            kernel.Bind<IRegionService>().To<RegionService>();   
            kernel.Bind<ICaseTypeService>().To<CaseTypeService>();   
            kernel.Bind<ISupplierService>().To<SupplierService>();   
            kernel.Bind<IPriorityService>().To<PriorityService>();   
            kernel.Bind<IStatusService>().To<StatusService>();   
            kernel.Bind<IWorkingGroupService>().To<WorkingGroupService>();   
            kernel.Bind<IProductAreaService>().To<ProductAreaService>();   
            kernel.Bind<IMailTemplateService>().To<MailTemplateService>();
            kernel.Bind<IEmailService>().To<EmailService>();
            kernel.Bind<IFeedbackTemplateService>().To<FeedbackTemplateService>();
            kernel.Bind<ICaseSettingsService>().To<CaseSettingsService>();
            kernel.Bind<ICalendarService>().To<CalendarService>();
            kernel.Bind<IInfoService>().To<InfoService>();
            kernel.Bind<ICaseFileService>().To<CaseFileService>();
            kernel.Bind<ILogFileService>().To<LogFileService>();
            kernel.Bind<IDepartmentService>().To<DepartmentService>();
            kernel.Bind<ISystemService>().To<SystemService>();
            kernel.Bind<ICategoryService>().To<CategoryService>();
            kernel.Bind<ICurrencyService>().To<CurrencyService>();
            kernel.Bind<ICountryService>().To<CountryService>();
            kernel.Bind<IComputerService>().To<ComputerService>();
            kernel.Bind<IContractCategoryService>().To<ContractCategoryService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<ICustomerUserService>().To<CustomerUserService>();
            kernel.Bind<ICaseSearchService>().To<CaseSearchService>();
            kernel.Bind<IGlobalSettingService>().To<GlobalSettingService>();
            kernel.Bind<IProblemLogService>().To<ProblemLogService>();
            kernel.Bind<IOUService>().To<OUService>();
            kernel.Bind<IImpactService>().To<ImpactService>();
            kernel.Bind<IProjectService>().To<ProjectService>();
            kernel.Bind<IFinishingCauseService>().To<FinishingCauseService>();
            kernel.Bind<IStateSecondaryService>().To<StateSecondaryService>();
            kernel.Bind<ICaseSolutionService>().To<CaseSolutionService>();
            kernel.Bind<IActionSettingService>().To<ActionSettingService>();
            kernel.Bind<IInvoiceArticleService>().To<InvoiceArticleService>();                        
            kernel.Bind<IBulletinBoardService>().To<BulletinBoardService>();
            kernel.Bind<ICaseSolutionSettingService>().To<CaseSolutionSettingService>();
            kernel.Bind<IDocumentService>().To<DocumentService>();
            kernel.Bind<ISurveyService>().To<SurveyService>();
            kernel.Bind<IHolidayService>().To<HolidayService>();
            kernel.Bind<IOrganizationService>().To<OrganizationService>();
			kernel.Bind<IOrganizationJsonService>().To<OrganizationJsonService>();
			kernel.Bind<ILinkService>().To<LinkService>();
            kernel.Bind<ICaseLockService>().To<CaseLockService>();
            kernel.Bind<ICaseInvoiceSettingsService>().To<CaseInvoiceSettingsService>();
            kernel.Bind<IOperationLogService>().To<OperationLogService>();
            kernel.Bind<IBusinessRuleService>().To<BusinessRuleService>();
            kernel.Bind<IEmailGroupService>().To<EmailGroupService>();
            kernel.Bind<IFaqService>().To<FaqService>();
            kernel.Bind<IUrgencyService>().To<UrgencyService>();
            kernel.Bind<ICaseExtraFollowersService>().To<CaseExtraFollowersService>();
            kernel.Bind<IFeedbackService>().To<FeedbackService>();
            kernel.Bind<ICircularService>().To<CircularService>();
            kernel.Bind<IMailTemplateServiceNew>().To<MailTemplateServiceNew>();
            kernel.Bind<IRegistrationSourceCustomerService>().To<RegistrationSourceCustomerService>();
            kernel.Bind<IRegistrationSourceCustomerRepository>().To<RegistrationSourceCustomerRepository>();
            kernel.Bind<ICaseSolutionConditionRepository>().To<CaseSolutionConditionRepository>();
            kernel.Bind<ICaseSolutionConditionService>().To<CaseSolutionConditionService>();
            kernel.Bind<IUniversalCaseService>().To<UniversalCaseService>();
            kernel.Bind<IExtendedCaseService>().To<ExtendedCaseService>();
            kernel.Bind<ITextTranslationService>().To<TextTranslationService>();
            kernel.Bind<IWatchDateCalendarService>().To<WatchDateCalendarService>();
            kernel.Bind<IEmployeeService>().To<EmployeeService>();
            kernel.Bind<IMetaDataService>().To<MetaDataService>();
            kernel.Bind<IWebApiService>().To<WebApiService>();
            kernel.Bind<IConditionService>().To<ConditionService>();
            kernel.Bind<ISettingsLogic>().To<SettingsLogic>();
            kernel.Bind<ICaseSectionService>().To<CaseSectionService>();
            kernel.Bind<ICaseStatisticService>().To<CaseStatisticService>();
            kernel.Bind<ICaseFollowUpService>().To<CaseFollowUpService>();
            kernel.Bind<ILogProgramService>().To<LogProgramService>();
            kernel.Bind<IInventoryService>().To<InventoryService>();
            kernel.Bind<IUserEmailsSearchService>().To<UserEmailsSearchService>();
            kernel.Bind<IFileIndexingRepository>().To<FileIndexingRepository>();
            kernel.Bind<IFeatureToggleService>().To<FeatureToggleService>();
			kernel.Bind<IContractLogRepository>().To<ContractLogRepository>();

            // Cache
            kernel.Bind<ICacheProvider>().To<CacheProvider>();

			// File view log
			kernel.Bind<IFileViewLogService>().To<FileViewLogService>();

			// FormLib
			var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DSN"].ConnectionString;

            kernel.Bind<ECT.Model.Abstract.IGlobalViewRepository>()
            .To<ECT.Model.Contrete.GlobalViewRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);

            kernel.Bind<ECT.Model.Abstract.IContractRepository>()
            .To<ECT.Model.Contrete.ContractRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);

            kernel.Bind<ECT.Model.Abstract.IUserRepository>()
            .To<ECT.Model.Contrete.UserRepository>().InRequestScope().WithConstructorArgument("connectionString", connectionString);

            kernel.Bind<ECT.Core.Service.IFileService>().To<ECT.Service.FileService>().InRequestScope();

			
		}        
    }
   
}

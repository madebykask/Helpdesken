using DH.Helpdesk.NewSelfService;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace DH.Helpdesk.NewSelfService
{
    using System;
    using System.Web;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.Repositories;
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
    using DH.Helpdesk.NewSelfService.Infrastructure.WorkContext;
    using DH.Helpdesk.NewSelfService.Infrastructure.WorkContext.Concrete;
    using DH.Helpdesk.NewSelfService.NinjectModules.Modules;
    using DH.Helpdesk.NewSelfService.NinjectModules.Modules;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Infrastructure.Concrete;
    using DH.Helpdesk.Services.Services;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using DH.Helpdesk.Dal.Infrastructure.Concrete;
    using DH.Helpdesk.Dal.Repositories.Concrete;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.NewSelfService.Infrastructure.Tools;
    using DH.Helpdesk.NewSelfService.Infrastructure.Tools.Concrete;
    using DH.Helpdesk.Dal.Repositories.Problem.Concrete;
    
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
            var kernel = new StandardKernel(new WorkContextModule(),  new UserModule() , new ProblemModule() , new CommonModule(), new EmailModule() , new NotifiersModule());
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Data Infrastructure
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IFilesStorage>().To<FilesStorage>().InRequestScope();
            kernel.Bind<IUserTemporaryFilesStorageFactory>().To<UserTemporaryFilesStorageFactory>().InRequestScope();
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();
            kernel.Bind<IEmailSendingSettingsProvider>().To<EmailSendingSettingsProvider>().InRequestScope();


            // Repositories
            kernel.Bind<ICustomerRepository>().To<CustomerRepository>();
            kernel.Bind<ITextRepository>().To<TextRepository>();
            kernel.Bind<ILanguageRepository>().To<LanguageRepository>();
            kernel.Bind<IReportCustomerRepository>().To<ReportCustomerRepository>();
            kernel.Bind<ICaseFieldSettingRepository>().To<CaseFieldSettingRepository>();
            kernel.Bind<ICaseFieldSettingLanguageRepository>().To<CaseFieldSettingLanguageRepository>();
            kernel.Bind<IReportRepository>().To<ReportRepository>();
            kernel.Bind<ISettingRepository>().To<SettingRepository>();
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
            kernel.Bind<IComputerRepository>().To<ComputerRepository>();
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
            kernel.Bind<IProjectRepository>().To<ProjectRepository>();
            kernel.Bind<IFinishingCauseRepository>().To<FinishingCauseRepository>();
            kernel.Bind<IFinishingCauseCategoryRepository>().To<FinishingCauseCategoryRepository>();
            kernel.Bind<IStateSecondaryRepository>().To<StateSecondaryRepository>();
            kernel.Bind<ICaseSolutionRepository>().To<CaseSolutionRepository>();
            kernel.Bind<ICaseSolutionCategoryRepository>().To<CaseSolutionCategoryRepository>();
            kernel.Bind<ICaseSolutionScheduleRepository>().To<CaseSolutionScheduleRepository>();
            kernel.Bind<INotifierFieldSettingLanguageRepository>().To<NotifierFieldSettingLanguageRepository>();
                                      
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
            kernel.Bind<ICaseSettingsService>().To<CaseSettingsService>();
            kernel.Bind<IInfoService>().To<InfoService>();
            kernel.Bind<ICaseFileService>().To<CaseFileService>();
            kernel.Bind<ILogFileService>().To<LogFileService>();
            kernel.Bind<IDepartmentService>().To<DepartmentService>();
            kernel.Bind<ISystemService>().To<SystemService>();
            kernel.Bind<ICategoryService>().To<CategoryService>();
            kernel.Bind<ICurrencyService>().To<CurrencyService>();
            kernel.Bind<ICountryService>().To<CountryService>();
            kernel.Bind<IComputerService>().To<ComputerService>();
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
                        
            // Cache
            kernel.Bind<ICacheProvider>().To<CacheProvider>();


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

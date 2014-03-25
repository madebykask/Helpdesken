using DH.Helpdesk.SelfService;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace DH.Helpdesk.SelfService
{
    using System;
    using System.Web;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using DH.Helpdesk.Dal.Infrastructure.Concrete;
    using DH.Helpdesk.Dal.Repositories.Concrete;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.SelfService.Infrastructure.Tools;
    using DH.Helpdesk.SelfService.Infrastructure.Tools.Concrete;

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
            var kernel = new StandardKernel();
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
            
              

            // Cache
            kernel.Bind<ICacheProvider>().To<CacheProvider>();
        }        
    }
}

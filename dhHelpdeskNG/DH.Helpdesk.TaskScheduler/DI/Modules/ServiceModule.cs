using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Dal.Infrastructure.Translate;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Gdpr.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Users.BusinessModelToEntity;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.ADFS;
using DH.Helpdesk.Dal.Repositories.ADFS.Concrete;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Dal.Repositories.Concrete;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Dal.Repositories.MetaData;
using DH.Helpdesk.Dal.Repositories.MetaData.Concrete;
using DH.Helpdesk.Dal.Repositories.Users;
using DH.Helpdesk.Dal.Repositories.Users.Concrete;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Domain.Users;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users.Concrete;
using DH.Helpdesk.Services.BusinessLogic.Gdpr;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Services.Services.EmployeeService;
using DH.Helpdesk.Services.Services.EmployeeService.Concrete;
using DH.Helpdesk.TaskScheduler.Infrastructure.Context;
using DH.Helpdesk.TaskScheduler.Infrastructure.Translate;
using DH.Helpdesk.TaskScheduler.Services;
using Ninject.Modules;
using DailyReportService = DH.Helpdesk.TaskScheduler.Services.DailyReportService;
using IDailyReportService = DH.Helpdesk.TaskScheduler.Services.IDailyReportService;

namespace DH.Helpdesk.TaskScheduler.DI.Modules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            // services
            Bind<IDailyReportService>().To<DailyReportService>();
            Bind<IImportInitiatorService>().To<ImportInitiatorService>();
            Bind<IOUService>().To<OUService>();
            Bind<IGDPRTasksService>().To<GDPRTasksService>();
            Bind<IGDPRDataPrivacyProcessor>().To<GDPRDataPrivacyProcessor>();
            Bind<IGDPRDataPrivacyCasesService>().To<GDPRService>();
            Bind<IGDPRDataPrivacyAccessRepository>().To<GDPRDataPrivacyAccessRepository>();
            Bind<IBusinessModelToEntityMapper<GdprFavoriteModel, GDPRDataPrivacyFavorite>>()
                    .To<GdprFavoriteModelToEntityMapper>()
                    .InSingletonScope();
            Bind<ISettingsLogic>().To<SettingsLogic>();
            Bind<ICaseDeletionService>().To<CaseDeletionService>();
            Bind<ICaseRepository>().To<CaseRepository>();
            Bind<ICaseFileRepository>().To<CaseFileRepository>();
            Bind<ILogRepository>().To<LogRepository>();
            Bind<IMasterDataService>().To<MasterDataService>();
            Bind<IUserService>().To<UserService>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserGroupRepository>().To<UserGroupRepository>();
            Bind<IAccountActivityRepository>().To<AccountActivityRepository>();
            Bind<ICustomerUserRepository>().To<CustomerUserRepository>();
            Bind<IOrderTypeRepository>().To<OrderTypeRepository>();
            Bind<IUserModuleRepository>().To<UserModuleRepository>();
            Bind<IModuleRepository>().To<ModuleRepository>();
            Bind<ICaseSettingRepository>().To<CaseSettingRepository>();
            Bind<IContractCategoryRepository>().To<ContractCategoryRepository>();
            Bind<ITranslator>().To<Translator>().InSingletonScope();
            Bind<IUsersPasswordHistoryRepository>().To<UsersPasswordHistoryRepository>();
            Bind<IUserPermissionsChecker>().To<UserPermissionsChecker>();
            Bind<IUserRoleRepository>().To<UserRoleRepository>();
            Bind<IWorkingGroupRepository>().To<WorkingGroupRepository>();
            Bind<IUserWorkingGroupRepository>().To<UserWorkingGroupRepository>();
            Bind<IUserContext>().To<UserContext>();
            Bind<IWorkContext>().To<WorkContext>();
            Bind<ILogFileRepository>().To<LogFileRepository>();
            Bind<ITextRepository>().To<TextRepository>();
            Bind<ICaseFieldSettingLanguageRepository>().To<CaseFieldSettingLanguageRepository>();
            Bind<ICacheProvider>().To<CacheProvider>();
            Bind<IADFSRepository>().To<ADFSRepository>();
            Bind<IEmployeeService>().To<EmployeeService>();
            Bind<IMetaDataService>().To<MetaDataService>();
            Bind<IMetaDataRepository>().To<MetaDataRepository>();
            Bind<IEntityInfoRepository>().To<EntityInfoRepository>();
            Bind<IComputerUsersRepository>().To<ComputerUsersRepository>();
            Bind<ILogProgramService>().To<LogProgramService>();
            Bind<ILogProgramRepository>().To<LogProgramRepository>();

            Bind<IBusinessModelToEntityMapper<UserModule, UserModuleEntity>>().To<UpdatedUserModuleToUserModuleEntityMapper>();
            Bind<IBusinessModelToEntityMapper<CaseModel, Case>>().To<CaseModelToEntityMapper>();
            Bind<IEntityToBusinessModelMapper<Case, CaseModel>>().To<CaseToCaseModelMapper>();
        }
    }
}
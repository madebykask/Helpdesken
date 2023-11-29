using DH.Helpdesk.BusinessData.Models.Case.Input;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers.Concrete;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Gdpr.EntityToBusinessModel;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Concrete;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Dal.Repositories.Notifiers;
using DH.Helpdesk.Dal.Repositories.Notifiers.Concrete;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Services.BusinessLogic.Gdpr;
using Ninject.Modules;
using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;
using UnitOfWork = DH.Helpdesk.Dal.Infrastructure.UnitOfWork;

namespace DH.Helpdesk.Mail2TicketCSharpBridge.DI.Modules
{
    public class DatabaseModule : NinjectModule
    {
        public override void Load()
        {
            //Dapper
            Bind<IDbQueryExecutor>().To<SqlDbQueryExecutor>();
            Bind<IDbQueryExecutorFactory>().To<SqlDbQueryExecutorFactory>();

            //EF
#pragma warning disable 0618
            Bind<IUnitOfWork>().To<UnitOfWork>();
#pragma warning restore 0618
            Bind<IDatabaseFactory>().To<DatabaseFactory>();
            
            Bind<ISessionFactory>().To<HelpdeskSessionFactory>();
            Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>();

            //EF: repositories
            Bind<INotifierFieldSettingsFactory>().To<NotifierFieldSettingsFactory>();
            Bind<INotifierFieldSettingRepository>().To<NotifierFieldSettingRepository>();
            Bind<INotifierRepository>().To<NotifierRepository>();
            Bind<IDepartmentRepository>().To<DepartmentRepository>();
            Bind<ILanguageRepository>().To<LanguageRepository>();
            Bind<IDivisionRepository>().To<DivisionRepository>();
            Bind<IDomainRepository>().To<DomainRepository>();
            Bind<ICustomerRepository>().To<CustomerRepository>();
            Bind<ISettingRepository>().To<SettingRepository>();
            Bind<IRegionRepository>().To<RegionRepository>();
            Bind<IOrganizationUnitRepository>().To<OrganizationUnitRepository>();
            Bind<IGlobalSettingRepository>().To<GlobalSettingRepository>();
            Bind<IGDPRTaskRepository>().To<GDPRTaskRepository>();
            Bind<IGDPROperationsAuditRespository>().To<GDPROperationsAuditRespository>();
            Bind<IGDPRDataPrivacyFavoriteRepository>().To<GDPRDataPrivacyFavoriteRepository>();
            Bind<IDataPrivacyTaskProgress>().To<DataPrivacyTaskProgress>();
            
            //Mappers
            RegisterMappers();
        }

        private void RegisterMappers()
        {
            Bind<IEntityToBusinessModelMapper<Setting, CustomerSettings>>()
                .To<CustomerSettingsToBusinessModelMapper>()
                .InSingletonScope();

            Bind<IEntityToBusinessModelMapper<GDPRDataPrivacyFavorite, GdprFavoriteModel>>()
                .To<GdprFavoriteEntityToModelMapper>()
                .InSingletonScope();

            Bind<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>()
                .To<CaseNotifierToEntityMapper>()
                .InSingletonScope();
        }
    }
}
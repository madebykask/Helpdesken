using DH.Helpdesk.BusinessData.Models.Case.Input;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers.Concrete;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Concrete;
using DH.Helpdesk.Dal.Repositories.Notifiers;
using DH.Helpdesk.Dal.Repositories.Notifiers.Concrete;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Computers;
using Ninject.Modules;

namespace DH.Helpdesk.TaskScheduler.DI
{
    public class DatabaseModule : NinjectModule
    {
        public override void Load()
        {
            //todo: check if all registrations have been moved correctly!

            //Dapper
            Bind<IDbQueryExecutor>().To<SqlDbQueryExecutor>();
            Bind<IDbQueryExecutorFactory>().To<SqlDbQueryExecutorFactory>();

            //EF
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IDatabaseFactory>().To<DatabaseFactory>();
            
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

            //Mappers
            RegisterMappers();
        }

        private void RegisterMappers()
        {
            Bind<IEntityToBusinessModelMapper<Setting, CustomerSettings>>()
                .To<CustomerSettingsToBusinessModelMapper>()
                .InSingletonScope();

            Bind<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>()
                .To<CaseNotifierToEntityMapper>()
                .InSingletonScope();
        }
    }
}
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.TaskScheduler.Jobs;
using DH.Helpdesk.TaskScheduler.Managers;
using DH.Helpdesk.TaskScheduler.Services;
using Ninject.Extensions.Conventions.Extensions;
using Quartz;
using DH.Helpdesk.Dal.Repositories.Notifiers;
using DH.Helpdesk.Dal.Repositories.Notifiers.Concrete;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers.Concrete;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.BusinessData.Models.Case.Input;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel;

namespace DH.Helpdesk.TaskScheduler.DI
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceConfigurationManager>().To<ServiceConfigurationManager>().InSingletonScope();

            Bind<IDbQueryExecutor>().To<SqlDbQueryExecutor>();
            Bind<IDbQueryExecutorFactory>().To<SqlDbQueryExecutorFactory>();
            Bind<IDailyReportService>().To<DailyReportService>();
            Bind<IImportInitiatorService>().To<ImportInitiatorService>();

            Bind<IJob>().To<ImportInitiatorJob>();
            Bind<IJob>().To<DailyReportJob>();//TODO: scan assambly for jobs

            Bind<IDatabaseFactory>().To<DatabaseFactory>();
            Bind<INotifierFieldSettingsFactory>().To<NotifierFieldSettingsFactory>();            
            Bind<INotifierFieldSettingRepository>().To<NotifierFieldSettingRepository>();
            Bind<INotifierRepository>().To<NotifierRepository>();
            Bind<IDepartmentRepository>().To<DepartmentRepository>();
            Bind<ILanguageRepository>().To<LanguageRepository>();
            Bind<IDivisionRepository>().To<DivisionRepository>();
            Bind<IDomainRepository>().To<DomainRepository>();
            Bind<ICustomerRepository>().To<CustomerRepository>();
            Bind<ISettingRepository>().To<SettingRepository>();

            Bind<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>()
                .To<CaseNotifierToEntityMapper>().InSingletonScope();
           
            Bind<IEntityToBusinessModelMapper<Setting, CustomerSettings>>()
                .To<CustomerSettingsToBusinessModelMapper>()
                .InSingletonScope();

        }
    }
}




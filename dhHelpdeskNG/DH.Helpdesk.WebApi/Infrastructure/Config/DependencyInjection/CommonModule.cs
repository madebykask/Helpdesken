using DH.Helpdesk.BusinessData.Models.Calendar.Output;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.BusinessData.Models.Case.Input;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.Condition;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.Models.Customer.Input;
using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.BusinessData.Models.Projects.Input;
using DH.Helpdesk.BusinessData.Models.Projects.Output;
using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.Common;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.Dal.Infrastructure.Translate;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Dal.MapperData.Logs;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Mappers.Calendars.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Calendars.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Condition;
using DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Logs;
using DH.Helpdesk.Dal.Mappers.ProductArea.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.ProductArea.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Projects;
using DH.Helpdesk.Dal.Mappers.Users.BusinessModelToEntity;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Domain.Projects;
using DH.Helpdesk.Domain.Users;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users.Concrete;
using DH.Helpdesk.WebApi.Infrastructure.Cache;
using DH.Helpdesk.WebApi.Infrastructure.Translate;
using Ninject.Modules;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
{
    /// <summary>
    /// The common module.
    /// </summary>
    public class CommonModule : NinjectModule
    {
        /// <summary>
        /// The load.
        /// </summary>
        public override void Load()
        {
            this.Bind<ICacheService>()
                .To<WebCacheService>();

            //this.Bind<IHelpdeskCache>()
            //    .To<HelpdeskCache>();

            //this.Bind<IModulesInfoFactory>().To<ModulesInfoFactory>().InSingletonScope();

            //this.Bind<IDbQueryExecutorFactory>()
            //    .To<SqlDbQueryExecutorFactory>()
            //    .InSingletonScope();

            //this.Bind<IJsonSerializeService>()
            //    .To<JsonSerializeService>()
            //    .InSingletonScope();

            Bind<IUserPermissionsChecker>().To<UserPermissionsChecker>();
            Bind<IApplicationConfiguration>().To<ApplicationConfiguration>();

            //    Bind<IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview>>()
            //        .To<ProductAreaToOverviewMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<ProductAreaOverview, ProductArea>>()
            //        .To<ProductAreaToEntityMapper>()
            //        .InSingletonScope();

            Bind<IEntityToBusinessModelMapper<Setting, CustomerSettings>>()
                .To<CustomerSettingsToBusinessModelMapper>()
                .InSingletonScope();

            Bind<IBusinessModelToEntityMapper<UserModule, UserModuleEntity>>()
                .To<UpdatedUserModuleToUserModuleEntityMapper>()
                .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>()
            //        .To<CaseNotifierToEntityMapper>()
            //        .InSingletonScope();

            Bind<ITranslator>().To<Translator>().InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<InvoiceArticle, InvoiceArticleEntity>>()
            //        .To<InvoiceArticleToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<InvoiceArticleUnit, InvoiceArticleUnitEntity>>()
            //        .To<InvoiceArticleUnitToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle>>()
            //        .To<InvoiceArticleToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit>>()
            //        .To<InvoiceArticleUnitToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle>>()
            //        .To<CaseInvoiceArticleToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity>>()
            //        .To<CaseInvoiceArticleToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<Case, CaseOverview>>()
            //       .To<CaseToBusinessModelMapper>()
            //       .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<Customer, CustomerOverview>>()
            //       .To<CustomerToBusinessModel>()
            //       .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice>>()
            //       .To<CaseInvoiceToBusinessModelMapper>()               
            //       .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity>>()
            //        .To<CaseInvoiceToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity>>()
            //        .To<CaseInvoiceOrderToEntityMapper>()
            //        .InSingletonScope();


            //    Bind<IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile>>()
            //        .To<CaseInvoiceOrderFileToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity>>()
            //        .To<CaseInvoiceOrderFileToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CaseLock, CaseLockEntity>>()
            //     .To<CaseLockToEntityMapper>()
            //     .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<CaseLockEntity, CaseLock>>()
            //        .To<CaseLockToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CalendarOverview, Calendar>>()
            //        .To<CalendarToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<Calendar, CalendarOverview>>()
            //        .To<CalendarToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CaseFilterFavorite, CaseFilterFavoriteEntity>>()
            //        .To<CaseFilterFavoritToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite>>()
            //        .To<CaseFilterFavoriteToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<CaseInvoiceSettingsEntity, CaseInvoiceSettings>>()
            //        .To<CaseInvoiceSettingsToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CaseInvoiceSettings, CaseInvoiceSettingsEntity>>()
            //        .To<CaseInvoiceSettingsToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<INewBusinessModelToEntityMapper<NewProject, Project>>()
            //       .To<NewProjectToProjectEntityMapper>()
            //       .InSingletonScope();

            //    Bind<INewBusinessModelToEntityMapper<NewProjectCollaborator, ProjectCollaborator>>()
            //       .To<NewProjectCollaboratorlToProjectCollaboratorEntityMapper>()
            //       .InSingletonScope();

            //    Bind<INewBusinessModelToEntityMapper<NewProjectFile, ProjectFile>>()
            //        .To<NewProjectFileToProjectFileEntityMapper>()
            //        .InSingletonScope();

            //    Bind<INewBusinessModelToEntityMapper<NewProjectSchedule, ProjectSchedule>>()
            //        .To<NewProjectScheduleToProjectScheduleEntityMapper>()
            //        .InSingletonScope();

            //    Bind<INewBusinessModelToEntityMapper<NewProjectLog, ProjectLog>>()
            //        .To<NewProjectLogToProjectLogEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<UpdatedProject, Project>>()
            //        .To<UpdatedProjectToProjectEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule>>()
            //        .To<UpdatedProjectScheduleToProjectScheduleEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<Project, ProjectOverview>>()
            //        .To<ProjectEntityToProjectOverviewMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview>>()
            //        .To<ProjectCollaboratorEntityToNewProjectCollaboratorMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview>>()
            //        .To<ProjectFileEntityToProjectFileOverviewMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview>>()
            //        .To<ProjectLogEntityToProjectLogOverviewMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview>>()
            //        .To<ProjectScheduleEntityToProjectScheduleOverviewMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<CaseExtraFollower, ExtraFollower>>()
            //        .To<CaseExtraFollowersToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<CaseSolutionConditionModel, CaseSolutionConditionEntity>>()
            //        .To<CaseSolutionConditionToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<CaseSolutionConditionEntity, CaseSolutionConditionModel>>()
            //        .To<CaseSolutionConditionToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<CaseHistoryMapperData, CaseHistoryOverview>>()
            //        .To<CaseHistoryToCaseHistoryOverviewMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<LogMapperData, LogOverview>>()
            //        .To<LogEntityToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IDbQueryExecutorFactory>()
            //        .To<SqlDbQueryExecutorFactory>()
            //        .InSingletonScope();


            //    Bind<IBusinessModelToEntityMapper<CaseModel, Case>>()
            //        .To<CaseModelToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<Case, CaseModel>>()
            //        .To<CaseToCaseModelMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<ExtendedCaseFormModel, ExtendedCaseFormEntity>>()
            //       .To<ExtendedCaseFormToEntityMapper>()
            //       .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel>>()
            //        .To<ExtendedCaseFormToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity>>()
            //        .To<ExtendedCaseDataToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel>>()
            //        .To<ExtendedCaseDataToBusinessModelMapper>()
            //        .InSingletonScope();

            //    Bind<IBusinessModelToEntityMapper<ExtendedCaseValueModel, ExtendedCaseValueEntity>>()
            //        .To<ExtendedCaseValueToEntityMapper>()
            //        .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<ExtendedCaseValueEntity, ExtendedCaseValueModel>>()
            //        .To<ExtendedCaseValueToBusinessModelMapper>()
            //        .InSingletonScope();


            //    Bind<IBusinessModelToEntityMapper<ConditionModel, ConditionEntity>>()
            //               .To<ConditionToEntityMapper>()
            //               .InSingletonScope();

            //    Bind<IEntityToBusinessModelMapper<ConditionEntity, ConditionModel>>()
            //        .To<ConditionToBusinessModelMapper>()
            //        .InSingletonScope();
        }
        }
}
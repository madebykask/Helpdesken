using Autofac;
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

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
{
    /// <summary>
    /// The common module.
    /// </summary>
    public class CommonModule : Module
    {
        /// <summary>
        /// The load.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WebCacheService>()
                .As<ICacheService>();
            
            //this. builder.RegisterType<IHelpdeskCache>()
            //    .As<HelpdeskCache>();

            //this. builder.RegisterType<IModulesInfoFactory>().As<ModulesInfoFactory>().SingleInstance();

            //this. builder.RegisterType<IDbQueryExecutorFactory>()
            //    .As<SqlDbQueryExecutorFactory>()
            //    .SingleInstance();

            //this. builder.RegisterType<IJsonSerializeService>()
            //    .As<JsonSerializeService>()
            //    .SingleInstance();

             builder.RegisterType<UserPermissionsChecker>().As<IUserPermissionsChecker>();
             builder.RegisterType<ApplicationConfiguration>().As<IApplicationConfiguration>().InstancePerRequest();

            //     builder.RegisterType<IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview>>()
            //        .As<ProductAreaToOverviewMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<ProductAreaOverview, ProductArea>>()
            //        .As<ProductAreaToEntityMapper>()
            //        .SingleInstance();

             builder.RegisterType<CustomerSettingsToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<Setting, CustomerSettings>>()
                .SingleInstance();

             builder.RegisterType<UpdatedUserModuleToUserModuleEntityMapper>()
                .As<IBusinessModelToEntityMapper<UserModule, UserModuleEntity>>()
                .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>()
            //        .As<CaseNotifierToEntityMapper>()
            //        .SingleInstance();

             builder.RegisterType<Translator>().As<ITranslator>().SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<InvoiceArticle, InvoiceArticleEntity>>()
            //        .As<InvoiceArticleToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<InvoiceArticleUnit, InvoiceArticleUnitEntity>>()
            //        .As<InvoiceArticleUnitToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle>>()
            //        .As<InvoiceArticleToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit>>()
            //        .As<InvoiceArticleUnitToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle>>()
            //        .As<CaseInvoiceArticleToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity>>()
            //        .As<CaseInvoiceArticleToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<Case, CaseOverview>>()
            //       .As<CaseToBusinessModelMapper>()
            //       .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<Customer, CustomerOverview>>()
            //       .As<CustomerToBusinessModel>()
            //       .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice>>()
            //       .As<CaseInvoiceToBusinessModelMapper>()               
            //       .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity>>()
            //        .As<CaseInvoiceToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity>>()
            //        .As<CaseInvoiceOrderToEntityMapper>()
            //        .SingleInstance();


            //     builder.RegisterType<IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile>>()
            //        .As<CaseInvoiceOrderFileToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity>>()
            //        .As<CaseInvoiceOrderFileToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseLock, CaseLockEntity>>()
            //     .As<CaseLockToEntityMapper>()
            //     .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<CaseLockEntity, CaseLock>>()
            //        .As<CaseLockToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CalendarOverview, Calendar>>()
            //        .As<CalendarToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<Calendar, CalendarOverview>>()
            //        .As<CalendarToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseFilterFavorite, CaseFilterFavoriteEntity>>()
            //        .As<CaseFilterFavoritToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite>>()
            //        .As<CaseFilterFavoriteToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<CaseInvoiceSettingsEntity, CaseInvoiceSettings>>()
            //        .As<CaseInvoiceSettingsToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseInvoiceSettings, CaseInvoiceSettingsEntity>>()
            //        .As<CaseInvoiceSettingsToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<INewBusinessModelToEntityMapper<NewProject, Project>>()
            //       .As<NewProjectToProjectEntityMapper>()
            //       .SingleInstance();

            //     builder.RegisterType<INewBusinessModelToEntityMapper<NewProjectCollaborator, ProjectCollaborator>>()
            //       .As<NewProjectCollaboratorlToProjectCollaboratorEntityMapper>()
            //       .SingleInstance();

            //     builder.RegisterType<INewBusinessModelToEntityMapper<NewProjectFile, ProjectFile>>()
            //        .As<NewProjectFileToProjectFileEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<INewBusinessModelToEntityMapper<NewProjectSchedule, ProjectSchedule>>()
            //        .As<NewProjectScheduleToProjectScheduleEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<INewBusinessModelToEntityMapper<NewProjectLog, ProjectLog>>()
            //        .As<NewProjectLogToProjectLogEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<UpdatedProject, Project>>()
            //        .As<UpdatedProjectToProjectEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule>>()
            //        .As<UpdatedProjectScheduleToProjectScheduleEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<Project, ProjectOverview>>()
            //        .As<ProjectEntityToProjectOverviewMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview>>()
            //        .As<ProjectCollaboratorEntityToNewProjectCollaboratorMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview>>()
            //        .As<ProjectFileEntityToProjectFileOverviewMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview>>()
            //        .As<ProjectLogEntityToProjectLogOverviewMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview>>()
            //        .As<ProjectScheduleEntityToProjectScheduleOverviewMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<CaseExtraFollower, ExtraFollower>>()
            //        .As<CaseExtraFollowersToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseSolutionConditionModel, CaseSolutionConditionEntity>>()
            //        .As<CaseSolutionConditionToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<CaseSolutionConditionEntity, CaseSolutionConditionModel>>()
            //        .As<CaseSolutionConditionToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<CaseHistoryMapperData, CaseHistoryOverview>>()
            //        .As<CaseHistoryToCaseHistoryOverviewMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<LogMapperData, LogOverview>>()
            //        .As<LogEntityToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IDbQueryExecutorFactory>()
            //        .As<SqlDbQueryExecutorFactory>()
            //        .SingleInstance();


            //     builder.RegisterType<IBusinessModelToEntityMapper<CaseModel, Case>>()
            //        .As<CaseModelToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<Case, CaseModel>>()
            //        .As<CaseToCaseModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<ExtendedCaseFormModel, ExtendedCaseFormEntity>>()
            //       .As<ExtendedCaseFormToEntityMapper>()
            //       .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel>>()
            //        .As<ExtendedCaseFormToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity>>()
            //        .As<ExtendedCaseDataToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel>>()
            //        .As<ExtendedCaseDataToBusinessModelMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IBusinessModelToEntityMapper<ExtendedCaseValueModel, ExtendedCaseValueEntity>>()
            //        .As<ExtendedCaseValueToEntityMapper>()
            //        .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<ExtendedCaseValueEntity, ExtendedCaseValueModel>>()
            //        .As<ExtendedCaseValueToBusinessModelMapper>()
            //        .SingleInstance();


            //     builder.RegisterType<IBusinessModelToEntityMapper<ConditionModel, ConditionEntity>>()
            //               .As<ConditionToEntityMapper>()
            //               .SingleInstance();

            //     builder.RegisterType<IEntityToBusinessModelMapper<ConditionEntity, ConditionModel>>()
            //        .As<ConditionToBusinessModelMapper>()
            //        .SingleInstance();
        }
        }
}
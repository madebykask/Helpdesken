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
using DH.Helpdesk.BusinessData.Models.FilewViewLog;
using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.BusinessData.Models.Problem.Input;
using DH.Helpdesk.BusinessData.Models.Problem.Output;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.BusinessData.Models.Projects.Input;
using DH.Helpdesk.BusinessData.Models.Projects.Output;
using DH.Helpdesk.BusinessData.Models.Users.Input;
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
using DH.Helpdesk.Dal.Mappers.FileViewLog.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.FileViewLog.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Logs;
using DH.Helpdesk.Dal.Mappers.Problems;
using DH.Helpdesk.Dal.Mappers.ProductArea.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.ProductArea.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Projects;
using DH.Helpdesk.Dal.Mappers.Users.BusinessModelToEntity;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Domain.Problems;
using DH.Helpdesk.Domain.Projects;
using DH.Helpdesk.Domain.Users;
using DH.Helpdesk.WebApi.Infrastructure.Translate;

namespace DH.Helpdesk.WebApi.DependencyInjection
{
    public class MappersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CausingPartToOverviewMapper>()
                .As<IEntityToBusinessModelMapper<CausingPart, CausingPartOverview>>()
                .SingleInstance();

            builder.RegisterType<CausingPartToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CausingPartOverview, CausingPart>>()
                .SingleInstance();

            builder.RegisterType<ProductAreaToOverviewMapper>()
                .As<IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview>>()
                .SingleInstance();

            builder.RegisterType<ProductAreaToEntityMapper>()
                .As<IBusinessModelToEntityMapper<ProductAreaOverview, ProductArea>>()
                .SingleInstance();


            builder.RegisterType<CustomerSettingsToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<Setting, CustomerSettings>>()
                .SingleInstance();

            builder.RegisterType<UpdatedUserModuleToUserModuleEntityMapper>()
                .As<IBusinessModelToEntityMapper<UserModule, UserModuleEntity>>()
                .SingleInstance();

            builder.RegisterType<CaseNotifierToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>()
                .SingleInstance();

            builder.RegisterType<Translator>().As<ITranslator>().SingleInstance();

            builder.RegisterType<InvoiceArticleToEntityMapper>()
                .As<IBusinessModelToEntityMapper<InvoiceArticle, InvoiceArticleEntity>>()
                .SingleInstance();

            builder.RegisterType<InvoiceArticleUnitToEntityMapper>()
                .As<IBusinessModelToEntityMapper<InvoiceArticleUnit, InvoiceArticleUnitEntity>>()
                .SingleInstance();

            builder.RegisterType<InvoiceArticleToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle>>()
                .SingleInstance();

            builder.RegisterType<InvoiceArticleUnitToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit>>()
                .SingleInstance();

            builder.RegisterType<CaseInvoiceArticleToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle>>()
                .SingleInstance();

            builder.RegisterType<CaseInvoiceArticleToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity>>()
                .SingleInstance();

            builder.RegisterType<CaseToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<Case, CaseOverview>>()
                .SingleInstance();

            builder.RegisterType<CustomerToBusinessModel>()
                .As<IEntityToBusinessModelMapper<Customer, CustomerOverview>>()
                .SingleInstance();

            builder.RegisterType<CaseInvoiceToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice>>()
                .SingleInstance();

            builder.RegisterType<CaseInvoiceToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity>>()
                .SingleInstance();

            builder.RegisterType<CaseInvoiceOrderToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity>>()
                .SingleInstance();


            builder.RegisterType<CaseInvoiceOrderFileToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile>>()
                .SingleInstance();

            builder.RegisterType<CaseInvoiceOrderFileToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity>>()
                .SingleInstance();

            builder.RegisterType<CaseLockToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseLock, CaseLockEntity>>()
                .SingleInstance();

            builder.RegisterType<CaseLockToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<CaseLockEntity, CaseLock>>()
                .SingleInstance();

            builder.RegisterType<CalendarToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CalendarOverview, Calendar>>()
                .SingleInstance();

            builder.RegisterType<CalendarToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<Calendar, CalendarOverview>>()
                .SingleInstance();

            builder.RegisterType<CaseFilterFavoritToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseFilterFavorite, CaseFilterFavoriteEntity>>()
                .SingleInstance();

            builder.RegisterType<CaseFilterFavoriteToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite>>()
                .SingleInstance();

            builder.RegisterType<CaseInvoiceSettingsToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<CaseInvoiceSettingsEntity, CaseInvoiceSettings>>()
                .SingleInstance();

            builder.RegisterType<CaseInvoiceSettingsToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseInvoiceSettings, CaseInvoiceSettingsEntity>>()
                .SingleInstance();

            builder.RegisterType<NewProjectToProjectEntityMapper>()
                .As<INewBusinessModelToEntityMapper<NewProject, Project>>()
                .SingleInstance();

            builder.RegisterType<NewProjectCollaboratorlToProjectCollaboratorEntityMapper>()
                .As<INewBusinessModelToEntityMapper<NewProjectCollaborator, ProjectCollaborator>>()
                .SingleInstance();

            builder.RegisterType<NewProjectFileToProjectFileEntityMapper>()
                .As<INewBusinessModelToEntityMapper<NewProjectFile, ProjectFile>>()
                .SingleInstance();

            builder.RegisterType<NewProjectScheduleToProjectScheduleEntityMapper>()
                .As<INewBusinessModelToEntityMapper<NewProjectSchedule, ProjectSchedule>>()
                .SingleInstance();

            builder.RegisterType<NewProjectLogToProjectLogEntityMapper>()
                .As<INewBusinessModelToEntityMapper<NewProjectLog, ProjectLog>>()
                .SingleInstance();

            builder.RegisterType<UpdatedProjectToProjectEntityMapper>()
                .As<IBusinessModelToEntityMapper<UpdatedProject, Project>>()
                .SingleInstance();

            builder.RegisterType<UpdatedProjectScheduleToProjectScheduleEntityMapper>()
                .As<IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule>>()
                .SingleInstance();

            builder.RegisterType<ProjectEntityToProjectOverviewMapper>()
                .As<IEntityToBusinessModelMapper<Project, ProjectOverview>>()
                .SingleInstance();

            builder.RegisterType<ProjectCollaboratorEntityToNewProjectCollaboratorMapper>()
                .As<IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview>>()
                .SingleInstance();

            builder.RegisterType<ProjectFileEntityToProjectFileOverviewMapper>()
                .As<IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview>>()
                .SingleInstance();

            builder.RegisterType<ProjectLogEntityToProjectLogOverviewMapper>()
                .As<IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview>>()
                .SingleInstance();

            builder.RegisterType<ProjectScheduleEntityToProjectScheduleOverviewMapper>()
                .As<IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview>>()
                .SingleInstance();

            builder.RegisterType<CaseExtraFollowersToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<CaseExtraFollower, ExtraFollower>>()
                .SingleInstance();

            builder.RegisterType<CaseSolutionConditionToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseSolutionConditionModel, CaseSolutionConditionEntity>>()
                .SingleInstance();

            builder.RegisterType<CaseSolutionConditionToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<CaseSolutionConditionEntity, CaseSolutionConditionModel>>()
                .SingleInstance();

            builder.RegisterType<CaseHistoryToCaseHistoryOverviewMapper>()
                .As<IEntityToBusinessModelMapper<CaseHistoryMapperData, CaseHistoryOverview>>()
                .SingleInstance();

            builder.RegisterType<LogEntityToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<LogMapperData, LogOverview>>()
                .SingleInstance();

            builder.RegisterType<CaseModelToEntityMapper>()
                .As<IBusinessModelToEntityMapper<CaseModel, Case>>()
                .SingleInstance();

            builder.RegisterType<CaseToCaseModelMapper>()
                .As<IEntityToBusinessModelMapper<Case, CaseModel>>()
                .SingleInstance();

            builder.RegisterType<ExtendedCaseFormToEntityMapper>()
                .As<IBusinessModelToEntityMapper<ExtendedCaseFormModel, ExtendedCaseFormEntity>>()
                .SingleInstance();

            builder.RegisterType<ExtendedCaseFormToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel>>()
                .SingleInstance();

            builder.RegisterType<ExtendedCaseDataToEntityMapper>()
                .As<IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity>>()
                .SingleInstance();

            builder.RegisterType<ExtendedCaseDataToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel>>()
                .SingleInstance();

            builder.RegisterType<ExtendedCaseValueToEntityMapper>()
                .As<IBusinessModelToEntityMapper<ExtendedCaseValueModel, ExtendedCaseValueEntity>>()
                .SingleInstance();

            builder.RegisterType<ExtendedCaseValueToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<ExtendedCaseValueEntity, ExtendedCaseValueModel>>()
                .SingleInstance();

            builder.RegisterType<ConditionToEntityMapper>()
                .As<IBusinessModelToEntityMapper<ConditionModel, ConditionEntity>>()
                .SingleInstance();

            builder.RegisterType<ConditionToBusinessModelMapper>()
                .As<IEntityToBusinessModelMapper<ConditionEntity, ConditionModel>>()
                .SingleInstance();

            builder.RegisterType<NewProblemToProblemEntityMapper>()
                .As<INewBusinessModelToEntityMapper<NewProblemDto, Problem>>()
                .SingleInstance();

            builder.RegisterType<ProblemEntityFromBusinessModelChanger>()
                .As<IBusinessModelToEntityMapper<NewProblemDto, Problem>>()
                .SingleInstance();

            builder.RegisterType<ProblemEntityToProblemOverviewMapper>()
                .As<IEntityToBusinessModelMapper<Problem, ProblemOverview>>()
                .SingleInstance();

            builder.RegisterType<NewProblemLogToProblemLogEntityMapper>()
                .As<INewBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog>>()
                .SingleInstance();

            builder.RegisterType<ProblemLogEntityFromBusinessModelChanger>()
                .As<IBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog>>()
                .SingleInstance();

            builder.RegisterType<ProblemLogEntityToProblemLogOverviewMapper>()
                .As<IEntityToBusinessModelMapper<ProblemLog, ProblemLogOverview>>()
                .SingleInstance();

            builder.RegisterType<ProblemLogEntityToNewProblemLogMapper>()
                .As<IEntityToBusinessModelMapper<ProblemLog, NewProblemLogDto>>()
                .SingleInstance();

			builder.RegisterType<FileViewLogToEntityMapper>()
				.As<IBusinessModelToEntityMapper<FileViewLogModel, FileViewLogEntity>>()
				.SingleInstance();

			builder.RegisterType<FileViewLogToBusinessModelMapper>()
				.As<IEntityToBusinessModelMapper<FileViewLogEntity, FileViewLogModel>>()
				.SingleInstance();
		}
    }
}
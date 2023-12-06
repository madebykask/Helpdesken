using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.Customer.Input;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.BusinessData.Models.Problem.Input;
using DH.Helpdesk.BusinessData.Models.Problem.Output;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.BusinessData.Models.Projects.Input;
using DH.Helpdesk.BusinessData.Models.Projects.Output;
using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email.Concrete;
using DH.Helpdesk.Dal.Infrastructure.Translate;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Dal.MapperData.Logs;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Gdpr.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Logs;
using DH.Helpdesk.Dal.Mappers.Problems;
using DH.Helpdesk.Dal.Mappers.ProductArea.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.ProductArea.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Projects;
using DH.Helpdesk.Dal.Mappers.Users.BusinessModelToEntity;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.ADFS;
using DH.Helpdesk.Dal.Repositories.ADFS.Concrete;
using DH.Helpdesk.Dal.Repositories.BusinessRules;
using DH.Helpdesk.Dal.Repositories.BusinessRules.Concrete;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Dal.Repositories.Cases.Concrete;
using DH.Helpdesk.Dal.Repositories.Concrete;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Dal.Repositories.Invoice;
using DH.Helpdesk.Dal.Repositories.Invoice.Concrete;
using DH.Helpdesk.Dal.Repositories.MailTemplates;
using DH.Helpdesk.Dal.Repositories.MailTemplates.Concrete;
using DH.Helpdesk.Dal.Repositories.MetaData;
using DH.Helpdesk.Dal.Repositories.MetaData.Concrete;
using DH.Helpdesk.Dal.Repositories.Problem;
using DH.Helpdesk.Dal.Repositories.Problem.Concrete;
using DH.Helpdesk.Dal.Repositories.Projects;
using DH.Helpdesk.Dal.Repositories.Projects.Concrete;
using DH.Helpdesk.Dal.Repositories.Questionnaire;
using DH.Helpdesk.Dal.Repositories.Questionnaire.Concrete;
using DH.Helpdesk.Dal.Repositories.Users;
using DH.Helpdesk.Dal.Repositories.Users.Concrete;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Domain.Problems;
using DH.Helpdesk.Domain.Projects;
using DH.Helpdesk.Domain.Users;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users.Concrete;
using DH.Helpdesk.Services.BusinessLogic.Gdpr;
using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.Services.Infrastructure.Concrete;
using DH.Helpdesk.Services.Infrastructure.Email;
using DH.Helpdesk.Services.Infrastructure.Email.Concrete;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Services.Services.CaseStatistic;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Services.Services.EmployeeService;
using DH.Helpdesk.Services.Services.EmployeeService.Concrete;
using DH.Helpdesk.Services.Services.Feedback;
using DH.Helpdesk.TaskScheduler.Infrastructure.Context;
using DH.Helpdesk.TaskScheduler.Infrastructure.Translate;
using DH.Helpdesk.VBCSharpBridge.Infrastructure.Cache;
using Ninject.Modules;


namespace DH.Helpdesk.VBCSharpBridge.DI.Modules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            // services

            //IFinishingCauseRepository
            Bind<IFinishingCauseRepository>().To<FinishingCauseRepository>();

            //IMail2TicketRepository
            Bind<IMail2TicketRepository>().To<Mail2TicketRepository>();

            //ICustomerService
            Bind<ICustomerService>().To<CustomerService>();

            //ICaseFieldSettingRepository
            Bind<ICaseFieldSettingRepository>().To<CaseFieldSettingRepository>();

            //IReportRepository
            Bind<IReportRepository>().To<ReportRepository>();

            //IReportCustomerRepository
            Bind<IReportCustomerRepository>().To<ReportCustomerRepository>();

            //ICaseFieldSettingService
            Bind<ICaseFieldSettingService>().To<CaseFieldSettingService>();

            //IInvoiceArticleService
            Bind<IInvoiceArticleService>().To<InvoiceArticleService>();

            //IInvoiceArticleUnitRepository
            Bind<IInvoiceArticleUnitRepository>().To<InvoiceArticleUnitRepository>();

            //IInvoiceArticleRepository
            Bind<IInvoiceArticleRepository>().To<InvoiceArticleRepository>();

            //ICaseInvoiceArticleRepository
            Bind<ICaseInvoiceArticleRepository>().To<CaseInvoiceArticleRepository>();

            //ICaseInvoiceSettingsService
            Bind<ICaseInvoiceSettingsService>().To<CaseInvoiceSettingsService>();

            //ICaseInvoiceSettingsRepository
            Bind<ICaseInvoiceSettingsRepository>().To<CaseInvoiceSettingsRepository>();

            //IProjectService
            Bind<IProjectService>().To<ProjectService>();

            //IProjectService
            Bind<IProjectRepository>().To<ProjectRepository>();

            //IProjectLogRepository
            Bind<IProjectLogRepository>().To<ProjectLogRepository>();

            //IProjectScheduleRepository
            Bind<IProjectScheduleRepository>().To<ProjectScheduleRepository>();

            //IProjectFileRepository
            Bind<IProjectFileRepository>().To<ProjectFileRepository>();

            //IProjectCollaboratorRepository
            Bind<IProjectCollaboratorRepository>().To<ProjectCollaboratorRepository>();

            //ICaseFileService
            Bind<ICaseFileService>().To<CaseFileService>();

            //IDepartmentService
            Bind<IDepartmentService>().To<DepartmentService>();

            //ISurveyService
            Bind<ISurveyService>().To<SurveyService>();

            //ICaseLockService
            Bind<ICaseLockService>().To<CaseLockService>();

            //ICaseLockRepository
            Bind<ICaseLockRepository>().To<CaseLockRepository>();

            //ICaseStatisticService
            Bind<ICaseStatisticService>().To<CaseStatisticService>();

            //IStateSecondaryService
            Bind<IStateSecondaryService>().To<StateSecondaryService>();

            //IStateSecondaryRepository
            Bind<IStateSecondaryRepository>().To<StateSecondaryRepository>();

            //ICaseFilterFavoriteRepository
            Bind<ICaseFilterFavoriteRepository>().To<CaseFilterFavoriteRepository>();

            //IBusinessRuleService
            Bind<IBusinessRuleService>().To<BusinessRuleService>();

            //IBusinessRuleRepository
            Bind<IBusinessRuleRepository>().To<BusinessRuleRepository>();

            //IEmailGroupService
            Bind<IEmailGroupService>().To<EmailGroupService>();

            //IEmailGroupService
            Bind<IEmailGroupRepository>().To<EmailGroupRepository>();

            //IEmailGroupEmailRepository
            Bind<IEmailGroupEmailRepository>().To<EmailGroupEmailRepository>();

            //IFeedbackTemplateService
            Bind<IFeedbackTemplateService>().To<FeedbackTemplateService>();

            //IFeedbackService
            Bind<IFeedbackService>().To<FeedbackService>();

            //IQuestionnaireRepository
            Bind<IQuestionnaireRepository>().To<QuestionnaireRepository>();

            //IQuestionnaireQuestionRepository
            Bind<IQuestionnaireQuestionRepository>().To<QuestionnaireQuestionRepository>();

            //ICircularService
            Bind<ICircularService>().To<CircularService>();

            //IMailTemplateFormatterNew
            Bind<IMailTemplateFormatterNew>().To<MailTemplateFormatterNew>();

            //IMailTemplateServiceNew
            Bind<IMailTemplateServiceNew>().To<MailTemplateServiceNew>();

            //IProductAreaService
            Bind<IProductAreaService>().To<ProductAreaService>();

            //IProductAreaRepository
            Bind<IProductAreaRepository>().To<ProductAreaRepository>();

            //IExtendedCaseFormRepository
            Bind<IExtendedCaseFormRepository>().To<ExtendedCaseFormRepository>();

            //IExtendedCaseDataRepository
            Bind<IExtendedCaseDataRepository>().To<ExtendedCaseDataRepository>();

            //IExtendedCaseValueRepository
            Bind<IExtendedCaseValueRepository>().To<ExtendedCaseValueRepository>();

            //ICaseFollowUpService
            Bind<ICaseFollowUpService>().To<CaseFollowUpService>();

            //ICaseFollowUpRepository
            Bind<ICaseFollowUpRepository>().To<CaseFollowUpRepository>();

            //ICaseSectionsRepository
            Bind<ICaseSectionsRepository>().To<CaseSectionsRepository>();

            //ICaseSolutionRepository
            Bind<ICaseSolutionRepository>().To<CaseSolutionRepository>();

            //ITranslateCacheService
            Bind<ITranslateCacheService>().To<TranslateCacheService>();

            //ICacheService
            Bind<ICacheService>().To<CacheService>();

            //ITextTranslationRepository
            Bind<ITextTranslationRepository>().To<TextTranslationRepository>();

            //ITextTypeRepository
            Bind<ITextTypeRepository>().To<TextTypeRepository>();

            //ICaseTypeRepository
            Bind<ICaseTypeRepository>().To<CaseTypeRepository>();

            //ICategoryRepository
            Bind<ICategoryRepository>().To<CategoryRepository>();

            //IContractLogRepository
            Bind<IContractLogRepository>().To<ContractLogRepository>();

            Bind<IProblemLogService>().To<ProblemLogService>();
            Bind<IProblemLogRepository>().To<ProblemLogRepository>();
            Bind<IProblemEMailLogRepository>().To<ProblemEMailLogRepository>();
            Bind<IProblemRepository>().To<ProblemRepository>();
            Bind<IFinishingCauseService>().To<FinishingCauseService>();
            Bind<IFinishingCauseCategoryRepository>().To<FinishingCauseCategoryRepository>();
            Bind<ILogService>().To<LogService>();
            Bind<IGlobalSettingService>().To<GlobalSettingService>();
            Bind<ICaseExtraFollowersRepository>().To<CaseExtraFollowersRepository>();
            Bind<ICaseExtraFollowersService>().To<CaseExtraFollowersService>();
            Bind<IEmailSendingSettingsProvider>().To<EmailSendingSettingsProvider>();
            Bind<IEmailFactory>().To<EmailFactory>().InSingletonScope();
            Bind<ICaseMailer>().To<CaseMailer>().InSingletonScope();
            Bind<IFormFieldValueRepository>().To<FormFieldValueRepository>();
            Bind<IEmailService>().To<EmailService>();
            Bind<IEmailLogAttemptRepository>().To<EmailLogAttemptRepository>();
            Bind<IEmailLogRepository>().To<EmailLogRepository>();
            Bind<IMailTemplateIdentifierRepository>().To<MailTemplateIdentifierRepository>();
            Bind<IMailTemplateLanguageRepository>().To<MailTemplateLanguageRepository>();
            Bind<IMailTemplateRepository>().To<MailTemplateRepository>();
            Bind<IMailTemplateService>().To<MailTemplateService>();
            Bind<ISettingService>().To<SettingService>();
            Bind<IWorkingGroupService>().To<WorkingGroupService>();
            Bind<IPriorityImpactUrgencyRepository>().To<PriorityImpactUrgencyRepository>();
            Bind<IPriorityLanguageRepository>().To<PriorityLanguageRepository>();
            Bind<IPriorityRepository>().To<PriorityRepository>();
            Bind<IPriorityService>().To<PriorityService>();
            Bind<ICaseHistoryRepository>().To<CaseHistoryRepository>();
            Bind<ICaseService>().To<CaseService>();
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

            Bind<IEntityToBusinessModelMapper<CaseExtraFollower, ExtraFollower>>().To<CaseExtraFollowersToBusinessModelMapper>();
            Bind<INewBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog>>().To<NewProblemLogToProblemLogEntityMapper>();
            Bind<IBusinessModelToEntityMapper<NewProblemLogDto, ProblemLog>>().To<ProblemLogEntityFromBusinessModelChanger>();

            Bind<IEntityToBusinessModelMapper<ProblemLog, ProblemLogOverview>>().To<ProblemLogEntityToProblemLogOverviewMapper>();
            Bind<IEntityToBusinessModelMapper<ProblemLog, NewProblemLogDto>>().To<ProblemLogEntityToNewProblemLogMapper>();

            Bind<INewBusinessModelToEntityMapper<NewProblemDto, Problem>>().To<NewProblemToProblemEntityMapper>();
            Bind<IBusinessModelToEntityMapper<NewProblemDto, Problem>>().To<ProblemEntityFromBusinessModelChanger>();

            Bind<IEntityToBusinessModelMapper<Problem, ProblemOverview>>().To<ProblemEntityToProblemOverviewMapper>();

            Bind<IEntityToBusinessModelMapper<LogMapperData, LogOverview>>().To<LogEntityToBusinessModelMapper>();

            Bind<IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit>>().To<InvoiceArticleUnitToBusinessModelMapper>();
            Bind<IBusinessModelToEntityMapper<InvoiceArticleUnit, InvoiceArticleUnitEntity>>().To<InvoiceArticleUnitToEntityMapper>();

            Bind<IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle>>().To<InvoiceArticleToBusinessModelMapper>();

            Bind<IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview>>().To<ProductAreaToOverviewMapper>();

            Bind<IEntityToBusinessModelMapper<Customer, CustomerOverview>>().To<CustomerToBusinessModel>();

            Bind<IBusinessModelToEntityMapper<InvoiceArticle, InvoiceArticleEntity>>().To<InvoiceArticleToEntityMapper>();

            Bind<IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice>>().To<CaseInvoiceToBusinessModelMapper>();
            Bind<IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity>>().To<CaseInvoiceToEntityMapper>();

            Bind<IEntityToBusinessModelMapper<Case, CaseOverview>>().To<CaseToBusinessModelMapper>();

            Bind<IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile>>().To<CaseInvoiceOrderFileToBusinessModelMapper>();
            Bind<IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity>>().To<CaseInvoiceOrderFileToEntityMapper>();

            Bind<IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity>>().To<CaseInvoiceOrderToEntityMapper>();

            Bind<IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle>>().To<CaseInvoiceArticleToBusinessModelMapper>();
            Bind<IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity>>().To<CaseInvoiceArticleToEntityMapper>();

            Bind<IEntityToBusinessModelMapper<CaseInvoiceSettingsEntity, CaseInvoiceSettings>>().To<CaseInvoiceSettingsToBusinessModelMapper>();
            Bind<IBusinessModelToEntityMapper<CaseInvoiceSettings, CaseInvoiceSettingsEntity>>().To<CaseInvoiceSettingsToEntityMapper>();

            Bind<INewBusinessModelToEntityMapper<NewProject, Project>>().To<NewProjectToProjectEntityMapper>();

            Bind<IBusinessModelToEntityMapper<UpdatedProject, Project>>().To<UpdatedProjectToProjectEntityMapper>();

            Bind<IEntityToBusinessModelMapper<Project, ProjectOverview>>().To<ProjectEntityToProjectOverviewMapper>();

            Bind<INewBusinessModelToEntityMapper<NewProjectLog, ProjectLog>>().To<NewProjectLogToProjectLogEntityMapper>();

            Bind<IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview>>().To<ProjectLogEntityToProjectLogOverviewMapper>();

            Bind<INewBusinessModelToEntityMapper<NewProjectSchedule, ProjectSchedule>>().To<NewProjectScheduleToProjectScheduleEntityMapper>();

            Bind<IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule>>().To<UpdatedProjectScheduleToProjectScheduleEntityMapper>();

            Bind<IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview>>().To<ProjectScheduleEntityToProjectScheduleOverviewMapper>();

            Bind<INewBusinessModelToEntityMapper<NewProjectFile, ProjectFile>>().To<NewProjectFileToProjectFileEntityMapper>();

            Bind<IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview>>().To<ProjectFileEntityToProjectFileOverviewMapper>();

            Bind<INewBusinessModelToEntityMapper<NewProjectCollaborator, ProjectCollaborator>>().To<NewProjectCollaboratorlToProjectCollaboratorEntityMapper>();

            Bind<IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview>>().To<ProjectCollaboratorEntityToNewProjectCollaboratorMapper>();

            Bind<IBusinessModelToEntityMapper<CaseLock, CaseLockEntity>>().To<CaseLockToEntityMapper>();
            Bind<IEntityToBusinessModelMapper<CaseLockEntity, CaseLock>>().To<CaseLockToBusinessModelMapper>();

            Bind<IBusinessModelToEntityMapper<CaseFilterFavorite, CaseFilterFavoriteEntity>>().To<CaseFilterFavoritToEntityMapper>();
            Bind<IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite>>().To<CaseFilterFavoriteToBusinessModelMapper>();

            //Bind<IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview>>().To<ProductAreaToOverviewMapper>();
            Bind<IBusinessModelToEntityMapper<ProductAreaOverview, ProductArea>>().To<ProductAreaToEntityMapper>();

            Bind<IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity>>().To<ExtendedCaseDataToEntityMapper>();
            Bind<IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel>>().To<ExtendedCaseDataToBusinessModelMapper>();

            Bind<IEntityToBusinessModelMapper<CaseHistoryMapperData, CaseHistoryOverview>>().To<CaseHistoryToCaseHistoryOverviewMapper>();
        }
    }
}
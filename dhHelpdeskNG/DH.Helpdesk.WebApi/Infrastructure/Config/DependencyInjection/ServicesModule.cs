using Autofac;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Services.Services.CaseStatistic;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Services.Services.Concrete.Changes;
using DH.Helpdesk.Services.Services.EmployeeService;
using DH.Helpdesk.Services.Services.EmployeeService.Concrete;
using DH.Helpdesk.Services.Services.Feedback;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
{
    public sealed class ServicesModule : Module
    {
        #region Public Methods and Operators

        protected override void Load(ContainerBuilder builder)
        {
            //services
            builder.RegisterType<TranslateCacheService>().As<ITranslateCacheService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<HolidayService>().As<IHolidayService>();
            builder.RegisterType<DepartmentService>().As<IDepartmentService>();
            builder.RegisterType<CaseSearchService>().As<ICaseSearchService>();
            builder.RegisterType<ProductAreaService>().As<IProductAreaService>();
            builder.RegisterType<GlobalSettingService>().As<IGlobalSettingService>();
            builder.RegisterType<SettingService>().As<ISettingService>();
            builder.RegisterType<CaseSettingsService>().As<ICaseSettingsService>();
            builder.RegisterType<CaseFieldSettingService>().As<ICaseFieldSettingService>();
            builder.RegisterType<CustomerUserService>().As<ICustomerUserService>();
            builder.RegisterType<TextTranslationService>().As<ITextTranslationService>();
            builder.RegisterType<LanguageService>().As<ILanguageService>();
            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterType<PriorityService>().As<IPriorityService>();
            builder.RegisterType<WorkingGroupService>().As<IWorkingGroupService>();
            builder.RegisterType<MailTemplateService>().As<IMailTemplateService>();
            builder.RegisterType<EmailService>().As<IEmailService>();
            builder.RegisterType<FeedbackTemplateService>().As<IFeedbackTemplateService>();
            builder.RegisterType<InvoiceArticleService>().As<IInvoiceArticleService>();
            builder.RegisterType<SurveyService>().As<ISurveyService>();
            builder.RegisterType<FinishingCauseService>().As<IFinishingCauseService>();
            builder.RegisterType<CaseLockService>().As<ICaseLockService>();
            builder.RegisterType<BusinessRuleService>().As<IBusinessRuleService>();
            builder.RegisterType<EmailGroupService>().As<IEmailGroupService>();
            builder.RegisterType<CaseExtraFollowersService>().As<ICaseExtraFollowersService>();
            builder.RegisterType<CaseFollowUpService>().As<ICaseFollowUpService>();
            builder.RegisterType<CaseStatisticService>().As<CaseStatisticService>();//TODO: create interface fo it
            builder.RegisterType<CaseInvoiceSettingsService>().As<ICaseInvoiceSettingsService>();
            builder.RegisterType<ProjectService>().As<IProjectService>();
            builder.RegisterType<CaseFileService>().As<ICaseFileService>();
            builder.RegisterType<MasterDataService>().As<IMasterDataService>();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>();
            builder.RegisterType<LogProgramService>().As<ILogProgramService>();
            builder.RegisterType<MetaDataService>().As<IMetaDataService>();
            builder.RegisterType<FeedbackService>().As<IFeedbackService>();
            builder.RegisterType<CircularService>().As<ICircularService>();
            builder.RegisterType<MailTemplateServiceNew>().As<IMailTemplateServiceNew>();
            builder.RegisterType<CaseService>().As<ICaseService>();
            builder.RegisterType<ComputerService>().As<IComputerService>();
            builder.RegisterType<SupplierService>().As<ISupplierService>();
            builder.RegisterType<RegionService>().As<IRegionService>();
            builder.RegisterType<OrganizationService>().As<IOrganizationService>();
            builder.RegisterType<RegistrationSourceCustomerService>().As<IRegistrationSourceCustomerService>();
            builder.RegisterType<SystemService>().As<ISystemService>();
            builder.RegisterType<UrgencyService>().As<IUrgencyService>();
            builder.RegisterType<ImpactService>().As<IImpactService>();
            builder.RegisterType<CountryService>().As<ICountryService>();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>();
            builder.RegisterType<StateSecondaryService>().As<IStateSecondaryService>();
            builder.RegisterType<StatusService>().As<IStatusService>();
            builder.RegisterType<ProblemService>().As<IProblemService>();
            builder.RegisterType<BaseChangesService>().As<IBaseChangesService>();
            builder.RegisterType<OUService>().As<IOUService>();
            builder.RegisterType<CaseTypeService>().As<ICaseTypeService>();
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<CaseSectionService>().As<ICaseSectionService>();
            builder.RegisterType<BaseCaseSolutionService>().As<IBaseCaseSolutionService>();

        }

        #endregion
    }
}
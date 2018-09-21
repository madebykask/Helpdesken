using Autofac;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.CaseStatistic;
using DH.Helpdesk.Services.Services.Concrete;
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
            
            
        }

        #endregion
    }
}
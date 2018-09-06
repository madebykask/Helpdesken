using Autofac;
using DH.Helpdesk.Services.Services;

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
            
        }

        #endregion
    }
}
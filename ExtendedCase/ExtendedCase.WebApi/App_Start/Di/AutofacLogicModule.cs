using Autofac;
using ExtendedCase.Logic.CustomDataSourceProviders;
using ExtendedCase.Logic.Di;
using ExtendedCase.Logic.OptionDataSourceProviders;
using ExtendedCase.Logic.Services;
using CustomDataSourceProviders = ExtendedCase.Logic.CustomDataSourceProviders;
using OptionDataSourceProviders = ExtendedCase.Logic.OptionDataSourceProviders;

namespace ExtendedCase.WebApi.Di
{
    public class AutofacLogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //see registration rules here http://docs.autofac.org/en/latest/register/index.html

            builder.RegisterType<FormService>().As<IFormService>().InstancePerRequest();
            builder.RegisterType<OptionDataSourceService>().As<IOptionDataSourceService>().InstancePerRequest();
            builder.RegisterType<CustomDataSourceService>().As<ICustomDataSourceService>().InstancePerRequest();
            builder.RegisterType<HelpdeskCaseSevice>().As<IHelpdeskCaseSevice>().InstancePerRequest();
            builder.RegisterType<ClientLogService>().As<IClientLogService>().InstancePerRequest();

            //options providers
            builder.RegisterType<OptionDataSourceProviderFactory>().As<IOptionDataSourceProviderFactory>().InstancePerRequest();
            builder.RegisterType<OptionDataSourceProviders.DbTableProvider>().As<IOptionDataSourceProvider>().InstancePerRequest();
            builder.RegisterType<OptionDataSourceProviders.DbQueryProvider>().As<IOptionDataSourceProvider>().InstancePerRequest();
            builder.RegisterType<OptionDataSourceProviders.DbSpProvider>().As<IOptionDataSourceProvider>().InstancePerRequest();

            //custom providers
            builder.RegisterType<CustomDataSourceProviderFactory>().As<ICustomDataSourceProviderFactory>().InstancePerRequest();
            builder.RegisterType<CustomDataSourceProviders.DbTableProvider>().As<ICustomDataSourceProvider>().InstancePerRequest();
            builder.RegisterType<CustomDataSourceProviders.DbQueryProvider>().As<ICustomDataSourceProvider>().InstancePerRequest();
            builder.RegisterType<CustomDataSourceProviders.DbSpProvider>().As<ICustomDataSourceProvider>().InstancePerRequest();

            RegisterChildrenComponents(builder);
        }

        private static void RegisterChildrenComponents(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacDalModule());
        }
    }
}
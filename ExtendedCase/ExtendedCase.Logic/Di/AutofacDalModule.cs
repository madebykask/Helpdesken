using Autofac;
using ExtendedCase.Dal.Connection;
using ExtendedCase.Dal.Repositories;

namespace ExtendedCase.Logic.Di
{
    public class AutofacDalModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //see registration rules here http://docs.autofac.org/en/latest/register/index.html

            builder.RegisterType<DbConnectionFactory>().As<IDbConnectionFactory>().InstancePerRequest();

            //repositories
            builder.RegisterType<FormRepository>().As<IFormRepository>().InstancePerRequest();
            builder.RegisterType<OptionDataSourceRepository>().As<IOptionDataSourceRepository>().InstancePerRequest();
            builder.RegisterType<CustomDataSourceRepository>().As<ICustomDataSourceRepository>().InstancePerRequest();
            builder.RegisterType<TranslationRepository>().As<ITranslationRepository>().InstancePerRequest();
        }
    }
}

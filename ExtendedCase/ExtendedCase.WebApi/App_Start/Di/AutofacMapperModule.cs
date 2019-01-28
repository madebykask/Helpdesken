using Autofac;
using ExtendedCase.WebApi.Mapping;

namespace ExtendedCase.WebApi.Di
{
    public class AutofacMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var mapper = MappingConfiguration.Configure().CreateMapper();
            builder.RegisterInstance(mapper)
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}
using Autofac;
using DH.Helpdesk.WebApi.Infrastructure.Config.Mapper;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
{
    public class MapperModule : Module
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
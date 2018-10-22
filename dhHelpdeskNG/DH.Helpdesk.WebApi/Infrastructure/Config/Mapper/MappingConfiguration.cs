using AutoMapper;
using DH.Helpdesk.WebApi.Infrastructure.Config.Mapper.Profiles;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.Mapper
{
    public static class MappingConfiguration
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelToEntitiesMappingProfile>();
            });

            return config;
        }
    }
}
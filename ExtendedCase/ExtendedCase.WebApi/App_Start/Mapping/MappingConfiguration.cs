using AutoMapper;
using ExtendedCase.Logic.Mapping.Profiles;

namespace ExtendedCase.WebApi.Mapping
{
    public static class MappingConfiguration
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelToEntitiesMappingProfile>();
                cfg.AddProfile<EntityToModelMappingProfile>();
            });

            return config;
        }
    }
}
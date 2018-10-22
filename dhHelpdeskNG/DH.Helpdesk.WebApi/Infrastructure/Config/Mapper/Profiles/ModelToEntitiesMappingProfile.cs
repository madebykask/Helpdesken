using AutoMapper;
using DH.Helpdesk.WebApi.Infrastructure.ClientLogger;
using DH.Helpdesk.WebApi.Models;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.Mapper.Profiles
{
    public class ModelToEntitiesMappingProfile : Profile
    {
        public ModelToEntitiesMappingProfile()
        {
            CreateMap<ClientLogItemModel, ClientLogEntry>();
        }
    }
}
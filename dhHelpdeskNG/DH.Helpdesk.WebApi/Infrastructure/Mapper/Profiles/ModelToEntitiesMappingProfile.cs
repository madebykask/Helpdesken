using AutoMapper;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.StateSecondaries;
using DH.Helpdesk.Models.WorkingGroup;
using DH.Helpdesk.WebApi.Infrastructure.ClientLogger;
using DH.Helpdesk.WebApi.Models;

namespace DH.Helpdesk.WebApi.Infrastructure.Mapper.Profiles
{
    public class ModelToEntitiesMappingProfile : Profile
    {
        public ModelToEntitiesMappingProfile()
        {
            CreateMap<ClientLogItemModel, ClientLogEntry>();
            CreateMap<WorkingGroupOutputModel, WorkingGroupEntity>();
            CreateMap<StateSecondaryOutputModel, StateSecondary>();
        }
    }
}
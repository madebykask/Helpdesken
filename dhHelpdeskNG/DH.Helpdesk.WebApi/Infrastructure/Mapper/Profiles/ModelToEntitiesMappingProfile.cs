using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.Domain;
using DH.Helpdesk.WebApi.Infrastructure.ClientLogger;
using DH.Helpdesk.WebApi.Models;

namespace DH.Helpdesk.WebApi.Infrastructure.Mapper.Profiles
{
    public class ModelToEntitiesMappingProfile : Profile
    {
        public ModelToEntitiesMappingProfile()
        {
            CreateMap<ClientLogItemModel, ClientLogEntry>();
        }
    }

    public class EntitiesToModelMappingProfile : Profile
    {

        public EntitiesToModelMappingProfile()
        {
            CreateMap<CaseLockInfo, CaseLockInputModel>();

            CreateMap<CaseSolution, CaseSolutionInfo>()
                .ForMember(dest => dest.CaseSolutionId, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(dest => dest.StateSecondaryId, opt => opt.MapFrom(s => s.StateSecondary_Id ?? 0));
        }
    }
}
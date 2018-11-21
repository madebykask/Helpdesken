using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
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
        }
    }
}
using AutoMapper;
using Common.Logging.ClientLogger;
using ExtendedCase.Dal.Data;
using ExtendedCase.Models;

namespace ExtendedCase.Logic.Mapping.Profiles
{
    public class ModelToEntitiesMappingProfile : Profile
    {
        public ModelToEntitiesMappingProfile()
        {
            CreateMap<ClientLogItemModel, ClientLogEntry>();
            CreateMap<FormStateItem, ExtendedCaseFormStateItem>();
        }
    }
}
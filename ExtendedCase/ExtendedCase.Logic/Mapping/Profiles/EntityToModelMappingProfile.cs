using AutoMapper;
using ExtendedCase.Dal.Data;
using ExtendedCase.Models;

namespace ExtendedCase.Logic.Mapping.Profiles
{
    public class EntityToModelMappingProfile : Profile
    {
        public EntityToModelMappingProfile()
        {
            CreateMap<ExtendedCaseFormStateItem, FormStateItem>();
        }
    }
}
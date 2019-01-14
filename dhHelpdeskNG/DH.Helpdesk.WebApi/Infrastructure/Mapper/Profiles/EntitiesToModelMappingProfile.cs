using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Models.Case.Logs;
using DH.Helpdesk.WebApi.Models;

namespace DH.Helpdesk.WebApi.Infrastructure.Mapper.Profiles
{
    public class EntitiesToModelMappingProfile : Profile
    {

        public EntitiesToModelMappingProfile()
        {
            CreateMap<CaseLockInfo, CaseLockInputModel>();

            CreateMap<CaseSolution, CaseSolutionInfo>()
                .ForMember(dest => dest.CaseSolutionId, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(dest => dest.StateSecondaryId, opt => opt.MapFrom(s => s.StateSecondary_Id ?? 0));

            CreateMap<Log, CaseLogOutputModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(s => s.LogDate))
                .ForMember(dest => dest.Type, opt => opt.ResolveUsing(r => 
                    string.IsNullOrEmpty(r.Text_Internal) ? CaseEventType.InternalLogNote : CaseEventType.ExternalLogNote))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(s => s.User));

            CreateMap<User, LogUserOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.SurName));
        }
    }
}
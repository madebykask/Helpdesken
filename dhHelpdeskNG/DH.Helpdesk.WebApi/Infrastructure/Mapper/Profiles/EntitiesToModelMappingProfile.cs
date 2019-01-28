using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.BusinessData.Models.Case.CaseLogs;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case;
using DH.Helpdesk.Models.Case.Logs;
using DH.Helpdesk.WebApi.Models;
using LogFileModel = DH.Helpdesk.Models.Case.Logs.LogFileModel;

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


            CreateMap<CaseLogData, CaseLogOutputModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(s => s.LogDate))
                .ForMember(dest => dest.Text, opt => opt.ResolveUsing(r => !string.IsNullOrEmpty(r.ExternalText) ? r.ExternalText: r.InternalText))
                .ForMember(dest => dest.IsExternal, opt => opt.ResolveUsing(r => !string.IsNullOrEmpty(r.ExternalText) ? r.ExternalText : r.InternalText))
                .ForMember(dest => dest.Type, opt => opt.ResolveUsing(r => 
                    string.IsNullOrEmpty(r.InternalText) ? CaseEventType.InternalLogNote : CaseEventType.ExternalLogNote))

                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(s => new LogUserOverview(s.Id, s.UserFirstName, s.UserSurName))
                );

            CreateMap<LogFileData, LogFileModel>();

            CreateMap<User, LogUserOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(dest => dest.SurName, opt => opt.MapFrom(s => s.SurName));
        }
    }
}
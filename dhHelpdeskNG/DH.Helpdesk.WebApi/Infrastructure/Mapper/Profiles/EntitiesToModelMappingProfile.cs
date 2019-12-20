using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.BusinessData.Models.Case.CaseLogs;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Models.Case.Logs;
using DH.Helpdesk.Models.StateSecondaries;
using DH.Helpdesk.Models.Statuses;
using DH.Helpdesk.WebApi.Models;
using DH.Helpdesk.WebApi.Models.Case;
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
                .ForMember(dest => dest.Text, opt => opt.ResolveUsing(r => !string.IsNullOrEmpty(r.ExternalText) ? r.ExternalText: r.InternalText))
                .ForMember(dest => dest.IsExternal, opt => opt.ResolveUsing(r => !string.IsNullOrEmpty(r.ExternalText)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(s => s.LogDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(s => s.UserId.HasValue ? $"{s.UserFirstName} {s.UserSurName}" : s.RegUserName)
                );

            CreateMap<Mail2TicketData, Mail2TicketModel>();

            CreateMap<LogFileData, LogFileModel>();

            CreateMap<EmailLogData, EmailLogModel>();

            CreateMap<User, LogUserOverview>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(dest => dest.SurName, opt => opt.MapFrom(s => s.SurName));

            CreateMap<StateSecondary, StateSecondaryOutputModel>()
                .ForMember(dest => dest.RecalculateWatchDate, opt => opt.ResolveUsing(src => src.RecalculateWatchDate.ToBool()));

            CreateMap<Status, StatusOutputModel>()
                .ForMember(dest => dest.IsDefault, opt => opt.ResolveUsing(src => src.IsDefault.ToBool()))
                .ForMember(dest => dest.IsActive, opt => opt.ResolveUsing(src => src.IsActive.ToBool()))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(s => s.Customer_Id))
                .ForMember(dest => dest.WorkingGroupId, opt => opt.MapFrom(s => s.WorkingGroup_Id))
                .ForMember(dest => dest.StateSecondaryId, opt => opt.MapFrom(s => s.StateSecondary_Id));

            CreateMap<CaseType, CaseTypeOverview>()
                .ForMember(dest => dest.ParentId, opt => opt.ResolveUsing(src => src.Parent_CaseType_Id))
                .ForMember(dest => dest.WorkingGroupId, opt => opt.ResolveUsing(src => src.WorkingGroup_Id))
                .ForMember(dest => dest.AdministratorId, opt => opt.ResolveUsing(src => src.User_Id))
                .ForMember(dest => dest.SubCaseTypes, opt => opt.Ignore());

            CreateMap<CaseSolution, CaseSolutionModel>()
                .ForMember(dest => dest.NoMailToNotifier, opt => opt.ResolveUsing(src => src.NoMailToNotifier.ToBool()))
                .ForMember(dest => dest.UpdateNotifierInformation,
                    opt => opt.ResolveUsing(src => src.UpdateNotifierInformation.ToBool()))
                .ForMember(dest => dest.Verified, opt => opt.ResolveUsing(src => src.Verified.ToBool()))
                .ForMember(dest => dest.SMS, opt => opt.ResolveUsing(src => src.SMS.ToBool()))
                .ForMember(dest => dest.IsActive, opt => opt.ResolveUsing(src => src.Status.ToBool()))
                .ForMember(dest => dest.ContactBeforeAction, opt => opt.ResolveUsing(src => src.ContactBeforeAction.ToBool()))
                .ForMember(dest => dest.FinalAction, opt => opt.MapFrom(src => src.SaveAndClose))
                .ForMember(dest => dest.WorkingGroup_Id, opt => opt.MapFrom(src => src.CaseWorkingGroup_Id));
        }
    }
}
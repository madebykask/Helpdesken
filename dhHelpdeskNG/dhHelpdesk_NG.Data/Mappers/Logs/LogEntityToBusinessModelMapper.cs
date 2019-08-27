using System.Linq;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Dal.MapperData.Logs;

namespace DH.Helpdesk.Dal.Mappers.Logs
{
    public class LogEntityToBusinessModelMapper : IEntityToBusinessModelMapper<LogMapperData, LogOverview>
    {
        public LogOverview Map(LogMapperData data)
        {
            var entity = data.Log;
            return new LogOverview
            {
                Id = entity.Id,
                CaseHistoryId = entity.CaseHistory_Id,
                CaseId = entity.Case_Id,
                ChangeTime = entity.ChangeTime,
                Charge = entity.Charge,
                EquipmentPrice = entity.EquipmentPrice,
                Export = entity.Export,
                ExportDate = entity.ExportDate,
                FinishingDate = entity.FinishingDate,
                FinishingType = entity.FinishingType,
                InformCustomer = entity.InformCustomer,
                LogDate = entity.LogDate,
                LogGuid = entity.LogGUID,
                LogType = entity.LogType,
                Price = entity.Price,
                RegTime = entity.RegTime,
                RegUser = entity.RegUser,
                TextExternal = entity.Text_External,
                TextInternal = entity.Text_Internal,
                UserId = entity.User_Id,
                WorkingTime = entity.WorkingTime,
                OverTime = entity.OverTime,

                EmailLogs =
                    data.EmailLogs.Where(el => el.Id.HasValue).Select(el => new EmailLogOverview
                    {
                        Id = el.Id ?? 0,
                        CaseHistoryId = el.CaseHistoryId ?? 0,
                        MailTemplate = (GlobalEnums.MailTemplates)el.MailId,
                        Email =  el.EmailAddress?.ToLower(),
                    }).ToList(),

                LogFiles =
                    data.LogFiles.Where(e => e.Id.HasValue && e.Id > 0)
                        .Select(f => new LogFileOverview(f.Id ?? 0, f.FileName, f.CaseId, f.LogId, (LogFileType)f.LogType.Value))
                        .ToList(),

                Mail2Tickets =
                    data.Mail2Tickets.Where(m => m.Id.HasValue).Select(m => new Mail2TicketOverview(m.Id.Value, m.Type, m.EMailAddress, m.EMailSubject)).ToList(),

                User = new LogUserOverview(data.User != null && data.User.Id.HasValue ? data.User.Id.Value : 0 , data.User?.FirstName, data.User?.SurName)
            };
        }
    }
}

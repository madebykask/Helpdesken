using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case.CaseLogs;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Dal.MapperData.Logs;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases
{
    public static class CaseLogDataMapper
    {
        public static List<CaseLogData> MapToCaseLogData(this IEnumerable<LogMapperData> caseLogsEntities, bool includeInternalLogs)
        {
            var items = 
               (from data in caseLogsEntities
                let log = data.Log
                orderby log.LogDate descending
                select new CaseLogData
                {
                    Id = log.Id,
                    CaseId = log.Case_Id,
                    UserId = log.User_Id,
                    UserFirstName = log.User?.FirstName,
                    UserSurName = log.User?.SurName,
                    LogDate = log.LogDate,
                    RegTime = log.RegTime,
                    RegUserName = log.RegUser,
                    InternalText = includeInternalLogs ? log.Text_Internal : string.Empty, //empty internal if exist
                    ExternalText = log.Text_External,

                    EmailLogs =
                        data.EmailLogs?.Where(t => t != null && t.Id > 0).Select(t => new EmailLogData()
                            {
                                Id = t.Id ?? 0,
                                MailId = t.MailId ?? 0,
                                Email = t.EmailAddress?.ToLower() ?? "",
                                CcEmail = t.CcEmailAddress?.ToLower() ?? ""
                            })
                            .OrderBy(s => s.Email)
                            .Distinct()
                            .ToList() ?? new List<EmailLogData>(),

                    Files =
                        data.LogFiles?.Where(f => f != null && f.Id > 0).Select(f => new LogFileData()
                        {
                            Id = f.Id.Value,
                            LogId = f.LogId ?? 0,
                            FileName = f.FileName,
                            CaseId = f.CaseId,
                            LogType = f.LogType ?? LogFileType.External

                        }).ToList() ?? new List<LogFileData>(),

                    Mail2Tickets =
                        data.Mail2Tickets?.Where(m => m.Id != null && m.Id > 0).Select(m => new Mail2TicketData(m.Id.Value, m.Type, m.EMailAddress, m.EMailSubject)).ToList() ?? new List<Mail2TicketData>()

                }).ToList();
            return items;
        }
    }
}
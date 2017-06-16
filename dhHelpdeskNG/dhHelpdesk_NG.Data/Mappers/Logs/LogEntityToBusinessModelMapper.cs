using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
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

                CaseHistory =
                    entity.CaseHistory_Id != null
                        ? new LogCaseHistoryOverview(entity.CaseHistory_Id.Value)
                        {
                            Emaillogs =
                                data.EmailLogs.Where(el => el.Id.HasValue)
                                    .Select(e => new EmailLogsOverview(e.Id.Value, e.EmailAddress))
                                    .ToList(),
                        }
                        : null,

                LogFiles =
                    data.LogFiles.Where(e => e.Id.HasValue)
                        .Select(t => new LogFileOverview(t.Id.Value, t.FileName))
                        .ToList(),

                User = new LogUserOverview( (data.User != null && data.User.Id.HasValue ? data.User.Id.Value : 0) , data.User?.FirstName, data.User?.SurName)
            };
        }
    }
}

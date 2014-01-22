using System;
using System.Linq;
using System.Collections.Generic;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs.Case;
using Log = dhHelpdesk_NG.Domain.Log;

namespace dhHelpdesk_NG.Service
{
    public interface ILogService
    {
        IDictionary<string, string> Validate(CaseLog logToValidate);
        void SaveLog(CaseLog caseLog, out IDictionary<string, string> errors);
        CaseLog InitCaseLog(int userId, string regUser);
        IList<Log> GetLogForCase(int caseId);
    }

    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogService(
            ILogRepository logRepository
            , IUnitOfWork unitOfWork)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(CaseLog logToValidate)
        {
            if (logToValidate == null)
                throw new ArgumentNullException("logtovalidate");

            var errors = new Dictionary<string, string>();
            return errors;
        }

        public IList<Log> GetLogForCase(int caseId)
        {
            return _logRepository.GetLogForCase(caseId).ToList(); 
        }

        public CaseLog InitCaseLog(int userId, string regUser)
        {
            CaseLog ret = new CaseLog();

            ret.RegUser = regUser; 
            ret.LogType = 0;
            ret.UserId = userId;

            return ret;
        }

        public void SaveLog(CaseLog caseLog, out IDictionary<string, string> errors)
        {
            if (caseLog == null)
                throw new ArgumentNullException();

            errors = Validate(caseLog);

            if (!string.IsNullOrWhiteSpace(caseLog.TextExternal)
                || !string.IsNullOrWhiteSpace(caseLog.TextInternal)
                || caseLog.FinishingType != null
                || caseLog.FinishingDate != null
                || caseLog.EquipmentPrice != 0
                || caseLog.Price != 0
                || caseLog.WorkingTimeHour != 0
                || caseLog.WorkingTimeMinute != 0
                || caseLog.Id != 0)
            {
                var log = GenerateLogFromCaseLog(caseLog);

                if (caseLog.Id == 0)
                    _logRepository.Add(log);
                else
                    _logRepository.Update(log);

                if (errors.Count == 0)
                    this.Commit();
            }
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        private Log GenerateLogFromCaseLog(CaseLog caseLog)
        {
            //var log = new dhHelpdesk_NG.Domain.Log();
            var log = new Log();

            if (caseLog.Id != 0)
                log = _logRepository.GetLogById(caseLog.Id);
            else
            {
                log.RegTime = DateTime.UtcNow;
                log.LogDate = DateTime.UtcNow;
                log.RegUser = caseLog.RegUser;
                log.RegUser = string.IsNullOrWhiteSpace(caseLog.RegUser) ? string.Empty : caseLog.RegUser;
                log.Export = 0;
                log.LogType = caseLog.LogType;
                log.LogGUID = Guid.NewGuid(); 
            }

            log.Id = caseLog.Id;
            log.Case_Id = caseLog.CaseId;
            log.User_Id = caseLog.UserId; 
            log.Charge = caseLog.Charge;
            log.EquipmentPrice = caseLog.EquipmentPrice;
            log.Price = caseLog.Price;
            log.FinishingDate = caseLog.FinishingDate;
            log.FinishingType = caseLog.FinishingType;
            log.ChangeTime = DateTime.UtcNow;
            log.InformCustomer = caseLog.InformCustomer;
            log.Text_External = string.IsNullOrWhiteSpace(caseLog.TextExternal) ? string.Empty : caseLog.TextExternal;
            log.Text_Internal = string.IsNullOrWhiteSpace(caseLog.TextInternal) ? string.Empty : caseLog.TextInternal;
            log.CaseHistory_Id = caseLog.CaseHistoryId; 
            //Todo calulate WorkingTime
            log.WorkingTime = caseLog.WorkingTimeHour + caseLog.WorkingTimeMinute;

            return log;
        }
    }
}

using System;
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

        public void SaveLog(CaseLog caseLog, out IDictionary<string, string> errors)
        {
            if (caseLog == null)
                throw new ArgumentNullException();

            errors = Validate(caseLog);

            var log = GenerateLogFromCaseLog(caseLog);
            
            if (caseLog.Id == 0)
                _logRepository.Add(log);
            else
                _logRepository.Update(log);

            if (errors.Count == 0)
                this.Commit();
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
                log.Export = 0;
                log.LogType = caseLog.LogType; 
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
            log.Text_External = caseLog.TextExternal;
            log.Text_Internal = caseLog.TextInternal;
            //todo calulate WorkingTime
            log.WorkingTime = caseLog.WorkingTimeHour + caseLog.WorkingTimeMinute;

            return log;
        }
    }
}

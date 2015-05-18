namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Logs.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;

    public interface ILogService
    {
        IDictionary<string, string> Validate(CaseLog logToValidate);
        int SaveLog(CaseLog caseLog, int noOfAttachedFiles, out IDictionary<string, string> errors);
        CaseLog InitCaseLog(int userId, string regUser);
        IList<Log> GetLogsByCaseId(int caseId);
        CaseLog GetLogById(int id);
        Guid Delete(int id);

        IEnumerable<LogOverview> GetCaseLogOverviews(int caseId);
    }

    public class LogService : ILogService
    {
        #region Private variables

        private readonly ILogRepository _logRepository;
        private readonly ILogFileRepository _logFileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFilesStorage _filesStorage;
        private readonly IFinishingCauseRepository _finishingCauseRepository;
        private readonly IFinishingCauseService _finishingCauseService;

        /// <summary>
        /// The case repository.
        /// </summary>
        private readonly ICaseRepository caseRepository;

        private readonly IProblemLogService problemLogService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="logRepository">
        /// The log repository.
        /// </param>
        /// <param name="logFileRepository">
        /// The log file repository.
        /// </param>
        /// <param name="filesStorage">
        /// The files storage.
        /// </param>
        /// <param name="unitOfWork">
        /// The unit of work.
        /// </param>
        /// <param name="caseRepository">
        /// The case repository.
        /// </param>
        /// <param name="problemLogService"></param>
        public LogService(
            ILogRepository logRepository,
            ILogFileRepository logFileRepository,
            IFilesStorage filesStorage,
            IUnitOfWork unitOfWork, 
            ICaseRepository caseRepository, 
            IProblemLogService problemLogService,
            IFinishingCauseRepository finishingCauseRepository,
            IFinishingCauseService finishingCauseService)
        {
            this._logRepository = logRepository;
            this._unitOfWork = unitOfWork;
            this.caseRepository = caseRepository;
            this.problemLogService = problemLogService;
            this._filesStorage = filesStorage;
            this._logFileRepository = logFileRepository;
            this._finishingCauseRepository = finishingCauseRepository;
            this._finishingCauseService = finishingCauseService; 
        }

        #endregion

        #region Public Methods and Operators

        public IDictionary<string, string> Validate(CaseLog logToValidate)
        {
            if (logToValidate == null)
                throw new ArgumentNullException("logtovalidate");

            var errors = new Dictionary<string, string>();
            return errors;
        }

        public Guid Delete(int id)
        {
            Guid ret = Guid.Empty;

            // delete log files
            var logFiles = this._logFileRepository.GetLogFilesByLogId(id);
            if (logFiles != null)
            {
                foreach (var f in logFiles)
                {
                    this._filesStorage.DeleteFile(ModuleName.Log, f.Log_Id, f.FileName);
                    this._logFileRepository.Delete(f);
                }
                this._logFileRepository.Commit();
            }

            var l = this._logRepository.GetById(id);
            ret = l.LogGUID;
            this._logRepository.Delete(l);
            this._logRepository.Commit();

            return ret;
        }

        /// <summary>
        /// The get case log overviews.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<LogOverview> GetCaseLogOverviews(int caseId)
        {
            var result = new List<LogOverview>();
            var caseLogs = this._logRepository.GetCaseLogOverviews(caseId);
            if (caseLogs != null)
            {
                result.AddRange(caseLogs);
            }

            var caseOverview = this.caseRepository.GetCaseOverview(caseId);
            if (caseOverview != null && caseOverview.ProblemId.HasValue)
            {
                var problemLogs = this.problemLogService.GetProblemLogs(caseOverview.ProblemId.Value);
                if (problemLogs != null)
                {
                    result.AddRange(problemLogs
                        .Where(p => p.IsShowOnCase())
                        .Select(p => new LogOverview()
                        {
                            LogDate  = p.CreatedDate,
                            RegUser = p.ChangedByUserName,
                            TextInternal = p.IsInternal() ? p.LogText : null,
                            TextExternal = p.IsExternal() ? p.LogText : null,
                            ProblemId = p.ProblemId
                        }));
                }
            }

            return result.OrderByDescending(l => l.LogDate);
        }

        public IList<Log> GetLogsByCaseId(int caseId)
        {
            return this._logRepository.GetLogForCase(caseId).ToList();   
        }

        public CaseLog GetLogById(int id)
        {
            return this.GetCaseLogFromLog(this._logRepository.GetLogById(id)); 
        }

        public CaseLog InitCaseLog(int userId, string regUser)
        {
            CaseLog ret = new CaseLog();

            ret.RegUser = regUser; 
            ret.LogType = 0;
            ret.UserId = userId;
            ret.LogGuid = Guid.NewGuid(); 

            return ret;
        }

        public int SaveLog(CaseLog caseLog, int noOfAttachedFiles, out IDictionary<string, string> errors)
        {
            if (caseLog == null)
                throw new ArgumentNullException();

            errors = this.Validate(caseLog);
            if (!string.IsNullOrWhiteSpace(caseLog.TextExternal)
                || !string.IsNullOrWhiteSpace(caseLog.TextInternal)
                || caseLog.FinishingType != null
                || caseLog.FinishingDate != null
                || caseLog.EquipmentPrice != 0
                || caseLog.Price != 0
                || caseLog.WorkingTimeHour != 0
                || caseLog.WorkingTimeMinute != 0
                || caseLog.Id != 0
                || noOfAttachedFiles > 0)
            {
                var log = this.GetLogFromCaseLog(caseLog);

                if (caseLog.Id == 0)
                    this._logRepository.Add(log);
                else
                    this._logRepository.Update(log);

                if (errors.Count == 0)
                    this.Commit();

                return log.Id;
            }

            return 0;
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        #endregion

        #region Private Methods and Operators

        private Log GetLogFromCaseLog(CaseLog caseLog)
        {
            var log = new Log();

            if (caseLog.Id != 0)
                log = this._logRepository.GetLogById(caseLog.Id);
            else
            {
                log.RegTime = DateTime.UtcNow;
                log.LogDate = DateTime.UtcNow;
                log.RegUser = caseLog.RegUser;
                log.RegUser = string.IsNullOrWhiteSpace(caseLog.RegUser) ? string.Empty : caseLog.RegUser;
                log.Export = 0;
                log.LogType = caseLog.LogType;
                log.LogGUID = caseLog.LogGuid;  
            }

            log.Id = caseLog.Id;
            log.Case_Id = caseLog.CaseId;
            log.User_Id = caseLog.UserId; 
            log.Charge = (caseLog.Charge == true ? 1 : 0);
            log.EquipmentPrice = caseLog.EquipmentPrice;
            log.Price = caseLog.Price;
            log.FinishingDate = caseLog.FinishingDate;
            log.FinishingType = caseLog.FinishingType;
            log.ChangeTime = DateTime.UtcNow;
            log.InformCustomer = caseLog.SendMailAboutCaseToNotifier == true ? 1 : 0 ;
            log.Text_External = string.IsNullOrWhiteSpace(caseLog.TextExternal) ? string.Empty : caseLog.TextExternal;
            log.Text_Internal = string.IsNullOrWhiteSpace(caseLog.TextInternal) ? string.Empty : caseLog.TextInternal;
            log.CaseHistory_Id = caseLog.CaseHistoryId; 
            log.WorkingTime = (caseLog.WorkingTimeHour * 60) + caseLog.WorkingTimeMinute;

            return log;
        }

        private CaseLog GetCaseLogFromLog(Log l)
        {
            var log = new CaseLog();

            log.LogDate = l.LogDate; 
            log.RegUser = l.RegUser;
            log.LogType = l.LogType;
            log.LogGuid = l.LogGUID;
            log.Id = l.Id;
            log.CaseId = l.Case_Id;
            log.UserId = l.User_Id;
            log.Charge = (l.Charge == 1 ? true : false);
            log.EquipmentPrice = l.EquipmentPrice;
            log.Price = l.Price;
            log.FinishingDate = l.FinishingDate;
            log.FinishingType = l.FinishingType;
            if (l.FinishingType != null)
            {                
                var caption = _finishingCauseService.GetFinishingTypeName(l.FinishingType.Value);                
                log.FinishingTypeName = caption;
            }
            else            
                log.FinishingTypeName = "--";            
            
            log.SendMailAboutCaseToNotifier = l.InformCustomer == 1 ? true : false;
            log.TextExternal = string.IsNullOrWhiteSpace(l.Text_External) ? string.Empty : l.Text_External;
            log.TextInternal = string.IsNullOrWhiteSpace(l.Text_Internal) ? string.Empty : l.Text_Internal;
            log.CaseHistoryId = l.CaseHistory_Id;
            log.WorkingTimeHour = CalculateWorkingTimeHour(l.WorkingTime);
            log.WorkingTimeMinute = CalculateWorkingTimeMinute(l.WorkingTime);  

            return log;
        }

         

        private int CalculateWorkingTimeHour(int workingTime)
        {
            return workingTime >= 60 ? (int)Math.Round((double)(workingTime / 60), 0) : 0;
        }

        private int CalculateWorkingTimeMinute(int workingTime)
        {
            return workingTime >= 60 ? (int)workingTime % 60 : workingTime;
        }

    #endregion
    }
}

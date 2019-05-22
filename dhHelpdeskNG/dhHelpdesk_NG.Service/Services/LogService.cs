using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case.CaseLogs;
using DH.Helpdesk.Common.Extensions.Boolean;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Dal.MapperData.Logs;
using DH.Helpdesk.Dal.Mappers;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Logs.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;

    using LinqLib.Operators;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    public interface ILogService
    {
        IDictionary<string, string> Validate(CaseLog logToValidate);
        int SaveLog(CaseLog caseLog, int noOfAttachedFiles, out IDictionary<string, string> errors);
        CaseLog InitCaseLog(int userId, string regUser);
        IList<Log> GetLogsByCaseId(int caseId);
        Task<List<CaseLogData>> GetLogsByCaseIdAsync(int caseId, bool includeInternalLogs = false);
        CaseLog GetLogById(int id);
        Guid Delete(int id, string basePath);

        void AddParentCaseLogToChildCases(int[] caseIds, CaseLog parentCaseLog);
        void AddChildCaseLogToParentCase(int caseId, CaseLog parentCaseLog);

        IList<LogOverview> GetCaseLogOverviews(int caseId);
        IList<Log> GetCaseLogs(DateTime? fromDate, DateTime? toDate);

        void SaveChildsLogs(CaseLog baseCaseLog, int[] childCasesIds, out IDictionary<string, string> errors);
        void UpdateLogInvoices(List<CaseLog> logs);

    }

    public class LogService : ILogService
    {
        #region Private variables

        private readonly ILogRepository _logRepository;
        private readonly ILogFileRepository _logFileRepository;
        
        private readonly IFilesStorage _filesStorage;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly IMail2TicketRepository _mail2TicketRepository;
        private readonly IEntityToBusinessModelMapper<LogMapperData, LogOverview> _logToLogOverviewMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICaseRepository _caseRepository;
        private readonly IProblemLogService _problemLogService;

        #endregion

        #region Constructor

        public LogService(
            ILogRepository logRepository,
            ILogFileRepository logFileRepository,
            IFilesStorage filesStorage,
            ICaseRepository caseRepository, 
            IProblemLogService problemLogService,
            IFinishingCauseService finishingCauseService, 
            IMail2TicketRepository mail2TicketRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IEntityToBusinessModelMapper<LogMapperData, LogOverview> logToLogOverviewMapper)
        {
            _logRepository = logRepository;
            _caseRepository = caseRepository;
            _problemLogService = problemLogService;
            _filesStorage = filesStorage;
            _logFileRepository = logFileRepository;
            _finishingCauseService = finishingCauseService;
            _mail2TicketRepository = mail2TicketRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _logToLogOverviewMapper = logToLogOverviewMapper;
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

        public Guid Delete(int id, string basePath)
        {
            var ret = Guid.Empty;

            // delete log files
            var logFiles = _logFileRepository.GetLogFilesByLogId(id);
            if (logFiles != null)
            {
                foreach (var f in logFiles)
                {
                    _filesStorage.DeleteFile(ModuleName.Log, f.Log_Id, basePath, f.FileName);
                    _logFileRepository.Delete(f);
                }
                _logFileRepository.Commit();
            }

            //remove reference from parent in child records
            var referencedFiles = _logFileRepository.GetReferencedFiles(id);
            referencedFiles?.ForEach(x => x.ParentLog_Id = null);

            _mail2TicketRepository.DeleteByLogId(id);
            _mail2TicketRepository.Commit();

            var l = _logRepository.GetById(id);

            _logRepository.Delete(l);
            _logRepository.Commit();

            return l.LogGUID;
        }

        public IList<Log> GetCaseLogs(DateTime? fromDate, DateTime? toDate)
        {
            var ret = _logRepository.GetCaseLogs(fromDate, toDate);
            return ret; 
        }

        public IList<LogOverview> GetCaseLogOverviews(int caseId)
        {
            var result = new List<LogOverview>();
            var caseLogsEntities = _logRepository.GetCaseLogOverviews(caseId);
            var caseLogs = caseLogsEntities.Select(_logToLogOverviewMapper.Map).ToList();

            result.AddRange(caseLogs);

            var caseOverview = _caseRepository.GetCaseOverview(caseId);

            if (caseOverview != null && caseOverview.ProblemId.HasValue)
            {
                var problemLogs = _problemLogService.GetProblemLogs(caseOverview.ProblemId.Value);
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

            return result.OrderByDescending(l => l.LogDate).ToList();
        }

        public void SaveChildsLogs(CaseLog baseCaseLog, int[] childCasesIds, out IDictionary<string, string> errors)
        {
            errors = Validate(baseCaseLog);
            if (errors.Any())
                return;

            if (childCasesIds.Any())
            {
                var logs = childCasesIds.Select(id => CreateLogFromCaseLog(id, baseCaseLog)).ToList();
                _logRepository.AddRange(logs);
                _logRepository.Commit();
            }
        }

        private Log CreateLogFromCaseLog(int id, CaseLog cl)
        {
            cl.Id = id;
            return GetLogFromCaseLog(cl);
        }

        public IList<Log> GetLogsByCaseId(int caseId)
        {
            return _logRepository.GetLogForCase(caseId).ToList();
        }

        public Task<List<CaseLogData>> GetLogsByCaseIdAsync(int caseId, bool includeInternalLogs = false)
        {
            var queryable = _logRepository.GetLogForCase(caseId);

            if (!includeInternalLogs)
            {
                //keep only notes that have external, internal text will be trimmed in mapping logic below
                queryable = queryable.Where(l => !string.IsNullOrEmpty(l.Text_External));
            }

            var caseLogs =
                (from log in queryable.AsQueryable()
                 select new CaseLogData()
                 {
                     Id = log.Id,
                     UserId = log.User_Id,
                     UserFirstName = log.User.FirstName,
                     UserSurName = log.User.SurName,
                     LogDate = log.LogDate,
                     RegUserName = log.RegUser,
                     InternalText = includeInternalLogs ? log.Text_Internal : string.Empty, //empty internal if exist
                     ExternalText = log.Text_External,
                     Emails = log.CaseHistory.Emaillogs.DefaultIfEmpty().Where(t => t != null).Select(t => t.EmailAddress.ToLower()).OrderBy(s => s).Distinct().ToList(),
                     Files = log.LogFiles.DefaultIfEmpty().Where(f => f != null).Select(f => new LogFileData()
                     {
                         Id = f.Id,
                         LogId = f.Log_Id,
                         FileName = f.FileName,
                         CaseId = f.IsCaseFile.HasValue && f.IsCaseFile.Value ? f.Log.Case_Id : (int?)null
                     }).ToList()
                 }).ToListAsync();

            return caseLogs;
        }

        public CaseLog GetLogById(int id)
        {
            var log = _logRepository.GetById(id);
            return GetCaseLogFromLog(log); 
        }

        public CaseLog InitCaseLog(int userId, string regUser)
        {
            var ret = new CaseLog
            {
                RegUser = regUser,
                LogType = 0,
                UserId = userId,
                LogGuid = Guid.NewGuid()
            };
            return ret;
        }

        public int SaveLog(CaseLog caseLog, int noOfAttachedFiles, out IDictionary<string, string> errors)
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
                || caseLog.WorkingTime != 0
                || caseLog.Overtime != 0
                || caseLog.Id != 0
                || noOfAttachedFiles > 0)
            {
                var log = GetLogFromCaseLog(caseLog);

                if (caseLog.Id == 0)
                    _logRepository.Add(log);
                else
                    _logRepository.Update(log);

                if (errors.Count == 0)
                    _logRepository.Commit();

                return log.Id;
            }

            return 0;
        }

        public void AddChildCaseLogToParentCase(int parentCaseId, CaseLog parentCaseLog)
        {
            if (parentCaseId == null)
            {
                throw new ArgumentException("caseLog is null");
            }

            IDictionary<string, string> errors;
            IEnumerable<CaseHistory> newCaseHistories;
            using (var uow = _unitOfWorkFactory.Create())
            {
                var caseHistoryRepository = uow.GetRepository<CaseHistory>();
                var maxCaseHistoryIds =
                    caseHistoryRepository.GetAll()
                        .Where(it => it.Case_Id == parentCaseId)
                        .GroupBy(it => it.Case_Id)
                        .ToDictionary(g => g.Key, g => g.Max(it => it.Id))
                        .Values.ToArray();
                var caseHistories =
                    caseHistoryRepository.GetAll().Where(it => maxCaseHistoryIds.Contains(it.Id)).ToArray();
                newCaseHistories =
                    caseHistories.Select(
                        it =>
                        new CaseHistory()
                        {
                            CaseHistoryGUID = Guid.NewGuid(),
                            Case_Id = it.Case_Id,
                            ReportedBy = it.ReportedBy,
                            PersonsName = it.PersonsName,
                            PersonsEmail = it.PersonsEmail,
                            PersonsPhone = it.PersonsPhone,
                            PersonsCellphone = it.PersonsCellphone,
                            Customer_Id = it.Customer_Id,
                            Region_Id = it.Region_Id,
                            Department_Id = it.Department_Id,
                            OU_Id = it.OU_Id,
                            Place = it.Place,
                            UserCode = it.UserCode,
                            InventoryNumber = it.InventoryNumber,
                            InventoryType = it.InventoryType,
                            InventoryLocation = it.InventoryLocation,
                            CaseNumber = it.CaseNumber,
                            User_Id = it.User_Id,
                            IpAddress = it.IpAddress,
                            CaseType_Id = it.CaseType_Id,
                            ProductArea_Id = it.ProductArea_Id,
                            System_Id = it.System_Id,
                            Urgency_Id = it.Urgency_Id,
                            Impact_Id = it.Impact_Id,
                            Category_Id = it.Category_Id,
                            Supplier_Id = it.Supplier_Id,
                            InvoiceNumber = it.InvoiceNumber,
                            ReferenceNumber = it.ReferenceNumber,
                            Caption = it.Caption,
                            Description = it.Description,
                            Miscellaneous = it.Miscellaneous,
                            ContactBeforeAction = it.ContactBeforeAction,
                            SMS = it.SMS,
                            AgreedDate = it.AgreedDate,
                            Available = it.Available,
                            Cost = it.Cost,
                            OtherCost = it.OtherCost,
                            Currency = it.Currency,
                            Performer_User_Id = it.Performer_User_Id,
                            CaseResponsibleUser_Id = it.CaseResponsibleUser_Id,
                            Priority_Id = it.Priority_Id,
                            Status_Id = it.Status_Id,
                            StateSecondary_Id = it.StateSecondary_Id,
                            ExternalTime = it.ExternalTime,
                            Project_Id = it.Project_Id,
                            Verified = it.Verified,
                            VerifiedDescription = it.VerifiedDescription,
                            SolutionRate = it.SolutionRate,
                            PlanDate = it.PlanDate,
                            ApprovedDate = it.ApprovedDate,
                            ApprovedBy_User_Id = it.ApprovedBy_User_Id,
                            WatchDate = it.WatchDate,
                            LockCaseToWorkingGroup_Id = it.LockCaseToWorkingGroup_Id,
                            WorkingGroup_Id = it.WorkingGroup_Id,
                            FinishingDate = it.FinishingDate,
                            FinishingDescription = it.FinishingDescription,
                            FollowUpDate = it.FollowUpDate,
                            RegistrationSource = it.RegistrationSource,
                            RelatedCaseNumber = it.RelatedCaseNumber,
                            Problem_Id = it.Problem_Id,
                            Change_Id = it.Change_Id,
                            Unread = it.Unread,
                            RegLanguage_Id = it.RegLanguage_Id,
                            RegUserId = it.RegUserId,
                            RegUserDomain = it.RegUserDomain,
                            ProductAreaQuestionVersion_Id = it.ProductAreaQuestionVersion_Id,
                            LeadTime = it.LeadTime,
                            CreatedDate = DateTime.UtcNow,
                            CreatedByUser = it.CreatedByUser,
                            Deleted = it.Deleted,
                            CausingPartId = it.CausingPartId,
                            DefaultOwnerWG_Id = it.DefaultOwnerWG_Id,
                            CaseFile = it.CaseFile,
                            LogFile = it.LogFile,
                            CaseLog = it.CaseLog,
                            ClosingReason = it.ClosingReason,
                            RegistrationSourceCustomer_Id = it.RegistrationSourceCustomer_Id,
                            IsAbout_Persons_Name = it.IsAbout_Persons_Name,
                            IsAbout_Department_Id = it.IsAbout_Department_Id,
                            IsAbout_Persons_Phone = it.IsAbout_Persons_Phone,
                            IsAbout_ReportedBy = it.IsAbout_ReportedBy,
                            IsAbout_UserCode = it.IsAbout_UserCode
                        })
                        .ToArray();
                newCaseHistories.ForEach(caseHistoryRepository.Add);
                uow.Save();
            }

            var caseLogs =
                newCaseHistories.Select(
                    it =>
                    new Log
                    {
                        Id = it.Id,
                        CaseHistory_Id = it.Id,
                        Case_Id = it.Case_Id,
                        User_Id = parentCaseLog.UserId,
                        RegTime = DateTime.UtcNow,
                        LogDate = DateTime.UtcNow,
                        RegUser =
                                string.IsNullOrWhiteSpace(parentCaseLog.RegUser)
                                    ? string.Empty
                                    : parentCaseLog.RegUser,
                        LogType = parentCaseLog.LogType,
                        LogGUID = parentCaseLog.LogGuid,
                        Text_Internal =
                                string.IsNullOrWhiteSpace(parentCaseLog.TextInternal)
                                    ? string.Empty
                                    : parentCaseLog.TextInternal,
                        Text_External = string.Empty,
                        ChangeTime = DateTime.UtcNow
                    }).ToArray();

            caseLogs.ForEach(_logRepository.Add);
            _logRepository.Commit();

            _caseRepository.MarkCaseAsUnread(parentCaseId);
        }

        public void AddParentCaseLogToChildCases(int[] caseIds, CaseLog parentCaseLog)
        {
            if (caseIds == null)
            {
                throw new ArgumentException("caseLog is null");
            }

            IEnumerable<CaseHistory> newCaseHistories;
            using (var uow = _unitOfWorkFactory.Create())
            {
                var caseHistoryRepository = uow.GetRepository<CaseHistory>();
                var maxCaseHistoryIds =
                    caseHistoryRepository.GetAll()
                        .Where(it => caseIds.Contains(it.Case_Id))
                        .GroupBy(it => it.Case_Id)
                        .ToDictionary(g => g.Key, g => g.Max(it => it.Id))
                        .Values.ToArray();

                var caseHistories =
                    caseHistoryRepository.GetAll().Where(it => maxCaseHistoryIds.Contains(it.Id)).ToArray();

                newCaseHistories =
                    caseHistories.Select(
                        it =>
                        new CaseHistory()
                            {
                                CaseHistoryGUID = Guid.NewGuid(),
                                Case_Id = it.Case_Id,
                                ReportedBy = it.ReportedBy,
                                PersonsName = it.PersonsName,
                                PersonsEmail = it.PersonsEmail,
                                PersonsPhone = it.PersonsPhone,
                                PersonsCellphone = it.PersonsCellphone,
                                Customer_Id = it.Customer_Id,
                                Region_Id = it.Region_Id,
                                Department_Id = it.Department_Id,
                                OU_Id = it.OU_Id,
                                Place = it.Place,
                                UserCode = it.UserCode,
                                InventoryNumber = it.InventoryNumber,
                                InventoryType = it.InventoryType,
                                InventoryLocation = it.InventoryLocation,
                                CaseNumber = it.CaseNumber,
                                User_Id = it.User_Id,
                                IpAddress = it.IpAddress,
                                CaseType_Id = it.CaseType_Id,
                                ProductArea_Id = it.ProductArea_Id,
                                System_Id = it.System_Id,
                                Urgency_Id = it.Urgency_Id,
                                Impact_Id = it.Impact_Id,
                                Category_Id = it.Category_Id,
                                Supplier_Id = it.Supplier_Id,
                                InvoiceNumber = it.InvoiceNumber,
                                ReferenceNumber = it.ReferenceNumber,
                                Caption = it.Caption,
                                Description = it.Description,
                                Miscellaneous = it.Miscellaneous,
                                ContactBeforeAction = it.ContactBeforeAction,
                                SMS = it.SMS,
                                AgreedDate = it.AgreedDate,
                                Available = it.Available,
                                Cost = it.Cost,
                                OtherCost = it.OtherCost,
                                Currency = it.Currency,
                                Performer_User_Id = it.Performer_User_Id,
                                CaseResponsibleUser_Id = it.CaseResponsibleUser_Id,
                                Priority_Id = it.Priority_Id,
                                Status_Id = it.Status_Id,
                                StateSecondary_Id = it.StateSecondary_Id,
                                ExternalTime = it.ExternalTime,
                                Project_Id = it.Project_Id,
                                Verified = it.Verified,
                                VerifiedDescription = it.VerifiedDescription,
                                SolutionRate = it.SolutionRate,
                                PlanDate = it.PlanDate,
                                ApprovedDate = it.ApprovedDate,
                                ApprovedBy_User_Id = it.ApprovedBy_User_Id,
                                WatchDate = it.WatchDate,
                                LockCaseToWorkingGroup_Id = it.LockCaseToWorkingGroup_Id,
                                WorkingGroup_Id = it.WorkingGroup_Id,
                                FinishingDate = it.FinishingDate,
                                FinishingDescription = it.FinishingDescription,
                                FollowUpDate = it.FollowUpDate,
                                RegistrationSource = it.RegistrationSource,
                                RelatedCaseNumber = it.RelatedCaseNumber,
                                Problem_Id = it.Problem_Id,
                                Change_Id = it.Change_Id,
                                Unread = it.Unread,
                                RegLanguage_Id = it.RegLanguage_Id,
                                RegUserId = it.RegUserId,
                                RegUserDomain = it.RegUserDomain,
                                ProductAreaQuestionVersion_Id = it.ProductAreaQuestionVersion_Id,
                                LeadTime = it.LeadTime,
                                CreatedDate = DateTime.UtcNow,
                                CreatedByUser = it.CreatedByUser,
                                Deleted = it.Deleted,
                                CausingPartId = it.CausingPartId,
                                DefaultOwnerWG_Id = it.DefaultOwnerWG_Id,
                                CaseFile = it.CaseFile,
                                LogFile = it.LogFile,
                                CaseLog = it.CaseLog,
                                ClosingReason = it.ClosingReason,
                                RegistrationSourceCustomer_Id = it.RegistrationSourceCustomer_Id,
                                IsAbout_Persons_Name = it.IsAbout_Persons_Name,
                                IsAbout_Department_Id = it.IsAbout_Department_Id,
                                IsAbout_Persons_Phone = it.IsAbout_Persons_Phone,
                                IsAbout_ReportedBy = it.IsAbout_ReportedBy,
                                IsAbout_UserCode = it.IsAbout_UserCode
                            })
                        .ToArray();
                newCaseHistories.ForEach(caseHistoryRepository.Add);
                uow.Save();
            }

            var caseLogs =
                newCaseHistories.Select(
                    it =>
                    new Log
                        {
                            Id = it.Id,
                            CaseHistory_Id = it.Id,
                            Case_Id = it.Case_Id,
                            User_Id = parentCaseLog.UserId,
                            RegTime = DateTime.UtcNow,
                            LogDate = DateTime.UtcNow,
                            RegUser =
                                string.IsNullOrWhiteSpace(parentCaseLog.RegUser)
                                    ? string.Empty
                                    : parentCaseLog.RegUser,
                            LogType = parentCaseLog.LogType,
                            LogGUID = parentCaseLog.LogGuid,
                            Text_Internal =
                                string.IsNullOrWhiteSpace(parentCaseLog.TextInternal)
                                    ? string.Empty
                                    : parentCaseLog.TextInternal,
                            Text_External = string.Empty,
                            ChangeTime = DateTime.UtcNow
                        }).ToArray();
            caseLogs.ForEach(_logRepository.Add);
            _logRepository.Commit();

            caseIds.ForEach(id => _caseRepository.MarkCaseAsUnread(id));
        }

        public void UpdateLogInvoices(List<CaseLog> logs)
        {
            foreach (var log in logs)
            {
                var oldLog = _logRepository.GetById(log.Id);
                if (oldLog != null)
                {
                    oldLog.WorkingTime = log.WorkingTime;
                    oldLog.OverTime = log.Overtime;
                    oldLog.EquipmentPrice = log.EquipmentPrice;
                    oldLog.Price = log.Price;
                    oldLog.Charge = log.Charge.ToInt();
                }
            }

            _logRepository.Commit();
        }

        #endregion

        #region Private Methods and Operators

        private Log GetLogFromCaseLog(CaseLog caseLog)
        {
            Log log;

            if (caseLog.Id != 0)
            {
                log = _logRepository.GetById(caseLog.Id);
            }
            else
            {
                log = new Log
                {
                    RegTime = DateTime.UtcNow,
                    LogDate = DateTime.UtcNow,
                    RegUser = string.IsNullOrWhiteSpace(caseLog.RegUser) ? string.Empty : caseLog.RegUser,
                    Export = 0,
                    LogType = caseLog.LogType,
                    LogGUID = caseLog.LogGuid
                };

            }

            log.Case_Id = caseLog.CaseId;
            log.User_Id = caseLog.UserId; 
            log.Charge = caseLog.Charge ? 1 : 0;
            log.EquipmentPrice = caseLog.EquipmentPrice;
            log.Price = caseLog.Price;
            log.FinishingDate = caseLog.FinishingDate;
            log.FinishingType = caseLog.FinishingType;
            log.ChangeTime = DateTime.UtcNow;
            log.InformCustomer = caseLog.SendMailAboutCaseToNotifier ? 1 : 0 ;
            log.Text_External = string.IsNullOrWhiteSpace(caseLog.TextExternal) ? string.Empty : caseLog.TextExternal;
            log.Text_Internal = string.IsNullOrWhiteSpace(caseLog.TextInternal) ? string.Empty : caseLog.TextInternal;
            log.CaseHistory_Id = caseLog.CaseHistoryId; 
            log.WorkingTime = caseLog.WorkingTime;
            log.OverTime = caseLog.Overtime;

            return log;
        }

        private CaseLog GetCaseLogFromLog(Log l)
        {
            var finishingTypeName = l.FinishingType != null
                ? _finishingCauseService.GetFinishingTypeName(l.FinishingType.Value)
                : "--";

            var log = new CaseLog
            {
                LogDate = l.LogDate,
                RegUser = l.RegUser,
                LogType = l.LogType,
                LogGuid = l.LogGUID,
                Id = l.Id,
                CaseId = l.Case_Id,
                UserId = l.User_Id,
                Charge = l.Charge == 1,
                EquipmentPrice = l.EquipmentPrice,
                Price = l.Price,
                FinishingDate = l.FinishingDate,
                FinishingType = l.FinishingType,
                FinishingTypeName = finishingTypeName,
                SendMailAboutCaseToNotifier = l.InformCustomer == 1,
                TextExternal = string.IsNullOrWhiteSpace(l.Text_External) ? string.Empty : l.Text_External,
                TextInternal = string.IsNullOrWhiteSpace(l.Text_Internal) ? string.Empty : l.Text_Internal,
                CaseHistoryId = l.CaseHistory_Id,
                WorkingTime = l.WorkingTime,
                Overtime = l.OverTime
            };

            return log;
        }

    #endregion
    }
}

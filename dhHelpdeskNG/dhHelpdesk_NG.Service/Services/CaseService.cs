using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
    using System.Reflection;

namespace DH.Helpdesk.Services.Services
{

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.ChidCase;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.DbContext;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Dal.Repositories.Cases.Concrete;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain.MailTemplates;
    using DH.Helpdesk.Domain.Problems;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Cases;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Customers;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Customers;
    using DH.Helpdesk.Services.Infrastructure.Email;
    using DH.Helpdesk.Services.Localization;
    using DH.Helpdesk.Services.Services.CaseStatistic;
    using DH.Helpdesk.Services.utils;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    public interface ICaseService
    {
        IList<Case> GetCases();

        IList<Case> GetCasesByCustomers(IEnumerable<int> customerIds);

        Case InitCase(int customerId, int userId, int languageId, string ipAddress, CaseRegistrationSource source, Setting customerSetting, string adUser);

        Case Copy(int copyFromCaseid, int userId, int languageId, string ipAddress, CaseRegistrationSource source, string adUser);

        Case InitChildCaseFromCase(
            int copyFromCaseid,
            int userId,
            string ipAddress,
            CaseRegistrationSource source,
            string adUser,
            out ParentCaseInfo parentCaseInfo);

        Case GetCaseById(int id, bool markCaseAsRead = false);
        Case GetDetachedCaseById(int id);
        Case GetCaseByGUID(Guid GUID);
        Case GetCaseByEMailGUID(Guid GUID);
        EmailLog GetEMailLogByGUID(Guid GUID);             
        IList<CaseHistory> GetCaseHistoryByCaseId(int caseId);
        List<DynamicCase> GetAllDynamicCases();
        DynamicCase GetDynamicCase(int id);

        int SaveCase(
            Case cases, 
            CaseLog caseLog, 
            CaseMailSetting caseMailSetting, 
            int userId, 
            string adUser,           
            out IDictionary<string, string> errors,
            CaseInvoice[] invoices = null,
            Case parentCase = null);

        int SaveCaseHistory(
            Case c,
            int userId,
            string adUser,
            out IDictionary<string, string> errors,
            string defaultUser = "",
            ExtraFieldCaseHistory extraField = null);

        void SendCaseEmail(int caseId, CaseMailSetting cms, int caseHistoryId, string basePath,
                           Case oldCase = null, CaseLog log = null, List<CaseFileDto> logFiles = null);
        void UpdateFollowUpDate(int caseId, DateTime? time);
        void MarkAsUnread(int caseId);
        void MarkAsRead(int caseId);
        void SendSelfServiceCaseLogEmail(int caseId, CaseMailSetting cms, int caseHistoryId, CaseLog log, string basePath, List<CaseFileDto> logFiles = null);
        void Activate(int caseId, int userId, string adUser, out IDictionary<string, string> errors);
        IList<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user);
        void Commit();

        Guid Delete(int id, string basePath, int? parentCaseId);

        /// <summary>
        /// The get case overview.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The <see cref="CaseOverview"/>.
        /// </returns>
        CaseOverview GetCaseOverview(int caseId);

        MyCase[] GetMyCases(int userId, int? count = null);

        CustomerCases[] GetCustomersCases(int[] customerIds, int userId);

        List<RelatedCase> GetCaseRelatedCases(int caseId, int customerId, string userId, UserOverview currentUser);

        int GetCaseRelatedCasesCount(int caseId, int customerId, string userId, UserOverview currentUser);

        ChildCaseOverview[] GetChildCasesFor(int caseId);

        ParentCaseInfo GetParentInfo(int caseId);

        int? SaveInternalLogMessage(int id, string textInternal, out IDictionary<string, string> errors);
    }

    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseFileRepository _caseFileRepository;
        private readonly ICaseHistoryRepository _caseHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegionService _regionService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ISupplierService _supplierServicee;
        private readonly IPriorityService _priorityService;
        private readonly IStatusService _statusService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly UserRepository userRepository;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly IEmailService _emailService;
        private readonly ISettingService _settingService;
        private readonly IFilesStorage _filesStorage;
        private readonly ILogRepository _logRepository;
        private readonly ILogService _logService;
        private readonly ILogFileRepository _logFileRepository;
        private readonly IFormFieldValueRepository _formFieldValueRepository;

        private readonly ICaseMailer caseMailer;

        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private ISurveyService surveyService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ICaseLockService _caseLockService;

        private readonly CaseStatisticService _caseStatService;

        public CaseService(
            ICaseRepository caseRepository,
            ICaseFileRepository caseFileRepository,
            ICaseHistoryRepository caseHistoryRepository,
            ILogRepository logRepository,
            ILogFileRepository logFileRepository,
            IRegionService regionService,
            ICaseTypeService caseTypeService, 
            ISupplierService supplierService, 
            IPriorityService priorityService,
            IStatusService statusService,
            IWorkingGroupService workingGroupService,
            IMailTemplateService mailTemplateService,
            IEmailLogRepository emailLogRepository,
            IEmailService emailService,
            ISettingService settingService,
            IFilesStorage filesStorage,
            IUnitOfWork unitOfWork,
            IFormFieldValueRepository formFieldValueRepository,
            UserRepository userRepository, 
            ICaseMailer caseMailer, 
            IInvoiceArticleService invoiceArticleService, 
            IUnitOfWorkFactory unitOfWorkFactory,
            ISurveyService surveyService,
            ILogService logService,
            IFinishingCauseService finishingCauseService,
            ICaseLockService caseLockService, CaseStatisticService caseStatService)
        {
            this._unitOfWork = unitOfWork;
            this._caseRepository = caseRepository;
            this._caseRepository = caseRepository;
            this._regionService = regionService;
            this._caseTypeService = caseTypeService;
            this._supplierServicee = supplierService;
            this._priorityService = priorityService;
            this._statusService = statusService;
            this._workingGroupService = workingGroupService;
            this.userRepository = userRepository;
            this.caseMailer = caseMailer;
            this.invoiceArticleService = invoiceArticleService;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this._caseHistoryRepository = caseHistoryRepository;
            this._mailTemplateService = mailTemplateService;
            this._emailLogRepository = emailLogRepository;
            this._emailService = emailService;
            this._settingService = settingService;
            this._caseFileRepository = caseFileRepository;
            this._filesStorage = filesStorage;
            this._logRepository = logRepository;
            this._logFileRepository = logFileRepository;
            this._formFieldValueRepository = formFieldValueRepository;
            this.surveyService = surveyService;
            this._logService = logService;
            this._finishingCauseService = finishingCauseService;
            this._caseLockService = caseLockService;
            this._caseStatService = caseStatService;
        }

        public Case GetCaseById(int id, bool markCaseAsRead = false)
        {
            return this._caseRepository.GetCaseById(id, markCaseAsRead);
        }

        public Case GetDetachedCaseById(int id)
        {
            return this._caseRepository.GetDetachedCaseById(id);
        }

        public Case GetCaseByGUID(Guid GUID)
        {
            return this._caseRepository.GetCaseByGUID(GUID);
        }

        public Case GetCaseByEMailGUID(Guid GUID)
        {
            return this._caseRepository.GetCaseByEmailGUID(GUID);
        }

        public EmailLog GetEMailLogByGUID(Guid GUID)
        {
            return _emailLogRepository.GetEmailLogsByGuid(GUID);
        }

        public List<DynamicCase> GetAllDynamicCases()
        {
            return _caseRepository.GetAllDynamicCases();
        }

        public DynamicCase GetDynamicCase(int id)
        {
            return _caseRepository.GetDynamicCase(id);
        }

        public Guid Delete(int id, string basePath, int? parentCaseId)
        {
            Guid ret = Guid.Empty; 

            if (parentCaseId.HasValue)
            {
                using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
                {
                    var relationsRepo = uow.GetRepository<ParentChildRelation>();
                    var relation = relationsRepo.GetAll().FirstOrDefault(it => it.DescendantId == id);
                    if (relation == null || relation.AncestorId != parentCaseId.Value)
                    {
                        throw new ArgumentException(string.Format("bad parentCaseId \"{0}\" for case id \"{1}\"", parentCaseId.Value, id));
                    }

                    relationsRepo.Delete(relation);
                    uow.Save();
                    //@TODO: make a record in parent history
                }
            }

            this.DeleteChildCasesFor(id);

            // delete form field values
            var ffv = this._formFieldValueRepository.GetFormFieldValuesByCaseId(id);
            if (ffv != null)
            {
                foreach (var v in ffv)
                {
                    this._formFieldValueRepository.Delete(v);
                }
                this._formFieldValueRepository.Commit();  
            }

            // delete log files
            var logFiles = this._logFileRepository.GetLogFilesByCaseId(id); 

            if (logFiles != null)
            {                
                foreach (var f in logFiles)
                {
                    this._filesStorage.DeleteFile(ModuleName.Log, f.Log_Id, basePath, f.FileName);
                    this._logFileRepository.Delete(f);
                }
                this._logFileRepository.Commit();  
            }

            // delete logs
            var logs = this._logRepository.GetLogForCase(id);
            if (logs != null)
            {
                foreach (var l in logs)
                {
                    this._logRepository.Delete(l);  
                }
                this._logRepository.Commit();  
            }

            // delete email logs
            var elogs = this._emailLogRepository.GetEmailLogsByCaseId(id);
            if (elogs != null)
            {
                foreach (var l in elogs)
                {
                    this._emailLogRepository.Delete(l);
                }
                this._emailLogRepository.Commit(); 
            }

            // delete caseHistory
            var caseHistories = this._caseHistoryRepository.GetCaseHistoryByCaseId(id);
            if (caseHistories != null)
            {
                foreach (var h in caseHistories)
                {
                    this._caseHistoryRepository.Delete(h);  
                }
            }

            this._caseHistoryRepository.Commit(); 

            //delete case lock
            this._caseLockService.UnlockCaseByCaseId(id);

            // delete case files
            var caseFiles = this._caseFileRepository.GetCaseFilesByCaseId(id);
            if (caseFiles != null)
            {
                foreach (var f in caseFiles)
                {
                    this._filesStorage.DeleteFile(ModuleName.Cases, f.Case_Id, basePath, f.FileName);
                    this._caseFileRepository.Delete(f);
                }
                this._caseFileRepository.Commit(); 
            }

            // delete File View Log
            this._caseFileRepository.DeleteFileViewLogs(id);
            this._caseFileRepository.Commit();

            // delete Invoice
            this.invoiceArticleService.DeleteCaseInvoices(id);
            var c = this._caseRepository.GetById(id);
            ret = c.CaseGUID; 
            this._caseRepository.Delete(c);
            this._caseRepository.Commit();

            return ret;
        }

        private void DeleteChildCasesFor(int caseId)
        {
            using (var uow = unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                uow.GetRepository<ParentChildRelation>().DeleteWhere(it => it.AncestorId == caseId);
                uow.Save();
            }
        }

        /// <summary>
        /// The get case overview.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The <see cref="CaseOverview"/>.
        /// </returns>
        public CaseOverview GetCaseOverview(int caseId)
        {
            return this._caseRepository.GetCaseOverview(caseId);
        }

        public MyCase[] GetMyCases(int userId, int? count = null)
        {
            return this._caseRepository.GetMyCases(userId, count);
        }

        public CustomerCases[] GetCustomersCases(int[] customerIds, int userId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var customerRepository = uow.GetRepository<Customer>();
                var problemsRep = uow.GetRepository<Problem>();

                var customerCases = customerRepository.GetAll()
                                    .GetByIds(customerIds)
                                    .MapToCustomerCases(problemsRep.GetAll(), userId);

                return customerCases;
            }
        }

        public List<RelatedCase> GetCaseRelatedCases(int caseId, int customerId, string userId, UserOverview currentUser)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var caseRep = uow.GetRepository<Case>();

                return caseRep.GetAll()
                        .GetByCustomer(customerId)
                        .GetRelatedCases(caseId, userId, currentUser)
                        .MapToRelatedCases();
            }
        }

        public int GetCaseRelatedCasesCount(int caseId, int customerId, string userId, UserOverview currentUser)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var caseRep = uow.GetRepository<Case>();

                return caseRep.GetAll()
                        .GetByCustomer(customerId)
                        .GetRelatedCases(caseId, userId, currentUser)
                        .Count();
            }
        }


        public ChildCaseOverview[] GetChildCasesFor(int caseId)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var childCaseRelations = uow.GetRepository<ParentChildRelation>().GetAll();
                var allCases = uow.GetRepository<Case>().GetAll();
                var allSecStates = uow.GetRepository<StateSecondary>().GetAll();
                var allPerformers = uow.GetRepository<User>().GetAll();
                var caseTypes = uow.GetRepository<CaseType>().GetAll();
                var res =
                    childCaseRelations.Where(it => it.AncestorId == caseId)
                        .Select(it => new { id = it.DescendantId, parentId = it.AncestorId })
                        .GroupJoin(
                            allCases,
                            it => it.id,
                            case_ => case_.Id,
                            (parentChild, case_) => new { parentChild, case_ })
                        .SelectMany(
                            t => t.case_.DefaultIfEmpty(),
                            (t, case_) =>
                            new
                                {
                                    id = t.parentChild.id,
                                    parentId = t.parentChild.parentId,
                                    caseNumber = case_.CaseNumber,
                                    subject = case_.Caption,
                                    performerId = case_.Performer_User_Id,
                                    substateId = case_.StateSecondary_Id,
                                    caseTypeId = case_.CaseType_Id,
                                    registrationDate = case_.RegTime,
                                    closingDate = case_.FinishingDate,
                                    approvedDate = case_.ApprovedDate
                                })
                        .GroupJoin(
                            allPerformers,
                            tempParentChildStruct => tempParentChildStruct.performerId,
                            performer => performer.Id,
                            (tmpParentChild, performer) => new { tmpParentChild, performer })
                        .SelectMany(
                            t => t.performer.DefaultIfEmpty(),
                            (t, performer) =>
                            new
                                {
                                    id = t.tmpParentChild.id,
                                    parentId = t.tmpParentChild.parentId,
                                    caseNumber = t.tmpParentChild.caseNumber,
                                    subject = t.tmpParentChild.subject,
                                    performerFirstName = performer == null ? string.Empty : performer.FirstName,
                                    performerLastName = performer == null ? string.Empty : performer.SurName,
                                    substateId = t.tmpParentChild.substateId,
                                    caseTypeId = t.tmpParentChild.caseTypeId,
                                    registrationDate = t.tmpParentChild.registrationDate,
                                    closingDate = t.tmpParentChild.closingDate,
                                    approvedDate = t.tmpParentChild.approvedDate
                                })
                        .GroupJoin(
                            allSecStates,
                            tempParentCildStruct => tempParentCildStruct.substateId,
                            subState => subState.Id,
                            (tmpParentChild, subState) => new { tmpParentChild, subState })
                        .SelectMany(
                            t => t.subState.DefaultIfEmpty(),
                            (t, subState) =>
                            new
                                {
                                    id = t.tmpParentChild.id,
                                    parentId = t.tmpParentChild.parentId,
                                    subject = t.tmpParentChild.subject,
                                    caseNumber = t.tmpParentChild.caseNumber,
                                    performerFirstName = t.tmpParentChild.performerFirstName,
                                    performerLastName = t.tmpParentChild.performerLastName,
                                    subState = subState == null ? string.Empty : subState.Name,
                                    caseTypeId = t.tmpParentChild.caseTypeId,
                                    registrationDate = t.tmpParentChild.registrationDate,
                                    closingDate = t.tmpParentChild.closingDate,
                                    approvedDate = t.tmpParentChild.approvedDate
                                })
                        .GroupJoin(
                            caseTypes,
                            tempParentCildStruct => tempParentCildStruct.caseTypeId,
                            subState => subState.Id,
                            (tmpParentChild, casetType) => new { tmpParentChild, casetType })
                        .SelectMany(
                            t => t.casetType.DefaultIfEmpty(),
                            (t, caseType) =>
                            new
                                {
                                    id = t.tmpParentChild.id,
                                    parentId = t.tmpParentChild.parentId,
                                    caseNumber = t.tmpParentChild.caseNumber,
                                    subject = t.tmpParentChild.subject,
                                    performerFirstName = t.tmpParentChild.performerFirstName,
                                    performerLastName = t.tmpParentChild.performerLastName,
                                    subState = t.tmpParentChild.subState,
                                    caseType = caseType == null ? string.Empty : caseType.Name,
                                    IsApprovingRequired = caseType != null && caseType.RequireApproving == 1,
                                    registrationDate = t.tmpParentChild.registrationDate,
                                    closingDate = t.tmpParentChild.closingDate,
                                    t.tmpParentChild.approvedDate
                                })
                        .AsQueryable();
                return res.Select(it => new ChildCaseOverview()
                                     {
                                         Id = it.id,
                                         CaseNo = (int)it.caseNumber,
                                         Subject = it.subject,
                                         CasePerformer = new UserNamesStruct()
                                                             {
                                                                 FirstName = it.performerFirstName,
                                                                 LastName = it.performerLastName
                                                             },
                                        CaseType = it.caseType,
                                        SubStatus = it.subState,
                                        RegistrationDate = it.registrationDate,
                                        ClosingDate = it.closingDate,
                                        ApprovedDate = it.approvedDate,
                                        IsRequriedToApprive = it.IsApprovingRequired
                                     }).ToArray();
            }
        }

        public ParentCaseInfo GetParentInfo(int caseId)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var allCases = uow.GetRepository<Case>().GetAll();
                var allRelations = uow.GetRepository<ParentChildRelation>().GetAll();
                var allPerformers = uow.GetRepository<User>().GetAll();
                var relationInfo = allRelations
                    .Where(it => it.DescendantId == caseId)
                    .Select(it => new { id = it.DescendantId, parentId = it.AncestorId })
                    .GroupJoin(
                            allCases,
                            it => it.parentId,
                            case_ => case_.Id,
                            (parentChild, case_) => new { parentChild, case_ })
                        .SelectMany(
                            t => t.case_.DefaultIfEmpty(),
                            (t, case_) =>
                            new
                            {
                                id = t.parentChild.id,
                                parentId = t.parentChild.parentId,
                                caseNumber = case_.CaseNumber,
                                subject = case_.Caption,
                                performerId = case_.Performer_User_Id,
                                substateId = case_.StateSecondary_Id,
                                caseTypeId = case_.CaseType_Id,
                                registrationDate = case_.RegTime,
                                finishingDate = case_.FinishingDate
                            })
                        .GroupJoin(
                            allPerformers,
                            tempParentChildStruct => tempParentChildStruct.performerId,
                            performer => performer.Id,
                            (tmpParentChild, performer) => new { tmpParentChild, performer })
                        .SelectMany(
                            t => t.performer.DefaultIfEmpty(),
                            (t, performer) =>
                            new
                            {
                                parentId = t.tmpParentChild.parentId,
                                caseNumber = t.tmpParentChild.caseNumber,
                                finishingDate = t.tmpParentChild.finishingDate,
                                performerFirstName = performer == null ? string.Empty : performer.FirstName,
                                performerLastName = performer == null ? string.Empty : performer.SurName,
                            })
                        .FirstOrDefault();
                if (relationInfo != null)
                {
                    return new ParentCaseInfo()
                               {
                                   ParentId = relationInfo.parentId,
                                   CaseNumber = relationInfo.caseNumber,
                                   CaseAdministrator =
                                       new UserNamesStruct()
                                           {
                                               FirstName = relationInfo.performerFirstName,
                                               LastName = relationInfo.performerLastName
                                           },
                                   FinishingDate = relationInfo.finishingDate
                               };
                }
            }

            return null;
        }

        public int? SaveInternalLogMessage(int id, string textInternal, out IDictionary<string, string> errors)
        {
            throw new NotImplementedException();
        }

        private static Case InitNewCaseCopy(
            Case c,
            int userId,
            string ipAddress,
            CaseRegistrationSource source,
            string adUser)
        {
            c.IpAddress = ipAddress;
            c.CaseGUID = Guid.NewGuid();
            c.Id = 0;
            c.CaseNumber = 0;
            c.CaseResponsibleUser_Id = userId;
            c.FinishingDate = null;
            c.RegistrationSource = (int)source;
            c.RegUserId = adUser.GetUserFromAdPath();
            c.RegUserDomain = adUser.GetDomainFromAdPath();
            c.CaseFiles = null;
            return c;
        }

        public Case InitChildCaseFromCase(
            int copyFromCaseid, 
            int userId, 
            string ipAddress, 
            CaseRegistrationSource source, 
            string adUser, 
            out ParentCaseInfo parentCaseInfo)
        {
            var c = this._caseRepository.GetDetachedCaseById(copyFromCaseid);
            if (c == null)
            {
                throw new ArgumentException(string.Format("bad parent case id {0}", copyFromCaseid));
            }

            parentCaseInfo = new ParentCaseInfo
                                     {
                                         ParentId = copyFromCaseid,
                                         CaseNumber = (int)c.CaseNumber,
                                         CaseAdministrator = new UserNamesStruct()
                                                                 {
                                                                     FirstName = c.Administrator != null ? c.Administrator.FirstName : string.Empty,
                                                                     LastName = c.Administrator != null ? c.Administrator.SurName : string.Empty
                                                                 },
                                                                 FinishingDate = c.FinishingDate
                                     };
            return InitNewCaseCopy(c, userId, ipAddress, source, adUser);
        }

        public Case Copy(int copyFromCaseid, int userId, int languageId, string ipAddress, CaseRegistrationSource source, string adUser)
        {
            var c = this._caseRepository.GetDetachedCaseById(copyFromCaseid);
            return InitNewCaseCopy(c, userId, ipAddress, source, adUser);
        }

        public void MarkAsUnread(int caseId)
        {
            this._caseRepository.MarkCaseAsUnread(caseId);
        }

        public void MarkAsRead(int caseId)
        {
            this._caseRepository.MarkCaseAsRead(caseId);
        }

        public IList<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user)
        {
            return this._caseRepository.GetRelatedCases(id, customerId, reportedBy, user).OrderByDescending(c => c.Id).ToList();      
        }

        public Case InitCase(int customerId, int userId, int languageId, string ipAddress, CaseRegistrationSource source, Setting customerSetting, string adUser)
        {
            var c = new Case
                        {
                            Customer_Id = customerId,
                            User_Id = userId,
                            CaseResponsibleUser_Id = userId,
                            IpAddress = ipAddress,
                            CaseGUID = Guid.NewGuid(),
                            RegLanguage_Id = languageId,
                            RegistrationSource = (int)source,
                            Deleted = 0,
                            Region_Id = this._regionService.GetDefaultId(customerId),
                            CaseType_Id = this._caseTypeService.GetDefaultId(customerId),
                            Supplier_Id = this._supplierServicee.GetDefaultId(customerId),
                            Priority_Id = this._priorityService.GetDefaultId(customerId),
                            Status_Id = this._statusService.GetDefaultId(customerId),
                            WorkingGroup_Id = this.userRepository.GetUserDefaultWorkingGroupId(userId, customerId),
                            RegUserId = adUser.GetUserFromAdPath(),
                            RegUserDomain = adUser.GetDomainFromAdPath()
                        };

            // http://redmine.fastdev.se/issues/10997
//            c.WorkingGroup_Id = this._workingGroupService.GetDefaultId(customerId, userId);

            if (customerSetting != null)
            {
                if (customerSetting.SetUserToAdministrator == 1)
                    c.Performer_User_Id = userId;            
                else
                {
                    if (customerSetting.DefaultAdministrator.HasValue) 
                        c.Performer_User_Id = customerSetting.DefaultAdministrator.GetValueOrDefault();
                }
            }
            
            return c;
        }

        public IList<Case> GetCases()
        {
            return this._caseRepository.GetAll().ToList();
        }

        public IList<Case> GetCasesByCustomers(IEnumerable<int> customerIds) 
        {
            return this._caseRepository
                .GetMany(x => customerIds.Contains(x.Customer_Id) && 
                         x.Deleted == 0)
                 .ToList();
        }

        public void UpdateFollowUpDate(int caseId, DateTime? time)
        {
            this._caseRepository.UpdateFollowUpDate(caseId, time);  
        }

        public void Activate(int caseId, int userId, string adUser, out IDictionary<string, string> errors)
        {
            this._caseRepository.Activate(caseId);
            var c = _caseRepository.GetDetachedCaseById(caseId);
            this._caseStatService.UpdateCaseStatistic(c);
            SaveCaseHistory(c, userId, adUser, out errors);  
        }

        public void SendSelfServiceCaseLogEmail(int caseId, CaseMailSetting cms, int caseHistoryId, CaseLog log, string basePath, List<CaseFileDto> logFiles = null)
        {
            if (_emailService.IsValidEmail(cms.HelpdeskMailFromAdress))
            {
                // get new case information
                var newCase = _caseRepository.GetDetachedCaseById(caseId);                

                List<Field> fields = GetCaseFieldsForEmail(newCase, log, cms, string.Empty, 99);

                //get sender email adress
                string helpdeskMailFromAdress = cms.HelpdeskMailFromAdress;
                if (newCase.Workinggroup != null)
                    if (!string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail) && _emailService.IsValidEmail(newCase.Workinggroup.EMail))
                        helpdeskMailFromAdress = newCase.Workinggroup.EMail;

                // if logfiles should be attached to the mail 
                List<string> files = null;
                if (logFiles != null && log != null)
                    if (logFiles.Count > 0)
                        files = logFiles.Select(f => _filesStorage.ComposeFilePath(ModuleName.Log, log.Id, basePath, f.FileName)).ToList();

                if (newCase.PersonsEmail != null)
                {
                    if (log.SendMailAboutCaseToNotifier && newCase.FinishingDate == null)
                    {
                        var to = newCase.PersonsEmail.Split(';', ',');
                        foreach (var t in to)
                        {
                            var curMail = t.Trim();
                            if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                            {
                                // Inform notifier about external lognote
                                int mailTemplateId = (int)GlobalEnums.MailTemplates.CaseIsUpdated;
                                MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                if (m != null)
                                {
                                    if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                                    {
                                        var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                        _emailLogRepository.Add(el);
                                        _emailLogRepository.Commit();
                                        _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, log.HighPriority, files);
                                    }
                                }
                            }
                        }
                    }
                }

                // mail about lognote to Working Group User or Working Group Mail
                if (log.SendMailAboutLog && !string.IsNullOrWhiteSpace(log.EmailRecepientsExternalLog) )
                {
                    int mailTemplateId = (int)GlobalEnums.MailTemplates.CaseIsUpdated;
                    MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                    if (m != null)
                    {
                        if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                        {
                            string[] to = log.EmailRecepientsExternalLog.Replace(Environment.NewLine, "|").Split('|');
                            for (int i = 0; i < to.Length; i++)
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                                _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, log.HighPriority, files);
                            }
                        }
                    }
                }
            }
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cases"></param>
        /// <param name="caseLog"></param>
        /// <param name="caseMailSetting"></param>
        /// <param name="userId"></param>
        /// <param name="adUser"></param>
        /// <param name="errors"></param>
        /// <param name="invoices"></param>
        /// <param name="parentCase"></param>
        /// <param name="sendLogToParentChild">
        /// Flag to send log to parent/child case
        /// </param>
        /// <returns></returns>
        public int SaveCase(
                Case cases, 
                CaseLog caseLog, 
                CaseMailSetting caseMailSetting, 
                int userId, 
                string adUser, 
                out IDictionary<string, string> errors,
                CaseInvoice[] invoices = null,
                Case parentCase = null)
        {
            int ret = 0;

            if (cases == null)
                throw new ArgumentNullException("cases");

            Case c = this.ValidateCaseRequiredValues(cases, caseLog); 

            // unread/status flag update if not case is closed and not changed by adminsitrator 
            //c.Unread = 0;
            if (c.Performer_User_Id != userId && !c.FinishingDate.HasValue)
                c.Unread = 1;

            if (c.Id == 0)
            {
                c.RegTime = DateTime.UtcNow;
                c.ChangeTime = DateTime.UtcNow;
                
                this._caseRepository.Add(c);
            }
            else
            {
                c.ChangeTime = DateTime.UtcNow;
                if (userId == 0)
                {
                    c.ChangeByUser_Id = null;
                }
                else
                {
                    c.ChangeByUser_Id = userId;
                }
                
                this._caseRepository.Update(c);
            }

            this._caseRepository.Commit();
            this._caseStatService.UpdateCaseStatistic(c);
            
            // save casehistory
            var extraFields = new ExtraFieldCaseHistory();
            if (caseLog != null && caseLog.FinishingType != null)
            {
                var fc = _finishingCauseService.GetFinishingTypeName(caseLog.FinishingType.Value);                                                     
                extraFields.ClosingReason = fc ;
            }

            if (parentCase != null)
            {
                this.AddChildCase(cases.Id, parentCase.Id, out errors);
            }

            ret = userId == 0 ? 
                this.SaveCaseHistory(c, userId, adUser, out errors, adUser, extraFields) : 
                this.SaveCaseHistory(c, userId, adUser, out errors, string.Empty, extraFields);

            if (invoices != null)
            {
                this.invoiceArticleService.SaveCaseInvoices(invoices, cases.Id);                
            }

            return ret;
        }
        
        private bool AddChildCase(int childCaseId, int parentCaseId, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var parentChildRelationRepo = uow.GetRepository<ParentChildRelation>();
                var allreadyExists = parentChildRelationRepo
                        .GetAll()
                        .Where(it => it.DescendantId == childCaseId // allready a child for [other|this] case
                            || it.AncestorId == childCaseId // child case is a parent already
                            || it.DescendantId == parentCaseId) // parent case is a child
                        .FirstOrDefault();
                if (allreadyExists != null)
                {

                    errors.Add(
                        "childCaseId",
                        "child case can not contain childs, parent child can not be a child case, child case already presented as child case");

                    return false;
                }
                
                parentChildRelationRepo.Add(new ParentChildRelation()
                                                {
                                                    AncestorId = parentCaseId,
                                                    DescendantId = childCaseId
                                                });
                uow.Save();
            }

            return true;
        }

        public int SaveCaseHistory(
            Case c,
            int userId,
            string adUser,
            out IDictionary<string, string> errors,
                                   string defaultUser = "",
            ExtraFieldCaseHistory extraField = null)
        {
            if (c == null)
                throw new ArgumentNullException("caseHistory");

            errors = new Dictionary<string, string>();
            var h = this.GenerateHistoryFromCase(c, userId, adUser, defaultUser, extraField);
            this._caseHistoryRepository.Add(h);

            if (errors.Count == 0)
                this._caseHistoryRepository.Commit();

            return h.Id;
        }

        public IList<CaseHistory> GetCaseHistoryByCaseId(int caseId)
        {
            return this._caseHistoryRepository.GetCaseHistoryByCaseId(caseId).ToList(); 
        }

        public void SendCaseEmail(
            int caseId, 
            CaseMailSetting cms, 
            int caseHistoryId, 
            string basePath, 
            Case oldCase = null, 
            CaseLog log = null, 
            List<CaseFileDto> logFiles = null)
        {
            //get sender email adress
            var helpdeskMailFromAdress = string.Empty;
            var containsProductAreaMail = false;

            if (!string.IsNullOrEmpty((cms.HelpdeskMailFromAdress)))
            {
                helpdeskMailFromAdress = cms.HelpdeskMailFromAdress.Trim();                
            }

            if (!this._emailService.IsValidEmail(helpdeskMailFromAdress))
            {
                return;
            }

            // get new case information
            var newCase = _caseRepository.GetDetachedCaseById(caseId);
            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);
            bool dontSendMailToNotfier = false;
            var isCreatingCase = oldCase == null || oldCase.Id == 0;
            var isClosingCase = newCase.FinishingDate != null;

            // get list of fields to replace [#1] tags in the subjcet and body texts
            List<Field> fields = GetCaseFieldsForEmail(newCase, log, cms, string.Empty , 0);
            
            if (newCase.Workinggroup != null)
                if (!string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail) && _emailService.IsValidEmail(newCase.Workinggroup.EMail))  
                    helpdeskMailFromAdress = newCase.Workinggroup.EMail;

            // if logfiles should be attached to the mail 
            List<string> files = null;
            if (logFiles != null && log != null)
                if (logFiles.Count > 0)
                    files = logFiles.Select(f => _filesStorage.ComposeFilePath(ModuleName.Log, log.Id,basePath, f.FileName)).ToList();

            // sub state should not generate email to notifier
            if (newCase.StateSecondary != null)
                dontSendMailToNotfier = newCase.StateSecondary.NoMailToNotifier == 1 ? true : false; 

            // send email about new case to notifier or tblCustomer.NewCaseEmailList
            //if (!isClosingCase && !isCreatingCase) Why this????
            if (!isClosingCase && isCreatingCase)
            {
                // get mail template 
                int mailTemplateId = (int)GlobalEnums.MailTemplates.NewCase;
                string customEmailSender1 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;

                if (string.IsNullOrWhiteSpace(customEmailSender1))
                    customEmailSender1 = cms.CustomeMailFromAddress.WGEmail;

                if (string.IsNullOrWhiteSpace(customEmailSender1))
                    customEmailSender1 = cms.CustomeMailFromAddress.SystemEmail;
                if (!string.IsNullOrEmpty(customEmailSender1))
                {
                    customEmailSender1 = customEmailSender1.Trim();
                }

                MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                if (m != null)
                {
                    if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                    {
                        if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier && !string.IsNullOrEmpty(newCase.PersonsEmail))
                        {
                            var to = newCase.PersonsEmail.Split(';', ',');
                            foreach (var t in to)
                            {
                                var curMail = t.Trim();
                                if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(customEmailSender1));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 1);

                                    _emailService.SendEmail(customEmailSender1, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, false, files);
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(cms.SendMailAboutNewCaseTo))
                        {
                            string[] to = cms.SendMailAboutNewCaseTo.Split(';');
                            for (int i = 0; i < to.Length; i++)
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(customEmailSender1));
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 2);
                                _emailService.SendEmail(customEmailSender1, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, false, files);
                            }
                        }
                    }
                }

                // send mail template from productArea if productarea has a mailtemplate
                if (newCase.ProductArea_Id.HasValue && newCase.ProductArea != null)
                {
                    // get mail template from productArea
                    mailTemplateId = 0;

                    if (newCase.ProductArea.MailID.HasValue)
                        mailTemplateId = newCase.ProductArea.MailTemplate.MailID;
                    

                    if (mailTemplateId > 0)
                    {
                        MailTemplateLanguageEntity mm = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                        if (mm != null)
                        {
                            if (!String.IsNullOrEmpty(mm.Body) && !String.IsNullOrEmpty(mm.Subject))
                            {
                                if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier && !string.IsNullOrEmpty(newCase.PersonsEmail))
                                {
                                    var to = newCase.PersonsEmail.Split(';', ',');
                                    foreach (var t in to)
                                    {
                                        var curMail = t.Trim();
                                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                        {
                                            var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(customEmailSender1));
                                            _emailLogRepository.Add(el);
                                            _emailLogRepository.Commit();
                                            fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 1);

                                            _emailService.SendEmail(customEmailSender1, el.EmailAddress, mm.Subject, mm.Body, fields, el.MessageId, false, files);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

            // send mail template from productArea if productarea has a mailtemplate
            if (!isCreatingCase && !isClosingCase)
            {
                if (newCase.ProductArea_Id.HasValue && newCase.ProductArea != null && oldCase.ProductAreaSetDate == null)
                {
                    
                    int mailTemplateId = 0;
                    string customEmailSender1 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;

                    if (string.IsNullOrWhiteSpace(customEmailSender1))
                        customEmailSender1 = cms.CustomeMailFromAddress.WGEmail;

                    if (string.IsNullOrWhiteSpace(customEmailSender1))
                        customEmailSender1 = cms.CustomeMailFromAddress.SystemEmail;

                    if (!string.IsNullOrEmpty(customEmailSender1))
                    {
                        customEmailSender1 = customEmailSender1.Trim();
                    }

                    // get mail template from productArea
                    if (newCase.ProductArea.MailID.HasValue)
                        mailTemplateId = newCase.ProductArea.MailTemplate.MailID;
                    
                    if (mailTemplateId > 0)
                    {
                        MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                            {
                                if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier && !string.IsNullOrEmpty(newCase.PersonsEmail))
                                {
                                    var to = newCase.PersonsEmail.Split(';', ',');
                                    foreach (var t in to)
                                    {
                                        var curMail = t.Trim();
                                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                        {
                                            var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(customEmailSender1));
                                            _emailLogRepository.Add(el);
                                            _emailLogRepository.Commit();
                                            fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 1);

                                            _emailService.SendEmail(customEmailSender1, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, false, files);
                                            containsProductAreaMail = true;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

            // send email to tblCase.Performer_User_Id
            if (((!isClosingCase && isCreatingCase && newCase.Performer_User_Id.HasValue) 
                || (!isCreatingCase && newCase.Performer_User_Id != oldCase.Performer_User_Id))
                && newCase.Administrator != null)
            {
                if (newCase.Administrator.AllocateCaseMail == 1 && this._emailService.IsValidEmail(newCase.Administrator.Email))
                {
                    this.SendTemplateEmail(GlobalEnums.MailTemplates.AssignedCaseToUser, newCase, log, caseHistoryId, cms, newCase.Administrator.Email);
                }

                // send sms to tblCase.Performer_User_Id 
                if (newCase.Administrator.AllocateCaseSMS == 1 && !string.IsNullOrWhiteSpace(newCase.Administrator.CellPhone) && newCase.Customer != null)
                {
                    int mailTemplateId = (int)GlobalEnums.MailTemplates.SmsAssignedCaseToUser;
                    MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                    if (m != null)
                    {
                        if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                        {
                            var smsTo = GetSmsRecipient(customerSetting, newCase.Administrator.CellPhone);
                            var el = new EmailLog(caseHistoryId, mailTemplateId, smsTo, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                            fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 4);
                            _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, GetSmsSubject(customerSetting), m.Body, fields, el.MessageId);
                        }
                    }
                }
            }
                

            // send email priority has changed
            if (newCase.FinishingDate == null && oldCase != null && oldCase.Id > 0)
                if (newCase.Priority_Id != oldCase.Priority_Id)
                {
                    if (newCase.Priority != null)
                    {
                        if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                        {
                            int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToPriority;
                            MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                            if (m != null)
                            {
                                if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.Priority.EMailList, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 5);
                                    _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }
                        }
                    }
                }

            // send email working group has changed
            if (!isClosingCase
                && newCase.Workinggroup != null
                && (isCreatingCase || (!isCreatingCase && newCase.WorkingGroup_Id != oldCase.WorkingGroup_Id)))
            {
                int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToWorkinggroup;
                MailTemplateLanguageEntity m =
                    _mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                        newCase.Customer_Id,
                        newCase.RegLanguage_Id,
                        mailTemplateId);
                if (m != null)
                {
                    if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                    {
                        string wgEmails = string.Empty;
                        if (!string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail))
                            wgEmails = newCase.Workinggroup.EMail;

                        if (newCase.Workinggroup.AllocateCaseMail == 1 && !string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail))
                        {
                            var el = new EmailLog(
                                caseHistoryId,
                                mailTemplateId,
                                wgEmails,
                                _emailService.GetMailMessageId(helpdeskMailFromAdress));
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                            fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 6);
                            _emailService.SendEmail(
                                helpdeskMailFromAdress,
                                el.EmailAddress,
                                m.Subject,
                                m.Body,
                                fields,
                                el.MessageId);
                        }
                    }
                }
            }

            // send email

            // send email when product area is set
            if (!isClosingCase && !isCreatingCase
                && oldCase.ProductAreaSetDate == null && newCase.RegistrationSource == 3
                && !cms.DontSendMailToNotifier
                && newCase.ProductArea != null
                && newCase.ProductArea.MailID.HasValue
                && newCase.ProductArea.MailID.Value > 0 && !string.IsNullOrEmpty(newCase.PersonsEmail))
                {
                    var to = newCase.PersonsEmail.Split(';', ',');
                    foreach (var t in to)
                    {
                        var curMail = t.Trim();
                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                        {
                            int mailTemplateId = newCase.ProductArea.MailID.Value;
                            MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                            if (m != null)
                            {
                                if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 7);
                                    _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, false, files);
                                }
                            }
                        }
                    }
                }

            // case closed email
            if (newCase.FinishingDate.HasValue && newCase.Customer != null)
            {
                int mailTemplateId = (int)GlobalEnums.MailTemplates.ClosedCase;

                string customEmailSender2 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;
                if (string.IsNullOrWhiteSpace(customEmailSender2))
                    customEmailSender2 = cms.CustomeMailFromAddress.WGEmail;
                if (string.IsNullOrWhiteSpace(customEmailSender2))
                    customEmailSender2 = cms.CustomeMailFromAddress.SystemEmail;

                if (!string.IsNullOrEmpty(customEmailSender2))
                {
                    customEmailSender2 = customEmailSender2.Trim();
                }

                MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                if (m != null)
                {
                    if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                    {
                        if (!string.IsNullOrWhiteSpace(newCase.Customer.CloseCaseEmailList))
                        {
                            string[] to = newCase.Customer.CloseCaseEmailList.Split(';');
                            for (int i = 0; i < to.Length; i++)
                            {
                                if (_emailService.IsValidEmail(to[i]))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(customEmailSender2));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 8);
                                    _emailService.SendEmail(customEmailSender2, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, false, files);
                                }
                            }
                        }

                        if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier)
                        {                                
                            var to = newCase.PersonsEmail.Split(';', ',');
                            foreach (var t in to)
                            {
                                var curMail = t.Trim();
                                if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                {

                                    var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(customEmailSender2));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 9);
                                    _emailService.SendEmail(customEmailSender2, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, false, files);
                                }
                            }
                                
                        }

                        // send sms
                        if (newCase.SMS == 1 && !dontSendMailToNotfier && !string.IsNullOrWhiteSpace(newCase.PersonsCellphone))
                        {
                            int smsMailTemplateId = (int)GlobalEnums.MailTemplates.SmsClosedCase;
                            MailTemplateLanguageEntity mt = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, smsMailTemplateId);
                            if (mt != null)
                            {
                                if (!String.IsNullOrEmpty(mt.Body) && !String.IsNullOrEmpty(mt.Subject))
                                {
                                    var smsTo = GetSmsRecipient(customerSetting, newCase.PersonsCellphone);
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, smsTo, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 10);
                                    _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, GetSmsSubject(customerSetting), mt.Body, fields, el.MessageId);
                                }
                            }
                        }
                    }
                }
            }

            // State Secondary Email TODO ikea ims only??
            // Commented out for now, will be added later with a better solution decided 20150626
            //if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier && !isClosedMailSentToNotifier && oldCase != null && oldCase.Id > 0)  
            //    if (newCase.StateSecondary_Id != oldCase.StateSecondary_Id && newCase.StateSecondary_Id > 0)
            //        if (_emailService.IsValidEmail(newCase.PersonsEmail))
            //        {
            //            int mailTemplateId = (int)GlobalEnums.MailTemplates.ClosedCase;

            //            string customEmailSender3 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;
            //            if (string.IsNullOrWhiteSpace(customEmailSender3))
            //                customEmailSender3 = cms.CustomeMailFromAddress.WGEmail;
            //            if (string.IsNullOrWhiteSpace(customEmailSender3))
            //                customEmailSender3 = cms.CustomeMailFromAddress.SystemEmail;

            //            MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
            //            if (m != null)
            //            {
            //                if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
            //                {
            //                    var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(customEmailSender3));
            //                    _emailLogRepository.Add(el);
            //                    _emailLogRepository.Commit();
            //                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 11);
            //                    _emailService.SendEmail(customEmailSender3, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
            //                }
            //            }
            //        }
            if (!containsProductAreaMail)
            {
                this.caseMailer.InformNotifierIfNeeded(
                                            caseHistoryId,
                                            fields,
                                            log,
                                            dontSendMailToNotfier,
                                            newCase,
                                            helpdeskMailFromAdress,
                                            files,
                                            cms.CustomeMailFromAddress, isCreatingCase);
            }

            this.caseMailer.InformAboutInternalLogIfNeeded(
                                        caseHistoryId,
                                        fields,
                                        log,
                                        newCase,
                                        helpdeskMailFromAdress,
                                        files);
        }

        private void SendTemplateEmail(
            GlobalEnums.MailTemplates mailTemplateEnum, 
            Case case_,
            CaseLog log,
            int caseHistoryId,
            CaseMailSetting cms,
            string recipient)
        {
            var mailTemplateId = (int)mailTemplateEnum;
            var m = this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(case_.Customer_Id, case_.RegLanguage_Id, mailTemplateId);
            if (m != null && !string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
            {
                var el = new EmailLog(
                    caseHistoryId, 
                    mailTemplateId, 
                    recipient, 
                    this._emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                this._emailLogRepository.Add(el);
                this._emailLogRepository.Commit();
                var fields = this.GetCaseFieldsForEmail(case_, log, cms, el.EmailLogGUID.ToString(), 3);
                this._emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
            }
        }

        private Case ValidateCaseRequiredValues(Case c, CaseLog caseLog)
        {
            Case ret = c;
            
            ret.PersonsCellphone = string.IsNullOrWhiteSpace(c.PersonsCellphone) ? string.Empty : c.PersonsCellphone;
            ret.PersonsEmail = string.IsNullOrWhiteSpace(c.PersonsEmail) ? string.Empty : c.PersonsEmail;
            ret.PersonsName = string.IsNullOrWhiteSpace(c.PersonsName) ? string.Empty : c.PersonsName;
            ret.PersonsPhone = string.IsNullOrWhiteSpace(c.PersonsPhone) ? string.Empty : c.PersonsPhone;
            ret.Place = string.IsNullOrWhiteSpace(c.Place) ? string.Empty : c.Place;
            ret.InventoryNumber = string.IsNullOrWhiteSpace(c.InventoryNumber) ? string.Empty : c.InventoryNumber;
            ret.InventoryType = string.IsNullOrWhiteSpace(c.InventoryType) ? string.Empty : c.InventoryType;
            ret.InventoryLocation = string.IsNullOrWhiteSpace(c.InventoryLocation) ? string.Empty : c.InventoryLocation;
            ret.InvoiceNumber = string.IsNullOrWhiteSpace(c.InvoiceNumber) ? string.Empty : c.InvoiceNumber;
            ret.Caption = string.IsNullOrWhiteSpace(c.Caption) ? string.Empty : c.Caption;
            ret.Description = string.IsNullOrWhiteSpace(c.Description) ? string.Empty : c.Description;
            ret.Miscellaneous = string.IsNullOrWhiteSpace(c.Miscellaneous) ? string.Empty : c.Miscellaneous;
            ret.Available = string.IsNullOrWhiteSpace(c.Available) ? string.Empty : c.Available;
            ret.IpAddress = string.IsNullOrWhiteSpace(c.IpAddress) ? string.Empty : c.IpAddress;
            
            return ret;
        }

        private CaseHistory GenerateHistoryFromCase(
            Case c, 
            int userId, 
            string adUser, 
            string defaultUser="", 
            ExtraFieldCaseHistory extraField = null)
        {
            var h = new CaseHistory();
            var user = this.userRepository.GetUser(userId);
            h.AgreedDate = c.AgreedDate;
            h.ApprovedDate = c.AgreedDate;
            h.ApprovedBy_User_Id = c.ApprovedBy_User_Id; 
            h.Available = c.Available;
            h.Caption = c.Caption;
            h.Case_Id = c.Id;
            h.CaseHistoryGUID = Guid.NewGuid(); 
            h.CaseNumber = c.CaseNumber;
            h.CaseResponsibleUser_Id = c.CaseResponsibleUser_Id;
            h.CaseType_Id = c.CaseType_Id; 
            h.Category_Id = c.Category_Id; 
            h.Change_Id = c.Change_Id; 
            h.ContactBeforeAction = c.ContactBeforeAction;
            h.Cost = c.Cost;
            h.CreatedDate = DateTime.UtcNow;
            if (defaultUser != string.Empty)
            {
                h.CreatedByUser = defaultUser; // used for Self Service Project
            }
            else
            {
                if (user != null)
                {
                h.CreatedByUser = user.FirstName + ' ' + user.SurName; 
                }
            }
                
            h.Currency = c.Currency;
            h.Customer_Id = c.Customer_Id;
            h.Deleted = c.Deleted; 
            h.Department_Id = c.Department_Id;  
            h.Description = c.Description;
            h.ExternalTime = c.ExternalTime;
            h.FinishingDate = c.FinishingDate;
            h.FinishingDescription = c.FinishingDescription;
            h.FollowUpDate = c.FollowUpDate;
            h.Impact_Id = c.Impact_Id;
            h.InvoiceNumber = c.InvoiceNumber; 
            h.InventoryLocation = c.InventoryLocation;
            h.InventoryNumber = c.InventoryNumber;
            h.InventoryType = c.InventoryType;
            h.IpAddress = c.IpAddress;
            h.Miscellaneous = c.Miscellaneous;
            h.LockCaseToWorkingGroup_Id = c.LockCaseToWorkingGroup_Id; 
            h.OU_Id = c.OU_Id; 
            h.OtherCost = c.OtherCost;
            h.Performer_User_Id = c.Performer_User_Id; 
            h.PersonsCellphone = c.PersonsCellphone;
            h.PersonsEmail = c.PersonsEmail;
            h.PersonsName = c.PersonsName;
            h.PersonsPhone = c.PersonsPhone;
            h.Place = c.Place;
            h.PlanDate = c.PlanDate;
            h.Priority_Id = c.Priority_Id; 
            h.ProductArea_Id = c.ProductArea_Id; 
            h.ProductAreaSetDate = c.ProductAreaSetDate;
            h.Project_Id = c.Project_Id;
            h.Problem_Id = c.Problem_Id; 
            h.ReferenceNumber = c.ReferenceNumber;
            h.RegistrationSource = c.RegistrationSource;
            h.RegLanguage_Id = c.RegLanguage_Id;
            if (!string.IsNullOrEmpty(adUser))
            {
            h.RegUserDomain = adUser.GetDomainFromAdPath();
            h.RegUserId = adUser.GetUserFromAdPath(); 
            }
            
            h.RelatedCaseNumber = c.RelatedCaseNumber;
            h.Region_Id = c.Region_Id; 
            h.ReportedBy = c.ReportedBy;
            h.Status_Id = c.Status_Id; 
            h.StateSecondary_Id = c.StateSecondary_Id; 
            h.SMS = c.SMS;
            h.SolutionRate = c.SolutionRate;
            h.Supplier_Id = c.Supplier_Id; 
            h.System_Id = c.System_Id; 
            h.UserCode = c.UserCode;
            h.User_Id = c.User_Id;
            h.Urgency_Id = c.Urgency_Id; 
            h.Unread = c.Unread; 
            h.WatchDate = c.WatchDate;
            h.Verified = c.Verified;
            h.VerifiedDescription = c.VerifiedDescription; 
            h.WorkingGroup_Id = c.WorkingGroup_Id;
            h.CausingPartId = c.CausingPartId;
            h.DefaultOwnerWG_Id = c.DefaultOwnerWG_Id;
            h.RegistrationSourceCustomer_Id = c.RegistrationSourceCustomer_Id;

            if (extraField != null)
            {
                h.CaseFile = extraField.CaseFile;
                h.LogFile  = extraField.LogFile;
                h.CaseLog  = extraField.CaseLog;
                h.ClosingReason = extraField.ClosingReason;
            }
            
            return h;
        }

        private List<Field> GetCaseFieldsForEmail(Case c, CaseLog l, CaseMailSetting cms, string emailLogGuid,int stateHelper)
        {
            List<Field> ret = new List<Field>();

            ret.Add(new Field { Key = "[#1]", StringValue = c.CaseNumber.ToString() } );
            ret.Add(new Field { Key = "[#16]", StringValue = c.RegTime.ToString() } ); 
            ret.Add(new Field { Key = "[#22]", StringValue = c.LastChangedByUser != null ? c.LastChangedByUser.FirstName + " " + c.LastChangedByUser.SurName : string.Empty }); 
            ret.Add(new Field { Key = "[#3]", StringValue = c.PersonsName } ); 
            ret.Add(new Field { Key = "[#8]", StringValue = c.PersonsEmail } ); 
            ret.Add(new Field { Key = "[#9]", StringValue = c.PersonsPhone } );
            ret.Add(new Field { Key = "[#18]", StringValue = c.PersonsCellphone } ); 
            ret.Add(new Field { Key = "[#2]", StringValue = c.Customer != null ? c.Customer.Name : string.Empty } ); 
            ret.Add(new Field { Key = "[#24]", StringValue = c.Place } ); 
            ret.Add(new Field { Key = "[#17]", StringValue = c.InventoryNumber } ); 
            ret.Add(new Field { Key = "[#25]", StringValue = c.CaseType != null ? c.CaseType.Name : string.Empty });
            ret.Add(new Field { Key = "[#26]", StringValue = c.Category != null ? c.Category.Name : string.Empty } ); 
            ret.Add(new Field { Key = "[#4]", StringValue = c.Caption } ); 
            ret.Add(new Field { Key = "[#5]", StringValue = c.Description } ); 
            ret.Add(new Field { Key = "[#23]", StringValue = c.Miscellaneous } ); 
            ret.Add(new Field { Key = "[#19]", StringValue = c.Available } ); 
            ret.Add(new Field { Key = "[#15]", StringValue = c.Workinggroup != null ? c.Workinggroup.WorkingGroupName : string.Empty });
            ret.Add(new Field { Key = "[#13]", StringValue = c.Workinggroup != null ? c.Workinggroup.EMail : string.Empty }); 
            ret.Add(new Field { Key = "[#6]", StringValue = c.Administrator != null ? c.Administrator.FirstName : string.Empty }); 
            ret.Add(new Field { Key = "[#7]", StringValue = c.Administrator != null ? c.Administrator.SurName : string.Empty }); 
            ret.Add(new Field { Key = "[#12]", StringValue = c.Priority != null ? c.Priority.Name : string.Empty });
            ret.Add(new Field { Key = "[#20]", StringValue = c.Priority != null ? c.Priority.Description : string.Empty });
            ret.Add(new Field { Key = "[#21]", StringValue = c.WatchDate.ToString() } );
            if (l != null)
            {
                ret.Add(new Field { Key = "[#10]", StringValue = l.TextExternal });
                ret.Add(new Field { Key = "[#11]", StringValue = l.TextInternal });
            }
            // selfservice site
            if (cms != null)
            {
                if (emailLogGuid == string.Empty)
                    emailLogGuid = " >> *" + stateHelper.ToString() + "*";
                string site = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + emailLogGuid;  
                string url = "<br><a href='" + site + "'>" + site + "</a>";
                ret.Add(new Field { Key = "[#98]", StringValue = url });
            }

            // heldesk site
            if (cms != null)
            {
                string site = cms.AbsoluterUrl + "Cases/edit/" + c.Id.ToString();
                string url = "<br><a href='" + site +  "'>" + site + "</a>";
                ret.Add(new Field { Key = "[#99]", StringValue = url });
            }

            // Survey template
            if (cms != null)
            {
                /// if case is closed and was no vote in survey - add HTML inormation about survey
                if (c.IsClosed() && (this.surveyService.GetByCaseId(c.Id) == null))
                {
                    var template = new SurveyTemplate()
                    {
                        VoteBadLink =
                            string.Format(
                                "{0}Survey/vote/{1}?voteId=bad",
                                cms.AbsoluterUrl,
                                c.Id),
                        VoteBadText = Translator.Translate("Inte nöjd"),
                        VoteNormalLink =
                            string.Format(
                                "{0}Survey/vote/{1}?voteId=normal",
                                cms.AbsoluterUrl,
                                c.Id),
                        VoteNormalText = Translator.Translate("Nöjd"),
                        VoteGoodLink =
                            string.Format(
                                "{0}Survey/vote/{1}?voteId=good",
                                cms.AbsoluterUrl,
                                c.Id),
                        VoteGoodText = Translator.Translate("Mycket nöjd"),
                    };
                    ret.Add(new Field { Key = "[#777]", StringValue = template.TransformText() });
                }
                else
                {
                    ret.Add(new Field { Key = "[#777]", StringValue = string.Empty });
                }
            }

            return ret;
        }

        private string GetSmsSubject(Setting cs)
        {
            string ret = string.Empty; 
            if (cs != null)
            {
                ret = cs.SMSEMailDomainUserName.Left(11);
                if (!string.IsNullOrWhiteSpace(cs.SMSEMailDomainPassword))
                    ret = ret + " " + cs.SMSEMailDomainPassword;
            }
            return ret;
        }

        private string GetSmsRecipient(Setting cs, string phone)
        {
            string ret = string.Empty; 
            if (cs != null)
            {
                ret = phone.RemoveNonNumericValuesFromString() + (cs.SMSEMailDomain.StartsWith("@") ? string.Empty : "@") + cs.SMSEMailDomain;
            }
            return ret;
        }
    }
}

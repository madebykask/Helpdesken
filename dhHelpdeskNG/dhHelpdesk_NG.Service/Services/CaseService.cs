using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Feedback;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.Services.BusinessLogic.Mappers.Feedback;

namespace DH.Helpdesk.Services.Services
{

    using DH.Helpdesk.BusinessData.Models.BusinessRules;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.ChidCase;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Email;    
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Constants;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Enums.BusinessRule;        
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Cases;    
    using DH.Helpdesk.Domain;    
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
    using DH.Helpdesk.Services.Infrastructure;
    using Feedback;
    using BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;


    public interface ICaseService
    {
        IList<Case> GetProjectCases(int customerId, int projectId);
        IList<Case> GetProblemCases(int customerId, int problemId);

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
        IList<CaseHistoryOverview> GetCaseHistories(int caseId);
        List<DynamicCase> GetAllDynamicCases();
        DynamicCase GetDynamicCase(int id);
        IList<Case> GetProblemCases(int problemId);
        IList<ExtendedCaseFormModel> GetExtendedCaseForm(int? caseSolutionId, int customerId, int? caseId, int userLanguageId, string userGuid, int? caseStateSecondaryId, int? caseWorkingGroupId, string extendedCasePath, int? userId, string userName, ApplicationType applicationType, int userWorkingGroupId);
        ExtendedCaseDataEntity GetExtendedCaseData(Guid extendedCaseGuid);

        void CreateExtendedCaseRelationship(int caseId, int extendedCaseDataId);

        int LookupLanguage(int custid, string notid, int regid, int depid, string notifierid);

        int SaveCase(
            Case cases,
            CaseLog caseLog,            
            int userId,
            string adUser,
            CaseExtraInfo caseExtraInfo,
            out IDictionary<string, string> errors,
            Case parentCase = null,
            string caseExtraFollowers = null);

        int SaveCaseHistory(
            Case c,
            int userId,
            string adUser,
            string createdByApp,
            out IDictionary<string, string> errors,
            string defaultUser = "",
            ExtraFieldCaseHistory extraField = null,
            string caseExtraFollowers = null);

        void SendCaseEmail(int caseId, CaseMailSetting cms, int caseHistoryId, string basePath, TimeZoneInfo userTimeZone,
                           Case oldCase = null, CaseLog log = null, List<CaseFileDto> logFiles = null, User currentLoggedInUser = null);

        List<BusinessRuleActionModel> CheckBusinessRules(BREventType occurredEvent, Case currentCase, Case oldCase = null);

        void ExecuteBusinessActions(List<BusinessRuleActionModel> actions, Case currentCase, CaseLog log, TimeZoneInfo userTimeZone,
                                    int caseHistoryId, string basePath, int currentLanguageId, CaseMailSetting caseMailSetting,
                                    List<CaseFileDto> logFiles = null
                                    );

        void UpdateFollowUpDate(int caseId, DateTime? time);
        void MarkAsUnread(int caseId);
        void MarkAsRead(int caseId);
        void SendSelfServiceCaseLogEmail(int caseId, CaseMailSetting cms, int caseHistoryId, CaseLog log, string basePath, TimeZoneInfo userTimeZone, List<CaseFileDto> logFiles = null, bool caseIsActivated = false);
        void Activate(int caseId, int userId, string adUser, string createByApp, out IDictionary<string, string> errors);
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

        Dictionary<int, string> GetCaseFiles(List<int> caseIds);

        List<CaseFilterFavorite> GetMyFavorites(int customerId, int userId);

        string SaveFavorite(CaseFilterFavorite favorite);

        string DeleteFavorite(int favoriteId);
        void DeleteChildCaseFromParent(int id, int parentId);
        bool AddParentCase(int id, int parentId, bool independent = false);
		void SetIndependentChild(int caseID, bool independentChild);
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
        private readonly UserRepository _userRepository;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly IEmailService _emailService;
        private readonly ISettingService _settingService;
        private readonly IFilesStorage _filesStorage;
        private readonly ILogRepository _logRepository;
        private readonly ILogService _logService;
        private readonly ILogFileRepository _logFileRepository;
        private readonly IFormFieldValueRepository _formFieldValueRepository;
        private readonly IFeedbackTemplateService _feedbackTemplateService;


        private readonly ICaseMailer caseMailer;

        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private ISurveyService surveyService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ICaseLockService _caseLockService;

        private readonly CaseStatisticService _caseStatService;
        private readonly ICaseFilterFavoriteRepository _caseFilterFavoriteRepository;

        private readonly IMail2TicketRepository _mail2TicketRepository;

        private readonly IBusinessRuleService _businessRuleService;
        private readonly IEmailGroupService _emailGroupService;
        private readonly IUserService _userService;
        private readonly IEmailSendingSettingsProvider _emailSendingSettingsProvider;
        private readonly ICaseExtraFollowersService _caseExtraFollowersService;
        private readonly IProductAreaService _productAreaService;
        private readonly IExtendedCaseFormRepository _extendedCaseFormRepository;
        private readonly IExtendedCaseDataRepository _extendedCaseDataRepository;


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
            ICaseLockService caseLockService,
            CaseStatisticService caseStatService,
            ICaseFilterFavoriteRepository caseFilterFavoriteRepository,
            IMail2TicketRepository mail2TicketRepository,
            IBusinessRuleService businessRuleService,
            IEmailGroupService emailGroupService,
            IUserService userService,
            IEmailSendingSettingsProvider emailSendingSettingsProvider,
            ICaseExtraFollowersService caseExtraFollowersService,
            IFeedbackTemplateService feedbackTemplateService,
            IProductAreaService productAreaService,
            IExtendedCaseFormRepository extendedCaseFormRepository,
            IExtendedCaseDataRepository extendedCaseDataRepository
            )

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
            this._userRepository = userRepository;
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
            this._caseFilterFavoriteRepository = caseFilterFavoriteRepository;
            this._mail2TicketRepository = mail2TicketRepository;
            this._businessRuleService = businessRuleService;
            this._emailGroupService = emailGroupService;
            this._userService = userService;
            this._emailSendingSettingsProvider = emailSendingSettingsProvider;
            _caseExtraFollowersService = caseExtraFollowersService;
            _feedbackTemplateService = feedbackTemplateService;
            _productAreaService = productAreaService;
            this._extendedCaseFormRepository = extendedCaseFormRepository;
            this._extendedCaseDataRepository = extendedCaseDataRepository;
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

        public IList<ExtendedCaseFormModel> GetExtendedCaseForm(int? caseSolutionId, int customerId, int? caseId, int userLanguageId, string userGuid, int? caseStateSecondaryId, int? caseWorkingGroupId, string extendedCasePath, int? userId, string userName, ApplicationType applicationType, int userWorkingGroupId)
        {
            return _extendedCaseFormRepository.GetExtendedCaseForm((caseSolutionId.HasValue ? caseSolutionId.Value: 0), customerId, (caseId.HasValue ? caseId.Value : 0), userLanguageId, userGuid, (caseStateSecondaryId.HasValue ? caseStateSecondaryId.Value : 0), (caseWorkingGroupId.HasValue ? caseWorkingGroupId.Value : 0), extendedCasePath, userId, userName, applicationType, userWorkingGroupId);
        }

        public ExtendedCaseDataEntity GetExtendedCaseData(Guid extendedCaseGuid)
        {
            return _extendedCaseDataRepository.GetExtendedCaseData(extendedCaseGuid);
        }

        public void CreateExtendedCaseRelationship(int caseId, int extendedCaseDataId)
        {
            using (var uow = unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var rep = uow.GetRepository<Case_ExtendedCaseEntity>();
                var relation = rep.Find(it => it.Case_Id == caseId && it.ExtendedCaseData_Id == extendedCaseDataId).FirstOrDefault();
                if (relation == null)
                {
                    rep.Add(new Case_ExtendedCaseEntity() { Case_Id = caseId, ExtendedCaseData_Id = extendedCaseDataId });
                    uow.Save();
                }                                
            }
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

            DeleteExtendedCase(id);

            //delete CaseIsAbout
            this.DeleteCaseIsAboutFor(id);

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
                // delete Mail2tickets with log
                foreach (var l in logs)
                {
                    this._mail2TicketRepository.DeleteByLogId(l.Id);
                }
                this._mail2TicketRepository.Commit();


                foreach (var l in logs)
                {
                    this._logRepository.Delete(l);
                }
                this._logRepository.Commit();
            }

            //Delete Mail2Tickets by caseId
            this._mail2TicketRepository.DeleteByCaseId(id);
            this._mail2TicketRepository.Commit();

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


        public int LookupLanguage(int custid, string notid, int regid, int depid, string notifierid)
        {
            int res = this._caseRepository.LookupLanguage(custid, notid, regid, depid, notifierid);
            return res;
        }


        public List<CaseFilterFavorite> GetMyFavorites(int customerId, int userId)
        {
            var ret = this._caseFilterFavoriteRepository.GetUserFavoriteFilters(customerId, userId);
            return ret;
        }

        public string SaveFavorite(CaseFilterFavorite favorite)
        {
            var res = this._caseFilterFavoriteRepository.SaveFavorite(favorite);
            if (res == string.Empty)
            {
                try
                {
                    this._caseFilterFavoriteRepository.Commit();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return res;
        }

        public string DeleteFavorite(int favoriteId)
        {
            var res = this._caseFilterFavoriteRepository.DeleteFavorite(favoriteId);
            if (res == string.Empty)
            {
                try
                {
                    this._caseFilterFavoriteRepository.Commit();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return res;
        }

        public void DeleteChildCaseFromParent(int id, int parentCaseId)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var relationsRepo = uow.GetRepository<ParentChildRelation>();
                var relation = relationsRepo.GetAll().FirstOrDefault(it => it.DescendantId == id);
                if (relation == null || relation.AncestorId != parentCaseId)
                {
                    throw new ArgumentException(string.Format("bad parentCaseId \"{0}\" for case id \"{1}\"", parentCaseId, id));
                }

                relationsRepo.Delete(relation);
                uow.Save();
            }
        }

        public bool AddParentCase(int childCaseId, int parentCaseId, bool independent = false)
        {
            if (childCaseId == parentCaseId)
                return false;
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var parentChildRelationRepo = uow.GetRepository<ParentChildRelation>();
                var allreadyExists = parentChildRelationRepo.GetAll()
                        .Where(it => it.DescendantId == childCaseId // allready a child for [other|this] case
                            || it.AncestorId == childCaseId // child case is a parent already
                            || it.DescendantId == parentCaseId) // parent case is a child
                        .FirstOrDefault();
                if (allreadyExists != null)
                {
                    return false;
                }

                parentChildRelationRepo.Add(new ParentChildRelation()
                {
                    AncestorId = parentCaseId,
                    DescendantId = childCaseId,
					Independent = independent
                });
                uow.Save();
            }
            return true;
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
                var caseFieldSettingsRep = uow.GetRepository<CaseFieldSetting>();

                var customerCases = customerRepository.GetAll()
                                    .GetByIds(customerIds)
                                    .MapToCustomerCases(caseFieldSettingsRep.GetAll(), problemsRep.GetAll(), userId);



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

        //TODO: Extremely needs to be refactored
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
                        .Select(it => new { id = it.DescendantId, parentId = it.AncestorId, independent = it.Independent })
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
                                independent = t.parentChild.independent,
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
                                independent = t.tmpParentChild.independent,
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
                                independent = t.tmpParentChild.independent,
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
                                independent = t.tmpParentChild.independent,
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
                    IsRequriedToApprive = it.IsApprovingRequired,
                    ParentId = it.parentId,
                    Indepandent = it.independent
                }).ToArray();
            }
        }

        //TODO: Extremely needs to be refactored
        public ParentCaseInfo GetParentInfo(int caseId)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var allCases = uow.GetRepository<Case>().GetAll();
                var allRelations = uow.GetRepository<ParentChildRelation>().GetAll();
                var allPerformers = uow.GetRepository<User>().GetAll();
                var relationInfo = allRelations
                    .Where(it => it.DescendantId == caseId)
                    .Select(it => new { id = it.DescendantId, parentId = it.AncestorId, independent = it.Independent })
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
								independent = t.parentChild.independent,
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
								isChildIndependent = t.tmpParentChild.independent
                            })
                        .FirstOrDefault();
                if (relationInfo != null)
                {
                    return new ParentCaseInfo()
                    {
                        ParentId = relationInfo.parentId,
                        CaseNumber = relationInfo.caseNumber,
						IsChildIndependent = relationInfo.isChildIndependent,
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

        public Case InitChildCaseFromCase(
            int copyFromCaseid,
            int userId,
            string ipAddress,
            CaseRegistrationSource source,
            string adUser,
            out ParentCaseInfo parentCaseInfo)
        {
            var c = this._caseRepository.GetDetachedCaseById(copyFromCaseid);
            if (c.IsAbout == null)
            {
                var tt = 1;
            }
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
            var c = this._caseRepository.GetDetachedCaseIncludesById(copyFromCaseid);
            c.User_Id = userId;
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
                WorkingGroup_Id = this._userRepository.GetUserDefaultWorkingGroupId(userId, customerId),
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
        
        public IList<Case> GetProblemCases(int problemId)
        {
            return this._caseRepository.GetProblemCases(problemId);
        }

        public IList<Case> GetProjectCases(int customerId, int projectId)
        {
            return this._caseRepository.GetProjectCases(customerId, projectId);
        }

        public IList<Case> GetProblemCases(int customerId, int problemId)
        {
            return this._caseRepository.GetProblemCases(customerId, problemId);
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

        public void Activate(int caseId, int userId, string adUser, string createdByApp, out IDictionary<string, string> errors)
        {
            this._caseRepository.Activate(caseId);
            var c = _caseRepository.GetDetachedCaseById(caseId);
            this._caseStatService.UpdateCaseStatistic(c);
            SaveCaseHistory(c, userId, adUser, createdByApp, out errors);
        }

        public void SendSelfServiceCaseLogEmail(int caseId, CaseMailSetting cms, int caseHistoryId, CaseLog log, string basePath, TimeZoneInfo userTimeZone, List<CaseFileDto> logFiles = null, bool caseIsActivated = false)
        {
            // get new case information
            var newCase = _caseRepository.GetDetachedCaseById(caseId);
            var mailTemplateId = 0;

            //get settings for smtp
            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);

            var performerUserEmail = string.Empty;
            var externalUpdateMail = 0;
            //get performerUser emailaddress
            if (newCase.Performer_User_Id.HasValue)
            {
                var performerUser = this._userService.GetUser(newCase.Performer_User_Id.Value);
                performerUserEmail = performerUser.Email;
                externalUpdateMail = performerUser.ExternalUpdateMail;
            }

            if (!caseIsActivated)
                mailTemplateId = (int)GlobalEnums.MailTemplates.CaseIsUpdated;
            else
                mailTemplateId = (int)GlobalEnums.MailTemplates.CaseIsActivated;

            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            if (_emailService.IsValidEmail(cms.HelpdeskMailFromAdress))
            {

                List<Field> fields = GetCaseFieldsForEmail(newCase, log, cms, string.Empty, 99, userTimeZone);

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

                if (!String.IsNullOrEmpty(performerUserEmail))
                {
                    if (log.SendMailAboutCaseToNotifier && newCase.FinishingDate == null && externalUpdateMail == 1)
                    {
                        var to = performerUserEmail.Split(';', ',').ToList();
                        var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => x.Follower).ToList();
                        to.AddRange(extraFollowers);
                        foreach (var t in to)
                        {
                            var curMail = t.Trim();
                            if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                            {
                                // Inform notifier about external lognote
                                MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                if (m != null)
                                {
                                    if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                                    {
                                        var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                        var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                        var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                        string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                        var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                        var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, log.HighPriority, files, siteSelfService, siteHelpdesk);
                                        el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                        var now = DateTime.Now;
                                        el.CreatedDate = now;
                                        el.ChangedDate = now;
                                        _emailLogRepository.Add(el);
                                        _emailLogRepository.Commit();
                                    }
                                }
                            }
                        }
                    }
                }

                // mail about lognote to Working Group User or Working Group Mail
                if ((!string.IsNullOrEmpty(log.EmailRecepientsInternalLogTo) || !string.IsNullOrEmpty(log.EmailRecepientsInternalLogCc)) && !string.IsNullOrWhiteSpace(log.EmailRecepientsExternalLog))
                {
                    MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                    if (m != null)
                    {
                        if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                        {
                            string[] to = log.EmailRecepientsExternalLog.Replace(Environment.NewLine, "|").Split('|');
                            for (int i = 0; i < to.Length; i++)
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, log.HighPriority, files, siteSelfService);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
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
        /// <param name="caseExtraFollowers"></param>
        /// <returns></returns>
        public int SaveCase(
                Case cases,
                CaseLog caseLog,
                int userId,
                string adUser,
                CaseExtraInfo caseExtraInfo,
                out IDictionary<string, string> errors,
                Case parentCase = null,
            string caseExtraFollowers = null)
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

            // save CaseIsAbout
            if (c.IsAbout != null)
                this.SaveIsAbout(c, out errors);

            // save casehistory
            var extraFields = new ExtraFieldCaseHistory();
            if (caseLog != null && caseLog.FinishingType != null)
            {
                var fc = _finishingCauseService.GetFinishingTypeName(caseLog.FinishingType.Value);
                extraFields.ClosingReason = fc;
            }

            if (parentCase != null)
            {
                this.AddChildCase(cases.Id, parentCase.Id, out errors);
            }

            extraFields.LeadTime = caseExtraInfo.LeadTimeForNow;
            extraFields.ActionLeadTime = caseExtraInfo.ActionLeadTime;
            extraFields.ActionExternalTime = caseExtraInfo.ActionExternalTime;

            ret = userId == 0 ?
                this.SaveCaseHistory(c, userId, adUser, caseExtraInfo.CreatedByApp, out errors, adUser, extraFields, caseExtraFollowers) :
                this.SaveCaseHistory(c, userId, adUser, caseExtraInfo.CreatedByApp, out errors, string.Empty, extraFields, caseExtraFollowers);

            return ret;
        }

        public int SaveCaseHistory(
            Case c,
            int userId,
            string adUser,
            string createdByApp,
            out IDictionary<string, string> errors,
            string defaultUser = "",
            ExtraFieldCaseHistory extraField = null,
            string caseExtraFollowers = null)
        {
            if (c == null)
                throw new ArgumentNullException("caseHistory");

            errors = new Dictionary<string, string>();
            var h = this.GenerateHistoryFromCase(c, userId, adUser, defaultUser, extraField, caseExtraFollowers);
            h.CreatedByApp = createdByApp;
            this._caseHistoryRepository.Add(h);

            if (errors.Count == 0)
                this._caseHistoryRepository.Commit();

            return h.Id;
        }

        public IList<CaseHistory> GetCaseHistoryByCaseId(int caseId)
        {
            return this._caseHistoryRepository.GetCaseHistoryByCaseId(caseId).ToList();
        }

        public IList<CaseHistoryOverview> GetCaseHistories(int caseId)
        {
            return this._caseHistoryRepository.GetCaseHistories(caseId).ToList();
        }

        public Dictionary<int, string> GetCaseFiles(List<int> caseIds)
        {
            var preCaseFiles = this._caseFileRepository.GetAll().Where(f => caseIds.Contains(f.Case_Id)).ToList();

            var groupedCaseFiles = preCaseFiles.GroupBy(f => f.Case_Id)
                                               .Select(g => new
                                               {
                                                   caseId = g.Key,
                                                   fileNames = g.Aggregate(string.Empty, (x, i) => x + Environment.NewLine + i.FileName)
                                               }).ToDictionary(d => d.caseId, d => d.fileNames);

            var casesWithoutFile = caseIds.Where(c => !groupedCaseFiles.ContainsKey(c)).ToDictionary(d => d, d => string.Empty);
            var ret = groupedCaseFiles.Union(casesWithoutFile).ToDictionary(d => d.Key, d => d.Value);
            return ret;
        }

        public void SendCaseEmail(
            int caseId,
            CaseMailSetting cms,
            int caseHistoryId,
            string basePath,
            TimeZoneInfo userTimeZone,
            Case oldCase = null,
            CaseLog log = null,
            List<CaseFileDto> logFiles = null,
            User currentLoggedInUser = null)
        {
            //get sender email adress
            var helpdeskMailFromAdress = string.Empty;
            var containsProductAreaMailOrNewCaseMail = false;

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
            List<Field> fields = GetCaseFieldsForEmail(newCase, log, cms, string.Empty, 0, userTimeZone);

            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            // if logfiles should be attached to the mail 
            List<string> files = null;
            if (logFiles != null && log != null)
                if (logFiles.Count > 0)
                {
                    var caseFiles = logFiles.Where(x => x.IsCaseFile).Select(x => _filesStorage.ComposeFilePath(ModuleName.Cases, x.ReferenceId, basePath, x.FileName)).ToList();
                    files = logFiles.Where(x => !x.IsCaseFile).Select(f => _filesStorage.ComposeFilePath(ModuleName.Log, f.ReferenceId, basePath, f.FileName)).ToList();
                    files.AddRange(caseFiles);
                }

            // sub state should not generate email to notifier
            if (newCase.StateSecondary != null)
                dontSendMailToNotfier = newCase.StateSecondary.NoMailToNotifier == 1 ? true : false;

            //if (!isClosingCase && !isCreatingCase) Why this????
            if (!isClosingCase && isCreatingCase)
            {
                #region Send email about new case to notifier or tblCustomer.NewCaseEmailList & (productarea template, priority template)

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
                        if (!cms.DontSendMailToNotifier && !string.IsNullOrEmpty(newCase.PersonsEmail))
                        {
                            var to = newCase.PersonsEmail.Split(';', ',').ToList();
                            var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => x.Follower).ToList();
                            to.AddRange(extraFollowers);
                            foreach (var t in to)
                            {
                                var curMail = t.Trim();
                                if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(customEmailSender1));
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 1, userTimeZone);
                                    string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                    var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                    var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                    var e_res = _emailService.SendEmail(el, customEmailSender1, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);

                                    el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                    var now = DateTime.Now;
                                    el.CreatedDate = now;
                                    el.ChangedDate = now;
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    containsProductAreaMailOrNewCaseMail = true;
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(cms.SendMailAboutNewCaseTo))
                        {
                            string[] to = cms.SendMailAboutNewCaseTo.Split(';');
                            for (int i = 0; i < to.Length; i++)
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(customEmailSender1));
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 2, userTimeZone);
                                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();

                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                var e_res = _emailService.SendEmail(el, customEmailSender1, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                            }
                        }
                    }
                }

                #region Send mail template from productArea if productarea has a mailtemplate
                if (newCase.ProductArea_Id.HasValue && newCase.ProductArea != null)
                {
                    // get mail template from productArea
                    mailTemplateId = 0;

                    if (newCase.ProductArea.MailTemplate != null)
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
                                    var to = newCase.PersonsEmail.Split(';', ',').ToList();
                                    var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => x.Follower).ToList();
                                    to.AddRange(extraFollowers);
                                    foreach (var t in to)
                                    {
                                        var curMail = t.Trim();
                                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                        {
                                            var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(customEmailSender1));
                                            fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 1, userTimeZone);

                                            string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                            var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                            var e_res = _emailService.SendEmail(el, customEmailSender1, el.EmailAddress, mm.Subject, mm.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                            el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                            var now = DateTime.Now;
                                            el.CreatedDate = now;
                                            el.ChangedDate = now;
                                            _emailLogRepository.Add(el);
                                            _emailLogRepository.Commit();
                                            containsProductAreaMailOrNewCaseMail = true;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }

                #endregion

                #region If priority has value and an emailaddress
                if (newCase.Priority != null)
                {
                    if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                    {
                        SendPriorityMail(newCase, log, cms, files, helpdeskMailFromAdress, caseHistoryId, caseId,
                            customerSetting, smtpInfo, userTimeZone);
                    }
                }
                #endregion

                #endregion
            }
            else
            {
                #region Send template email if priority has value and Internal or External log is filled

                if (newCase.Priority != null)
                {
                    if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                    {
                        SendPriorityMailSpecial(newCase, log, cms, files, helpdeskMailFromAdress, caseHistoryId, caseId, customerSetting, smtpInfo, userTimeZone);
                    }
                    else
                    {
                        if (newCase.Priority != null && log != null && (!string.IsNullOrEmpty(log.TextExternal) || !string.IsNullOrEmpty(log.TextInternal)))
                        {
                            var caseHis = _caseHistoryRepository.GetCloneOfPenultimate(caseId);
                            if (caseHis != null && caseHis.Priority_Id.HasValue)
                            {
                                var prevPriority = _priorityService.GetPriority(caseHis.Priority_Id.Value);
                                if (!string.IsNullOrWhiteSpace(prevPriority.EMailList))
                                {
                                    var copyNewCase = _caseRepository.GetDetachedCaseById(caseId);
                                    copyNewCase.Priority = prevPriority;
                                    SendPriorityMailSpecial(copyNewCase, log, cms, files, helpdeskMailFromAdress, caseHistoryId, caseId, customerSetting, smtpInfo, userTimeZone);
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            #region Send mail template from productArea if productarea has a mailtemplate
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
                    if (newCase.ProductArea.MailTemplate != null)
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
                                    var to = newCase.PersonsEmail.Split(';', ',').ToList();
                                    var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => x.Follower).ToList();
                                    to.AddRange(extraFollowers);
                                    foreach (var t in to)
                                    {
                                        var curMail = t.Trim();
                                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                        {
                                            var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(customEmailSender1));
                                            fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 1, userTimeZone);

                                            string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                            var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                            var e_res = _emailService.SendEmail(el, customEmailSender1, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                            el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                            var now = DateTime.Now;
                                            el.CreatedDate = now;
                                            el.ChangedDate = now;
                                            _emailLogRepository.Add(el);
                                            _emailLogRepository.Commit();
                                            containsProductAreaMailOrNewCaseMail = true;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

            #endregion

            #region Send email to tblCase.Performer_User_Id
            if (((!isClosingCase && isCreatingCase && newCase.Performer_User_Id.HasValue)
                || (!isCreatingCase && newCase.Performer_User_Id != oldCase.Performer_User_Id))
                && newCase.Administrator != null)
            {
                if (newCase.Administrator.AllocateCaseMail == 1 && this._emailService.IsValidEmail(newCase.Administrator.Email))
                {
                    if (currentLoggedInUser != null)
                    {
                        if (currentLoggedInUser.SettingForNoMail == 1 || (currentLoggedInUser.Id == newCase.Performer_User_Id && currentLoggedInUser.SettingForNoMail == 1)
                            || (currentLoggedInUser.Id != newCase.Performer_User_Id && currentLoggedInUser.SettingForNoMail == 0))
                        {
                            this.SendTemplateEmail(GlobalEnums.MailTemplates.AssignedCaseToUser, newCase, log, caseHistoryId, cms, newCase.Administrator.Email, userTimeZone);
                        }
                    }
                    else
                    {
                        this.SendTemplateEmail(GlobalEnums.MailTemplates.AssignedCaseToUser, newCase, log, caseHistoryId, cms, newCase.Administrator.Email, userTimeZone);
                    }

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
                            fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 4, userTimeZone);

                            string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                            var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                            var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, GetSmsSubject(customerSetting), m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                            el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                            var now = DateTime.Now;
                            el.CreatedDate = now;
                            el.ChangedDate = now;
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                        }
                    }
                }
            }

            #endregion

            #region Send email priority has changed
            if (newCase.FinishingDate == null && oldCase != null && oldCase.Id > 0)
            {
                if (newCase.Priority_Id != oldCase.Priority_Id)
                {
                    if (newCase.Priority_Id != null)
                    {
                        if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                        {
                            int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToPriority;
                            MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                            if (m != null)
                            {
                                if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                                {
                                    var to = newCase.Priority.EMailList.Split(';', ',').ToList();
                                    foreach (var t in to)
                                    {
                                        var curMail = t.Trim();
                                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                        {
                                            if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                                            {
                                                var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 5, userTimeZone);

                                                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                                var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                                var now = DateTime.Now;
                                                el.CreatedDate = now;
                                                el.ChangedDate = now;
                                                _emailLogRepository.Add(el);
                                                _emailLogRepository.Commit();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Send email working group has changed
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
                        else
                        {
                            if (newCase.Workinggroup.UserWorkingGroups != null)
                                foreach (var ur in newCase.Workinggroup.UserWorkingGroups)
                                {
                                    if (ur.User != null)
                                        if (ur.User.IsActive == 1 && ur.User.AllocateCaseMail == 1 && _emailService.IsValidEmail(ur.User.Email) && ur.UserRole == 2)
                                        {
                                            if (newCase.Department_Id != null && ur.User.Departments != null && ur.User.Departments.Count > 0)
                                            {
                                                if (ur.User.Departments.Any(x => x.Id == newCase.Department_Id.Value))
                                                {
                                                    wgEmails = wgEmails + ur.User.Email + ";";
                                                }
                                            }
                                            else
                                            {
                                                wgEmails = wgEmails + ur.User.Email + ";";
                                            }
                                        }
                                }
                        }

                        if (newCase.Workinggroup.AllocateCaseMail == 1 && !string.IsNullOrWhiteSpace(wgEmails))
                        {
                            var to = wgEmails.Split(';', ',').ToList();

                            foreach (var t in to)
                            {
                                var curMail = t.Trim();
                                if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));

                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 6, userTimeZone);

                                    string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();

                                    var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                    var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                    var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);

                                    el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                    var now = DateTime.Now;
                                    el.CreatedDate = now;
                                    el.ChangedDate = now;
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Send email when product area is set
            if (!isClosingCase && !isCreatingCase && !containsProductAreaMailOrNewCaseMail
                && oldCase.ProductAreaSetDate == null && newCase.RegistrationSource == 3
                && !cms.DontSendMailToNotifier
                && newCase.ProductArea != null
                && newCase.ProductArea.MailTemplate != null
                && newCase.ProductArea.MailTemplate.MailID > 0
                && !string.IsNullOrEmpty(newCase.PersonsEmail))
            {
                var to = newCase.PersonsEmail.Split(';', ',').ToList();
                var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => x.Follower).ToList();
                to.AddRange(extraFollowers);
                foreach (var t in to)
                {
                    var curMail = t.Trim();
                    if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                    {
                        int mailTemplateId = newCase.ProductArea.MailTemplate.MailID;
                        MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 7, userTimeZone);
                                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                            }
                        }
                    }
                }
            }

            #endregion

            #region Case closed email
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
                            var adminEmails = newCase.Customer.UsersAvailable.Where(x => x.UserGroup_Id != UserGroups.User).Select(x => x.Email).ToList();
                            for (int i = 0; i < to.Length; i++)
                            {
                                if (_emailService.IsValidEmail(to[i]))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(customEmailSender2));
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 8, userTimeZone);

                                    var identifiers = _feedbackTemplateService.FindIdentifiers(m.Body).ToList();
                                    //dont send feedback to admins
                                    var identifiersToDel = new List<string>();
                                    var templateFields = _feedbackTemplateService.GetCustomerTemplates(identifiers, newCase.Customer_Id, newCase.RegLanguage_Id, newCase.Id, cms.AbsoluterUrl);
                                    foreach (var templateField in templateFields)
                                    {
                                        if (templateField.ExcludeAdministrators && adminEmails.Any(x => x.Equals(to[i])))
                                        {
                                            identifiersToDel.Add(templateField.Key);
                                        }
                                        else
                                        {
                                            var tf = templateField.MapToFields();
                                            fields.Add(tf);
                                        }
                                    }
                                    foreach (var identifier in identifiersToDel)
                                    {
                                        if (!string.IsNullOrEmpty(identifier))
                                        {
                                            m.Body = m.Body.Replace(identifier, string.Empty);
                                        }
                                    }

                                    string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                    var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                    var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                    var e_res = _emailService.SendEmail(el, customEmailSender2, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                    el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                    var now = DateTime.Now;
                                    el.CreatedDate = now;
                                    el.ChangedDate = now;
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();

                                    foreach (var field in templateFields.Where(f => !string.IsNullOrEmpty(f.StringValue)))
                                        _feedbackTemplateService.UpdateFeedbackStatus(field);
                                }
                            }
                        }

                        if (!cms.DontSendMailToNotifier)
                        {
                            var to = newCase.PersonsEmail.Split(';', ',').Select(x => new Tuple<string, bool>(x, true)).ToList();
                            var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => new Tuple<string, bool>(x.Follower, false)).ToList();
                            to.AddRange(extraFollowers);
                            var adminEmails = newCase.Customer.UsersAvailable.Where(x => x.UserGroup_Id != UserGroups.User).Select(x => x.Email).ToList();
                            foreach (var t in to)
                            {
                                var mailBody = m.Body;
                                var curMail = t.Item1.Trim();
                                if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(customEmailSender2));
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 9, userTimeZone);
                                    var templateFields = new List<FeedbackField>();
                                    var identifiers = _feedbackTemplateService.FindIdentifiers(mailBody).ToList();
                                    //dont send feedback to followers and admins
                                    var identifiersToDel = new List<string>();
                                    if (t.Item2)
                                    {
                                        templateFields = _feedbackTemplateService.GetCustomerTemplates(identifiers, newCase.Customer_Id, newCase.RegLanguage_Id, newCase.Id, cms.AbsoluterUrl);
                                        foreach (var templateField in templateFields)
                                        {
                                            if (templateField.ExcludeAdministrators && adminEmails.Any(x => x.Equals(curMail)))
                                            {
                                                identifiersToDel.Add(templateField.Key);
                                            }
                                            else
                                            {
                                                var tf = templateField.MapToFields();
                                                fields.Add(tf);
                                            }
                                        }
                                    }
                                    foreach (var identifier in identifiersToDel)
                                    {
                                        if (!string.IsNullOrEmpty(identifier))
                                        {
                                            mailBody = m.Body.Replace(identifier, string.Empty);
                                        }
                                    }

                                    string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();

                                    var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                    var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                    var e_res = _emailService.SendEmail(el, customEmailSender2, el.EmailAddress, m.Subject, mailBody, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                    el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                    var now = DateTime.Now;
                                    el.CreatedDate = now;
                                    el.ChangedDate = now;
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();

                                    foreach (var field in templateFields.Where(f => !string.IsNullOrEmpty(f.StringValue)))
                                        _feedbackTemplateService.UpdateFeedbackStatus(field);
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
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 10, userTimeZone);
                                    var identifiers = _feedbackTemplateService.FindIdentifiers(m.Body);
                                    var templateFields = _feedbackTemplateService.GetCustomerTemplates(identifiers,
                                        newCase.Customer_Id, newCase.RegLanguage_Id, newCase.Id, cms.AbsoluterUrl);
                                    fields.AddRange(templateFields.Select(tf => tf.MapToFields()));

                                    string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();

                                    var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                    var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                    var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, GetSmsSubject(customerSetting), mt.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                    el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                    var now = DateTime.Now;
                                    el.CreatedDate = now;
                                    el.ChangedDate = now;
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();

                                    foreach (var field in templateFields.Where(f => !string.IsNullOrEmpty(f.StringValue)))
                                        _feedbackTemplateService.UpdateFeedbackStatus(field);

                                }
                            }
                        }
                    }
                }
            }

            #endregion

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
            if (!containsProductAreaMailOrNewCaseMail)
            {
                this.caseMailer.InformNotifierIfNeeded(
                                            caseHistoryId,
                                            fields,
                                            log,
                                            dontSendMailToNotfier,
                                            newCase,
                                            helpdeskMailFromAdress,
                                            files,
                                            cms.CustomeMailFromAddress, isCreatingCase, cms.DontSendMailToNotifier, cms.AbsoluterUrl);
            }

            this.caseMailer.InformAboutInternalLogIfNeeded(
                                        caseHistoryId,
                                        fields,
                                        log,
                                        newCase,
                                        helpdeskMailFromAdress,
                                        files, cms.AbsoluterUrl, cms.CustomeMailFromAddress);
        }

        public List<BusinessRuleActionModel> CheckBusinessRules(BREventType occurredEvent, Case currentCase, Case oldCase = null)
        {
            var ret = new List<BusinessRuleActionModel>();

            if (currentCase.Id == 0)
                return ret;

            var customerId = currentCase.Customer_Id;
            var rules = _businessRuleService.GetRules(customerId, occurredEvent);
            if (rules.Any())
            {
                ret = GetAllNeededAction(rules, currentCase, oldCase);
            }

            return ret;
        }

        public void ExecuteBusinessActions(List<BusinessRuleActionModel> actions, Case currentCase, CaseLog log, TimeZoneInfo userTimeZone,
                                           int caseHistoryId, string basePath, int currentLanguageId, CaseMailSetting caseMailSetting,
                                           List<CaseFileDto> logFiles = null)
        {
            foreach (var action in actions)
            {
                switch (action.ActionType)
                {
                    case BRActionType.SendEmail:
                        DoAction_SendEmail(action, currentCase, log, userTimeZone, caseHistoryId, basePath, currentLanguageId, caseMailSetting, logFiles);
                        break;
                }
            }
        }

        #region Private methods

        private void DeleteChildCasesFor(int caseId)
        {
            using (var uow = unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                uow.GetRepository<ParentChildRelation>().DeleteWhere(it => it.AncestorId == caseId);
                uow.Save();
            }
        }

        private void DeleteExtendedCase(int caseId)
        {
            using (var uow = unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                uow.GetRepository<Case_ExtendedCaseEntity>().DeleteWhere(it => it.Case_Id == caseId);
                uow.Save();
            }
        }

        private void DeleteCaseIsAboutFor(int caseId)
        {
            using (var uow = unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                uow.GetRepository<CaseIsAboutEntity>().DeleteWhere(isa => isa.Case.Id == caseId);
                uow.Save();
            }
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
            c.LeadTime = 0;
            c.ExternalTime = 0;

            return c;
        }

        private void SaveIsAbout(Case c, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var isAboutEntity = uow.GetRepository<CaseIsAboutEntity>();
                var allreadyExists = isAboutEntity
                        .GetAll()
                        .Where(it => it.Id == c.Id)
                        .FirstOrDefault();
                if (allreadyExists == null)
                {
                    if (CaseHasIsAbout(c))
                    {
                        isAboutEntity.Add(new CaseIsAboutEntity()
                        {
                            Id = c.Id,
                            CostCentre = c.IsAbout.CostCentre,
                            Department_Id = c.IsAbout.Department_Id,
                            OU_Id = c.IsAbout.OU_Id,
                            Person_Cellphone = c.IsAbout.Person_Cellphone,
                            Person_Email = c.IsAbout.Person_Email,
                            Person_Name = c.IsAbout.Person_Name,
                            Person_Phone = c.IsAbout.Person_Phone,
                            Place = c.IsAbout.Place,
                            Region_Id = c.IsAbout.Region_Id,
                            ReportedBy = c.IsAbout.ReportedBy,
                            UserCode = c.IsAbout.UserCode
                        });
                        uow.Save();
                    }
                }
                else
                {
                    if (CaseHasIsAbout(c))
                    {
                        allreadyExists.CostCentre = c.IsAbout.CostCentre;
                        allreadyExists.Department_Id = c.IsAbout.Department_Id;
                        allreadyExists.OU_Id = c.IsAbout.OU_Id;
                        allreadyExists.Person_Cellphone = c.IsAbout.Person_Cellphone;
                        allreadyExists.Person_Email = c.IsAbout.Person_Email;
                        allreadyExists.Person_Name = c.IsAbout.Person_Name;
                        allreadyExists.Person_Phone = c.IsAbout.Person_Phone;
                        allreadyExists.Place = c.IsAbout.Place;
                        allreadyExists.Region_Id = c.IsAbout.Region_Id;
                        allreadyExists.ReportedBy = c.IsAbout.ReportedBy;
                        allreadyExists.UserCode = c.IsAbout.UserCode;
                        isAboutEntity.Update(allreadyExists);
                    }
                    else
                        isAboutEntity.DeleteById(c.Id);

                    uow.Save();
                }
            }
        }

        private bool CaseHasIsAbout(Case c)
        {
            if (c.IsAbout == null ||
                (c.IsAbout != null &&
                 string.IsNullOrEmpty(c.IsAbout.CostCentre) &&
                 string.IsNullOrEmpty(c.IsAbout.Person_Cellphone) &&
                 string.IsNullOrEmpty(c.IsAbout.Person_Email) &&
                 string.IsNullOrEmpty(c.IsAbout.Person_Name) &&
                 string.IsNullOrEmpty(c.IsAbout.Person_Phone) &&
                 string.IsNullOrEmpty(c.IsAbout.Place) &&
                 string.IsNullOrEmpty(c.IsAbout.ReportedBy) &&
                 string.IsNullOrEmpty(c.IsAbout.UserCode) &&
                 c.IsAbout.Region_Id == null &&
                 c.IsAbout.Department_Id == null &&
                 c.IsAbout.OU_Id == null))
                return false;
            else
                return true;
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

        private void DoAction_SendEmail(BusinessRuleActionModel action, Case currentCase, CaseLog log, TimeZoneInfo userTimeZone,
                                        int caseHistoryId, string basePath, int currentLanguageId, CaseMailSetting caseMailSetting,
                                        List<CaseFileDto> logFiles = null)
        {
            var customerId = currentCase.Customer_Id;
            var mailTemplate = new MailTemplateLanguageEntity();
            var sep = new char[] { ';' };

            var emailList = new List<string>();
            foreach (var param in action.ActionParams)
            {
                var dataList = !string.IsNullOrEmpty(param.ParamValue) ? param.ParamValue.Split(BRConstItem.Value_Separator, StringSplitOptions.RemoveEmptyEntries) : null;
                switch (param.ParamType)
                {
                    case BRActionParamType.EMailTemplate:
                        if (!string.IsNullOrEmpty(param.ParamValue))
                        {
                            int templateId = 0;
                            int.TryParse(param.ParamValue, out templateId);
                            mailTemplate = _mailTemplateService.GetMailTemplateLanguageForCustomer(templateId, customerId, currentCase.RegLanguage_Id);
                        }
                        break;

                    case BRActionParamType.EmailGroup:
                        if (dataList != null && dataList.Any())
                        {
                            var groups = _emailGroupService.GetEmailGroups(customerId).Where(e => dataList.Contains(e.Id.ToString())).ToList();
                            if (groups.Any())
                            {
                                var lineSep = new string[] { Environment.NewLine };
                                foreach (var group in groups)
                                {
                                    var groupEmails = group.Members.Split(lineSep, StringSplitOptions.RemoveEmptyEntries);
                                    emailList.AddRange(groupEmails);
                                }
                            }
                        }
                        break;

                    case BRActionParamType.WorkingGroup:
                        if (dataList != null && dataList.Any())
                        {
                            var wgs = _workingGroupService.GetAllWorkingGroupsForCustomer(customerId).Where(e => dataList.Contains(e.Id.ToString())).ToList();
                            if (wgs.Any())
                            {
                                foreach (var wg in wgs)
                                {
                                    if (wg.IsActive != 0 && wg.UserWorkingGroups != null && wg.UserWorkingGroups.Any())
                                    {
                                        var usEmails = wg.UserWorkingGroups.Where(w => w.User != null && w.User.IsActive != 0)
                                                                     .Select(w => w.User.Email)
                                                                     .ToList();
                                        emailList.AddRange(usEmails);
                                    }
                                }
                            }
                        }
                        break;

                    case BRActionParamType.Administrator:
                        if (dataList != null && dataList.Any())
                        {
                            var admins = _userService.GetAvailablePerformersOrUserId(customerId).Where(e => dataList.Contains(e.Id.ToString())).ToList();
                            if (admins.Any())
                            {
                                foreach (var admin in admins)
                                {
                                    if (admin.IsActive != 0)
                                    {
                                        emailList.Add(admin.Email);
                                    }
                                }
                            }
                        }
                        break;

                    case BRActionParamType.Recipients:
                        if (!string.IsNullOrEmpty(param.ParamValue))
                        {
                            var emails = param.ParamValue.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            emailList.AddRange(emails);
                        }
                        break;

                    case BRActionParamType.CaseCreator:
                        if (!string.IsNullOrEmpty(param.ParamValue))
                        {
                            var emails = param.ParamValue.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            emailList.AddRange(emails);
                        }
                        break;

                    case BRActionParamType.Initiator:
                        if (!string.IsNullOrEmpty(param.ParamValue))
                        {
                            var emails = param.ParamValue.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            emailList.AddRange(emails);
                        }
                        break;

                    case BRActionParamType.CaseIsAbout:
                        if (!string.IsNullOrEmpty(param.ParamValue))
                        {
                            var emails = param.ParamValue.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            emailList.AddRange(emails);
                        }
                        break;

                }
            }

            if (mailTemplate != null && !string.IsNullOrEmpty(mailTemplate.Body) &&
                !string.IsNullOrEmpty(mailTemplate.Subject) && emailList.Any())
            {
                var distinctedEmails = emailList.Select(x => x.Trim().ToLower()).Distinct().ToList();
                SendEmail(distinctedEmails, mailTemplate, currentCase, log, userTimeZone, caseHistoryId, basePath,
                          currentLanguageId, caseMailSetting, logFiles);
            }

        }


        private void SendEmail(List<string> receivers, MailTemplateLanguageEntity mailTemplate, Case currentCase,
                               CaseLog log, TimeZoneInfo userTimeZone, int caseHistoryId, string basePath,
                               int currentLanguageId, CaseMailSetting caseMailSetting,
                               List<CaseFileDto> logFiles = null)
        {

            if (mailTemplate.MailTemplate == null)
                return;

            if (!string.IsNullOrEmpty(caseMailSetting.HelpdeskMailFromAdress))
                caseMailSetting.HelpdeskMailFromAdress = caseMailSetting.HelpdeskMailFromAdress.Trim();
            else
                return;

            var customerSetting = _settingService.GetCustomerSetting(currentCase.Customer_Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            List<string> files = null;
            if (logFiles != null && log != null)
                if (logFiles.Count > 0)
                {
                    var caseFiles = logFiles.Where(x => x.IsCaseFile).Select(x => _filesStorage.ComposeFilePath(ModuleName.Cases, x.ReferenceId, basePath, x.FileName)).ToList();
                    files = logFiles.Where(x => !x.IsCaseFile).Select(f => _filesStorage.ComposeFilePath(ModuleName.Log, f.ReferenceId, basePath, f.FileName)).ToList();
                    files.AddRange(caseFiles);
                }

            foreach (var receiver in receivers)
            {
                var curMail = receiver.Trim();
                if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                {
                    var emailLog = new EmailLog(caseHistoryId, mailTemplate.MailTemplate.MailID, curMail, _emailService.GetMailMessageId(caseMailSetting.HelpdeskMailFromAdress));
                    var fields = GetCaseFieldsForEmail(currentCase, log, caseMailSetting, emailLog.EmailLogGUID.ToString(), 1, userTimeZone);
                    var siteSelfService = ConfigurationManager.AppSettings[AppSettingsKey.SelfServiceAddress].ToString() + emailLog.EmailLogGUID.ToString();
                    var siteHelpdesk = caseMailSetting.AbsoluterUrl + "Cases/edit/" + currentCase.Id.ToString();
                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                    var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                    var sendResult = _emailService.SendEmail(emailLog, caseMailSetting.HelpdeskMailFromAdress, emailLog.EmailAddress,
                                                             mailTemplate.Subject, mailTemplate.Body, fields,
                                                             mailSetting,
                                                             emailLog.MessageId, false, files, siteSelfService, siteHelpdesk);
                    emailLog.SetResponse(sendResult.SendTime, sendResult.ResponseMessage);
                    var now = DateTime.Now;
                    emailLog.CreatedDate = now;
                    emailLog.ChangedDate = now;
                    _emailLogRepository.Add(emailLog);
                }
            }
            _emailLogRepository.Commit();
        }


        private void SendTemplateEmail(
            GlobalEnums.MailTemplates mailTemplateEnum,
            Case case_,
            CaseLog log,
            int caseHistoryId,
            CaseMailSetting cms,
            string recipient,
            TimeZoneInfo userTimeZone)
        {

            if (!string.IsNullOrEmpty((cms.HelpdeskMailFromAdress)))
            {
                cms.HelpdeskMailFromAdress = cms.HelpdeskMailFromAdress.Trim();
            }

            var customerSetting = _settingService.GetCustomerSetting(case_.Customer_Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            var mailTemplateId = (int)mailTemplateEnum;
            var m = this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(case_.Customer_Id, case_.RegLanguage_Id, mailTemplateId);
            if (m != null && !string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
            {
                var el = new EmailLog(
                    caseHistoryId,
                    mailTemplateId,
                    recipient,
                    this._emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));

                var fields = this.GetCaseFieldsForEmail(case_, log, cms, el.EmailLogGUID.ToString(), 3, userTimeZone);

                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                //string urlSelfService;
                //if (m.Body.Contains("[/#98]"))
                //{
                //    string str1 = "[#98]";
                //    string str2 = "[/#98]";
                //    string LinkText;

                //    int Pos1 = m.Body.IndexOf(str1) + str1.Length;
                //    int Pos2 = m.Body.IndexOf(str2);
                //    LinkText = m.Body.Substring(Pos1, Pos2 - Pos1);

                //    urlSelfService = "<a href='" + siteSelfService + "'>" + LinkText + "</a>";

                //}
                //else
                //{
                //    urlSelfService = "<a href='" + siteSelfService + "'>" + siteSelfService + "</a>";
                //}

                //foreach (var field in fields)
                //    if (field.Key == "[#98]")
                //        field.StringValue = urlSelfService;

                //var urlHelpdesk = "";
                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + case_.Id.ToString();

                //if (m.Body.Contains("[/#99]"))
                //{
                //    string str1 = "[#99]";
                //    string str2 = "[/#99]";
                //    string LinkText;

                //    int Pos1 = m.Body.IndexOf(str1) + str1.Length;
                //    int Pos2 = m.Body.IndexOf(str2);
                //    LinkText = m.Body.Substring(Pos1, Pos2 - Pos1);

                //    urlHelpdesk = "<a href='" + siteHelpdesk + "'>" + LinkText + "</a>";

                //}
                //else
                //{
                //    urlHelpdesk = "<a href='" + siteHelpdesk + "'>" + siteHelpdesk + "</a>";
                //}

                //foreach (var field in fields)
                //    if (field.Key == "[#99]")
                //        field.StringValue = urlHelpdesk;
                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                var e_res = this._emailService.SendEmail(el, cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, null, siteSelfService, siteHelpdesk);
                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                var now = DateTime.Now;
                el.CreatedDate = now;
                el.ChangedDate = now;
                _emailLogRepository.Add(el);
                _emailLogRepository.Commit();
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

            if (caseLog != null && caseLog.TextExternal != null && caseLog.TextExternal.Length > 3000)
                caseLog.TextExternal = caseLog.TextExternal.Substring(0, 3000);

            if (caseLog != null && caseLog.TextInternal != null && caseLog.TextInternal.Length > 3000)
                caseLog.TextInternal = caseLog.TextInternal.Substring(0, 3000);

            return ret;
        }

        private CaseHistory GenerateHistoryFromCase(
            Case c,
            int userId,
            string adUser,
            string defaultUser = "",
            ExtraFieldCaseHistory extraField = null,
            string caseExtraFollowers = null)
        {
            var h = new CaseHistory();
            var user = this._userRepository.GetUser(userId);
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

            if (c.CaseFollowers != null && c.CaseFollowers.Any() && string.IsNullOrEmpty(caseExtraFollowers))
            {
                caseExtraFollowers = string.Join(BRConstItem.Email_Separator, c.CaseFollowers.Select(x => x.Follower)) + BRConstItem.Email_Separator;
            }

            h.Currency = c.Currency;
            h.CaseExtraFollowers = string.IsNullOrEmpty(caseExtraFollowers) ? string.Empty : caseExtraFollowers;
            h.Customer_Id = c.Customer_Id;
            h.Deleted = c.Deleted;
            h.Department_Id = c.Department_Id;
            h.Description = c.Description;
            h.ExternalTime = c.ExternalTime;
            h.LeadTime = c.LeadTime;
            h.ActionLeadTime = 0;
            h.ActionExternalTime = 0;
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
            h.LatestSLACountDate = c.LatestSLACountDate;

            if (c.IsAbout != null)
            {
                h.IsAbout_Persons_Name = c.IsAbout.Person_Name;
                h.IsAbout_UserCode = c.IsAbout.UserCode;
                h.IsAbout_ReportedBy = c.IsAbout.ReportedBy;
                h.IsAbout_Persons_Phone = c.IsAbout.Person_Phone;
                h.IsAbout_Department_Id = c.IsAbout.Department_Id;
            }

            if (extraField != null)
            {
                h.CaseFile = extraField.CaseFile;
                h.LogFile = extraField.LogFile;
                h.CaseLog = extraField.CaseLog;
                h.ClosingReason = extraField.ClosingReason;
                h.LeadTime = extraField.LeadTime;
                h.ActionLeadTime = extraField.ActionLeadTime;
                h.ActionExternalTime = extraField.ActionExternalTime;
            }

            return h;
        }

        private List<Field> GetCaseFieldsForEmail(Case c, CaseLog l, CaseMailSetting cms, string emailLogGuid, int stateHelper, TimeZoneInfo userTimeZone)
        {
            List<Field> ret = new List<Field>();

            var userLocal_RegTime = TimeZoneInfo.ConvertTimeFromUtc(c.RegTime, userTimeZone);

            ret.Add(new Field { Key = "[#1]", StringValue = c.CaseNumber.ToString() });
            ret.Add(new Field { Key = "[#16]", StringValue = userLocal_RegTime.ToString() });
            ret.Add(new Field { Key = "[#22]", StringValue = c.LastChangedByUser != null ? c.LastChangedByUser.FirstName + " " + c.LastChangedByUser.SurName : string.Empty });
            ret.Add(new Field { Key = "[#3]", StringValue = c.PersonsName });
            ret.Add(new Field { Key = "[#8]", StringValue = c.PersonsEmail });
            ret.Add(new Field { Key = "[#9]", StringValue = c.PersonsPhone });
            ret.Add(new Field { Key = "[#18]", StringValue = c.PersonsCellphone });
            ret.Add(new Field { Key = "[#2]", StringValue = c.Customer != null ? c.Customer.Name : string.Empty });
            ret.Add(new Field { Key = "[#24]", StringValue = c.Place });
            ret.Add(new Field { Key = "[#17]", StringValue = c.InventoryNumber });
            ret.Add(new Field { Key = "[#25]", StringValue = c.CaseType != null ? c.CaseType.Name : string.Empty });
            ret.Add(new Field { Key = "[#26]", StringValue = c.Category != null ? c.Category.Name : string.Empty });
            ret.Add(new Field { Key = "[#4]", StringValue = c.Caption });
            ret.Add(new Field { Key = "[#5]", StringValue = c.Description });
            ret.Add(new Field { Key = "[#23]", StringValue = c.Miscellaneous });
            ret.Add(new Field { Key = "[#19]", StringValue = c.Available });
            ret.Add(new Field { Key = "[#15]", StringValue = c.Workinggroup != null ? c.Workinggroup.WorkingGroupName : string.Empty });
            ret.Add(new Field { Key = "[#13]", StringValue = c.Workinggroup != null ? c.Workinggroup.EMail : string.Empty });
            ret.Add(new Field { Key = "[#6]", StringValue = c.Administrator != null ? c.Administrator.FirstName : string.Empty });
            ret.Add(new Field { Key = "[#7]", StringValue = c.Administrator != null ? c.Administrator.SurName : string.Empty });
            ret.Add(new Field { Key = "[#12]", StringValue = c.Priority != null ? c.Priority.Name : string.Empty });
            ret.Add(new Field { Key = "[#20]", StringValue = c.Priority != null ? c.Priority.Description : string.Empty });
            ret.Add(new Field { Key = "[#21]", StringValue = c.WatchDate.ToString() });
            if (c.ProductArea?.Parent_ProductArea_Id != null)
            {
                var names = _productAreaService.GetParentPath(c.ProductArea.Id, c.Customer_Id).ToList();
                ret.Add(new Field { Key = "[#28]", StringValue = string.Join(" - ", names) });
            }
            else
            {
                ret.Add(new Field { Key = "[#28]", StringValue = c.ProductArea != null ? c.ProductArea.Name : string.Empty });
            }
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
                string url = "<br><a href='" + site + "'>" + site + "</a>";
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

        private List<BusinessRuleActionModel> GetAllNeededAction(IList<BusinessRuleModel> rules, Case currentCase, Case oldCase = null)
        {
            var ret = new List<BusinessRuleActionModel>();
            foreach (var rule in rules)
            {
                if (rule.RuleActive)
                    ret.AddRange(GetNeededActionsToRun(rule, currentCase, oldCase));
            }

            return ret;
        }

        private List<BusinessRuleActionModel> GetNeededActionsToRun(BusinessRuleModel rule, Case currentCase, Case oldCase = null)
        {
            var ret = new List<BusinessRuleActionModel>();

            #region Check Process field

            var oldProcessId = oldCase != null ? (oldCase.ProductArea_Id.HasValue ? oldCase.ProductArea_Id.Value : BRConstItem.NULL) : BRConstItem.NULL;
            var newProcessId = currentCase.ProductArea_Id.HasValue ? currentCase.ProductArea_Id.Value : BRConstItem.NULL;

            var processCondition = false;
            if (!rule.ProcessFrom.Any() && !rule.ProcessTo.Any())
                processCondition = true;
            else
            {
                if (rule.ProcessFrom.Contains(BRConstItem.ANY) || rule.ProcessFrom.Contains(oldProcessId))
                    if (rule.ProcessTo.Contains(BRConstItem.ANY) || rule.ProcessTo.Contains(newProcessId))
                    {
                        if (rule.ProcessFrom.Contains(BRConstItem.ANY) && rule.ProcessTo.Contains(BRConstItem.ANY))
                        {
                            if (oldProcessId != newProcessId)
                                processCondition = true;
                        }
                        else
                            processCondition = true;
                    }
            }

            #endregion

            #region Check SubStatus field

            var oldSubStatusId = oldCase != null ? (oldCase.StateSecondary_Id.HasValue ? oldCase.StateSecondary_Id.Value : BRConstItem.NULL) : BRConstItem.NULL;
            var newSubStatusId = currentCase.StateSecondary_Id.HasValue ? currentCase.StateSecondary_Id.Value : BRConstItem.NULL;

            var subStatusCondition = false;

            if (!rule.SubStatusFrom.Any() && !rule.SubStatusTo.Any())
                subStatusCondition = true;
            else
            {
                if (rule.SubStatusFrom.Contains(BRConstItem.ANY) || rule.SubStatusFrom.Contains(oldSubStatusId))
                    if (rule.SubStatusTo.Contains(BRConstItem.ANY) || rule.SubStatusTo.Contains(newSubStatusId))
                    {
                        if (rule.SubStatusFrom.Contains(BRConstItem.ANY) && rule.SubStatusTo.Contains(BRConstItem.ANY))
                        {
                            if (oldSubStatusId != newSubStatusId)
                                subStatusCondition = true;
                        }
                        else
                            subStatusCondition = true;
                    }
            }
            #endregion

            if (processCondition && subStatusCondition)
            {
                // TODO: we only have send mail action, in the future it must accept dynamic actions 
                var newAction = new BusinessRuleActionModel(rule.Id, BRActionType.SendEmail);

                var param = new BusinessRuleActionParamModel(BRActionParamType.EMailTemplate, rule.EmailTemplate.ToString());
                newAction.AddActionParam(param);

                if (rule.WorkingGroups.Contains(BRConstItem.CURRENT_VALUE))
                {
                    rule.WorkingGroups.Remove(BRConstItem.CURRENT_VALUE);
                    if (currentCase.WorkingGroup_Id.HasValue && !rule.WorkingGroups.Contains(currentCase.WorkingGroup_Id.Value))
                        rule.WorkingGroups.Add(currentCase.WorkingGroup_Id.Value);
                }
                param = new BusinessRuleActionParamModel(BRActionParamType.WorkingGroup, rule.WorkingGroups.GetSelectedStr());
                newAction.AddActionParam(param);


                param = new BusinessRuleActionParamModel(BRActionParamType.EmailGroup, rule.EmailGroups.GetSelectedStr());
                newAction.AddActionParam(param);

                if (rule.Administrators.Contains(BRConstItem.CURRENT_VALUE))
                {
                    rule.Administrators.Remove(BRConstItem.CURRENT_VALUE);
                    if (currentCase.Performer_User_Id.HasValue && !rule.Administrators.Contains(currentCase.Performer_User_Id.Value))
                        rule.Administrators.Add(currentCase.Performer_User_Id.Value);
                }
                param = new BusinessRuleActionParamModel(BRActionParamType.Administrator, rule.Administrators.GetSelectedStr());
                newAction.AddActionParam(param);

                param = new BusinessRuleActionParamModel(BRActionParamType.Recipients, rule.Recipients != null && rule.Recipients.Any() ?
                                                         string.Join(BRConstItem.Email_Separator, rule.Recipients) : string.Empty);
                newAction.AddActionParam(param);


                if (rule.CaseCreator && currentCase.User_Id != null)
                {
                    var creatorUser = _userService.GetUser(currentCase.User_Id.Value);
                    if (creatorUser != null && creatorUser.IsActive != 0 && !string.IsNullOrEmpty(creatorUser.Email))
                    {
                        param = new BusinessRuleActionParamModel(BRActionParamType.CaseCreator, creatorUser.Email);
                        newAction.AddActionParam(param);
                    }
                }

                if (rule.Initiator && !string.IsNullOrEmpty(currentCase.PersonsEmail))
                {
                    param = new BusinessRuleActionParamModel(BRActionParamType.Initiator, currentCase.PersonsEmail);
                    newAction.AddActionParam(param);
                }

                if (rule.CaseIsAbout && currentCase.IsAbout != null && !string.IsNullOrEmpty(currentCase.IsAbout.Person_Email))
                {
                    param = new BusinessRuleActionParamModel(BRActionParamType.CaseIsAbout, currentCase.IsAbout.Person_Email);
                    newAction.AddActionParam(param);
                }

                ret.Add(newAction);
            }
            return ret;
        }

        private void SendPriorityMail(Case newCase, CaseLog log, CaseMailSetting cms, List<string> files, string helpdeskMailFromAdress, int caseHistoryId, int caseId, Setting customerSetting, MailSMTPSetting smtpInfo, TimeZoneInfo userTimeZone)
        {
            var mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToPriority;
            var mt = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
            if (mt != null)
            {
                if (!string.IsNullOrEmpty(mt.Body) && !string.IsNullOrEmpty(mt.Subject))
                {
                    var to = newCase.Priority.EMailList.Split(';', ',').ToList();
                    foreach (var t in to)
                    {
                        var curMail = t.Trim();
                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                        {
                            var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                            var fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 5, userTimeZone);

                            var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                            var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                            var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, mt.Subject, mt.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                            el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                            var now = DateTime.Now;
                            el.CreatedDate = now;
                            el.ChangedDate = now;
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                        }
                    }
                }
            }
        }

        private void SendPriorityMailSpecial(Case newCase, CaseLog log, CaseMailSetting cms, List<string> files, string helpdeskMailFromAdress, int caseHistoryId, int caseId, Setting customerSetting, MailSMTPSetting smtpInfo, TimeZoneInfo userTimeZone)
        {
            var mailTemplate = new CustomMailTemplate { };

            if (newCase.Priority.MailID_Change.HasValue)
                mailTemplate = this._mailTemplateService.GetCustomMailTemplate(newCase.Priority.MailID_Change.Value);
            {

                //var mailTemplateId = mailTemplate.MailID;

                var mt = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplate.MailId);
                if (mt != null)
                {
                    if (!string.IsNullOrEmpty(mt.Body) && !string.IsNullOrEmpty(mt.Subject))
                    {
                        var to = newCase.Priority.EMailList.Split(';', ',').ToList();
                        foreach (var t in to)
                        {
                            var curMail = t.Trim();
                            if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplate.MailId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                var fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 5, userTimeZone);

                                var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, mt.Subject, mt.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                            }
                        }
                    }
                }
            }
        }

		public void SetIndependentChild(int caseID, bool independentChild)
		{
			using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
			{
				var parentCaseRelation = uow.GetRepository<ParentChildRelation>()
					.GetAll()
					.Where(o => o.DescendantId == caseID)
					.SingleOrDefault();

				if (parentCaseRelation == null)
				{
					throw new ArgumentException($"No parent for case id {caseID}");
				}

				parentCaseRelation.Independent = independentChild;
				uow.Save();
			}
		}
		#endregion
	}
}

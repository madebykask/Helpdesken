using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Dal.Infrastructure.Extensions;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Services.Services.Cache;

namespace DH.Helpdesk.Services.Services
{

    using DH.Helpdesk.BusinessData.Models.BusinessRules;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.ChidCase;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.User.Input;
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
    using DH.Helpdesk.Domain.Problems;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Cases;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Customers;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Customers;
    using DH.Helpdesk.Services.Infrastructure.Email;
    using DH.Helpdesk.Services.Services.CaseStatistic;
    using DH.Helpdesk.Services.utils;
    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;
    using DH.Helpdesk.Services.Infrastructure;
    using Feedback;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;
    using System.Linq.Expressions;
    using Common.Enums.Cases;
    using Common.Extensions.String;
    using Utils;
    using DH.Helpdesk.BusinessData.Models.User;
    using DH.Helpdesk.BusinessData.Models.Case.MergedCase;
    using DH.Helpdesk.Dal.Repositories.Cases.Concrete;

    public partial class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseFileRepository _caseFileRepository;
        private readonly ICaseHistoryRepository _caseHistoryRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618
        private readonly IPriorityService _priorityService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IUserRepository _userRepository;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly IEmailLogAttemptRepository _emailLogAttemptRepository;
        private readonly IEmailService _emailService;
        private readonly ISettingService _settingService;
        private readonly IFilesStorage _filesStorage;
        private readonly ILogRepository _logRepository;
        private readonly ILogFileRepository _logFileRepository;
        private readonly IFormFieldValueRepository _formFieldValueRepository;
        private readonly IFeedbackTemplateService _feedbackTemplateService;
        private readonly ICaseSectionsRepository _caseSectionsRepository;
        private readonly ICaseMailer _caseMailer;
        private readonly IInvoiceArticleService _invoiceArticleService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ISurveyService _surveyService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ICaseLockService _caseLockService;
        private readonly ICaseStatisticService _caseStatService;
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
        private readonly ICaseFollowUpService _caseFollowUpService;
        private readonly ICaseSolutionRepository _caseSolutionRepository;
        private readonly IStateSecondaryRepository _stateSecondaryRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerService _customerService;
        private readonly IDepartmentService _departmentService;
        private readonly IEntityToBusinessModelMapper<CaseHistoryMapperData, CaseHistoryOverview> _caseHistoryOverviewMapper;
        private readonly ITranslateCacheService _translateCacheService;
        private readonly ICaseTypeRepository _caseTypeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite> _caseFilterFavoriteToBusinessModelMapper;
        private readonly ICustomerUserRepository _customerUserRepository;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IContractLogRepository _contractLogRepository;
        private readonly ICircularService _circularService;

#pragma warning disable 0618
        public CaseService(
            ICaseRepository caseRepository,
            ICaseFileRepository caseFileRepository,
            ICaseHistoryRepository caseHistoryRepository,
            ILogRepository logRepository,
            ILogFileRepository logFileRepository,
            IPriorityService priorityService,
            IWorkingGroupService workingGroupService,
            IMailTemplateService mailTemplateService,
            IEmailLogRepository emailLogRepository,
            IEmailLogAttemptRepository emailLogAttemptRepository,
            IEmailService emailService,
            ISettingService settingService,
            IFilesStorage filesStorage,
            IUnitOfWork unitOfWork,
            IFormFieldValueRepository formFieldValueRepository,
            IUserRepository userRepository,
            ICaseMailer caseMailer,
            IInvoiceArticleService invoiceArticleService,
            IUnitOfWorkFactory unitOfWorkFactory,
            ISurveyService surveyService,
            IFinishingCauseService finishingCauseService,
            ICaseLockService caseLockService,
            ICaseStatisticService caseStatService,
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
            IExtendedCaseDataRepository extendedCaseDataRepository,
            ICaseFollowUpService caseFollowUpService,
            ICaseSectionsRepository caseSectionsRepository,
            ICaseSolutionRepository caseSolutionRepository,
            IStateSecondaryRepository stateSecondaryRepository,
            ICustomerRepository customerRepository,
            ICustomerService customerService,
            IDepartmentService departmentService,
            IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite> caseFilterFavoriteToBusinessModelMapper,
            IEntityToBusinessModelMapper<CaseHistoryMapperData, CaseHistoryOverview> caseHistoryOverviewMapper,
            ITranslateCacheService translateCacheService,
            ICaseTypeRepository caseTypeRepository,
            ICategoryRepository categoryRepository,
            ICustomerUserRepository customerUserRepository,
            IGlobalSettingService globalSettingService,
            IContractLogRepository contractLogRepository,
            ICircularService circularService)
        {
            _customerUserRepository = customerUserRepository;
            _caseFilterFavoriteToBusinessModelMapper = caseFilterFavoriteToBusinessModelMapper;
            _unitOfWork = unitOfWork;
            _caseRepository = caseRepository;
            _caseRepository = caseRepository;
            _priorityService = priorityService;
            _workingGroupService = workingGroupService;
            _userRepository = userRepository;
            _caseMailer = caseMailer;
            _invoiceArticleService = invoiceArticleService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _caseHistoryRepository = caseHistoryRepository;
            _mailTemplateService = mailTemplateService;
            _emailLogRepository = emailLogRepository;
            _emailLogAttemptRepository = emailLogAttemptRepository;
            _emailService = emailService;
            _settingService = settingService;
            _caseFileRepository = caseFileRepository;
            _filesStorage = filesStorage;
            _logRepository = logRepository;
            _logFileRepository = logFileRepository;
            _formFieldValueRepository = formFieldValueRepository;
            _surveyService = surveyService;
            _finishingCauseService = finishingCauseService;
            _caseLockService = caseLockService;
            _caseStatService = caseStatService;
            _caseFilterFavoriteRepository = caseFilterFavoriteRepository;
            _mail2TicketRepository = mail2TicketRepository;
            _businessRuleService = businessRuleService;
            _emailGroupService = emailGroupService;
            _userService = userService;
            _emailSendingSettingsProvider = emailSendingSettingsProvider;
            _caseExtraFollowersService = caseExtraFollowersService;
            _feedbackTemplateService = feedbackTemplateService;
            _productAreaService = productAreaService;
            _extendedCaseFormRepository = extendedCaseFormRepository;
            _extendedCaseDataRepository = extendedCaseDataRepository;
            _caseFollowUpService = caseFollowUpService;
            _caseSectionsRepository = caseSectionsRepository;
            _caseSolutionRepository = caseSolutionRepository;
            _stateSecondaryRepository = stateSecondaryRepository;
            _customerRepository = customerRepository;
            _customerService = customerService;
            _departmentService = departmentService;

            _caseHistoryOverviewMapper = caseHistoryOverviewMapper;
            _translateCacheService = translateCacheService;
            _caseTypeRepository = caseTypeRepository;
            _categoryRepository = categoryRepository;
            _globalSettingService = globalSettingService;
            _contractLogRepository = contractLogRepository;
            _circularService = circularService;
        }

#pragma warning restore 0618

        public Case GetCaseById(int id, bool markCaseAsRead = false)
        {
            return _caseRepository.GetCaseById(id, markCaseAsRead);
        }

        public int GetCaseCustomerId(int caseId)
        {
            var customerId = _caseRepository.GetCaseCustomerId(caseId);
            return customerId;
        }

        public Customer GetCaseCustomer(int caseId)
        {
            var customerId = _caseRepository.GetCaseCustomerId(caseId);
            return _customerService.GetCustomer(customerId);
        }

        public Case GetDetachedCaseById(int id)
        {
            return _caseRepository.GetDetachedCaseById(id);
        }

        public Case GetCaseByGUID(Guid GUID)
        {
            return _caseRepository.GetCaseByGUID(GUID);
        }

        public int GetCaseIdByEmailGUID(Guid GUID)
        {
            return _caseRepository.GetCaseIdByEmailGUID(GUID);
        }

        public Case GetCaseByEMailGUID(Guid GUID)
        {
            return _caseRepository.GetCaseByEmailGUID(GUID);
        }

        public EmailLog GetEMailLogByGUID(Guid GUID)
        {
            return _emailLogRepository.GetEmailLogsByGuid(GUID);
        }

        public List<DynamicCase> GetAllDynamicCases(int customerId, int[] caseIds)
        {
            return _caseRepository.GetAllDynamicCases(customerId, caseIds);
        }

        public DynamicCase GetDynamicCase(int id)
        {
            return _caseRepository.GetDynamicCase(id);
        }

        public bool IsCaseExist(int id)
        {
            return _caseRepository.IsCaseExists(id);
        }

        // retrieve extended case form for case       
        public ExtendedCaseDataOverview GetCaseExtendedCaseForm(int caseSolutionId, int customerId, int caseId, string userGuid, int caseStateSecondaryId)
        {
            var caseSolution = caseSolutionId > 0
                ? _caseSolutionRepository.GetGetSolutionInfo(caseSolutionId, customerId)
                : null;

            var stateSecondaryId =
                caseStateSecondaryId > 0
                    ? _stateSecondaryRepository.GetById(caseStateSecondaryId).StateSecondaryId
                    : 0;

            //fallback to casesolution.StateSecondaryId if case is new
            if (caseStateSecondaryId == 0 && caseSolution != null && caseSolution.StateSecondaryId > 0)
                stateSecondaryId = _stateSecondaryRepository.GetById(caseSolution.StateSecondaryId).StateSecondaryId;

            var extendedCaseFormData =
                caseId == 0
                    ? _extendedCaseFormRepository.GetExtendedCaseFormForSolution(caseSolutionId, customerId)
                    : _extendedCaseFormRepository.GetExtendedCaseFormForCase(caseId, customerId);

            if (extendedCaseFormData == null) return null;

            extendedCaseFormData.StateSecondaryId = stateSecondaryId;

            if (string.IsNullOrWhiteSpace(extendedCaseFormData.ExtendedCaseFormName))
            {
                extendedCaseFormData.ExtendedCaseFormName = caseSolution != null ? caseSolution.Name : string.Empty;
            }

            //create extendedcase empty record
            if (caseId == 0)
            {
                extendedCaseFormData.ExtendedCaseGuid =
                    _extendedCaseFormRepository.CreateExtendedCaseData(extendedCaseFormData.ExtendedCaseFormId, userGuid);
            }

            return extendedCaseFormData;
        }

        public bool HasExtendedCase(int caseId, int customerId)
        {
            var data = _extendedCaseFormRepository.GetExtendedCaseFormForCase(caseId, customerId);
            return data != null && data.ExtendedCaseFormId > 0;
        }

        // retrieve extended case forms for case sections
        public ExtendedCaseDataOverview GetCaseSectionExtendedCaseForm(int caseSolutionId, int customerId, int caseId, int caseSectionType, string userGuid, int caseStateSecondaryId)
        {
            if (caseSolutionId == 0)
                return null;

            var caseSolution = _caseSolutionRepository.GetGetSolutionInfo(caseSolutionId, customerId);

            if (caseSolution == null)
                return null;

            if (caseSolutionId > 0)
            {
                //fallback to casesolution.StateSecondaryId if case is new
                if (caseStateSecondaryId == 0 && caseSolution.StateSecondaryId > 0)
                {
                    caseStateSecondaryId = _stateSecondaryRepository.GetById(caseSolution.StateSecondaryId).StateSecondaryId;
                }
            }

            var extendedCaseFormData =
                caseId == 0
                    ? _extendedCaseFormRepository.GetCaseSectionExtendedCaseFormForSolution(caseSolutionId, customerId, caseSectionType)
                    : _extendedCaseFormRepository.GetCaseSectionExtendedCaseFormForCase(caseId, customerId);

            if (extendedCaseFormData != null)
            {
                extendedCaseFormData.StateSecondaryId = caseStateSecondaryId;
                if (string.IsNullOrWhiteSpace(extendedCaseFormData.ExtendedCaseFormName))
                {
                    extendedCaseFormData.ExtendedCaseFormName = caseSolution.Name;
                }

                //create extendedcase empty record
                if (caseId == 0)
                {
                    extendedCaseFormData.ExtendedCaseGuid =
                        _extendedCaseFormRepository.CreateExtendedCaseData(extendedCaseFormData.ExtendedCaseFormId, userGuid);
                }
            }

            return extendedCaseFormData;
        }

        public List<ExtendedCaseDataOverview> GetExtendedCaseSectionForms(int caseId, int customerId)
        {
            return _extendedCaseFormRepository.GetExtendedCaseFormsForSections(caseId, customerId);
        }

        public ExtendedCaseDataEntity GetExtendedCaseData(Guid extendedCaseGuid)
        {
            return _extendedCaseDataRepository.GetExtendedCaseData(extendedCaseGuid);
        }

        public void CreateExtendedCaseRelationship(int caseId, int extendedCaseDataId, int? extendedCaseFormId = null)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var rep = uow.GetRepository<Case_ExtendedCaseEntity>();
                var relation = rep.Find(it => it.Case_Id == caseId && it.ExtendedCaseData_Id == extendedCaseDataId).FirstOrDefault();
                if (relation == null)
                {
                    rep.Add(new Case_ExtendedCaseEntity() { Case_Id = caseId, ExtendedCaseData_Id = extendedCaseDataId, ExtendedCaseForm_Id = extendedCaseFormId });
                    uow.Save();

                }
            }

            if (extendedCaseFormId.HasValue)
            {
                using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
                {
                    var rep = uow.GetRepository<ExtendedCaseDataEntity>();
                    var entity = rep.Find(it => it.Id == extendedCaseDataId).FirstOrDefault();
                    if (entity != null)
                    {
                        if (entity.ExtendedCaseFormId != extendedCaseFormId.Value)
                        {
                            entity.ExtendedCaseFormId = extendedCaseFormId.Value;

                            rep.Update(entity);
                            uow.Save();
                        }

                    }
                }
            }
        }

        public void DeleteExCaseWhenCaseMove(int id)
        {
            DeleteExtendedCase(id);
            //To do : when move case delete section as well if exists
            //DeletetblCase_tblCaseSection_ExtendedCaseData(id);
        }

        public int LookupLanguage(int custid, string notid, int regid, int depid, string notifierid)
        {
            int res = _caseRepository.LookupLanguage(custid, notid, regid, depid, notifierid);
            return res;
        }


        public List<CaseFilterFavorite> GetMyFavoritesWithFields(int customerId, int userId)
        {
            var favorites =
                _caseFilterFavoriteRepository.GetUserFavoriteFilters(customerId, userId).ToList();

            var res = favorites.Select(_caseFilterFavoriteToBusinessModelMapper.Map).ToList();
            return res;
        }

        public async Task<List<CaseFilterFavorite>> GetMyFavoritesWithFieldsAsync(int customerId, int userId)
        {
            var favorites =
                await _caseFilterFavoriteRepository.GetUserFavoriteFilters(customerId, userId).ToListAsync();

            var res = favorites.Select(_caseFilterFavoriteToBusinessModelMapper.Map).ToList();
            return res;
        }

        public string SaveFavorite(CaseFilterFavorite favorite)
        {
            var res = _caseFilterFavoriteRepository.SaveFavorite(favorite);
            if (res == string.Empty)
            {
                try
                {
                    _caseFilterFavoriteRepository.Commit();
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
            var res = _caseFilterFavoriteRepository.DeleteFavorite(favoriteId);
            if (res == string.Empty)
            {
                try
                {
                    _caseFilterFavoriteRepository.Commit();
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
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
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
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
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
        public bool MergeChildToParentCase(int childCaseId, int parentCaseId)
        {
            if (childCaseId == parentCaseId)
                return false;

            //Todo - Redo again?
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var merged = uow.GetRepository<MergedCases>();
                var allreadyExists = merged.GetAll()
                        .Where(it => it.MergedChildId == childCaseId // allready a child for [other|this] case
                            && it.MergedParentId == parentCaseId) // child case is a parent already
                        .FirstOrDefault();
                if (allreadyExists != null)
                {
                    return false;
                }

                merged.Add(new MergedCases()
                {
                    MergedParentId = parentCaseId,
                    MergedChildId = childCaseId,
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
            return _caseRepository.GetCaseOverview(caseId);
        }

        public MyCase[] GetMyCases(int userId, int? count = null)
        {
            return _caseRepository.GetMyCases(userId, count);
        }

        public StateSecondary GetCaseSubStatus(int caseId)
        {
            return _caseRepository.GetCaseSubStatus(caseId);
        }

        public CustomerCases[] GetCustomersCases(int[] customerIds, int userId)
        {
            using (var uow = _unitOfWorkFactory.Create())
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
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var caseRep = uow.GetRepository<Case>();

                var restrictToOwnCasesOnly =
                    uow.GetRepository<CustomerUser>().GetAll().Where(x => x.Customer_Id == customerId && x.User_Id == currentUser.Id).Single().RestrictedCasePermission;

                return caseRep.GetAll()
                        .GetByCustomer(customerId)
                        .GetRelatedCases(caseId, userId, currentUser, restrictToOwnCasesOnly)
                        .MapToRelatedCases();
            }
        }

        public int GetCaseRelatedCasesCount(int caseId, int customerId, string userId, UserOverview currentUser)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var caseRep = uow.GetRepository<Case>();

                var restrictToOwnCasesOnly =
                    uow.GetRepository<CustomerUser>().GetAll().Where(x => x.Customer_Id == customerId && x.User_Id == currentUser.Id).Single().RestrictedCasePermission;

                return caseRep.GetAll()
                        .GetByCustomer(customerId)
                        .GetRelatedCases(caseId, userId, currentUser, restrictToOwnCasesOnly)
                        .Count();
            }
        }

        public bool IsRelated(int caseId)
        {
            if (caseId <= 0)
                return false;

            var isParentCase = GetChildCasesFor(caseId);

            if (isParentCase != null && isParentCase.Count() > 0)
            {
                return true;
            }

            var isChildCase = GetParentInfo(caseId);

            if (isChildCase != null)
            {
                return true;
            }

            return false;
        }

        public List<ChildCaseOverview> GetChildCasesFor(int caseId)
        {
            return _caseRepository.GetChildCasesFor(caseId);
        }

        public ParentCaseInfo GetParentInfo(int caseId)
        {
            var parentCaseInfo = _caseRepository.GetParentInfo(caseId);
            return parentCaseInfo;
        }

        public List<MergedChildOverview> GetMergedCasesFor(int caseId)
        {
            return _caseRepository.GetMergedCasesFor(caseId);
        }

        public MergedParentInfo GetMergedParentInfo(int caseId)
        {
            var parentCaseInfo = _caseRepository.GetMergedParentInfo(caseId);
            return parentCaseInfo;
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
            var c = _caseRepository.GetDetachedCaseById(copyFromCaseid);
            if (c.IsAbout == null)
            {

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
            c.User_Id = userId;
            return InitNewCaseCopy(c, userId, ipAddress, source, adUser);
        }

        public Case Copy(int copyFromCaseid, int userId, int languageId, string ipAddress, CaseRegistrationSource source, string adUser)
        {
            var c = _caseRepository.GetDetachedCaseIncludesById(copyFromCaseid);
            c.User_Id = userId;
            return InitNewCaseCopy(c, userId, ipAddress, source, adUser);
        }

        public void MarkAsUnread(int caseId)
        {
            _caseRepository.MarkCaseAsUnread(caseId);
        }

        public void MarkAsRead(int caseId)
        {
            _caseRepository.MarkCaseAsRead(caseId);
        }

        public IList<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user)
        {
            return _caseRepository.GetRelatedCases(id, customerId, reportedBy, user).OrderByDescending(c => c.Id).ToList();
        }

        public Case InitCase(int customerId, int userId, int languageId, string ipAddress, CaseRegistrationSource source, Setting customerSetting, string adUser)
        {
            var customerDefaults = _customerRepository.GetCustomerDefaults(customerId, source == CaseRegistrationSource.SelfService);

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
                Region_Id = customerDefaults.RegionId,
                CaseType_Id = customerDefaults.CaseTypeId,
                Supplier_Id = customerDefaults.SupplierId,
                Priority_Id = customerDefaults.PriorityId,
                Status_Id = customerDefaults.StatusId,
                //State
                WorkingGroup_Id = _userRepository.GetUserDefaultWorkingGroupId(userId, customerId),
                RegUserId = adUser.GetUserFromAdPath(),
                RegUserDomain = adUser.GetDomainFromAdPath()
            };

            // http://redmine.fastdev.se/issues/10997
            //            c.WorkingGroup_Id = _workingGroupService.GetDefaultId(customerId, userId);

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
            return _caseRepository.GetProblemCases(problemId);
        }

        public IList<Case> GetProjectCases(int customerId, int projectId)
        {
            return _caseRepository.GetProjectCases(customerId, projectId);
        }

        public IList<Case> GetProblemCases(int customerId, int problemId)
        {
            return _caseRepository.GetProblemCases(customerId, problemId);
        }

        public IList<Case> GetCasesByCustomers(IEnumerable<int> customerIds)
        {
            return _caseRepository
                .GetMany(x => customerIds.Contains(x.Customer_Id) &&
                         x.Deleted == 0)
                 .ToList();
        }
        public void UpdateFollowUpDate(int caseId, DateTime? time)
        {
            _caseRepository.UpdateFollowUpDate(caseId, time);
        }


        public void Activate(int caseId, int userId, string adUser, string createdByApp, out IDictionary<string, string> errors)
        {
            var _case = _caseRepository.GetCaseById(caseId);
            var customer = _customerService.GetCustomer(_case.Customer_Id);
            var departmentIds = _departmentService.GetDepartments(customer.Id)
                .Select(o => o.Id)
                .ToArray();

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZoneId);

            var workTimeCalcFactory = new WorkTimeCalculatorFactory(
                ManualDependencyResolver.Get<IHolidayService>(),
                customer.WorkingDayStart,
                customer.WorkingDayEnd,
                timeZone);

            var utcNow = DateTime.UtcNow;
            var workTimeCalc = workTimeCalcFactory.Build(_case.RegTime, utcNow, departmentIds);
            var externalTimeToAdd = workTimeCalc.CalculateWorkTime(
                _case.ChangeTime,
                utcNow,
                _case.Department_Id);

            var possibleWorktime = workTimeCalc.CalculateWorkTime(
                _case.RegTime,
                utcNow,
                _case.Department_Id);

            var leadTime = possibleWorktime - _case.ExternalTime - externalTimeToAdd;

            _caseRepository.Activate(caseId, leadTime, externalTimeToAdd);
            var c = _caseRepository.GetDetachedCaseById(caseId);
            _caseStatService.UpdateCaseStatistic(c);

            var extraFields = new ExtraFieldCaseHistory
            {
                ActionExternalTime = externalTimeToAdd,
                ActionLeadTime = leadTime - _case.LeadTime,
                LeadTime = leadTime
            };

            SaveCaseHistory(c, userId, adUser, createdByApp, out errors, "", extraFields);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
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
            var ret = 0;

            if (cases == null)
                throw new ArgumentNullException(nameof(cases));

            var c = ValidateCaseRequiredValues(cases, caseLog);

            // unread/status flag update if not case is closed and not changed by adminsitrator 
            //c.Unread = 0;
            if (c.Performer_User_Id != userId && !c.FinishingDate.HasValue)
                c.Unread = 1;

            if (c.Id == 0)
            {
                c.RegTime = DateTime.UtcNow;
                c.ChangeTime = DateTime.UtcNow;

                _caseRepository.Add(c);
            }
            else
            {
                c.ChangeTime = DateTime.UtcNow;
                c.ChangeByUser_Id = userId == 0 ? (int?)null : userId;

                _caseRepository.Update(c);
            }

            _caseRepository.Commit();
            _caseStatService.UpdateCaseStatistic(c);

            // save CaseIsAbout
            if (c.IsAbout != null)
                SaveIsAbout(c, out errors);

            // save casehistory
            var extraFields = new ExtraFieldCaseHistory();
            if (caseLog != null && caseLog.FinishingType != null)
            {
                //This because it was not translated if Merged case
                if (!string.IsNullOrEmpty(caseLog.FinishingTypeName))
                    extraFields.ClosingReason = caseLog.FinishingTypeName;
                else
                {
                    var fc = _finishingCauseService.GetFinishingTypeName(caseLog.FinishingType.Value);

                    extraFields.ClosingReason = fc;
                }

            }
            //Check if it is a merged case
            var mergeParent = GetMergedParentInfo(cases.Id);
            if (parentCase != null && mergeParent == null)
            {
                this.AddChildCase(cases.Id, parentCase.Id, out errors);
            }

            extraFields.LeadTime = caseExtraInfo.LeadTimeForNow;
            extraFields.ActionLeadTime = caseExtraInfo.ActionLeadTime;
            extraFields.ActionExternalTime = caseExtraInfo.ActionExternalTime;

            ret = userId == 0 ?
                SaveCaseHistory(c, userId, adUser, caseExtraInfo.CreatedByApp, out errors, adUser, extraFields, caseExtraFollowers) :
                SaveCaseHistory(c, userId, adUser, caseExtraInfo.CreatedByApp, out errors, string.Empty, extraFields, caseExtraFollowers);

            return ret;
        }

        public int SaveFileDeleteHistory(Case c, string fileName, int userId, string adUser, out IDictionary<string, string> errors, string appName = null)
        {
            var extraField = new ExtraFieldCaseHistory { CaseFile = StringTags.Delete + fileName };
            return SaveCaseHistory(c, userId, adUser, appName ?? CreatedByApplications.Helpdesk5, out errors, string.Empty, extraField);
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
                throw new ArgumentNullException("case");

            errors = new Dictionary<string, string>();
            var h = this.GenerateHistoryFromCase(c, userId, adUser, defaultUser, extraField, caseExtraFollowers);
            h.CreatedByApp = createdByApp;
            _caseHistoryRepository.Add(h);

            if (errors.Count == 0)
                _caseHistoryRepository.Commit();

            return h.Id;
        }

        public IList<CaseHistory> GetCaseHistoryByCaseId(int caseId)
        {
            return _caseHistoryRepository.GetCaseHistoryByCaseId(caseId).AsQueryable().OrderBy(h => h.Id).ToList();
        }

        public IList<CaseHistoryOverview> GetCaseHistories(int caseId)
        {
            var items = _caseHistoryRepository.GetCaseHistories(caseId);
            return items.Select(_caseHistoryOverviewMapper.Map).ToList();
        }

        public Dictionary<int, string> GetCaseFiles(List<int> caseIds)
        {
            var preCaseFiles = _caseFileRepository.GetMany(f => caseIds.Contains(f.Case_Id)).ToList();

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

        public void ExecuteBusinessActions(List<BusinessRuleActionModel> actions, int currentCaseId, CaseLog log, TimeZoneInfo userTimeZone,
                                           int caseHistoryId, string basePath, int currentLanguageId, CaseMailSetting caseMailSetting,
                                           List<CaseLogFileDto> logFiles = null)
        {
            foreach (var action in actions)
            {
                switch (action.ActionType)
                {
                    case BRActionType.SendEmail:
                        if (caseMailSetting != null)
                            DoAction_SendEmail(action, currentCaseId, log, userTimeZone, caseHistoryId, basePath, currentLanguageId, caseMailSetting, logFiles);
                        break;
                }
            }
        }

        public IList<Case> GetTop100CasesForTest()
        {
            return _caseRepository.GetTop100CasesToTest();
        }

        public int GetCaseRelatedInventoryCount(int customerId, string userId, UserOverview currentUser)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var computerRep = uow.GetRepository<Computer>();

                return computerRep.GetAll().GetRelatedInventoriesCount(userId, currentUser, customerId);
            }
        }

        public int GetCaseQuickOpen(UserOverview user, int customerId, string searchFor)
        {
            var casePermissionFilter = _userService.GetCasePermissionFilter(user, customerId);

            searchFor = searchFor.Tidy();

            if (searchFor.Length > 0)
            {
                var case_ = _caseRepository.GetCaseQuickOpen(user, casePermissionFilter, searchFor);
                if (case_ != null)
                    return case_.Id;
            }

            return 0;
        }

        public void HandleSendMailAboutCaseToPerformer(CustomerUserInfo performerUser, int currentUserId, CaseLog caseLog)
        {
            if (performerUser != null)
            {
                if (performerUser.Id > 0)
                {
                    if (caseLog.SendMailAboutCaseToPerformer && performerUser.Id != currentUserId)
                    {
                        caseLog.EmailRecepientsInternalLogTo = caseLog.EmailRecepientsInternalLogTo ?? String.Empty;

                        if (!caseLog.EmailRecepientsInternalLogTo.Contains(performerUser.Email))
                        {
                            caseLog.EmailRecepientsInternalLogTo += performerUser.Email + ";";
                        }
                    }
                }
            }
        }

        #region Private methods

        private void DeleteChildCasesFor(int caseId)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                uow.GetRepository<ParentChildRelation>().DeleteWhere(it => it.AncestorId == caseId);
                uow.Save();
            }
        }

        private void DeleteExtendedCase(int caseId)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                uow.GetRepository<Case_ExtendedCaseEntity>().DeleteWhere(it => it.Case_Id == caseId);
                uow.Save();
            }
        }

        private void DeletetblCase_tblCaseSection_ExtendedCaseData(int caseId)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                uow.GetRepository<Case_CaseSection_ExtendedCase>().DeleteWhere(it => it.Case_Id == caseId);
                uow.Save();
            }
        }

        private void DeleteCaseById(int caseId)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                uow.GetRepository<Case>().DeleteWhere(it => it.Id == caseId);
                uow.Save();
            }
        }

        private void DeleteCaseIsAboutFor(int caseId)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
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
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
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

        public bool AddChildCase(int childCaseId, int parentCaseId, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
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

        private void DoAction_SendEmail(BusinessRuleActionModel action, int currentCaseId, CaseLog log, TimeZoneInfo userTimeZone,
                                        int caseHistoryId, string basePath, int currentLanguageId, CaseMailSetting caseMailSetting,
                                        List<CaseLogFileDto> logFiles = null)
        {

            var currentCase = _caseRepository.GetCaseIncluding(currentCaseId);
            var customerId = currentCase.Customer_Id;
            var customerSettings = _settingService.GetCustomerSetting(customerId);
            var sep = new[] { ';' };
            var templateId = 0;
            var emailList = new List<string>();

            foreach (var param in action.ActionParams)
            {
                var dataList = !string.IsNullOrEmpty(param.ParamValue) ? param.ParamValue.Split(BRConstItem.Value_Separator, StringSplitOptions.RemoveEmptyEntries) : null;
                switch (param.ParamType)
                {
                    case BRActionParamType.EMailTemplate:
                        if (!string.IsNullOrEmpty(param.ParamValue))
                        {
                            templateId = 0;
                            int.TryParse(param.ParamValue, out templateId);
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
                                        var usEmails = wg.UserWorkingGroups.Where(w => w.User != null && w.User.IsActive != 0).Select(w => w.User.Email).ToList();
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

            if (templateId > 0 && emailList.Any())
            {
                var files = PrepareAttachedFiles(logFiles, basePath);

                SendTemplateEmail(templateId,
                    currentCase,
                    log,
                    caseHistoryId,
                    customerSettings,
                    caseMailSetting,
                    caseMailSetting.HelpdeskMailFromAdress?.Trim(),
                    emailList.ToDistintList(true),
                    userTimeZone,
                    files,
                    1);
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

            //if (caseLog != null && caseLog.TextExternal != null && caseLog.TextExternal.Length > 3000)
            //    caseLog.TextExternal = caseLog.TextExternal.Substring(0, 3000);

            //if (caseLog != null && caseLog.TextInternal != null && caseLog.TextInternal.Length > 3000)
            //    caseLog.TextInternal = caseLog.TextInternal.Substring(0, 3000);

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
            var user = _userRepository.GetUser(userId);
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
            h.CostCentre = c.CostCentre;
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
                h.IsAbout_Persons_EMail = c.IsAbout.Person_Email;
                h.IsAbout_Persons_CellPhone = c.IsAbout.Person_Cellphone;
                h.IsAbout_Region_Id = c.IsAbout.Region_Id;
                h.IsAbout_OU_Id = c.IsAbout.OU_Id;
                h.IsAbout_CostCentre = c.IsAbout.CostCentre;
                h.IsAbout_Place = c.IsAbout.Place;
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

        private List<Field> GetCaseFieldsForEmail(Case c, CaseLog l, CaseMailSetting cms, string emailLogGuid, int stateHelper, TimeZoneInfo userTimeZone, Case mergeParent = null)
        {
            var ret = new List<Field>();
            var userLocal_RegTime = TimeZoneInfo.ConvertTimeFromUtc(c.RegTime, userTimeZone);

            ret.Add(new Field { Key = "[#1]", StringValue = c.CaseNumber.ToString() });
            ret.Add(new Field { Key = "[#16]", StringValue = userLocal_RegTime.ToString() });
            var lastUserName = string.Empty;
            if (c.ChangeByUser_Id.HasValue)
            {
                var user = _userRepository.GetUserName(c.ChangeByUser_Id.Value);
                if (user != null)
                {
                    lastUserName = user.GetFullName();
                }

            }
            else
            {
                if (c.User_Id.HasValue)
                {
                    var user = _userRepository.GetUserName(c.User_Id.Value);
                    lastUserName = user != null ? user.GetFullName() : string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(c.RegUserName))
                    {
                        lastUserName = c.RegUserName;
                    }

                    if (!string.IsNullOrEmpty(c.RegUserId))
                    {
                        lastUserName = c.RegUserId;
                    }
                }
            }
            ret.Add(new Field { Key = "[#22]", StringValue = lastUserName });
            ret.Add(new Field { Key = "[#3]", StringValue = c.PersonsName });
            ret.Add(new Field { Key = "[#8]", StringValue = c.PersonsEmail });
            ret.Add(new Field { Key = "[#9]", StringValue = c.PersonsPhone });
            ret.Add(new Field { Key = "[#18]", StringValue = c.PersonsCellphone });
            ret.Add(new Field { Key = "[#2]", StringValue = _customerRepository.GetCustomerName(c.Customer_Id) });
            ret.Add(new Field { Key = "[#24]", StringValue = c.Place });
            ret.Add(new Field { Key = "[#17]", StringValue = c.InventoryNumber });
            var caseTypeName = _caseTypeRepository.GetCaseType(c.CaseType_Id).Select(ct => ct.Name).DefaultIfEmpty(string.Empty).FirstOrDefault();
            ret.Add(new Field { Key = "[#25]", StringValue = caseTypeName });
            var catName = c.Category_Id.HasValue ?
                _categoryRepository.GetCategory(c.Category_Id.Value).Select(ct => ct.Name).DefaultIfEmpty(string.Empty).FirstOrDefault() :
                string.Empty;
            ret.Add(new Field { Key = "[#26]", StringValue = catName });
            ret.Add(new Field { Key = "[#4]", StringValue = c.Caption });
            ret.Add(new Field { Key = "[#5]", StringValue = c.Description });
            ret.Add(new Field { Key = "[#23]", StringValue = c.Miscellaneous });
            ret.Add(new Field { Key = "[#19]", StringValue = c.Available });
            var wg = c.WorkingGroup_Id.HasValue ? _workingGroupService.GetWorkingGroup(c.WorkingGroup_Id.Value) : null;
            ret.Add(new Field { Key = "[#15]", StringValue = wg != null ? c.Workinggroup.WorkingGroupName : string.Empty });
            ret.Add(new Field { Key = "[#13]", StringValue = wg != null ? c.Workinggroup.EMail : string.Empty });
            var admin = c.Performer_User_Id.HasValue ? _userRepository.GetUserName(c.Performer_User_Id.Value) : null;
            var adminFields = c.Performer_User_Id.HasValue ? _userRepository.GetUserInfo(c.Performer_User_Id.Value) : null;
            ret.Add(new Field { Key = "[#6]", StringValue = admin != null ? admin.FirstName : string.Empty });
            ret.Add(new Field { Key = "[#7]", StringValue = admin != null ? admin.LastName : string.Empty });
            ret.Add(new Field { Key = "[#70]", StringValue = admin != null ? adminFields.Phone : string.Empty });
            ret.Add(new Field { Key = "[#71]", StringValue = admin != null ? adminFields.CellPhone : string.Empty });
            ret.Add(new Field { Key = "[#72]", StringValue = admin != null ? adminFields.Email : string.Empty });
            var priority = c.Priority_Id.HasValue ? _priorityService.GetPriority(c.Priority_Id.Value) : null;
            ret.Add(new Field { Key = "[#12]", StringValue = priority != null ? priority.Name : string.Empty });
            ret.Add(new Field { Key = "[#20]", StringValue = priority != null ? priority.Description : string.Empty });
            ret.Add(new Field { Key = "[#21]", StringValue = c.WatchDate.ToString() });

            if (c.User_Id.HasValue)
            {
                var user = _userRepository.GetUserName(c.User_Id.Value);
                ret.Add(new Field { Key = "[#29]", StringValue = user != null ? user.GetFullName() : string.Empty });
            }
            else
            {
                if (!string.IsNullOrEmpty(c.RegUserName))
                {
                    ret.Add(new Field { Key = "[#29]", StringValue = c.RegUserName });
                }

                if (!string.IsNullOrEmpty(c.RegUserId))
                {
                    ret.Add(new Field { Key = "[#29]", StringValue = c.RegUserId });
                }
            }

            if (c.ProductArea_Id.HasValue)
            {
                if (c.ProductArea.Parent_ProductArea_Id.HasValue && c.ProductArea.Parent_ProductArea_Id > 0)
                {
                    var names = _productAreaService.GetParentPath(c.ProductArea_Id.Value, c.Customer_Id).ToList();
                    ret.Add(new Field { Key = "[#28]", StringValue = string.Join(" - ", names) });
                }
                else
                {
                    ret.Add(new Field { Key = "[#28]", StringValue = c.ProductArea != null ? c.ProductArea.Name : string.Empty });
                }
            }
            else
            {
                ret.Add(new Field
                {
                    Key = "[#28]",
                    StringValue = string.Empty
                });
            }
            ret.Add(new Field { Key = "[#10]", StringValue = l?.TextExternal ?? "" });
            ret.Add(new Field { Key = "[#11]", StringValue = l?.TextInternal ?? "" });

            // selfservice site
            if (cms != null)
            {
                if (emailLogGuid == string.Empty)
                    emailLogGuid = " >> *" + stateHelper.ToString() + "*";

                var site = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + emailLogGuid;
                var url = "<br><a href='" + site + "'>" + site + "</a>";
                ret.Add(new Field { Key = "[#98]", StringValue = url });
            }

            // heldesk site
            if (cms != null)
            {
                var globalSetting = _globalSettingService.GetGlobalSettings().First();
                var editCasePath = globalSetting.UseMobileRouting ?
                    CasePaths.EDIT_CASE_MOBILEROUTE :
                    CasePaths.EDIT_CASE_DESKTOP;

                var site = cms.AbsoluterUrl + editCasePath + c.Id.ToString();
                var url = "<br><a href='" + site + "'>" + site + "</a>";
                ret.Add(new Field { Key = "[#99]", StringValue = url });
            }

            // Survey template
            if (cms != null)
            {
                // if case is closed and was no vote in survey - add HTML inormation about survey
                if (c.IsClosed() && (_surveyService.GetByCaseId(c.Id) == null) && mergeParent == null)
                {
                    var template = new SurveyTemplate()
                    {
                        VoteBadLink =
                            string.Format(
                                "{0}Survey/vote/{1}?voteId=bad",
                                cms.AbsoluterUrl,
                                c.Id),
                        VoteBadText = _translateCacheService.GetTextTranslation("Inte nöjd", c.RegLanguage_Id),
                        VoteNormalLink =
                            string.Format(
                                "{0}Survey/vote/{1}?voteId=normal",
                                cms.AbsoluterUrl,
                                c.Id),
                        VoteNormalText = _translateCacheService.GetTextTranslation("Nöjd", c.RegLanguage_Id),
                        VoteGoodLink =
                            string.Format(
                                "{0}Survey/vote/{1}?voteId=good",
                                cms.AbsoluterUrl,
                                c.Id),
                        VoteGoodText = _translateCacheService.GetTextTranslation("Mycket nöjd", c.RegLanguage_Id),
                    };
                    ret.Add(new Field { Key = "[#777]", StringValue = template.TransformText() });
                }
                else
                {
                    ret.Add(new Field { Key = "[#777]", StringValue = string.Empty });
                }
            }

            //Merge Parent
            if (mergeParent != null)
            {
                userLocal_RegTime = TimeZoneInfo.ConvertTimeFromUtc(mergeParent.RegTime, userTimeZone);
                ret.Add(new Field { Key = "[#MP1]", StringValue = mergeParent.CaseNumber.ToString() });
                ret.Add(new Field { Key = "[#MP16]", StringValue = userLocal_RegTime.ToString() });
                ret.Add(new Field { Key = "[#MP3]", StringValue = mergeParent.PersonsName });
                ret.Add(new Field { Key = "[#MP4]", StringValue = mergeParent.Caption });
                ret.Add(new Field { Key = "[#MP5]", StringValue = mergeParent.Description });
                if (mergeParent.User_Id.HasValue)
                {
                    var user = _userRepository.GetUserName(mergeParent.User_Id.Value);
                    ret.Add(new Field { Key = "[#MP29]", StringValue = user != null ? user.GetFullName() : string.Empty });
                }
                else
                {
                    if (!string.IsNullOrEmpty(c.RegUserName))
                    {
                        ret.Add(new Field { Key = "[#MP29]", StringValue = c.RegUserName });
                    }

                    if (!string.IsNullOrEmpty(c.RegUserId))
                    {
                        ret.Add(new Field { Key = "[#MP29]", StringValue = c.RegUserId });
                    }
                }
                //helpdesk site
                if (cms != null)
                {
                    var globalSetting = _globalSettingService.GetGlobalSettings().First();
                    var editCasePath = globalSetting.UseMobileRouting ?
                        CasePaths.EDIT_CASE_MOBILEROUTE :
                        CasePaths.EDIT_CASE_DESKTOP;

                    var site = cms.AbsoluterUrl + editCasePath + mergeParent.Id.ToString();
                    var url = "<br><a href='" + site + "'>" + site + "</a>";
                    ret.Add(new Field { Key = "[#MP99]", StringValue = url });
                }
                // selfservice site link to merge parent
                if (cms != null)
                {
                    var mergeParentHistoryId = _caseHistoryRepository.GetCaseHistoryByCaseId(mergeParent.Id).Last().Id;
                    var emailLog = new EmailLog(mergeParentHistoryId, 18, cms.HelpdeskMailFromAdress,
                        _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                    emailLog.SendTime = DateTime.Now;
                    emailLog.CreatedDate = DateTime.Now;
                    emailLog.ChangedDate = DateTime.Now;
                    emailLog.SendStatus = 0;
                    _emailLogRepository.Add(emailLog);
                    _emailLogRepository.Commit();

                    if (emailLogGuid == string.Empty)
                        emailLogGuid = " >> *" + stateHelper.ToString() + "*";

                    var site = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + emailLog.EmailLogGUID;
                    var url = "<br><a href='" + site + "'>" + site + "</a>";
                    ret.Add(new Field { Key = "[#MP98]", StringValue = url });
                    ret.Add(new Field { Key = "[#MP98Link]", StringValue = site });
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

        public void SetIndependentChild(int caseID, bool independentChild)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
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

        public void CreateExtendedCaseSectionRelationship(int caseID, int extendedCaseDataID, CaseSectionType sectionType, int customerID)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var caseSections = _caseSectionsRepository.GetCaseSections(customerID);
                var caseSection = caseSections.Single(o => o.SectionType == (int)sectionType);

                var rep = uow.GetRepository<Case_CaseSection_ExtendedCase>();

                // Other relation on section and case and remove
                var otherRelation = rep.Find(it => it.Case_Id == caseID && it.ExtendedCaseData_Id != extendedCaseDataID && it.CaseSection_Id == caseSection.Id).FirstOrDefault();

                if (otherRelation != null)
                {
                    rep.Delete(otherRelation);
                }

                var relation = rep.Find(it => it.Case_Id == caseID && it.ExtendedCaseData_Id == extendedCaseDataID && it.CaseSection_Id == caseSection.Id).ToList();

                // If already exist do nothing
                if (!relation.Any())
                {
                    rep.Add(new Case_CaseSection_ExtendedCase() { Case_Id = caseID, ExtendedCaseData_Id = extendedCaseDataID, CaseSection_Id = caseSection.Id });

                }
                uow.Save();
            }
        }

        public void RemoveAllExtendedCaseSectionData(int caseID, int customerID, CaseSectionType sectionType)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var caseSections = _caseSectionsRepository.GetCaseSections(customerID);
                var caseSection = caseSections.SingleOrDefault(o => o.SectionType == (int)sectionType);

                if (caseSection != null)
                {

                    var rep = uow.GetRepository<Case_CaseSection_ExtendedCase>();

                    var all = rep.Find(o => o.Case_Id == caseID && o.CaseSection_Id == caseSection.Id);

                    foreach (var r in all)
                    {
                        rep.Delete(r);
                    }
                    uow.Save();
                }
            }
        }

        public void CheckAndUpdateExtendedCaseSectionData(int extendedCaseDataID, int caseID, int customerID, CaseSectionType sectionType)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var caseSections = _caseSectionsRepository.GetCaseSections(customerID);
                var caseSection = caseSections.SingleOrDefault(o => o.SectionType == (int)sectionType);

                if (caseSection != null)
                {
                    var rep = uow.GetRepository<Case_CaseSection_ExtendedCase>();

                    var all = rep.Find(o => o.Case_Id == caseID && o.CaseSection_Id == caseSection.Id).ToList();

                    var toRemove = all.Where(o => o.ExtendedCaseData_Id != extendedCaseDataID).ToList();

                    foreach (var r in toRemove)
                    {
                        rep.Delete(r);
                    }

                    // If there isn't already a connection
                    if (!all.Any(o => o.ExtendedCaseData_Id == extendedCaseDataID))
                    {
                        rep.Add(new Case_CaseSection_ExtendedCase
                        {
                            Case_Id = caseID,
                            CaseSection_Id = caseSection.Id,
                            ExtendedCaseData_Id = extendedCaseDataID
                        });
                    }

                    uow.Save();
                }
            }

        }        

        #endregion
    }
}

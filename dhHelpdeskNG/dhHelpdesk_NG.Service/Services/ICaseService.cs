using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.BusinessRules;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.Models.Case.ChidCase;
using DH.Helpdesk.BusinessData.Models.Case.MergedCase;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.ExtendedCaseEntity;

namespace DH.Helpdesk.Services.Services
{
    public interface ICaseService
    {
        IList<Case> GetProjectCases(int customerId, int projectId);
        IList<Case> GetProblemCases(int customerId, int problemId);

        IList<Case> GetCasesByCustomers(IEnumerable<int> customerIds);
        bool MergeChildToParentCase(int childCaseId, int parentCaseId);
        Case InitCase(int customerId, int userId, int languageId, string ipAddress, CaseRegistrationSource source, Setting customerSetting, string adUser);

        Case Copy(int copyFromCaseid, int userId, int languageId, string ipAddress, CaseRegistrationSource source, string adUser);

        Case InitChildCaseFromCase(
            int copyFromCaseid,
            int userId,
            string ipAddress,
            CaseRegistrationSource source,
            string adUser,
            out ParentCaseInfo parentCaseInfo);


        CaseOverview GetCaseBasic(int id);
        Task<Case> GetCaseByIdAsync(int id, bool markCaseAsRead = false);
        Case GetCaseById(int id, bool markCaseAsRead = false);
        Case GetDetachedCaseById(int id);
        Case GetCaseByGUID(Guid GUID);
        int GetCaseIdByEmailGUID(Guid GUID);
        Case GetCaseByEMailGUID(Guid GUID);
        EmailLog GetEMailLogByGUID(Guid GUID);
        IList<CaseHistory> GetCaseHistoryByCaseId(int caseId);
        IList<CaseHistoryOverview> GetCaseHistories(int caseId);
        List<DynamicCase> GetAllDynamicCases(int customerId, int[] caseIds);
        DynamicCase GetDynamicCase(int id);
        IList<Case> GetProblemCases(int problemId);

        ExtendedCaseDataOverview GetCaseExtendedCaseForm(int caseSolutionId, int customerId, int caseId, string userGuid, int caseStateSecondaryId);
        ExtendedCaseDataOverview GetCaseSectionExtendedCaseForm(int caseSolutionId, int customerId, int caseId, int caseSectionType, string userGuid, int caseStateSecondaryId);
        List<ExtendedCaseDataOverview> GetExtendedCaseSectionForms(int caseId, int customerId);
        bool HasExtendedCase(int caseId, int customerId);

        //todo:review
        ExtendedCaseDataEntity GetExtendedCaseData(Guid extendedCaseGuid);

        bool AddChildCase(int childCaseId, int parentCaseId, out IDictionary<string, string> errors);

        void CreateExtendedCaseRelationship(int caseId, int extendedCaseDataId, int? extendedCaseFormId = null);

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

        int SaveFileDeleteHistory(Case c, string fileName, int userId, string adUser, out IDictionary<string, string> errors, string appName = null);

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
                           Case oldCase = null, CaseLog log = null, List<CaseLogFileDto> logFiles = null, User currentLoggedInUser = null,
                            string extraFollowersEmails = null);

        List<BusinessRuleActionModel> CheckBusinessRules(BREventType occurredEvent, Case currentCase, Case oldCase = null);

        void ExecuteBusinessActions(List<BusinessRuleActionModel> actions, int currentCaseId, CaseLog log, TimeZoneInfo userTimeZone,
                                    int caseHistoryId, string basePath, int currentLanguageId, CaseMailSetting caseMailSetting,
                                    List<CaseLogFileDto> logFiles = null
                                    );

        void UpdateFollowUpDate(int caseId, DateTime? time);
        void MarkAsUnread(int caseId);
        void MarkAsRead(int caseId);
        void SendSelfServiceCaseLogEmail(int caseId, CaseMailSetting cms, int caseHistoryId, CaseLog log, string basePath, TimeZoneInfo userTimeZone, List<CaseLogFileDto> logFiles = null, bool caseIsActivated = false);
        void Activate(int caseId, int userId, string adUser, string createByApp, out IDictionary<string, string> errors);
        IList<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user);
        void Commit();

        void DeleteExCaseWhenCaseMove(int id);

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

		StateSecondary GetCaseSubStatus(int caseId);

        CustomerCases[] GetCustomersCases(int[] customerIds, int userId);

        List<RelatedCase> GetCaseRelatedCases(int caseId, int customerId, string userId, UserOverview currentUser);

        int GetCaseRelatedCasesCount(int caseId, int customerId, string userId, UserOverview currentUser);

        bool IsRelated(int caseId);
        bool IsCaseExist(int id);

        List<ChildCaseOverview> GetChildCasesFor(int caseId);

        ParentCaseInfo GetParentInfo(int caseId);

        List<MergedChildOverview> GetMergedCasesFor(int parentCaseId);
        MergedParentInfo GetMergedParentInfo(int childCaseId);

        int? SaveInternalLogMessage(int id, string textInternal, out IDictionary<string, string> errors);

        Dictionary<int, string> GetCaseFiles(List<int> caseIds);

        List<CaseFilterFavorite> GetMyFavoritesWithFields(int customerId, int userId);
        Task<List<CaseFilterFavorite>> GetMyFavoritesWithFieldsAsync(int customerId, int userId);

        string SaveFavorite(CaseFilterFavorite favorite);

        string DeleteFavorite(int favoriteId);
        void DeleteChildCaseFromParent(int id, int parentId);
        bool AddParentCase(int id, int parentId, bool independent = false);
        void SetIndependentChild(int caseID, bool independentChild);

        void CreateExtendedCaseSectionRelationship(int caseID, int extendedCaseDataID, CaseSectionType sectionType, int customerID);
        void CheckAndUpdateExtendedCaseSectionData(int extendedCaseDataID, int caseID, int customerID, CaseSectionType sectionType);
        void RemoveAllExtendedCaseSectionData(int caseID, int customerID, CaseSectionType initiator);

        IList<Case> GetTop100CasesForTest();
        int GetCaseRelatedInventoryCount(int customerId, string userId, UserOverview currentUser);
        int GetCaseQuickOpen(UserOverview user, int customerId, string searchFor);
        void SendProblemLogEmail(Case cs, CaseMailSetting caseMailSetting, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog caseLog, bool isClosedCaseSending);
        int GetCaseCustomerId(int caseId);
        Customer GetCaseCustomer(int caseId);
        Task<List<CaseHistoryMapperData>> GetCaseHistoriesAsync(int caseId);
        Task<CustomerCasesStatus> GetCustomerCasesStatusAsync(int customerId, int userId);
        Task<Case> GetDetachedCaseByIdAsync(int id);
        void HandleSendMailAboutCaseToPerformer(CustomerUserInfo performerUser, int currentUserId, CaseLog caseLog);
        void SendMergedCaseEmail(Case mergedCase, Case mergeParent, CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog caseLog, IList<string> ccEmailList);
    }
}

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ICaseFieldSettingService
    {
        IList<CaseListToCase> ListToShowOnCasePage(int customerId, int languageId);
        IList<CaseFieldSetting> GetAllCaseFieldSettings();
        IList<CaseFieldSetting> GetCaseFieldSettings(int customerId);
        IList<CaseFieldSettingLanguage> GetCaseFieldSettingLanguages();
        IList<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguages(int customerId, int languageId);
        IList<ListCases> ListToShowOnCaseSummaryPage(int? customerId, int? languageId, int? UserGroupId);
        IList<ListCases> ListToShowOnCustomerSettingSummaryPage(int? customerId, int? languageId, int? UserGroupId);

        CaseFieldSettingLanguage GetCaseFieldSettingLanguage(int id, int languageId);
    }

    public class CaseFieldSettingService : ICaseFieldSettingService
    {
        private readonly ICaseFieldSettingRepository _caseFieldSettingRepository;
        private readonly ICaseFieldSettingLanguageRepository _caseFieldSettingLanguageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CaseFieldSettingService(
            ICaseFieldSettingRepository caseFieldSettingRepository,
            ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository,
            IUnitOfWork unitOfWork)
        {
            this._caseFieldSettingRepository = caseFieldSettingRepository;
            this._caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<CaseListToCase> ListToShowOnCasePage(int customerId, int languageId)
        {
            return this._caseFieldSettingRepository.GetListToCustomerCase(customerId, languageId).ToList();
        }

        public IList<CaseFieldSetting> GetAllCaseFieldSettings()
        {
            return this._caseFieldSettingRepository.GetAll().ToList();
        }

        public IList<CaseFieldSetting> GetCaseFieldSettings(int customerId)
        {
            return this._caseFieldSettingRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<CaseFieldSettingLanguage> GetCaseFieldSettingLanguages()
        {
            return this._caseFieldSettingLanguageRepository.GetAll().ToList();
        }

        public IList<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguages(int customerId, int languageId)
        {
            return this._caseFieldSettingLanguageRepository.GetCaseFieldSettingsWithLanguages(customerId, languageId).ToList();
        }

        public CaseFieldSettingLanguage GetCaseFieldSettingLanguage(int id, int languageId)
        {
            return this._caseFieldSettingLanguageRepository.Get(x => x.CaseFieldSetting.Id == id && x.Language_Id == languageId);
        }

        public IList<ListCases> ListToShowOnCaseSummaryPage(int? customerId, int? languageId, int? userGroupId)
        {
            return this._caseFieldSettingRepository.GetListCasesToCaseSummary(customerId, languageId, userGroupId).ToList();
        }

        public IList<ListCases> ListToShowOnCustomerSettingSummaryPage(int? customerId, int? languageId, int? userGroupId)
        {
            return this._caseFieldSettingRepository.GetCaseFieldSettingsListToCustomerCaseSummary(customerId, languageId, userGroupId).ToList();
        }
    }
}

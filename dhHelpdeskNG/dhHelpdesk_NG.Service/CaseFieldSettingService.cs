using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Service
{
    public interface ICaseFieldSettingService
    {
        IList<CaseListToCase> ListToShowOnCasePage(int customerId, int languageId);
        IList<CaseFieldSetting> GetAllCaseFieldSettings();
        IList<CaseFieldSetting> GetCaseFieldSettings(int customerId);
        IList<CaseFieldSettingLanguage> GetCaseFieldSettingLanguages();
        IList<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguages(int customerId, int languageId);
        IList<ListCases> ListToShowOnCaseSummaryPage(int? customerId, int? languageId, int? UserGroupId);
        IList<ListCases> ListToShowOnCustomerSettingSummaryPage(int? customerId, int? languageId, int? UserGroupId);
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
            _caseFieldSettingRepository = caseFieldSettingRepository;
            _caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<CaseListToCase> ListToShowOnCasePage(int customerId, int languageId)
        {
            return _caseFieldSettingRepository.GetListToCustomerCase(customerId, languageId).ToList();
        }

        public IList<CaseFieldSetting> GetAllCaseFieldSettings()
        {
            return _caseFieldSettingRepository.GetAll().ToList();
        }

        public IList<CaseFieldSetting> GetCaseFieldSettings(int customerId)
        {
            return _caseFieldSettingRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<CaseFieldSettingLanguage> GetCaseFieldSettingLanguages()
        {
            return _caseFieldSettingLanguageRepository.GetAll().ToList();
        }

        public IList<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguages(int customerId, int languageId)
        {
            return _caseFieldSettingLanguageRepository.GetCaseFieldSettingsWithLanguages(customerId, languageId).ToList();
        }

        public IList<ListCases> ListToShowOnCaseSummaryPage(int? customerId, int? languageId, int? userGroupId)
        {
            return _caseFieldSettingRepository.GetListCasesToCaseSummary(customerId, languageId, userGroupId).ToList();
        }

        public IList<ListCases> ListToShowOnCustomerSettingSummaryPage(int? customerId, int? languageId, int? userGroupId)
        {
            return _caseFieldSettingRepository.GetCaseFieldSettingsListToCustomerCaseSummary(customerId, languageId, userGroupId).ToList();
        }
    }
}

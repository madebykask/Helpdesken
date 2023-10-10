using System.Threading.Tasks;
using System.Data.Entity;

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
        IList<CaseFieldSetting> GetCaseFieldSettingsByName(int customerId, string name);
        CaseFieldSetting GetCaseFieldSetting(int customerId, string fieldId);
        IList<CaseFieldSetting> GetCaseFieldSettings(int customerId, int? languageId = null);
        Task<IList<CaseFieldSetting>> GetCaseFieldSettingsAsync(int customerId, int? languageId = null);
        IList<CaseFieldSetting> GetCustomerEnabledCaseFieldSettings(int customerId);
        
        IList<CaseFieldSettingLanguage> GetCaseFieldSettingLanguages();
        IList<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguages(int? customerId, int languageId);
        IList<CaseFieldSettingsWithLanguage> GetAllCaseFieldSettingsWithLanguages(int? customerId, int languageId);

        IList<CaseFieldSettingsForTranslation> GetCustomerCaseTranslations(int customerId);
        Task<IList<CaseFieldSettingsForTranslation>> GetCustomerCaseTranslationsAsync(int customerId);
        IList<ListCases> ListToShowOnCaseSummaryPage(int? customerId, int? languageId, int? userGroupId);
        IList<ListCases> ListToShowOnCustomerSettingSummaryPage(int? customerId, int? languageId, int? userGroupId);


        IList<CaseFieldSetting> GetCaseFieldSettingsForDefaultCust();
        IList<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguagesForDefaultCust(int languageId);

        CaseFieldSettingLanguage GetCaseFieldSettingLanguage(int id, int languageId);

        void SaveFieldSettingsDefaultValue(int fieldId, string defaultValue);
    }

    public class CaseFieldSettingService : ICaseFieldSettingService
    {
        private readonly ICaseFieldSettingRepository _caseFieldSettingRepository;
        private readonly ICaseFieldSettingLanguageRepository _caseFieldSettingLanguageRepository;

        public CaseFieldSettingService(
            ICaseFieldSettingRepository caseFieldSettingRepository,
            ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository)
        {
            _caseFieldSettingRepository = caseFieldSettingRepository;
            _caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository;
        }

        public IList<CaseListToCase> ListToShowOnCasePage(int customerId, int languageId)
        {
            return _caseFieldSettingRepository.GetListToCustomerCase(customerId, languageId).ToList();
        }

        public IList<CaseFieldSetting> GetAllCaseFieldSettings()
        {
            return _caseFieldSettingRepository.GetAll().ToList();
        }

        public CaseFieldSetting GetCaseFieldSetting(int customerId, string fieldId)
        {
            return _caseFieldSettingRepository.GetMany(f => f.Customer_Id == customerId && f.Name == fieldId).FirstOrDefault();
        }

        /// <summary>
        /// Note: only gets cfs.ShowOnStartPage == 1 from repo
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IList<CaseFieldSetting> GetCaseFieldSettings(int customerId, int? languageId = null)
        {
            return _caseFieldSettingRepository.GetCustomerCaseFieldSettings(customerId, languageId).ToList();
        }

        public async Task<IList<CaseFieldSetting>> GetCaseFieldSettingsAsync(int customerId, int? languageId = null)
        {
            return await _caseFieldSettingRepository.GetCustomerCaseFieldSettings(customerId, languageId).AsQueryable().ToListAsync();
        }

        public IList<CaseFieldSetting> GetCustomerEnabledCaseFieldSettings(int customerId)
        {
            return _caseFieldSettingRepository.GetMany(x => x.Customer_Id == customerId && x.ShowOnStartPage == 1).ToList();
        }
        
        public IList<CaseFieldSetting> GetCaseFieldSettingsForDefaultCust()
        {
            var list = _caseFieldSettingRepository.GetAll().Where(x => x.Customer_Id == null).ToList();
            //Todo
            //DataContext.Database.SqlQuery<CaseFieldSetting>("SELECT * FROM CaseFieldSettings WHERE Customer_Id IS NULL").ToList();

            return list;
        
        }

        public IList<CaseFieldSettingLanguage> GetCaseFieldSettingLanguages()
        {
            return _caseFieldSettingLanguageRepository.GetAll().ToList();
        }

        public IList<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguages(int? customerId, int languageId)
        {
            return _caseFieldSettingLanguageRepository.GetCaseFieldSettingsWithLanguages(customerId, languageId)
                .Where(c => !c.Name.ToLower().Contains("isabout_") && !c.Name.ToLower().Contains("addfollowersbtn") && !c.Name.ToLower().Contains("adduserbtn"))
                .ToList();
        }

        public IList<CaseFieldSettingsWithLanguage> GetAllCaseFieldSettingsWithLanguages(int? customerId, int languageId)
        {
            return _caseFieldSettingLanguageRepository.GetAllCaseFieldSettingsWithLanguages(customerId, languageId).ToList();
        }

        public IList<CaseFieldSettingsWithLanguage> GetCaseFieldSettingsWithLanguagesForDefaultCust(int languageId)
        {
            return _caseFieldSettingLanguageRepository.GetCaseFieldSettingsWithLanguagesForDefaultCust(languageId).ToList();
        }

        public CaseFieldSettingLanguage GetCaseFieldSettingLanguage(int id, int languageId)
        {
            return _caseFieldSettingLanguageRepository.Get(x => x.CaseFieldSetting.Id == id && x.Language_Id == languageId);
        }

        public IList<CaseFieldSettingsForTranslation> GetCustomerCaseTranslations(int customerId)
        {
            return _caseFieldSettingLanguageRepository.GetCustomerCaseFieldSettingsForTranslation(customerId).ToList();
        }

        public async Task<IList<CaseFieldSettingsForTranslation>> GetCustomerCaseTranslationsAsync(int customerId)
        {
            return await _caseFieldSettingLanguageRepository.GetCustomerCaseFieldSettingsForTranslation(customerId).AsQueryable().ToListAsync();
        }

        public IList<ListCases> ListToShowOnCaseSummaryPage(int? customerId, int? languageId, int? userGroupId)
        {
            return _caseFieldSettingRepository.GetListCasesToCaseSummary(customerId, languageId, userGroupId).ToList();
        }

        public IList<ListCases> ListToShowOnCustomerSettingSummaryPage(int? customerId, int? languageId, int? userGroupId)
        {
            return _caseFieldSettingRepository.GetCaseFieldSettingsListToCustomerCaseSummary(customerId, languageId, userGroupId).ToList();
        }

        public void SaveFieldSettingsDefaultValue(int fieldId, string defaultValue)
        {
            var fieldSettings = _caseFieldSettingRepository.GetById(fieldId);
            if (fieldSettings != null)
                fieldSettings.DefaultValue = defaultValue;

            _caseFieldSettingRepository.Update(fieldSettings);
            _caseFieldSettingRepository.Commit();
        }

        public IList<CaseFieldSetting> GetCaseFieldSettingsByName(int customerId, string name)
        {
            return _caseFieldSettingRepository
                .GetMany(x => x.Name == name && x.Customer_Id == customerId).ToList();
        }
    }
}

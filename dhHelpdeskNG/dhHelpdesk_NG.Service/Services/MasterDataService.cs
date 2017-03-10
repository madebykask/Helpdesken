﻿using System.Collections.Generic;
using System.Linq;
namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.ADFS.Input;
    using DH.Helpdesk.Dal.Repositories.ADFS;
    using System;
    using BusinessData.Models.Notifiers;
    using Dal.Repositories.Notifiers;

    public interface IMasterDataService
    {
        IList<Customer> GetCustomers(int userId);
        Customer GetCustomer(int customerId);
        User GetUser(int userId);
        Setting GetCustomerSetting(int customerId);
        IList<Language> GetLanguages();
        IList<Text> GetTranslationTexts();
        IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int userId);
        IList<CaseFieldSettingsForTranslation> GetCustomerCaseTranslations(int customerId);
        IList<CaseFieldSettingsForTranslation> GetCaseTranslations();
        Language GetLanguage(int id);
        UserOverview GetUserForLogin(string userid);
        void ClearCache();
        void SaveSSOLog(NewSSOLog SSOLog);
        void SaveADFSSetting(ADFSSetting adfsSetting);
        ADFSSetting GetADFSSetting();
        IList<GlobalSetting> GetGlobalSettings();
        int? GetCustomerIdByEMailGUID(Guid GUID);
        string GetFilePath(int customerId);
        string GetVirtualDirectoryPath(int customerId);
        Notifier GetInitiatorByUserId(string userId, int customerId, bool activeOnly = true);
    }

    public class MasterDataService : IMasterDataService
    {
        private readonly ICaseFieldSettingLanguageRepository _caseFieldSettingLanguageRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly ITextRepository _textRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICacheProvider _cache;
        private readonly IADFSRepository _adfsRepository;
        private readonly IGlobalSettingRepository _globalSettingRepository;
        private readonly INotifierRepository _computerUserRepository;

        public MasterDataService(
            ICustomerRepository customerRepository,
            ILanguageRepository languageRepository,
            ISettingRepository settingRepository,
            ITextRepository textRepository,
            IUserRepository userRepository,
            ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository,
            ICacheProvider cache,
            IGlobalSettingRepository globalSettingRepository,
            IADFSRepository adfsRepository,
            INotifierRepository computerUserRepository)
        {
            this._customerRepository = customerRepository;
            this._languageRepository = languageRepository;
            this._settingRepository = settingRepository; 
            this._textRepository = textRepository;
            this._userRepository = userRepository;
            this._caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository; 
            this._cache = cache;
            this._adfsRepository = adfsRepository;
            this._globalSettingRepository = globalSettingRepository;
            _computerUserRepository = computerUserRepository;
        }

        public IList<Customer> GetCustomers(int userId)
        {
            return this._customerRepository.CustomersForUser(userId);
        }

        public int? GetCustomerIdByEMailGUID(Guid GUID)
        {
            return this._customerRepository.GetCustomerIdByEMailGUID(GUID);
        }       

        public Customer GetCustomer(int customerId)
        {
            return this._customerRepository.GetById(customerId);  
        }

        public User GetUser(int userId)
        {
            return this._userRepository.GetById(userId); 
        }

        public Setting GetCustomerSetting(int customerId)
        {
            return this._settingRepository.GetCustomerSetting(customerId);  
        }

        public IList<GlobalSetting> GetGlobalSettings()
        {
            return this._globalSettingRepository.GetAll().ToList();
        }

        public IList<Language> GetLanguages()
        {
            IList<Language> languages = this._cache.Get("language") as IList<Language>;

            if(languages == null)
            {
                languages = this._languageRepository.GetAll().ToList();

                if(languages.Any())
                    this._cache.Set("language", languages, 60);
            }

            return languages;
        }

        public Notifier GetInitiatorByUserId(string userId, int customerId, bool activeOnly = true)
        {
            return _computerUserRepository.GetInitiatorByUserId(userId, customerId, activeOnly);
        }

        public IList<Text> GetTranslationTexts()
        {
            return _textRepository.GetAllWithTranslation().ToList();
        }

        public IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int userId)
        {
            return this._caseFieldSettingLanguageRepository.GetCaseFieldSettingsForTranslation(userId).ToList();   
        }

        public IList<CaseFieldSettingsForTranslation> GetCustomerCaseTranslations(int customerId)
        {
            return this._caseFieldSettingLanguageRepository.GetCustomerCaseFieldSettingsForTranslation(customerId).ToList();
        }

        public IList<CaseFieldSettingsForTranslation> GetCaseTranslations()
        {
            return this._caseFieldSettingLanguageRepository.GetCaseFieldSettingsForTranslation().ToList();         
        }

        public Language GetLanguage(int id)
        {
            return this._languageRepository.GetById(id);
        }

        public UserOverview GetUserForLogin(string userid)
        {
            return this._userRepository.GetUserByLogin(userid);
        }

        public void ClearCache()
        {
            this._cache.Invalidate("text");
            this._cache.Invalidate("language");
            this._cache.Invalidate("casetranslation");
        }

        public void SaveSSOLog(NewSSOLog SSOLog)
        {
            this._adfsRepository.SaveSSOLog(SSOLog);
            this._adfsRepository.Commit();
        }

        public void SaveADFSSetting(ADFSSetting adfsSetting)
        {
            this._adfsRepository.SaveADFSSetting(adfsSetting);
            this._adfsRepository.Commit();
        }

        public ADFSSetting GetADFSSetting()
        {
            return this._adfsRepository.GetADFSSetting();
        }

        public string GetFilePath(int customerId)
        {            
            var customerFilePath = this._settingRepository.GetAll()
                                                          .Where(s => s.Customer_Id == customerId)
                                                          .Select(s => s.PhysicalFilePath)
                                                          .FirstOrDefault();
            if (string.IsNullOrEmpty(customerFilePath))
            {
                var globalSetting = this._globalSettingRepository.GetAll().FirstOrDefault();
                if (globalSetting != null)
                    customerFilePath = globalSetting.AttachedFileFolder;
            }

            return (string.IsNullOrEmpty(customerFilePath)? string.Empty: customerFilePath);
        }

        public string GetVirtualDirectoryPath(int customerId)
        {
            var customerFilePath = this._settingRepository.GetAll()
                                                          .Where(s => s.Customer_Id == customerId)
                                                          .Select(s => s.VirtualFilePath)
                                                          .FirstOrDefault();
            if (string.IsNullOrEmpty(customerFilePath)) {
                var globalSetting = this._globalSettingRepository.GetAll().FirstOrDefault();
                if (globalSetting != null)
                    customerFilePath = globalSetting.VirtualFileFolder;
            }
            return (string.IsNullOrEmpty(customerFilePath) ? string.Empty : customerFilePath);
        }

    }
}

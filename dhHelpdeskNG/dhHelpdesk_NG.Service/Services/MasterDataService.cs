using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models.Customer.Input;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Settings;

namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.ADFS.Input;
    using DH.Helpdesk.Dal.Repositories.ADFS;
    using BusinessData.Models.Notifiers;
    using Dal.Repositories.Notifiers;
    using EmployeeService;
    using BusinessData.Models.Employee;
    using BusinessData.Models.WebApi;
    using BusinessData.Models.LogProgram;
    using Concrete;

    public interface IMasterDataService
    {
        IList<CustomerOverview> GetCustomers(int userId);
        Customer GetCustomer(int customerId);
        Setting GetCustomerSettings(int customerId);
        bool IsCustomerUser(int customerId, int userId);
        User GetUser(int userId);
        IList<Language> GetLanguages();
        IList<Text> GetTranslationTexts();
        IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int userId);
        IList<CaseFieldSettingsForTranslation> GetCustomerCaseTranslations(int customerId);
        IList<CaseFieldSettingsForTranslation> GetCaseTranslations();
        Language GetLanguage(int id);
        UserOverview GetUserForLogin(string userid, int? customerId = null);
        void ClearCache();
        void SaveSSOLog(NewSSOLog SSOLog);
        void SaveADFSSetting(ADFSSetting adfsSetting);
        ADFSSetting GetADFSSetting();
        IList<GlobalSetting> GetGlobalSettings();
        int? GetCustomerIdByEMailGUID(Guid GUID);
        string GetFilePath(int customerId);
        string GetVirtualDirectoryPath(int customerId);
        Notifier GetInitiatorByUserId(string userId, int customerId, bool activeOnly = true);
        EmployeeModel GetEmployee(int customerId, string employeeNumber, bool useApi = false, WebApiCredentialModel credentialModel = null);
        void UpdateUserLogin(LogProgram logProgram);
        IList<UserPermission> GetUserPermissions(int userId);
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
        private readonly ICustomerUserRepository _customerUserRepository;
        private readonly IEmployeeService _employeeService;
        private readonly ILogProgramService _logProgramService;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly ISettingsLogic _settingsLogic;
        

        private Dictionary<int, Setting> _customersSettingsCache = new Dictionary<int, Setting>();

        public MasterDataService(
            ICustomerRepository customerRepository,
            ILanguageRepository languageRepository,
            ISettingRepository settingRepository, //todo
            ITextRepository textRepository,
            IUserRepository userRepository,
            ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository,
            ICacheProvider cache,
            IGlobalSettingRepository globalSettingRepository,
            IADFSRepository adfsRepository,
            INotifierRepository computerUserRepository,
            ICustomerUserRepository customerUserRepository,
            IEmployeeService employeeService,
            ILogProgramService logProgramService,
            IUserPermissionsChecker userPermissionsChecker,
            ISettingsLogic settingsLogic)
        {
            _customerRepository = customerRepository;
            _languageRepository = languageRepository;
            _settingRepository = settingRepository; 
            _textRepository = textRepository;
            _userRepository = userRepository;
            _caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository; 
            _cache = cache;
            _adfsRepository = adfsRepository;
            _globalSettingRepository = globalSettingRepository;
            _computerUserRepository = computerUserRepository;
            _customerUserRepository = customerUserRepository;
            _employeeService = employeeService;
            _logProgramService = logProgramService;
            _userPermissionsChecker = userPermissionsChecker;
            _settingsLogic = settingsLogic;
        }

        public IList<CustomerOverview> GetCustomers(int userId)
        {
            var items =
                this._customerRepository.CustomersForUser(userId)
                    .Select(cus => new CustomerOverview
                    {
                        Id = cus.Id,
                        Name = cus.Name
                    }).ToList();

            return items;
        }

        public int? GetCustomerIdByEMailGUID(Guid GUID)
        {
            return this._customerRepository.GetCustomerIdByEMailGUID(GUID);
        }       

        public Customer GetCustomer(int customerId)
        {
            return this._customerRepository.GetById(customerId);  
        }

        public bool IsCustomerUser(int customerId, int userId)
        {
            return this._customerUserRepository.IsCustomerUser(customerId, userId);
        }

        public User GetUser(int userId)
        {
            return this._userRepository.GetById(userId); 
        }

        public Setting GetCustomerSettings(int customerId)
        {
            // store settings to be used during request processing
            if (!_customersSettingsCache.ContainsKey(customerId))
            {
                var settings = this._settingRepository.GetCustomerSetting(customerId);
                _customersSettingsCache.Add(customerId, settings);
            }

            return _customersSettingsCache[customerId];
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

        // TODO: review how it is used. Now it is put for every user in a session - potential memory leak on high load. Use cache instead
        public IList<Text> GetTranslationTexts()
        {
            return _textRepository.GetAllWithTranslation().ToList();
        }

        // TODO: review how it is used. Now it is put for every user in a session - potential memory leak on high load. Use cache instead
        public IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int userId)
        {
            return this._caseFieldSettingLanguageRepository.GetCaseFieldSettingsForTranslation(userId).ToList();   
        }

        public IList<CaseFieldSettingsForTranslation> GetCustomerCaseTranslations(int customerId)
        {
            return this._caseFieldSettingLanguageRepository.GetCustomerCaseFieldSettingsForTranslation(customerId).ToList();
        }

        // TODO: review how it is used. Now it is put for every user in a session - potential memory leak on high load. Use cache instead
        public IList<CaseFieldSettingsForTranslation> GetCaseTranslations()
        {
            return this._caseFieldSettingLanguageRepository.GetCaseFieldSettingsForTranslation().ToList();         
        }

        public Language GetLanguage(int id)
        {
            return this._languageRepository.GetById(id);
        }

        public UserOverview GetUserForLogin(string userid, int? customerId = null)
        {
            return this._userRepository.GetUserByLogin(userid, customerId);
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
            return _settingsLogic.GetFilePath(customerId);
        }

        public string GetVirtualDirectoryPath(int customerId)
        {
            return _settingsLogic.GetVirtualDirectoryPath(customerId);
        }

        public EmployeeModel GetEmployee(int customerId, string employeeNumber, bool useApi = false, WebApiCredentialModel credentialModel = null)
        {            
            return _employeeService.GetEmployee(customerId, employeeNumber, useApi, credentialModel);
        }

        public void UpdateUserLogin(LogProgram logProgram)
        {
            _logProgramService.UpdateUserLogin(logProgram);
        }

        public IList<UserPermission> GetUserPermissions(int userId)
        {
            var user = GetUser(userId);
            return _userPermissionsChecker.GetUserPermissions(user);
        }
    }
}

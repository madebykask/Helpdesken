namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.ADFS.Input;
    using DH.Helpdesk.Dal.Repositories.ADFS;

    public interface IMasterDataService
    {
        IList<Customer> GetCustomers(int userId);
        Customer GetCustomer(int customerId);
        User GetUser(int userId);
        Setting GetCustomerSetting(int customerId);
        IList<Language> GetLanguages();
        IList<Text> GetTranslationTexts();
        IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int userId);
        IList<CaseFieldSettingsForTranslation> GetCaseTranslations();
        Language GetLanguage(int id);
        UserOverview GetUserForLogin(string userid);
        void ClearCache();
        void SaveSSOLog(NewSSOLog SSOLog);
        void SaveADFSSetting(ADFSSetting adfsSetting);
        ADFSSetting GetADFSSetting();
        
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

        public MasterDataService(
            ICustomerRepository customerRepository,
            ILanguageRepository languageRepository,
            ISettingRepository settingRepository,
            ITextRepository textRepository,
            IUserRepository userRepository,
            ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository,
            ICacheProvider cache,
            IADFSRepository adfsRepository)
        {
            this._customerRepository = customerRepository;
            this._languageRepository = languageRepository;
            this._settingRepository = settingRepository; 
            this._textRepository = textRepository;
            this._userRepository = userRepository;
            this._caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository; 
            this._cache = cache;
            this._adfsRepository = adfsRepository;
        }

        public IList<Customer> GetCustomers(int userId)
        {
            return this._customerRepository.CustomersForUser(userId);
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

        public IList<Text> GetTranslationTexts()
        {
            IList<Text> texts = this._cache.Get("text") as IList<Text>;

            if (texts == null)
            {
                texts = this._textRepository.GetAllWithTranslation().ToList();

                if (texts.Any())
                    this._cache.Set("text", texts, 60);
            }

            return texts;
        }

        public IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int userId)
        {
            return this._caseFieldSettingLanguageRepository.GetCaseFieldSettingsForTranslation(userId).ToList();   
            //IList<CaseFieldSettingsForTranslation> languages = this._cache.Get("casetranslation") as IList<CaseFieldSettingsForTranslation>;

            //if (languages == null)
            //{
            //    languages = this._caseFieldSettingLanguageRepository.GetCaseFieldSettingsForTranslation(userId).ToList();   

            //    if (languages.Any())
            //        this._cache.Set("casetranslation", languages, 60);
            //}

            //return languages;
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
    }
}

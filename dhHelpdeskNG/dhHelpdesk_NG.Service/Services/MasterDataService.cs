namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IMasterDataService
    {
        IList<Customer> GetCustomers(int userId);
        Customer GetCustomer(int customerId);
        IList<Language> GetLanguages();
        IList<Text> GetTranslationTexts();
        IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int userId);
        Language GetLanguage(int id);
        UserOverview GetUserForLogin(string userid);
        void ClearCache();
    }

    public class MasterDataService : IMasterDataService
    {
        private readonly ICaseFieldSettingLanguageRepository _caseFieldSettingLanguageRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ITextRepository _textRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICacheProvider _cache;

        public MasterDataService(
            ICustomerRepository customerRepository,
            ILanguageRepository languageRepository,
            ITextRepository textRepository,
            IUserRepository userRepository,
            ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository,
            ICacheProvider cache)
        {
            this._customerRepository = customerRepository;
            this._languageRepository = languageRepository;
            this._textRepository = textRepository;
            this._userRepository = userRepository;
            this._caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository; 
            this._cache = cache;
        }

        public IList<Customer> GetCustomers(int userId)
        {
            return this._customerRepository.CustomersForUser(userId);
        }

        public Customer GetCustomer(int customerId)
        {
            return this._customerRepository.GetById(customerId);  
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
    }
}

using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;  

namespace dhHelpdesk_NG.Service
{
    using dhHelpdesk_NG.DTO.DTOs.User.Input;

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
            _customerRepository = customerRepository;
            _languageRepository = languageRepository;
            _textRepository = textRepository;
            _userRepository = userRepository;
            _caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository; 
            _cache = cache;
        }

        public IList<Customer> GetCustomers(int userId)
        {
            return _customerRepository.CustomersForUser(userId);
        }

        public Customer GetCustomer(int customerId)
        {
            return _customerRepository.GetById(customerId);  
        }

        public IList<Language> GetLanguages()
        {
            IList<Language> languages = _cache.Get("language") as IList<Language>;

            if(languages == null)
            {
                languages = _languageRepository.GetAll().ToList();

                if(languages.Any())
                    _cache.Set("language", languages, 60);
            }

            return languages;
        }

        public IList<Text> GetTranslationTexts()
        {
            IList<Text> texts = _cache.Get("text") as IList<Text>;

            if (texts == null)
            {
                texts = _textRepository.GetAllWithTranslation().ToList();

                if (texts.Any())
                    _cache.Set("text", texts, 60);
            }

            return texts;
        }

        public IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int userId)
        {
            IList<CaseFieldSettingsForTranslation> languages = _cache.Get("casetranslation") as IList<CaseFieldSettingsForTranslation>;

            if (languages == null)
            {
                languages = _caseFieldSettingLanguageRepository.GetCaseFieldSettingsForTranslation(userId).ToList();   

                if (languages.Any())
                    _cache.Set("casetranslation", languages, 60);
            }

            return languages;
        }

        public Language GetLanguage(int id)
        {
            return _languageRepository.GetById(id);
        }

        public UserOverview GetUserForLogin(string userid)
        {
            return _userRepository.GetUserByLogin(userid);
        }

        public void ClearCache()
        {
            _cache.Invalidate("text");
            _cache.Invalidate("language");
            _cache.Invalidate("casetranslation");
        }
    }
}

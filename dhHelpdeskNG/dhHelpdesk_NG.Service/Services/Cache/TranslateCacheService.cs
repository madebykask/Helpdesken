using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services.Cache
{
    public class TranslateCacheService : ITranslateCacheService
    {
        private string _caseTranslationsKey = "CaseTranslations";
        private string _caseTranslationsFormat = "{0}_{1}";
        private string _textTranslationsKey = "TextTranslations";

        private readonly ICacheService _cacheService;
        private readonly ICaseFieldSettingLanguageRepository _caseFieldSettingLanguageRepository;
        private readonly ITextRepository _textRepository;
        private readonly ITextTranslationRepository _textTranslationRepository;

        #region ctor()

        public TranslateCacheService(ICacheService cacheService, ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository,
            ITextRepository textRepository, ITextTranslationRepository textTranslationRepository)
        {
            _cacheService = cacheService;
            _caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository;
            _textRepository = textRepository;
            _textTranslationRepository = textTranslationRepository;
        }

        #endregion

        #region Public Methods

        public IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int customerId)
        {
            var cacheKey = string.Format(_caseTranslationsFormat, _caseTranslationsKey, customerId);
            return _cacheService.Get(cacheKey, () => GetCustomerCaseTranslations(customerId), DateTime.UtcNow.AddMinutes(10));
        }

        public IList<Text> GetAllTextTranslations()
        {
            return _cacheService.Get(_textTranslationsKey, GetTranslationTexts, DateTime.UtcNow.AddMinutes(10));
        }

        public IList<CustomKeyValue<string, string>> GetTextTranslationsForLanguage(int languageId, int typeId = 0) // TODO: ClearCache
        {
            var cacheKey = $"{ _caseTranslationsKey }_TranslationsForLanguage_ { languageId }_{ typeId }";
            return _cacheService.Get(cacheKey, () => _textTranslationRepository.GetTextTranslationsFor(languageId, typeId), DateTime.UtcNow.AddMinutes(10));
        }

        public string GetTextTranslation(string translate, int languageId)
        {
            var translation = GetTextTranslationsForLanguage(languageId).FirstOrDefault(x => string.Equals(x.Key, translate, StringComparison.CurrentCultureIgnoreCase));
            //if (DiffTextType) //TODO: check in old code if it is required
            //{
            //    translation = Cache.GetTextTranslations().FirstOrDefault(x => x.TextToTranslate.ToLower() == translate.ToLower() && x.Type == TextTypeId);
            //}

            if (translation == null) return translate;

            var text = translation.Value ?? string.Empty;
            return !string.IsNullOrEmpty(text) ? text : translate;
        }

        public void ClearCaseTranslations(int customerId)
        {
            _cacheService.Delete(string.Format(_caseTranslationsFormat, _caseTranslationsKey, customerId));
        }

        public void ClearAllTextTranslations()
        {
            _cacheService.Delete(_textTranslationsKey);
        }

        #endregion

        #region Private Data Methods

        private IList<CaseFieldSettingsForTranslation> GetCustomerCaseTranslations(int customerId)
        {
            return _caseFieldSettingLanguageRepository.GetCustomerCaseFieldSettingsForTranslation(customerId).ToList();
        }

        private IList<Text> GetTranslationTexts()
        {
            return _textRepository.GetAllWithTranslation().ToList();
        }

        #endregion
    }

    public interface ITranslateCacheService
    {
        IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int customerId);
        IList<CustomKeyValue<string, string>> GetTextTranslationsForLanguage(int languageId, int typeId = 0);
        void ClearCaseTranslations(int customerId);
        IList<Text> GetAllTextTranslations();
        string GetTextTranslation(string translate, int languageId);
        void ClearAllTextTranslations();
    }
}
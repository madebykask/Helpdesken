using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services.Cache
{
    public class TranslateCacheService: ITranslateCacheService
    {
        private string _caseTranslationsKey = "CaseTranslations";
        private string _caseTranslationsFormat = "{0}_{1}";
        private string _textTranslationsKey = "TextTranslations";

        private readonly ICacheService _cacheService;
        private readonly ICaseFieldSettingLanguageRepository _caseFieldSettingLanguageRepository;
        private readonly ITextRepository _textRepository;

        #region ctor()

        public TranslateCacheService()
        {
            
        }

        public TranslateCacheService(ICacheService cacheService, 
                                     ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository,
                                     ITextRepository textRepository)
        {
            _cacheService = cacheService;
            _caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository;
            _textRepository = textRepository;
        }

        #endregion

        #region Public Methods

        public IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int customerId)
        {
            var cacheKey = string.Format(_caseTranslationsFormat, _caseTranslationsKey, customerId);
            return _cacheService.Get(cacheKey, () => GetCustomerCaseTranslations(customerId), DateTime.UtcNow.AddMinutes(10));
        }

        public IList<Text> GetTextTranslations()
        {
            return _cacheService.Get(_textTranslationsKey, GetTranslationTexts, DateTime.UtcNow.AddMinutes(10));
        }

        public void ClearCaseTranslations(int customerId)
        {
            _cacheService.Delete(string.Format(_caseTranslationsFormat, _caseTranslationsKey, customerId));
        }

        public void ClearTextTranslations()
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
        void ClearCaseTranslations(int customerId);
        IList<Text> GetTextTranslations();
        void ClearTextTranslations();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure.Translate;

namespace DH.Helpdesk.WebApi.Logic.Case
{
    public interface ICaseTranslationService
    {
        string GetCaseTranslation(string translate, int languageId, int customerId);

        string GetFieldLabel(GlobalEnums.TranslationCaseFields field, int languageId, int customerId,
            IList<CaseFieldSettingsForTranslation> caseFieldSettingsForTranslations);

        string TranslateFieldLabel(int languageId, string label);
    }

    public class CaseTranslationService : ICaseTranslationService
    {
        private readonly ITranslateCacheService _translateCacheService;

        public CaseTranslationService(ITranslateCacheService translateCacheService)
        {
            _translateCacheService = translateCacheService;
        }

        public string GetCaseTranslation(string translate, int languageId, int customerId)
        {
            var translation = 
                _translateCacheService.GetCaseTranslations(customerId).FirstOrDefault(x => x.Name.ToLower() == translate.GetCaseFieldName().ToLower() && x.Language_Id == languageId);

            if (translation != null && !string.IsNullOrEmpty(translation.Label))
            {
                translate = translation.Label;
            }
            else
            {
                var translateByText = string.Empty;
                var instanceWord = GetInstanceWord(translate);
                if (!string.IsNullOrEmpty(instanceWord))
                {
                    translateByText = _translateCacheService.GetTextTranslation(instanceWord.ToLower(), languageId);
                }

                translate = 
                    string.IsNullOrEmpty(translateByText) ? translate.GetDefaultValue(languageId) : translateByText; 
            }

            return translate;
        }

        public string GetFieldLabel(GlobalEnums.TranslationCaseFields field, int languageId, int customerId, IList<CaseFieldSettingsForTranslation> caseFieldSettingsForTranslations)
        {
            var fieldName = field.ToString(); 
            var caption = FieldSettingsUiNames.Names.ContainsKey(fieldName) ? FieldSettingsUiNames.Names[fieldName] : fieldName;

            var caseFieldName = fieldName.GetCaseFieldName();
            var settingEx = caseFieldSettingsForTranslations.FirstOrDefault(x => x.Name.Equals(caseFieldName, StringComparison.OrdinalIgnoreCase)  && x.Language_Id == languageId);
            caption = !string.IsNullOrWhiteSpace(settingEx?.Label) ? settingEx.Label : TranslateFieldLabel(languageId, caption);

            return caption;
        }

        public string TranslateFieldLabel(int languageId, string label)
        {
            var translation = _translateCacheService.GetTextTranslation(label, languageId);
            var caption = !string.IsNullOrEmpty(translation) ? translation : label.GetDefaultValue(languageId);
            return caption;
        }


        //todo: move to common class. Copied from Translation.cs
        private static string GetInstanceWord(string word)
        {
            switch (word.ToLower())
            {
                case "_temporary_leadtime":
                    return "Tid kvar";

                case "ledtid":
                    return "Ledtid";
            }

            return string.Empty;
        }
    }
}
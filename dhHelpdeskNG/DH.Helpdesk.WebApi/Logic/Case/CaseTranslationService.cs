using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Web.Common.Constants.Case;

namespace DH.Helpdesk.WebApi.Logic.Case
{
    public interface ICaseTranslationService
    {
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

        public string GetFieldLabel(GlobalEnums.TranslationCaseFields field, int languageId, int customerId,
            IList<CaseFieldSettingsForTranslation> caseFieldSettingsForTranslations)
        {
            var fieldName = field.ToString();
            var caption = FieldSettingsUiNames.Names[fieldName];

            var settingEx = caseFieldSettingsForTranslations.FirstOrDefault(x => x.Name.Equals(fieldName.Replace("tblLog_", "tblLog."), StringComparison.OrdinalIgnoreCase)  && x.Language_Id == languageId);
            if (!string.IsNullOrWhiteSpace(settingEx?.Label))
            {
                caption = settingEx.Label;
            }
            else
            {
                //var instanceWord = Translation.GetInstanceWord(translate); // todo: check if required - see Translation.cs\CaseTranslation
                //if (!string.IsNullOrEmpty(instanceWord))
                caption = TranslateFieldLabel(languageId, caption);
            }

            return caption;
        }

        public string TranslateFieldLabel(int languageId, string label)
        {
            var translation = _translateCacheService.GetTextTranslation(label, languageId);
            var caption = !string.IsNullOrEmpty(translation) ? translation : label.GetDefaultValue(languageId);
            return caption;
        }
    }
}
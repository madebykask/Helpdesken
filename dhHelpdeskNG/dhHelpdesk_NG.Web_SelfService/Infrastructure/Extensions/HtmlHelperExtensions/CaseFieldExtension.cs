using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.SelfService.Infrastructure.Extensions.HtmlHelperExtensions
{
    public static class CaseFieldExtension
    {
        public static MvcHtmlString CaseFieldCaption(
                                this HtmlHelper html,
                                IList<CaseFieldSetting> settings,
                                IEnumerable<CaseFieldSettingsWithLanguage> settingsEx,
                                GlobalEnums.TranslationCaseFields field,
                                int customerId,
                                int languageId = 0,
                                bool withMandatory = true)
        {
            var caption = languageId == 0
                ? Translation.Get(field.ToString(), Enums.TranslationSource.CaseTranslation, customerId)
                : Translation.Get(field.ToString(), languageId, Enums.TranslationSource.CaseTranslation, customerId);

            var setting = settings.getCaseSettingsValue(field.ToString());
            var description = settingsEx.getFieldHelp(field.ToString());

            var result = new StringBuilder();
            var tag = new TagBuilder("span");
            if (!string.IsNullOrEmpty(description))
            {
                tag.MergeAttribute("title", description);
            }

            tag.SetInnerText(caption);
            result.Append(tag);

            if (withMandatory && setting.Required.ToBool())
            {
                tag = new TagBuilder("span");
                tag.MergeAttribute("class", "mandatorystar");
                tag.SetInnerText(" * ");
                result.Append(tag);
            }

            if (!string.IsNullOrEmpty(description))
            {
                tag = new TagBuilder("span");
                tag.MergeAttribute("class", "glyphicon glyphicon-info-sign ml15");
                tag.MergeAttribute("rel", "tooltip");
                tag.MergeAttribute("title", description);
                result.Append(tag);
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
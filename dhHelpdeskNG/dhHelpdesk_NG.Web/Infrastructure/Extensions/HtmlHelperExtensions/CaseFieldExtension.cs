namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;

    public static class CaseFieldExtension
    {
        public static MvcHtmlString CaseFieldCaption(
                                this HtmlHelper html,
                                IList<CaseFieldSetting> settings,
                                IEnumerable<CaseFieldSettingsWithLanguage> settingsEx,
                                GlobalEnums.TranslationCaseFields field,
                                int customerId)
        {
            var caption = Translation.Get(field.ToString(), Enums.TranslationSource.CaseTranslation, customerId);
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

            if (setting.Required.ToBool())
            {
                tag = new TagBuilder("span");
                tag.MergeAttribute("class", "mandatorystar");
                tag.SetInnerText(" * ");
                result.Append(tag);
            }

            return MvcHtmlString.Create(result.ToString());            
        }
    }
}
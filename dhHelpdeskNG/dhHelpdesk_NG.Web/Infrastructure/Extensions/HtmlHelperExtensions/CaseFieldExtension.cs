namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Collections.Generic;
    using System.Security.Policy;
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
            var url = new UrlHelper(html.ViewContext.RequestContext);

            var result = new StringBuilder();
            result.Append(caption);

            if (setting.Required.ToBool())
            {
                var tag = new TagBuilder("span");
                tag.MergeAttribute("class", "mandatorystar");
                tag.SetInnerText(" * ");
                result.Append(tag);
            }

            if (!string.IsNullOrEmpty(description))
            {
                var tag = new TagBuilder("img");
                tag.MergeAttribute("src", url.Content("~/Content/icons/info.png"));
                tag.MergeAttribute("class", "cursor-pointer ml05");
                tag.MergeAttribute("data-case-field-caption", caption);
                tag.MergeAttribute("data-case-field-description", description);
                result.Append(tag);
            }

            return MvcHtmlString.Create(result.ToString());            
        }
    }
}
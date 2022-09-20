namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
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
                                int customerId,
                                bool isCaseReopened,
                                int languageId = 0,
                                string defaultCaption = "")
        {
            var caption = "";

#pragma warning disable 0618
            if (languageId == 0)
                caption = Translation.Get(field.ToString(), Enums.TranslationSource.CaseTranslation, customerId);
            else
                caption = Translation.Get(field.ToString(), languageId, Enums.TranslationSource.CaseTranslation, customerId);

            if (string.IsNullOrEmpty(caption) && defaultCaption != "")
                caption = Translation.Get(defaultCaption, languageId, Enums.TranslationSource.TextTranslation, customerId);
#pragma warning restore 0618

            var setting = settings.getCaseSettingsValue(field.ToString());
            var description = settingsEx.getFieldHelp(field.ToString());
            var url = new UrlHelper(html.ViewContext.RequestContext);

            var result = new StringBuilder();
            var tag = new TagBuilder("span");
            if (!string.IsNullOrEmpty(description))
            {
                tag.MergeAttribute("title", description);
            }

            tag.SetInnerText(caption);
            result.Append(tag);

            if (setting.Required.ToBool() || (isCaseReopened && setting.RequiredIfReopened != 0))
            {
                tag = new TagBuilder("span");
                tag.MergeAttribute("class", "mandatorystar");
                tag.SetInnerText(" * ");
                result.Append(tag);
            }

            if (!string.IsNullOrEmpty(description))
            {
                //tag = new TagBuilder("img");
                //tag.MergeAttribute("src", url.Content("~/Content/icons/info.png"));
                //tag.MergeAttribute("class", "cursor-pointer ml05");
                tag = new TagBuilder("span");
                tag.MergeAttribute("class", "icon-info-sign ml15");
                tag.MergeAttribute("rel", "tooltip");
                tag.MergeAttribute("title", description);
                result.Append(tag);
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
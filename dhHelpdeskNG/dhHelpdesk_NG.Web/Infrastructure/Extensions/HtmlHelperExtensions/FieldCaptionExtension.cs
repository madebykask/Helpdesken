using System.Web.Mvc;
using DH.Helpdesk.Web.Models;

namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    public static class FieldCaptionExtension
    {
        public static MvcHtmlString FieldCaption(this HtmlHelper html, IConfigurableFieldModel field)
        {
            var fieldCaption = Translation.GetCoreTextTranslation(field.Caption);
            var s = field.IsRequired ? $"{fieldCaption} *" : fieldCaption;
            return MvcHtmlString.Create(s); 
        }

        public static MvcHtmlString PureFieldCaption(this HtmlHelper html, IConfigurableFieldModel field)
        {
            var result = Translation.GetCoreTextTranslation(field.Caption);
            return MvcHtmlString.Create(result);
        }
    }
}
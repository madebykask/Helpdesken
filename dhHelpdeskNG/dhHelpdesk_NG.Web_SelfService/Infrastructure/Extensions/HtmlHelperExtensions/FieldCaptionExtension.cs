using System.Text;
using System.Web.Mvc;
using DH.Helpdesk.SelfService.Models;

namespace DH.Helpdesk.SelfService.Infrastructure.Extensions.HtmlHelperExtensions
{
    public static class FieldCaptionExtension
    {
        public static MvcHtmlString FieldCaption(
                        this HtmlHelper html,
                        IConfigurableFieldModel field)
        {
            var result = new StringBuilder();
            result.Append(Translation.Get(field.Caption));
            if (field.IsRequired)
            {
                result.Append(" *");
            }

            return MvcHtmlString.Create(result.ToString()); 
        }

        public static MvcHtmlString PureFieldCaption(
                        this HtmlHelper html,
                        IConfigurableFieldModel field)
        {
            var result = new StringBuilder();
            result.Append(Translation.Get(field.Caption));           
            return MvcHtmlString.Create(result.ToString());
        }
    }
}
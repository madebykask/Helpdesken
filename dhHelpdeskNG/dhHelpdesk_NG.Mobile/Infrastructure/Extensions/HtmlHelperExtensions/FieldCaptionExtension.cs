namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Text;
    using System.Web.Mvc;

    using DH.Helpdesk.Web.Models;

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
    }
}
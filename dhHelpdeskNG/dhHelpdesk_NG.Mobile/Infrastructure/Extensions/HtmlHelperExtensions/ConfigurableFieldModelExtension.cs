namespace DH.Helpdesk.Mobile.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit;

    public static class ConfigurableFieldModelExtension
    {
        public static MvcHtmlString ConfigurableFieldModelTextBoxFor<TModel, TValue>(
                                                    this HtmlHelper<TModel> html, 
                                                    Expression<Func<TModel, TValue>> expression,
                                                    ConfigurableFieldModel<string> configurableField,
                                                    object htmlAttributes)
        {
            var dict = new RouteValueDictionary(htmlAttributes);
            return html.ConfigurableFieldModelTextBoxFor(expression, configurableField, dict);
        }

        public static MvcHtmlString ConfigurableFieldModelTextBoxFor<TModel, TValue>(
                                                    this HtmlHelper<TModel> html, 
                                                    Expression<Func<TModel, TValue>> expression,
                                                    ConfigurableFieldModel<string> configurableField)
        {
            var htmlAttributes = new Dictionary<string, object>();
            return html.ConfigurableFieldModelTextBoxFor(expression, configurableField, htmlAttributes);
        }

        public static MvcHtmlString ConfigurableFieldModelTextBoxFor<TModel, TValue>(
                                                    this HtmlHelper<TModel> html, 
                                                    Expression<Func<TModel, TValue>> expression, 
                                                    ConfigurableFieldModel<string> configurableField,
                                                    IDictionary<string, object> htmlAttributes)
        {
            if (configurableField.IsRequired)
            {
                htmlAttributes.Add("data-val", "true");
                htmlAttributes.Add("data-val-required", string.Format(@"The field ""{0}"" is required.", configurableField.Caption));
            }

            return html.TextBoxFor(expression, htmlAttributes);          
        }
    }
}
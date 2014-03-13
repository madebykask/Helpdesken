namespace DH.Helpdesk.Web.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public abstract class ConditionalValidationAttribute : ValidationAttribute
    {
        protected TValue GetInstancePropertyValue<TValue>(ControllerContext context, string propertyName)
        {
            var viewContext = (ViewContext)context;
            var sourceObject = viewContext.ViewData.TemplateInfo.FormattedModelValue;
            var property = sourceObject.GetType().GetProperty(propertyName);
            return (TValue)property.GetValue(sourceObject, null);
        }
    }
}
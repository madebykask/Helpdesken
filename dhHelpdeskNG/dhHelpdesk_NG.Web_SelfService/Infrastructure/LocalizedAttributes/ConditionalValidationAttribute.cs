namespace DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Exceptions;
    using DH.Helpdesk.Common.Tools;

    public abstract class ConditionalValidationAttribute : ValidationAttribute
    {
        #region Methods

        protected TValue GetInstancePropertyValue<TValue>(ControllerContext context, string propertyName)
        {
            var viewContext = (ViewContext)context;

            var visitedObjects =
                ReflectionHelper.GetInstancePropertyValue<HashSet<object>>(
                    viewContext.ViewData.TemplateInfo,
                    "VisitedObjects");

            for (var i = visitedObjects.Count - 1; i >= 0; i--)
            {
                var visitedObject = visitedObjects.ElementAt(i);

                if (ReflectionHelper.InstanceHasProperty(visitedObject, propertyName))
                {
                    return ReflectionHelper.GetInstancePropertyValue<TValue>(visitedObject, propertyName);
                }
            }

            throw new PropertyNotFoundException("Property with specified name not found.", propertyName);
        }

        #endregion
    }
}
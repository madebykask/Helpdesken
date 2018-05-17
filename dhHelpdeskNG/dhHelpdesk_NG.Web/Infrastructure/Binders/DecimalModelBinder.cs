using System;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Infrastructure.Binders
{
    internal sealed class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState
            {
                Value = valueResult
            };

            object actualValue = null;
            try
            {
                actualValue = BinderHelper.ParseDecimal(valueResult);
            }
            catch (Exception ex)
            {
                modelState.Errors.Add(ex);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}
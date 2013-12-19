using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dhHelpdesk_NG.Web.Infrastructure
{

    public class StringModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string modelName = bindingContext.ModelName;
            string attemptedValue = bindingContext.ValueProvider.GetValue(modelName).AttemptedValue;

            if (attemptedValue == string.Empty)
                return "";

            return attemptedValue;
        }
    }
 
}
namespace DH.Helpdesk.Web.Infrastructure
{
    using System.Web.Mvc;

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
namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public static class ModelExtensions
    {
        public static IEnumerable<ModelError> GetErrors(this ModelStateDictionary modelState)
        {
            var modelErrors = new List<ModelError>();

            foreach (var key in modelState.Keys)
            {
                var value = modelState[key];
                if (value.Errors.Any())
                {
                    modelErrors.Add(new ModelError(key, value.Value?.AttemptedValue, value.Errors.Select(e => e.ErrorMessage)));
                }
            }

            return modelErrors;
        }
    }
}
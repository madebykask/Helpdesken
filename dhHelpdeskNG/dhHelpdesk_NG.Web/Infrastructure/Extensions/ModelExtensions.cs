namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using BusinessData.Models.Shared.Input;
    using Newtonsoft.Json;
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

        public static string ToJson<T>(this T model) where T: INewBusinessModel
        {
            if (model == null)
                return string.Empty;

            return JsonConvert.SerializeObject(model);            
        }

    }

}
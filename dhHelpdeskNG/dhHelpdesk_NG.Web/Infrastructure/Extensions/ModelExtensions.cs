using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    

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

        public static string GetErrorsText(this ModelStateDictionary modelStateErrors)
        {
            //error example: {"Message":"The request is invalid.","ModelState":{"saveData.FormId":["Unexpected character encountered while parsing value: t. Path 'FormId', line 1, position 80."]}}
            var strBld = new StringBuilder();
            strBld.AppendLine("Invalid model: ");

            foreach (var kv in modelStateErrors)
            {
                var errors = string.Join(",", kv.Value.Errors.SelectMany(x => x.ErrorMessage).ToArray());
                strBld.AppendFormat("{0}: {1};", kv.Key, errors).AppendLine();
            }

            return strBld.ToString();
        }

        public static string Serialize(this ProcessResult processRes)
        {
            return (processRes == null) ? string.Empty : JsonConvert.SerializeObject(processRes);
        }
    }
}
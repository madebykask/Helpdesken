using System.Linq;
using System.Text;
using System.Web.Http.ModelBinding;

namespace DH.Helpdesk.WebApi.Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        public static string BuildModelStateErrorSummary(this ModelStateDictionary stateDictionary, bool includeStackTrace = true)
        {
            //error example: {"Message":"The request is invalid.","ModelState":{"saveData.FormId":["Unexpected character encountered while parsing value: t. Path 'FormId', line 1, position 80."]}}
            var strBld = new StringBuilder();
            strBld.AppendLine("Invalid model: ");

            foreach (var kv in stateDictionary)
            {
                var errors = string.Join(",", kv.Value.Errors.Select(x => BuildErrorMessage(x, includeStackTrace)).ToArray());
                strBld.AppendFormat("{0}: {1};", kv.Key, errors).AppendLine();
            }

            return strBld.ToString();
        }

        private static string BuildErrorMessage(this ModelError modelError, bool includeStackTrace)
        {
            return
                includeStackTrace
                    ? $"Error: {modelError.ErrorMessage ?? ""}, Exception: {modelError.Exception?.ToString() ?? ""}"
                    : $"Error: {modelError.ErrorMessage ?? ""}";
        }
    }
}
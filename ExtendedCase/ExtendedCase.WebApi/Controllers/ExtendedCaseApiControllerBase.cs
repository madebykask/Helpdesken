using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ExtendedCase.WebApi.Controllers
{
    public class ExtendedCaseApiControllerBase : ApiController
    {
        protected string BuildModelStateErrorSummary(ModelStateDictionary stateDictionary)
        {
            //{"Message":"The request is invalid.","ModelState":{"saveData.FormId":["Unexpected character encountered while parsing value: t. Path 'FormId', line 1, position 80."]}}
            var strBld = new StringBuilder();
            strBld.AppendLine("ModelState errors.");
            foreach (var kv in stateDictionary)
            {
                var errors = string.Join(",", kv.Value.Errors.Select(BuildErrorMessage).ToArray());
                strBld.AppendFormat("{0}: {1}", kv.Key, errors).AppendLine();
            }
            return strBld.ToString();
        }

        private string BuildErrorMessage(ModelError modelError)
        {
            var msg = modelError.ErrorMessage;
            if (string.IsNullOrEmpty(msg))
                msg = modelError.Exception?.Message;

            return string.IsNullOrEmpty(msg) ? "Unknown error" : msg;
        }
    }
}
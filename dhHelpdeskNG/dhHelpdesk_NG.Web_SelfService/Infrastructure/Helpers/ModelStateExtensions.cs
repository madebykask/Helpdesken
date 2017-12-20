using System.Linq;
using System.Web.Mvc;

namespace DH.Helpdesk.SelfService.Infrastructure.Helpers
{
    public static class ModelStateExtensions
    {
        public static string GetErrors(this ModelStateDictionary modeStateErrors, string sep = ";")
        {
            var errors = modeStateErrors.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
            string messages = string.Join(sep, errors);
            return messages;
        }
    }
}
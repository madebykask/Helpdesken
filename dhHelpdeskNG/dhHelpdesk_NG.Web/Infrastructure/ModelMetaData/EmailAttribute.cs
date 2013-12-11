using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace dhHelpdesk_NG.Web.Infrastructure.ModelMetaData
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        private const string _defaultErrorMessage = "The {0} field is not a valid email address.";

        public EmailAttribute() : base("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")
        {
            ErrorMessage = _defaultErrorMessage;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name);
        }
    }
}
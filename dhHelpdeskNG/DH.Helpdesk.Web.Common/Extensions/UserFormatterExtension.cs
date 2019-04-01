using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Common.Extensions
{
    public static class UserBasicOvierviewExtensions
    {
        public static string FormatUserName(this UserBasicOvierview user, bool firstNameFirst = true)
        {
            return FormatUserNameInner(user.FirstName, user.SurName, firstNameFirst);
        }

        public static string FormatUserNameInner(string firstName, string surName, bool firstNameFirst)
        {
            return firstNameFirst
                ? string.Format("{0} {1}", firstName, surName)
                : string.Format("{0} {1}", surName, firstName);
        }
    }
}
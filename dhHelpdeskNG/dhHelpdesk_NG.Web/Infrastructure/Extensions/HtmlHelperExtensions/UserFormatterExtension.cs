using DH.Helpdesk.BusinessData.Models.User.Interfaces;
using DH.Helpdesk.Web.Enums;

namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public static class UserFormatterExtension
    {
        public static string FormatUserName(this HtmlHelper helper, User user, Setting setting)
        {
            return FormatUserNameInner(user.FirstName, user.SurName, setting.IsUserFirstLastNameRepresentation == 1);
        }

        public static string FormatUserName(this HtmlHelper helper, IUserInfo user, Setting setting)
        {
            return FormatUserNameInner(user.FirstName, user.SurName, setting.IsUserFirstLastNameRepresentation == 1);
        }

        private static string FormatUserNameInner(string firstName, string surName, bool firstNameFirst)
        {
            return firstNameFirst
                ? string.Format("{0} {1}", firstName, surName)
                : string.Format("{0} {1}", surName, firstName);
        }
    }
}
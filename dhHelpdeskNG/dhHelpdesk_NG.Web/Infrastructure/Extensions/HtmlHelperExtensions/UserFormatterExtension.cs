namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public static class UserFormatterExtension
    {
        public static string FormatUserName(this HtmlHelper helper,  User user, Setting setting)
        {
            if (setting.IsUserFirstLastNameRepresentation == 1)
            {
                return string.Format("{0} {1}", user.FirstName, user.SurName);
            }
            return string.Format("{0} {1}", user.SurName, user.FirstName);
        }
    }
}
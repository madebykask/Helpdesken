namespace DH.Helpdesk.Common.Tools
{
    using System.Text.RegularExpressions;

    public static class EmailHelper
    {
        private const string EmailValidationRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                 @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                 @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public static bool IsValid(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var regex = new Regex(EmailValidationRegex);
            return regex.IsMatch(email);
        }         
    }
}
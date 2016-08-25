namespace DH.Helpdesk.Common.Tools
{    
    using System.Text.RegularExpressions;
      
    public static class PasswordHelper
    {
        private static string complexPasswordRule = @"^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{8,}$";

        public static bool IsValid(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
                       
            var checker = new Regex(complexPasswordRule);
            return checker.IsMatch(password);
        }         
    }
}

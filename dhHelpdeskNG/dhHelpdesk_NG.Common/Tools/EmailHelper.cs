﻿namespace DH.Helpdesk.Common.Tools
{
    using System.Text.RegularExpressions;

    public static class EmailHelper
    {
        public static bool IsValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }         
    }
}
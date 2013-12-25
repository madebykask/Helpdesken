using System;

namespace dhHelpdesk_NG.Web.Infrastructure
{
    public class Enums
    {
        public enum TranslationSource
        {
            TextTranslation = 0,
            CaseTranslation = 1
        }

        public enum ActiveStatus
        {
            Inactive = 0,
            Active = 1
        }

        [Flags]
        public enum Permissions
        {
            System_User = 1,
            Administrator = 2,
            Customer_Administrator = 3,
            System_Administrator = 4
        }

        public enum Show
        {
            Inactive = 0,
            
            Active = 1,
            
            All = 2
        }

        public static class PageName
        {
            public static readonly string Notifiers = "Notifiers";
        }
    }
}
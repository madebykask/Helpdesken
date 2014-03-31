namespace DH.Helpdesk.Web.Infrastructure
{
    using System;

    public class Enums
    {
        public enum TranslationSource
        {
            TextTranslation = 0,
            CaseTranslation = 1
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

            public static readonly string Problems = "Problems";

            public static readonly string Projects = "Projects";

            public static readonly string Changes = "Changes";

            public static readonly string Inventory = "Inventory";
        }
    }
}
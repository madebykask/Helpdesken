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

        public enum AccessMode
        {
            NoAccess = 0,
            ReadOnly = 1,
            FullAccess = 2
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
    }
}
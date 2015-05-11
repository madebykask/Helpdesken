namespace DH.Helpdesk.Web.Infrastructure
{
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

        public enum Show
        {
            Inactive = 0,
            Active = 1,
            All = 2
        }
    }
}
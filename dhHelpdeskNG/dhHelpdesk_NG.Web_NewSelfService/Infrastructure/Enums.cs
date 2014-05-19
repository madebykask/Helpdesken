namespace DH.Helpdesk.NewSelfService.Infrastructure
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
        }

        public static class SubtopicName
        {
            public static readonly string Registration = "Registration";

            public static readonly string Analyze = "Analyze";

            public static readonly string Implementation = "Implementation";

            public static readonly string Evaluation = "Evaluation";
        }

        public static class DeletedItemKey
        {
            public static readonly string DeletedLogs = "DeletedLogs";
        }
    }
}
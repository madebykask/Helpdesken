namespace DH.Helpdesk.SelfService.Infrastructure
{
    using System;

    public static class Enums
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

            public static readonly string Orders = "Orders";
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

        public static class CaseFieldGroups
        {
            public static readonly string UserInformation = "UserInformation";

            public static readonly string ComputerInformation = "ComputerInformation";

            public static readonly string CaseInfo = "CaseInfo";

            public static readonly string Other = "Other";

            public static readonly string CaseLog = "CaseLog";
            
        }

        public enum LogNote
        {
            UseExternalLogNote = 0,
            UseInternalLogNote = 1
        }

        public static class MimeType
        {
            public static readonly string ExcelFile = "application/vnd.ms-excel";

            public static readonly string BinaryFile = "application/octet-stream";
        }

        public enum SortOrder
        {
            Asc = 0,
            Desc = 1
        }
    }
}
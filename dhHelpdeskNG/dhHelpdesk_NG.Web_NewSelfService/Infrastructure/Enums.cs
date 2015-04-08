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

        public static class ApplicationTypes
        {
            public static readonly string LineManager = "LineManager";

            public static readonly string SelfService = "SelfService";            
        }
        
        public static class CaseListTypes
        {
            public static readonly string manager = "manager";

            public static readonly string coworkers = "coworkers";

            public static readonly string ManagerCoworkers = "manager,coworkers";
        }

        public static class LoginModes
        {
            public static readonly string SSO = "SSO";

            public static readonly string Windows = "Windows";
        }

        public static class FederationServiceKeys
        {
            public static readonly string ClaimDomain = "ClaimDomain";

            public static readonly string ClaimUserId = "ClaimUserId";

            public static readonly string ClaimEmployeeNumber = "ClaimEmployeeNumber";

            public static readonly string ClaimFirstName = "ClaimFirstName";

            public static readonly string ClaimLastName = "ClaimLastName";

            public static readonly string ClaimEmail = "ClaimEmail";
        }                 

        public static class ApplicationKeys
        {
            public static readonly string LoginMode = "LoginMode";

            public static readonly string CurrentApplicationType = "CurrentApplication";

            public static readonly string CaseList = "CaseList";

            public static readonly string DefaultEmployeeNumber = "DefaultEmployeeNumber";

            public static readonly string SSOLog = "SSOLog";

            public static readonly string ApplicationId = "ApplicationId";

        }

        

    }
}
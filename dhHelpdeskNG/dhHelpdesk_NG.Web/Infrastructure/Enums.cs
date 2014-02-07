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

        public static class AnalyzeField
        {
            public static readonly string Category = "Analyze.Category";

            public static readonly string Priority = "Analyze.Priority";

            public static readonly string Responsible = "Analyze.Responsible";

            public static readonly string Solution = "Analyze.Solution";

            public static readonly string Cost = "Analyze.Cost";

            public static readonly string YearlyCost = "Analyze.YearlyCost";

            public static readonly string TimeEstimatesHours = "Analyze.TimeEstimatesHours";

            public static readonly string Risk = "Analyze.Risk";

            public static readonly string StartDate = "Analyze.StartDate";

            public static readonly string FinishDate = "Analyze.FinishDate";

            public static readonly string ImplementationPlan = "Analyze.ImplementationPlan";

            public static readonly string RecoveryPlan = "Analyze.RecoveryPlan";

            public static readonly string Recommendation = "Analyze.Recommendation";

            public static readonly string Approval = "Analyze.Approval";
        }

        public static class EvaluationField
        {
            public static readonly string Evaluation = "Evaluation.Evaluation";

            public static readonly string EvaluationReady = "Evaluation.EvaluationReady";
        }

        public static class GeneralField
        {
            public static readonly string Priority = "General.Priority";

            public static readonly string Title = "General.Title";

            public static readonly string State = "General.State";

            public static readonly string System = "General.System";

            public static readonly string Object = "General.Object";

            public static readonly string Inventory = "General.Inventory";

            public static readonly string Owner = "General.Owner";

            public static readonly string WorkingGroup = "General.WorkingGroup";

            public static readonly string Administrator = "General.Administrator";

            public static readonly string FinishingDate = "General.FinishingDate";

            public static readonly string Rss = "General.Rss";
        }

        public static class ImplementationField
        {
            public static readonly string State = "Implementation.State";

            public static readonly string RealStartDate = "Implementation.RealStartDate";

            public static readonly string BuildAndTextImplemented = "Implementation.BuildAndTextImplemented";

            public static readonly string ImplementationPlanUsed = "Implementation.ImplementationPlanUsed";

            public static readonly string Deviation = "Implementation.Deviation";

            public static readonly string RecoveryPlanUsed = "Implementation.RecoveryPlanUsed";

            public static readonly string FinishingDate = "Implementation.FinishingDate";

            public static readonly string ImplementationReady = "Implementation.ImplementationReady";
        }

        public static class OrdererField
        {
            public static readonly string Id = "Orderer.Id";

            public static readonly string Name = "Orderer.Name";

            public static readonly string Phone = "Orderer.Phone";

            public static readonly string CellPhone = "Orderer.CellPhone";

            public static readonly string Email = "Orderer.Email";

            public static readonly string Department = "Orderer.Department";
        }

        public static class RegistrationField
        {
            public static readonly string Description = "Registration.Description";

            public static readonly string BusinessBenefits = "Registration.BusinessBenefits";

            public static readonly string Consequence = "Registration.Consequence";

            public static readonly string Impact = "Registration.Impact";

            public static readonly string DesiredDate = "Registration.DesiredDate";

            public static readonly string Verified = "Registration.Verified";

            public static readonly string Approval = "Registration.Approval";

            public static readonly string Explanation = "Registration.Explanation";
        }
    }
}
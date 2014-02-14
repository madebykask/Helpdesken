namespace DH.Helpdesk.Dal.Enums.Changes
{
    internal static class AnalyzeField
    {
        public static readonly string Category = "ChangeCategory_Id";

        public static readonly string Priority = "ChangePriority_Id";

        public static readonly string Responsible = "ResponsibleUser_Id";

        public static readonly string Solution = "ChangeSolution";

        public static readonly string Cost = "TotalCost";

        public static readonly string YearlyCost = "YearlyCost";

        public static readonly string EstimatedTimeInHours = "TimeEstimatesHours";

        public static readonly string Risk = "ChangeRisk";

        public static readonly string StartDate = "ScheduledStartTime";

        public static readonly string FinishDate = "ScheduledEndTime";

        public static readonly string HasImplementationPlan = "ImplementationPlan";

        public static readonly string HasRecoveryPlan = "RecoveryPlan";

        public static readonly string AttachedFiles = "AnalysisFilename";

        public static readonly string Logs = "AnalysisLog";

        public static readonly string Approval = "AnalysisApproval";

        public static readonly string RejectExplanation = "ChangeRecommendation";
    }
}

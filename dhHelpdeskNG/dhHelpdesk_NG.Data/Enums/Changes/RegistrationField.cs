namespace DH.Helpdesk.Dal.Enums.Changes
{
    internal static class RegistrationField
    {
        public static readonly string Owner = "ChangeGroup_Id";

        public static readonly string AffectedProcesses = "tblChange_tblChangeGroup";

        public static readonly string AffectedDepartments = "tblChange_tblDepartment";

        public static readonly string Description = "ChangeDescription";

        public static readonly string BusinessBenefits = "ChangeBenefits";

        public static readonly string Consequence = "ChangeConsequence";

        public static readonly string Impact = "ChangeImpact";

        public static readonly string DesiredDate = "DesiredDate";

        public static readonly string Verified = "Verified";

        public static readonly string AttachedFiles = "Filename";

        public static readonly string Approval = "ApprovalDate";

        public static readonly string RejectExplanation = "ChangeExplanation";
    }
}

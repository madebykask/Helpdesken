namespace DH.Helpdesk.BusinessData.Enums.Changes.Fields
{
    public static class RegistrationField
    {
        public static readonly string Name = "Registration.Name";

        public static readonly string Phone = "Registration.Phone";

        public static readonly string Email = "Registration.Mail";

        public static readonly string Company = "Registration.Company";

        public static readonly string Owner = "Registration.Owner";

        public static readonly string AffectedProcesses = "Registration.AffectedProcesses";

        public static readonly string AffectedDepartments = "Registration.AffectedDepartments";

        public static readonly string Description = "Registration.Description";

        public static readonly string BusinessBenefits = "Registration.BusinessBenefits";

        public static readonly string Consequence = "Registration.Consequence";

        public static readonly string Impact = "Registration.Impact";

        public static readonly string DesiredDate = "Registration.DesiredDate";

        public static readonly string Verified = "Registration.Verified";

        public static readonly string AttachedFiles = "Registration.AttachedFiles";

        public static readonly string Approval = "Registration.Approval";

        public static readonly string RejectExplanation = "Registration.RejectExplanation";

        public static string GetAffectedProcess(string value)
        {
            return string.Format("{0}.{1}", AffectedProcesses, value);
        }
    }
}
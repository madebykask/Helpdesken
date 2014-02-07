namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistrationFieldSettings
    {
        public RegistrationFieldSettings(
            FieldSetting name,
            FieldSetting phone,
            FieldSetting email,
            FieldSetting company,
            FieldSetting processAffected,
            FieldSetting departmentAffected,
            StringFieldSetting description,
            StringFieldSetting businessBenefits,
            StringFieldSetting consequence,
            FieldSetting impact,
            FieldSetting desiredDate,
            FieldSetting verified,
            FieldSetting attachedFile,
            FieldSetting approval,
            FieldSetting explanation)
        {
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Company = company;
            this.ProcessAffected = processAffected;
            this.DepartmentAffected = departmentAffected;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.AttachedFile = attachedFile;
            this.Approval = approval;
            this.Explanation = explanation;
        }

        [NotNull]
        public FieldSetting Name { get; private set; }

        [NotNull]
        public FieldSetting Phone { get; private set; }

        [NotNull]
        public FieldSetting Email { get; private set; }

        [NotNull]
        public FieldSetting Company { get; private set; }

        [NotNull]
        public FieldSetting ProcessAffected { get; private set; }

        [NotNull]
        public FieldSetting DepartmentAffected { get; private set; }

        [NotNull]
        public StringFieldSetting Description { get; private set; }

        [NotNull]
        public StringFieldSetting BusinessBenefits { get; private set; }

        [NotNull]
        public StringFieldSetting Consequence { get; private set; }

        [NotNull]
        public FieldSetting Impact { get; private set; }

        [NotNull]
        public FieldSetting DesiredDate { get; private set; }

        [NotNull]
        public FieldSetting Verified { get; private set; }

        [NotNull]
        public FieldSetting AttachedFile { get; private set; }

        [NotNull]
        public FieldSetting Approval { get; private set; }

        [NotNull]
        public FieldSetting Explanation { get; private set; }
    }
}

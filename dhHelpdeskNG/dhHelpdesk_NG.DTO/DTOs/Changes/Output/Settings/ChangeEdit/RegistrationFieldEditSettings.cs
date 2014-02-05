namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class RegistrationFieldEditSettings
    {
        public RegistrationFieldEditSettings(
            FieldEditSetting name,
            FieldEditSetting phone,
            FieldEditSetting email,
            FieldEditSetting company,
            FieldEditSetting processesAffected,
            FieldEditSetting departmentsAffected,
            TextFieldEditSetting description,
            TextFieldEditSetting businessBenefits,
            TextFieldEditSetting consequence,
            FieldEditSetting impact,
            FieldEditSetting desiredDate,
            FieldEditSetting verified,
            FieldEditSetting attachedFile,
            FieldEditSetting approval,
            FieldEditSetting rejectExplanation)
        {
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Company = company;
            this.ProcessesAffected = processesAffected;
            this.DepartmentsAffected = departmentsAffected;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.AttachedFile = attachedFile;
            this.Approval = approval;
            this.RejectExplanation = rejectExplanation;
        }

        [NotNull]
        public FieldEditSetting Name { get; private set; }

        [NotNull]
        public FieldEditSetting Phone { get; private set; }

        [NotNull]
        public FieldEditSetting Email { get; private set; }

        [NotNull]
        public FieldEditSetting Company { get; private set; }

        [NotNull]
        public FieldEditSetting ProcessesAffected { get; private set; }

        [NotNull]
        public FieldEditSetting DepartmentsAffected { get; private set; }

        [NotNull]
        public TextFieldEditSetting Description { get; private set; }

        [NotNull]
        public TextFieldEditSetting BusinessBenefits { get; private set; }

        [NotNull]
        public TextFieldEditSetting Consequence { get; private set; }

        [NotNull]
        public FieldEditSetting Impact { get; private set; }

        [NotNull]
        public FieldEditSetting DesiredDate { get; private set; }

        [NotNull]
        public FieldEditSetting Verified { get; private set; }

        [NotNull]
        public FieldEditSetting AttachedFile { get; private set; }

        [NotNull]
        public FieldEditSetting Approval { get; private set; }

        [NotNull]
        public FieldEditSetting RejectExplanation { get; private set; }
    }
}

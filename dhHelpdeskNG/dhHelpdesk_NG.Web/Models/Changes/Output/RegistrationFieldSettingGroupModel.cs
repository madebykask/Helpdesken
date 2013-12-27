namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class RegistrationFieldSettingGroupModel
    {
        public RegistrationFieldSettingGroupModel(
            FieldSettingModel name,
            FieldSettingModel phone,
            FieldSettingModel email,
            FieldSettingModel company,
            FieldSettingModel processAffected,
            FieldSettingModel departmentAffected,
            StringFieldSettingModel description,
            StringFieldSettingModel businessBenefits,
            StringFieldSettingModel consequence,
            FieldSettingModel impact,
            FieldSettingModel desiredDate,
            FieldSettingModel verified,
            FieldSettingModel attachedFile,
            FieldSettingModel approval,
            FieldSettingModel explanation)
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
        public FieldSettingModel Name { get; private set; }

        [NotNull]
        public FieldSettingModel Phone { get; private set; }

        [NotNull]
        public FieldSettingModel Email { get; private set; }

        [NotNull]
        public FieldSettingModel Company { get; private set; }

        [NotNull]
        public FieldSettingModel ProcessAffected { get; private set; }

        [NotNull]
        public FieldSettingModel DepartmentAffected { get; private set; }

        [NotNull]
        public StringFieldSettingModel Description { get; private set; }

        [NotNull]
        public StringFieldSettingModel BusinessBenefits { get; private set; }

        [NotNull]
        public StringFieldSettingModel Consequence { get; private set; }

        [NotNull]
        public FieldSettingModel Impact { get; private set; }

        [NotNull]
        public FieldSettingModel DesiredDate { get; private set; }

        [NotNull]
        public FieldSettingModel Verified { get; private set; }

        [NotNull]
        public FieldSettingModel AttachedFile { get; private set; }

        [NotNull]
        public FieldSettingModel Approval { get; private set; }

        [NotNull]
        public FieldSettingModel Explanation { get; private set; }
    }
}

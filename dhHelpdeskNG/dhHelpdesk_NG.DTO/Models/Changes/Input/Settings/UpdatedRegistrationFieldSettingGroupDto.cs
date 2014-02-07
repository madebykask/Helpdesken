namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedRegistrationFieldSettingGroupDto
    {
        public UpdatedRegistrationFieldSettingGroupDto(
            UpdatedFieldSettingDto name,
            UpdatedFieldSettingDto phone,
            UpdatedFieldSettingDto email,
            UpdatedFieldSettingDto company,
            UpdatedFieldSettingDto processAffected,
            UpdatedFieldSettingDto departmentAffected,
            UpdatedStringFieldSettingDto description,
            UpdatedStringFieldSettingDto businessBenefits,
            UpdatedStringFieldSettingDto consequence,
            UpdatedFieldSettingDto impact,
            UpdatedFieldSettingDto desiredDate,
            UpdatedFieldSettingDto verified,
            UpdatedFieldSettingDto attachedFile,
            UpdatedFieldSettingDto approval,
            UpdatedFieldSettingDto explanation)
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
        public UpdatedFieldSettingDto Name { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Phone { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Email { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Company { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto ProcessAffected { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto DepartmentAffected { get; private set; }

        [NotNull]
        public UpdatedStringFieldSettingDto Description { get; private set; }

        [NotNull]
        public UpdatedStringFieldSettingDto BusinessBenefits { get; private set; }

        [NotNull]
        public UpdatedStringFieldSettingDto Consequence { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Impact { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto DesiredDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Verified { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto AttachedFile { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Approval { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Explanation { get; private set; }
    }
}

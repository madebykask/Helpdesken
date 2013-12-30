namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class RegistrationFieldSettingGroupModel
    {
        public RegistrationFieldSettingGroupModel()
        {
        }

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
        [LocalizedDisplay("Name")]
        public FieldSettingModel Name { get; private set; }

        [NotNull]
        [LocalizedDisplay("Phone")]
        public FieldSettingModel Phone { get; private set; }

        [NotNull]
        [LocalizedDisplay("E-mail")]
        public FieldSettingModel Email { get; private set; }

        [NotNull]
        [LocalizedDisplay("Company")]
        public FieldSettingModel Company { get; private set; }

        [NotNull]
        [LocalizedDisplay("Process affected")]
        public FieldSettingModel ProcessAffected { get; private set; }

        [NotNull]
        [LocalizedDisplay("Department affected")]
        public FieldSettingModel DepartmentAffected { get; private set; }

        [NotNull]
        [LocalizedDisplay("Description")]
        public StringFieldSettingModel Description { get; private set; }

        [NotNull]
        [LocalizedDisplay("Business benefits")]
        public StringFieldSettingModel BusinessBenefits { get; private set; }

        [NotNull]
        [LocalizedDisplay("Consequence")]
        public StringFieldSettingModel Consequence { get; private set; }

        [NotNull]
        [LocalizedDisplay("Impact")]
        public FieldSettingModel Impact { get; private set; }

        [NotNull]
        [LocalizedDisplay("Desired date")]
        public FieldSettingModel DesiredDate { get; private set; }

        [NotNull]
        [LocalizedDisplay("Verified")]
        public FieldSettingModel Verified { get; private set; }

        [NotNull]
        [LocalizedDisplay("Attached file")]
        public FieldSettingModel AttachedFile { get; private set; }

        [NotNull]
        [LocalizedDisplay("Approval")]
        public FieldSettingModel Approval { get; private set; }

        [NotNull]
        [LocalizedDisplay("Explanation")]
        public FieldSettingModel Explanation { get; private set; }
    }
}

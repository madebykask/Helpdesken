namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class RegistrationFieldSettingsModel
    {
        public RegistrationFieldSettingsModel()
        {
        }

        public RegistrationFieldSettingsModel(
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
        public FieldSettingModel Name { get; set; }

        [NotNull]
        [LocalizedDisplay("Phone")]
        public FieldSettingModel Phone { get; set; }

        [NotNull]
        [LocalizedDisplay("E-mail")]
        public FieldSettingModel Email { get; set; }

        [NotNull]
        [LocalizedDisplay("Company")]
        public FieldSettingModel Company { get; set; }

        [NotNull]
        [LocalizedDisplay("Process affected")]
        public FieldSettingModel ProcessAffected { get; set; }

        [NotNull]
        [LocalizedDisplay("Department affected")]
        public FieldSettingModel DepartmentAffected { get; set; }

        [NotNull]
        [LocalizedDisplay("Description")]
        public StringFieldSettingModel Description { get; set; }

        [NotNull]
        [LocalizedDisplay("Business benefits")]
        public StringFieldSettingModel BusinessBenefits { get; set; }

        [NotNull]
        [LocalizedDisplay("Consequence")]
        public StringFieldSettingModel Consequence { get; set; }

        [NotNull]
        [LocalizedDisplay("Impact")]
        public FieldSettingModel Impact { get; set; }

        [NotNull]
        [LocalizedDisplay("Desired date")]
        public FieldSettingModel DesiredDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Verified")]
        public FieldSettingModel Verified { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached file")]
        public FieldSettingModel AttachedFile { get; set; }

        [NotNull]
        [LocalizedDisplay("Approval")]
        public FieldSettingModel Approval { get; set; }

        [NotNull]
        [LocalizedDisplay("Explanation")]
        public FieldSettingModel Explanation { get; set; }
    }
}

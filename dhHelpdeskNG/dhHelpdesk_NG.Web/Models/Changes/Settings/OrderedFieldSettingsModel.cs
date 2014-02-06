namespace dhHelpdesk_NG.Web.Models.Changes.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class OrderedFieldSettingsModel
    {
        public OrderedFieldSettingsModel()
        {
        }

        public OrderedFieldSettingsModel(
            FieldSettingModel id,
            FieldSettingModel name,
            FieldSettingModel phone,
            FieldSettingModel cellPhone,
            FieldSettingModel email,
            FieldSettingModel department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        [LocalizedDisplay("ID")]
        public FieldSettingModel Id { get; set; }

        [NotNull]
        [LocalizedDisplay("Name")]
        public FieldSettingModel Name { get; set; }

        [NotNull]
        [LocalizedDisplay("Phone")]
        public FieldSettingModel Phone { get; set; }

        [NotNull]
        [LocalizedDisplay("Cell phone")]
        public FieldSettingModel CellPhone { get; set; }

        [NotNull]
        [LocalizedDisplay("E-mail")]
        public FieldSettingModel Email { get; set; }

        [NotNull]
        [LocalizedDisplay("Department")]
        public FieldSettingModel Department { get; set; }
    }
}

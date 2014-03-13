namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;

    public sealed class OrdererFieldSettingsModel
    {
        public OrdererFieldSettingsModel()
        {
        }

        public OrdererFieldSettingsModel(
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
        [LocalizedDisplay("Cell Phone")]
        public FieldSettingModel CellPhone { get; set; }

        [NotNull]
        [LocalizedDisplay("E-mail")]
        public FieldSettingModel Email { get; set; }

        [NotNull]
        [LocalizedDisplay("Department")]
        public FieldSettingModel Department { get; set; }
    }
}

namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.ComponentModel.DataAnnotations;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class OrderedFieldSettingGroupModel
    {
        public OrderedFieldSettingGroupModel()
        {
        }

        public OrderedFieldSettingGroupModel(
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
        public FieldSettingModel Id { get; private set; }

        [NotNull]
        [LocalizedDisplay("Name")]
        public FieldSettingModel Name { get; private set; }

        [NotNull]
        [LocalizedDisplay("Phone")]
        public FieldSettingModel Phone { get; private set; }

        [NotNull]
        [LocalizedDisplay("Cell phone")]
        public FieldSettingModel CellPhone { get; private set; }

        [NotNull]
        [LocalizedDisplay("E-mail")]
        public FieldSettingModel Email { get; private set; }

        [NotNull]
        [LocalizedDisplay("Department")]
        public FieldSettingModel Department { get; private set; }
    }
}

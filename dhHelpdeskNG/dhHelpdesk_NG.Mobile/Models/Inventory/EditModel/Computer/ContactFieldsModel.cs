namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactFieldsModel
    {
        public ContactFieldsModel()
        {
        }

        public ContactFieldsModel(ConfigurableFieldModel<string> name, ConfigurableFieldModel<string> phone, ConfigurableFieldModel<string> email)
        {
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
        }

        [NotNull]
        public ConfigurableFieldModel<string> Name { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Phone { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Email { get; set; }
    }
}
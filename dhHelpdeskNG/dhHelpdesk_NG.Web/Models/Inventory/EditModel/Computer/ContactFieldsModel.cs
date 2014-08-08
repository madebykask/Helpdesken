namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    public class ContactFieldsModel
    {
        public ContactFieldsModel(ConfigurableFieldModel<string> name, ConfigurableFieldModel<string> phone, ConfigurableFieldModel<string> email)
        {
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
        }

        public ConfigurableFieldModel<string> Name { get; set; }

        public ConfigurableFieldModel<string> Phone { get; set; }

        public ConfigurableFieldModel<string> Email { get; set; }
    }
}
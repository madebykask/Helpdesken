namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;

    public sealed class Contact
    {
        public Contact()
        {
        }

        public Contact(
            ConfigurableFieldModelMultipleChoices<string> ids,
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> email)
        {
            this.Ids = ids;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
        }

        public ConfigurableFieldModelMultipleChoices<string> Ids { get; set; }

        public ConfigurableFieldModel<string> Name { get; set; }

        public ConfigurableFieldModel<string> Phone { get; set; }

        public ConfigurableFieldModel<string> Email { get; set; }
    }
}
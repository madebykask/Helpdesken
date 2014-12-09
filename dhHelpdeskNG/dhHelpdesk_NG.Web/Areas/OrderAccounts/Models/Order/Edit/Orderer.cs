namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;

    public sealed class Orderer
    {
        public Orderer()
        {
        }

        public Orderer(
            ConfigurableFieldModel<string> id,
            ConfigurableFieldModel<string> firstName,
            ConfigurableFieldModel<string> lastName,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> email)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Email = email;
        }

        public ConfigurableFieldModel<string> Id { get;  set; }

        public ConfigurableFieldModel<string> FirstName { get;  set; }

        public ConfigurableFieldModel<string> LastName { get;  set; }

        public ConfigurableFieldModel<string> Phone { get;  set; }

        public ConfigurableFieldModel<string> Email { get;  set; }
    }
}
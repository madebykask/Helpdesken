namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;

    public sealed class DeliveryInformation
    {
        public DeliveryInformation()
        {
        }

        public DeliveryInformation(
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> address,
            ConfigurableFieldModel<string> postalAddress)
        {
            this.Name = name;
            this.Phone = phone;
            this.Address = address;
            this.PostalAddress = postalAddress;
        }

        public ConfigurableFieldModel<string> Name { get; set; }

        public ConfigurableFieldModel<string> Phone { get; set; }

        public ConfigurableFieldModel<string> Address { get; set; }

        public ConfigurableFieldModel<string> PostalAddress { get; set; }
    }
}
using DH.Helpdesk.SelfService.Models.Orders.FieldModels;

namespace DH.Helpdesk.SelfService.Models.Orders.OrderEdit
{
    public class ContactEditModel
    {
        public ContactEditModel()
        {
        }

        public ContactEditModel(
            ConfigurableFieldModel<string> id,
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> email)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
        }

        public ConfigurableFieldModel<string> Id { get; set; }

        public ConfigurableFieldModel<string> Name { get; set; }

        public ConfigurableFieldModel<string> Phone { get; set; }

        public ConfigurableFieldModel<string> Email { get; set; }

        public static ContactEditModel CreateEmpty()
        {
            return new ContactEditModel(
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return Id.Show ||
                   Name.Show ||
                   Phone.Show ||
                   Email.Show ;
        }
    }
}
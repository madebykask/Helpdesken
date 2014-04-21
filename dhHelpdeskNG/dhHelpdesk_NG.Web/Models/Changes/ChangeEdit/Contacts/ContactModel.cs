namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit.Contacts
{
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class ContactModel
    {
        public ContactModel()
        {
        }

        public ContactModel(
            int id,
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> email,
            ConfigurableFieldModel<string> company)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Company = company;
        }

        public int Id { get; set; }

        public ConfigurableFieldModel<string> Name { get; set; }

        public ConfigurableFieldModel<string> Phone { get; set; }

        public ConfigurableFieldModel<string> Email { get; set; }

        public ConfigurableFieldModel<string> Company { get; set; }
    }
}
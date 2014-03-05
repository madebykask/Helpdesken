namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Overview.Computer
{
    public class ContactFieldsSettings
    {
        public ContactFieldsSettings(string name, string phone, string email)
        {
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
        }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
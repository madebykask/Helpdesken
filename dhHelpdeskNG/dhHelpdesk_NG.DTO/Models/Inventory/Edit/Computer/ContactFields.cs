namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    public class ContactFields
    {
        public ContactFields(string name, string phone, string email)
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
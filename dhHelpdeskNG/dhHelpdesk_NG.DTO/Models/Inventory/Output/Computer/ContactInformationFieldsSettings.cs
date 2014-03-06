namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class ContactInformationFieldsSettings
    {
        public ContactInformationFieldsSettings(string userId, string department, string unit)
        {
            this.UserId = userId;
            this.Department = department;
            this.Unit = unit;
        }

        public string UserId { get; set; }

        public string Department { get; set; }

        public string Unit { get; set; }
    }
}
namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class ContactInformationFields
    {
        public ContactInformationFields(string userId, string region, string department, string unit)
        {
            this.UserId = userId;
            Region = region;
            this.Department = department;
            this.Unit = unit;
        }

        public string UserId { get; set; }
        public string Region { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
    }
}
namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsSettings
    {
        public ContactInformationFieldsSettings(string userId, string department, string unit, UserName userName)
        {
            this.UserId = userId;
            this.Department = department;
            this.Unit = unit;
            this.UserName = userName;
        }

        public string UserId { get; set; }

        public string Department { get; set; }

        public string Unit { get; set; }

        [NotNull]
        public UserName UserName { get; set; }
    }
}
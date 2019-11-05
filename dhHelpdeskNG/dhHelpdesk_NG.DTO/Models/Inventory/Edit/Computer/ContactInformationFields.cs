namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFields
    {
        // todo
        public ContactInformationFields(int? userId)
        {
            this.UserId = userId;
        }

        public ContactInformationFields(
            int? userId,
            string userStringId,
            string region,
            string department,
            string unit,
            UserName userName)
        {
            this.UserId = userId;
            this.UserStringId = userStringId;
            this.Region = region;
            this.Department = department;
            this.Unit = unit;
            this.UserName = userName;
        }

        [IsId]
        public int? UserId { get; set; }
        public string UserStringId { get; set; }
        public UserName UserName { get; set; }
        public string Region { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public static ContactInformationFields CreateDefault()
        {
            return new ContactInformationFields(null);
        }
    }
}
namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFields
    {
        public ContactInformationFields(string userStringId, string department, string unit, UserName userName)
        {
            this.UserStringId = userStringId;
            this.Department = department;
            this.Unit = unit;
            this.UserName = userName;
        }

        [IsId]
        public int? UserId { get; set; }

        public string UserStringId { get; set; }

        [NotNull]
        public UserName UserName { get; set; }

        public string Department { get; set; }

        public string Unit { get; set; }
    }
}
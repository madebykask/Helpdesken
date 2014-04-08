namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsModel
    {
        public ContactInformationFieldsModel(ConfigurableFieldModel<string> userStringId, string department, string unit, UserName userName)
        {
            this.UserStringId = userStringId;
            this.Department = department;
            this.Unit = unit;
            this.UserName = userName;
        }

        [IsId]
        public int? UserId { get; set; }

        public ConfigurableFieldModel<string> UserStringId { get; set; }

        [NotNull]
        public UserName UserName { get; set; }

        public string Department { get; set; }

        public string Unit { get; set; }
    }
}
namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsModel
    {
        public ContactInformationFieldsModel()
        {
        }

        public ContactInformationFieldsModel(
            int? userId,
            ConfigurableFieldModel<string> userStringId,
            string department,
            string unit,
            UserName userName)
        {
            this.UserId = userId;
            this.UserStringId = userStringId;
            this.Department = department;
            this.Unit = unit;
            this.UserName = userName;
        }

        [IsId]
        public int? UserId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> UserStringId { get; set; }

        public UserName UserName { get; set; }

        public string Department { get; set; }

        public string Unit { get; set; }
    }
}
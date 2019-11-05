namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsModel
    {
        public ContactInformationFieldsModel()
        {
        }

        public ContactInformationFieldsModel(
            int? userId,
            ConfigurableFieldModel<string> userStringId,
            ConfigurableFieldModel<string> firstName,
            ConfigurableFieldModel<string> lastName,
            ConfigurableFieldModel<string> region,
            ConfigurableFieldModel<string> department,
            ConfigurableFieldModel<string> unit)
        {
            UserId = userId;
            UserStringId = userStringId;
            FirstName = firstName;
            LastName = lastName;
            Region = region;
            Department = department;
            Unit = unit;
        }

        [IsId]
        public int? UserId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> UserStringId { get; set; }
        public ConfigurableFieldModel<string> FirstName { get; set; }
        public ConfigurableFieldModel<string> LastName { get; set; }
        public ConfigurableFieldModel<string> Region { get; set; }
        public ConfigurableFieldModel<string> Department { get; set; }
        public ConfigurableFieldModel<string> Unit { get; set; }
    }
}
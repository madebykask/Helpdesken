namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFields
    {
        public StateFields(UserName createdBy)
        {
            this.CreatedBy = createdBy;
        }

        [NotNull]
        public UserName CreatedBy { get; set; }
    }
}
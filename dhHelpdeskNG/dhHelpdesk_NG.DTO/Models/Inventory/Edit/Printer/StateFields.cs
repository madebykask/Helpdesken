namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFields
    {
        public StateFields(UserName changedByUserName)
        {
            this.ChangedByUserName = changedByUserName;
        }

        [NotNull]
        public UserName ChangedByUserName { get; set; }
    }
}
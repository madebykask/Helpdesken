namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using DH.Helpdesk.Common.Types;

    public class StateFieldsModel
    {
        public StateFieldsModel()
        {
        }

        public StateFieldsModel(UserName changedByUserName)
        {
            this.ChangedByUserName = changedByUserName;
        }

        public UserName ChangedByUserName { get; set; }
    }
}

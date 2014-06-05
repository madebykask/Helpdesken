namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSettingForModelEdit
    {
        public InventoryFieldSettingForModelEdit(
            string caption,
            int propertySize,
            bool showInDetails)
        {
            this.Caption = caption;
            this.PropertySize = propertySize;
            this.ShowInDetails = showInDetails;
        }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public int PropertySize { get; private set; }

        public bool ShowInDetails { get; private set; }
    }
}
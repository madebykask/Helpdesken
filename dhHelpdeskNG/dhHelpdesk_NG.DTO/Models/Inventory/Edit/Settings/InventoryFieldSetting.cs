namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSetting
    {
        public InventoryFieldSetting(
            string caption,
            int? propertySize,
            bool showInDetails,
            bool showInList,
            string xml,
            bool readOnly)
        {
            this.Caption = caption;
            this.PropertySize = propertySize;
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.XMLTag = xml;
            this.ReadOnly = readOnly;
        }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public int? PropertySize { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        public string XMLTag { get; set; }

        public bool ReadOnly { get; set; }
    }
}
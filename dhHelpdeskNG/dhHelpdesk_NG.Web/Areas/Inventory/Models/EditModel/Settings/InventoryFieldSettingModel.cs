namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class InventoryFieldSettingModel
    {
        public InventoryFieldSettingModel()
        {
        }

        public InventoryFieldSettingModel(
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

        private InventoryFieldSettingModel(int? propertySize, string caption)
        {
            this.PropertySize = propertySize;
            this.Caption = caption;
        }

        [NotNull]
        [LocalizedRequired]
        public string Caption { get; set; }

        [Max(0)]
        public int? PropertySize { get; set; }

        public string XMLTag { get; set; }

        public bool ReadOnly { get; set; }

        public bool ShowInDetails { get; set; }

        public bool ShowInList { get; set; }

        public static InventoryFieldSettingModel GetDefault(int? propertySize, string caption)
        {
            return new InventoryFieldSettingModel(propertySize, caption);
        }
    }
}
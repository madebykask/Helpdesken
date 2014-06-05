namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings
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
            bool showInList)
        {
            this.Caption = caption;
            this.PropertySize = propertySize;
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
        }

        private InventoryFieldSettingModel(int? propertySize)
        {
            this.PropertySize = propertySize;
        }

        [NotNull]
        [LocalizedRequired]
        public string Caption { get; private set; }

        [Max(0)]
        public int? PropertySize { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        public static InventoryFieldSettingModel GetDefault(int? propertySize)
        {
            return new InventoryFieldSettingModel(propertySize);
        }
    }
}
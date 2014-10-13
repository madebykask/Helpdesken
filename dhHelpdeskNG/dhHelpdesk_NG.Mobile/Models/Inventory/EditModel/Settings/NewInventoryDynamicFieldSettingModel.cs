namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewInventoryDynamicFieldSettingModel
    {
        public NewInventoryDynamicFieldSettingModel()
        {
        }

        public NewInventoryDynamicFieldSettingModel(
            int id,
            int? inventoryTypeGroupId,
            string caption,
            int position,
            FieldTypes? fieldType,
            int? propertySize,
            bool showInDetails,
            bool showInList)
        {
            this.Id = id;
            this.InventoryTypeGroupId = inventoryTypeGroupId;
            this.Caption = caption;
            this.Position = position;
            this.FieldType = fieldType;
            this.PropertySize = propertySize;
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? InventoryTypeGroupId { get; set; }

        public string Caption { get; set; }

        [Min(0)]
        public int Position { get; set; }

        public FieldTypes? FieldType { get; set; }

        [Min(0)]
        public int? PropertySize { get; set; }

        public bool ShowInDetails { get; set; }

        public bool ShowInList { get; set; }
    }
}
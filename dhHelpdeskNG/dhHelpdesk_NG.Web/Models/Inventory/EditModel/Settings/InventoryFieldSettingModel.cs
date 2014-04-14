namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings
{
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class InventoryFieldSettingModel
    {
        public InventoryFieldSettingModel()
        {
        }

        public InventoryFieldSettingModel(
            int id,
            int inventoryTypeId,
            int? inventoryTypeGroupId,
            string caption,
            int position,
            FieldTypes? fieldType,
            int propertySize,
            bool showInDetails,
            bool showInList,
            SelectList groups)
        {
            this.Id = id;
            this.InventoryTypeId = inventoryTypeId;
            this.InventoryTypeGroupId = inventoryTypeGroupId;
            this.Caption = caption;
            this.Position = position;
            this.FieldType = fieldType;
            this.PropertySize = propertySize;
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.Groups = groups;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [IsId]
        public int? InventoryTypeGroupId { get; private set; }

        [NotNull]
        [LocalizedRequired]
        public string Caption { get; private set; }

        public int Position { get; private set; }

        public FieldTypes? FieldType { get; private set; }

        public int PropertySize { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        public SelectList Groups { get; private set; }
    }
}
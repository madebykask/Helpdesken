namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldsModel
    {
        public InventoryFieldsModel()
        {
        }

        public InventoryFieldsModel(ConfigurableFieldModel<string> barCode)
        {
            this.BarCode = barCode;
        }

        [NotNull]
        public ConfigurableFieldModel<string> BarCode { get; set; }

    }
}
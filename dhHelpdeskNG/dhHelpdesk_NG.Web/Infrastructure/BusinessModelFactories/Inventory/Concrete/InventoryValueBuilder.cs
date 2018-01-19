namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory;

    public class InventoryValueBuilder : IInventoryValueBuilder
    {
        public const string True = "1";
        public const string False = "0";

        public const string MvcTrue = "true";

        public List<InventoryValueForWrite> BuildForWrite(int inventoryId, List<DynamicFieldModel> models)
        {
            var businessModels = new List<InventoryValueForWrite>();
            if (models != null)
            {
                foreach (DynamicFieldModel model in models)
                {
                    string value = model.Value;

                    if (model.FieldTypes == FieldTypes.Bool)
                    {
                        value = model.Value == MvcTrue ? True : False;
                    }

                    businessModels.Add(new InventoryValueForWrite(inventoryId, model.InventoryTypePropertyId, value));
                }
            }

            return businessModels;
        }
    }
}
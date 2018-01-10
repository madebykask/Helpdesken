namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory;

    public class DynamicsFieldsModelBuilder : IDynamicsFieldsModelBuilder
    {
        public List<DynamicFieldModel> BuildViewModel(
            List<InventoryValue> dynamicData,
            List<InventoryDynamicFieldSettingForModelEdit> settings,
            int inventoryId)
        {
            var data = new List<DynamicFieldModel>();

            foreach (var setting in settings)
            {
                var dynamicField =
                    dynamicData.SingleOrDefault(
                        x => x.InventoryTypePropertyId == setting.Id && x.InventoryId == inventoryId);

                var dynamicFieldModel = new DynamicFieldModel(
                    setting.Id,
                    setting.InventoryTypeGroupId ?? 0,
                    setting.Caption,
                    setting.Position,
                    setting.PropertySize,
                    setting.FieldType,
                    dynamicField != null ? dynamicField.Value : null,
                    setting.IsReadOnly);

                data.Add(dynamicFieldModel);
            }

            return data;
        }

        public List<DynamicFieldModel> BuildViewModel(List<InventoryDynamicFieldSettingForModelEdit> settings)
        {
            var data = new List<DynamicFieldModel>();

            foreach (var setting in settings)
            {
                var dynamicFieldModel = new DynamicFieldModel(
                    setting.Id,
                    setting.InventoryTypeGroupId ?? 0,
                    setting.Caption,
                    setting.Position,
                    setting.PropertySize,
                    setting.FieldType,
                    null,
                    setting.IsReadOnly);

                data.Add(dynamicFieldModel);
            }

            return data;
        }
    }
}
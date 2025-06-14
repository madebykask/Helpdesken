﻿namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory;

    public interface IDynamicsFieldsModelBuilder
    {
        List<DynamicFieldModel> BuildViewModel(
            List<InventoryValue> dynamicData,
            List<InventoryDynamicFieldSettingForModelEdit> settings,
            int inventoryId);

        List<DynamicFieldModel> BuildViewModel(List<InventoryDynamicFieldSettingForModelEdit> settings);
    }
}
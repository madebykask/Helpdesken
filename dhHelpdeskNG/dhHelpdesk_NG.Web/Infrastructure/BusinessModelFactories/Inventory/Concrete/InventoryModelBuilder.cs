namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory;

    public class InventoryModelBuilder : IInventoryModelBuilder
    {
        public InventoryForUpdate BuildForUpdate(InventoryViewModel model, OperationContext contex)
        {
            var fieldsModel = model.DefaultFieldsViewModel.DefaultFieldsModel;

            var departmentId = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.DepartmentId);
            var name = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Name);
            var inventoryModel = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Model);
            var manufacturer = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Manufacturer);
            var serialNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.SerialNumber);
            var theftMark = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.TheftMark);

            var barCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.BarCode);
            var purchaseDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.PurchaseDate);

            var info = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Info);

            var roomId = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.RoomId);

            var businessModel = new InventoryForUpdate(
                model.Id,
                departmentId,
                roomId,
                name,
                inventoryModel,
                manufacturer,
                serialNumber,
                theftMark,
                barCode,
                purchaseDate,
                info,
                contex.DateAndTime,
                contex.UserId);

            return businessModel;
        }

        public InventoryForInsert BuildForAdd(InventoryViewModel model, OperationContext context)
        {
            var fieldsModel = model.DefaultFieldsViewModel.DefaultFieldsModel;

            var departmentId = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.DepartmentId);
            var name = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Name);
            var inventoryModel = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Model);
            var manufacturer = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Manufacturer);
            var serialNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.SerialNumber);
            var theftMark = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.TheftMark);

            var barCode = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.BarCode);
            var purchaseDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(fieldsModel.PurchaseDate);

            var info = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Info);

            var roomId = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.RoomId);

            var businessModel = new InventoryForInsert(
                departmentId,
                roomId,
                name,
                inventoryModel,
                manufacturer,
                serialNumber,
                theftMark,
                barCode,
                purchaseDate,
                info,
                context.DateAndTime,
                model.InventoryTypeId,
                context.UserId);

            return businessModel;
        }
    }
}
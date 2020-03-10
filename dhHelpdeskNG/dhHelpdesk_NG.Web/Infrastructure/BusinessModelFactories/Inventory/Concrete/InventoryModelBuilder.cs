namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory;

    public class InventoryModelBuilder : IInventoryModelBuilder
    {
        public InventoryForUpdate BuildForUpdate(InventoryViewModel model, OperationContext context)
        {
            if (model.DefaultFieldsViewModel == null || model.DefaultFieldsViewModel.DefaultFieldsModel == null)
            {
                return CreateDefaultInventoryForUpdate(model.Id, context.DateAndTime, context.UserId);
            }

            var fieldsModel = model.DefaultFieldsViewModel.DefaultFieldsModel;

            var departmentId = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.DepartmentId);
            var name = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Name);
            var inventoryModel = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Model);
            var manufacturer = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Manufacturer);
            var serialNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.SerialNumber);
            var theftMark = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.TheftMark);
            var computerTypeId = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.Type);


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
                computerTypeId,
                barCode,
                purchaseDate,
                info,
                context.DateAndTime,
                context.UserId);

            return businessModel;
        }

        public InventoryForInsert BuildForAdd(InventoryViewModel model, OperationContext context)
        {
            if (model.DefaultFieldsViewModel == null || model.DefaultFieldsViewModel.DefaultFieldsModel == null)
            {
                return CreateDefaultInventoryForInsert(context.DateAndTime, model.InventoryTypeId, context.UserId);
            }

            var fieldsModel = model.DefaultFieldsViewModel.DefaultFieldsModel;

            var departmentId = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.DepartmentId);
            var name = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Name);
            var inventoryModel = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Model);
            var manufacturer = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.Manufacturer);
            var serialNumber = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.SerialNumber);
            var theftMark = ConfigurableFieldModel<string>.GetValueOrDefault(fieldsModel.TheftMark);
            var computerTypeId = ConfigurableFieldModel<int?>.GetValueOrDefault(fieldsModel.Type);

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
                computerTypeId,
                barCode,
                purchaseDate,
                info,
                context.DateAndTime,
                model.InventoryTypeId,
                context.UserId);

            return businessModel;
        }

        private static InventoryForInsert CreateDefaultInventoryForInsert(
            DateTime dateAndTime,
            int inventoryTypeId,
            int? userId)
        {
            return new InventoryForInsert(
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                dateAndTime,
                inventoryTypeId,
                userId);
        }

        // todo refactor this
        private static InventoryForUpdate CreateDefaultInventoryForUpdate(int id, DateTime dateAndTime, int? userId)
        {
            return new InventoryForUpdate(
                id,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                dateAndTime,
                userId);
        }
    }
}
namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Orders.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;

    public sealed class OrderRestorer : Restorer, IOrderRestorer
    {
        public void Restore(FullOrderEditFields updatedOrder, FullOrderEditFields existingOrder, FullOrderEditSettings settings)
        {
            this.RestoreDelivery(updatedOrder.Delivery, existingOrder.Delivery, settings.Delivery);
            this.RestoreGeneral(updatedOrder.General, existingOrder.General, settings.General);
            this.RestoreLog(updatedOrder.Log, existingOrder.Log, settings.Log);
            this.RestoreOrderer(updatedOrder.Orderer, existingOrder.Orderer, settings.Orderer);
            this.RestoreOrder(updatedOrder.Order, existingOrder.Order, settings.Order);
            this.RestoreOther(updatedOrder.Other, existingOrder.Other, settings.Other);
            this.RestoreProgram(updatedOrder.Program, existingOrder.Program, settings.Program);
            this.RestoreReceiver(updatedOrder.Receiver, existingOrder.Receiver, settings.Receiver);
            this.RestoreSupplier(updatedOrder.Supplier, existingOrder.Supplier, settings.Supplier);
            this.RestoreUser(updatedOrder.User, existingOrder.User, settings.User);
            this.RestoreAccountInfo(updatedOrder.AccountInfo, existingOrder.AccountInfo, settings.AccountInfo);
        }

        private void RestoreDelivery(
            DeliveryEditFields updated,
            DeliveryEditFields existing,
            DeliveryEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryDate, existing.DeliveryDate, settings.DeliveryDate.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.InstallDate, existing.InstallDate, settings.InstallDate.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryDepartmentId, existing.DeliveryDepartmentId, settings.DeliveryDepartment.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryOu, existing.DeliveryOu, settings.DeliveryOu.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryAddress, existing.DeliveryAddress, settings.DeliveryAddress.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryPostalCode, existing.DeliveryPostalCode, settings.DeliveryPostalCode.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryPostalAddress, existing.DeliveryPostalAddress, settings.DeliveryPostalAddress.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryLocation, existing.DeliveryLocation, settings.DeliveryLocation.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryInfo1, existing.DeliveryInfo1, settings.DeliveryInfo1.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryInfo2, existing.DeliveryInfo2, settings.DeliveryInfo2.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryInfo3, existing.DeliveryInfo3, settings.DeliveryInfo3.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DeliveryOuIdId, existing.DeliveryOuIdId, settings.DeliveryOuId.Show);
        }

        private void RestoreGeneral(
            GeneralEditFields updated,
            GeneralEditFields existing,
            GeneralEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.OrderNumber, existing.OrderNumber, settings.OrderNumber.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.Customer, existing.Customer, settings.Customer.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.AdministratorId, existing.AdministratorId, settings.Administrator.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DomainId, existing.DomainId, settings.Domain.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderDate, existing.OrderDate, settings.OrderDate.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.StatusId, existing.StatusId, settings.Status.Show);
        }

        private void RestoreLog(
            LogEditFields updated,
            LogEditFields existing,
            LogEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.Logs, existing.Logs, settings.Log.Show);
        }

        private void RestoreOrderer(
            OrdererEditFields updated,
            OrdererEditFields existing,
            OrdererEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.OrdererId, existing.OrdererId, settings.OrdererId.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrdererName, existing.OrdererName, settings.OrdererName.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrdererLocation, existing.OrdererLocation, settings.OrdererLocation.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrdererEmail, existing.OrdererEmail, settings.OrdererEmail.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrdererPhone, existing.OrdererPhone, settings.OrdererPhone.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrdererCode, existing.OrdererCode, settings.OrdererCode.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.DepartmentId, existing.DepartmentId, settings.Department.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UnitId, existing.UnitId, settings.Unit.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrdererAddress, existing.OrdererAddress, settings.OrdererAddress.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrdererInvoiceAddress, existing.OrdererInvoiceAddress, settings.OrdererInvoiceAddress.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrdererReferenceNumber, existing.OrdererReferenceNumber, settings.OrdererReferenceNumber.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.AccountingDimension1, existing.AccountingDimension1, settings.AccountingDimension1.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.AccountingDimension2, existing.AccountingDimension2, settings.AccountingDimension2.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.AccountingDimension3, existing.AccountingDimension3, settings.AccountingDimension3.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.AccountingDimension4, existing.AccountingDimension4, settings.AccountingDimension4.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.AccountingDimension5, existing.AccountingDimension5, settings.AccountingDimension5.Show);
        }

        private void RestoreOrder(
            OrderEditFields updated,
            OrderEditFields existing,
            OrderEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.PropertyId, existing.PropertyId, settings.Property.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderRow1, existing.OrderRow1, settings.OrderRow1.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderRow2, existing.OrderRow2, settings.OrderRow2.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderRow3, existing.OrderRow3, settings.OrderRow3.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderRow4, existing.OrderRow4, settings.OrderRow4.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderRow5, existing.OrderRow5, settings.OrderRow5.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderRow6, existing.OrderRow6, settings.OrderRow6.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderRow7, existing.OrderRow7, settings.OrderRow7.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderRow8, existing.OrderRow8, settings.OrderRow8.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.Configuration, existing.Configuration, settings.Configuration.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderInfo, existing.OrderInfo, settings.OrderInfo.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.OrderInfo2, existing.OrderInfo2, settings.OrderInfo2.Show);
        }

        private void RestoreOther(
            OtherEditFields updated,
            OtherEditFields existing,
            OtherEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.FileName, existing.FileName, settings.FileName.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.CaseNumber, existing.CaseNumber, settings.CaseNumber.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.Info, existing.Info, settings.Info.Show);
        }

        private void RestoreProgram(
            ProgramEditFields updated,
            ProgramEditFields existing,
            ProgramEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.Programs, existing.Programs, settings.Program.Show);
        }

        private void RestoreReceiver(
            ReceiverEditFields updated,
            ReceiverEditFields existing,
            ReceiverEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.ReceiverId, existing.ReceiverId, settings.ReceiverId.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.ReceiverName, existing.ReceiverName, settings.ReceiverName.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.ReceiverEmail, existing.ReceiverEmail, settings.ReceiverEmail.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.ReceiverPhone, existing.ReceiverPhone, settings.ReceiverPhone.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.ReceiverLocation, existing.ReceiverLocation, settings.ReceiverLocation.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.MarkOfGoods, existing.MarkOfGoods, settings.MarkOfGoods.Show);
        }

        private void RestoreSupplier(
            SupplierEditFields updated,
            SupplierEditFields existing,
            SupplierEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.SupplierOrderNumber, existing.SupplierOrderNumber, settings.SupplierOrderNumber.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.SupplierOrderDate, existing.SupplierOrderDate, settings.SupplierOrderDate.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.SupplierOrderInfo, existing.SupplierOrderInfo, settings.SupplierOrderInfo.Show);
        }

        private void RestoreUser(
            UserEditFields updated,
            UserEditFields existing,
            UserEditSettings settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.UserId, existing.UserId, settings.UserId.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserFirstName, existing.UserFirstName, settings.UserFirstName.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserLastName, existing.UserLastName, settings.UserLastName.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserPhone, existing.UserPhone, settings.UserPhone.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserEMail, existing.UserEMail, settings.UserEMail.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserExtension, existing.UserExtension, settings.Extension.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserInitials, existing.UserInitials, settings.Initials.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserLocation, existing.UserLocation, settings.Location.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserPersonalIdentityNumber, existing.UserPersonalIdentityNumber, settings.PersonalIdentityNumber.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserPostalAddress, existing.UserPostalAddress, settings.PostalAddress.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserRoomNumber, existing.UserRoomNumber, settings.RoomNumber.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserTitle, existing.UserTitle, settings.Title.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserDepartment_Id1, existing.UserDepartment_Id1, settings.DepartmentId1.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserDepartment_Id2, existing.UserDepartment_Id2, settings.DepartmentId2.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.UserOU_Id, existing.UserOU_Id, settings.UnitId.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.InfoUser, existing.InfoUser, settings.Info.Show);
            this.RestoreFieldIfNeeded(updated, () => updated.EmploymentType_Id, existing.EmploymentType_Id, settings.EmploymentType.Show);
        }
        private void RestoreAccountInfo(
                    AccountInfoEditFields updated,
                    AccountInfoEditFields existing,
                    AccountInfoEditSettings settings)
        {
            RestoreFieldIfNeeded(updated, () => updated.StartedDate, existing.StartedDate, settings.StartedDate.Show);
            RestoreFieldIfNeeded(updated, () => updated.FinishDate, existing.FinishDate, settings.FinishDate.Show);
            RestoreFieldIfNeeded(updated, () => updated.EMailTypeId, existing.EMailTypeId, settings.EMailTypeId.Show);
            RestoreFieldIfNeeded(updated, () => updated.HomeDirectory, existing.HomeDirectory, settings.HomeDirectory.Show);
            RestoreFieldIfNeeded(updated, () => updated.Profile, existing.Profile, settings.Profile.Show);
            RestoreFieldIfNeeded(updated, () => updated.InventoryNumber, existing.InventoryNumber, settings.InventoryNumber.Show);
            RestoreFieldIfNeeded(updated, () => updated.Info, existing.Info, settings.Info.Show);
        }
    }
}
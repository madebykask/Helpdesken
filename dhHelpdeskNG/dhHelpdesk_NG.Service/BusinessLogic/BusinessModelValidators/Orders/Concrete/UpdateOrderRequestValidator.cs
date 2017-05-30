namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Orders.Concrete
{
    using DH.Helpdesk.BusinessData.Enums.Orders.Fields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;

    public sealed class UpdateOrderRequestValidator : IUpdateOrderRequestValidator
    {
        private readonly IElementaryRulesValidator elementaryRulesValidator;

        public UpdateOrderRequestValidator(IElementaryRulesValidator elementaryRulesValidator)
        {
            this.elementaryRulesValidator = elementaryRulesValidator;
        }

        public void Validate(FullOrderEditFields updatedOrder, FullOrderEditFields existingOrder, FullOrderEditSettings settings)
        {
            this.ValidateDeliveryFields(updatedOrder.Delivery, existingOrder.Delivery, settings.Delivery);
            this.ValidateGeneralFields(updatedOrder.General, existingOrder.General, settings.General);
            this.ValidateLogFields(updatedOrder.Log, existingOrder.Log, settings.Log);
            this.ValidateOrdererFields(updatedOrder.Orderer, existingOrder.Orderer, settings.Orderer);
            this.ValidateOrderFields(updatedOrder.Order, existingOrder.Order, settings.Order);
            this.ValidateOtherFields(updatedOrder.Other, existingOrder.Other, settings.Other);
            this.ValidateProgramFields(updatedOrder.Program, existingOrder.Program, settings.Program);
            this.ValidateReceiverFields(updatedOrder.Receiver, existingOrder.Receiver, settings.Receiver);
            this.ValidateSupplierFields(updatedOrder.Supplier, existingOrder.Supplier, settings.Supplier);
            this.ValidateUserFields(updatedOrder.User, existingOrder.User, settings.User);
            ValidateAccountInfoFields(updatedOrder.AccountInfo, existingOrder.AccountInfo, settings.AccountInfo);
        }

        private void ValidateDeliveryFields(
            DeliveryEditFields updatedFields,
            DeliveryEditFields existingFields,
            DeliveryEditSettings settings)
        {
            this.elementaryRulesValidator.ValidateDateTimeField(updatedFields.DeliveryDate, existingFields.DeliveryDate, OrderFields.DeliveryDate, new ElementaryValidationRule(!settings.DeliveryDate.Show, settings.DeliveryDate.Required));
            this.elementaryRulesValidator.ValidateDateTimeField(updatedFields.InstallDate, existingFields.InstallDate, OrderFields.DeliveryInstallDate, new ElementaryValidationRule(!settings.InstallDate.Show, settings.InstallDate.Required));
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.DeliveryDepartmentId, existingFields.DeliveryDepartmentId, OrderFields.DeliveryDepartment, new ElementaryValidationRule(!settings.DeliveryDepartment.Show, settings.DeliveryDepartment.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryOu, existingFields.DeliveryOu, OrderFields.DeliveryOu, new ElementaryValidationRule(!settings.DeliveryOu.Show, settings.DeliveryOu.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryAddress, existingFields.DeliveryAddress, OrderFields.DeliveryAddress, new ElementaryValidationRule(!settings.DeliveryAddress.Show, settings.DeliveryAddress.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryPostalCode, existingFields.DeliveryPostalCode, OrderFields.DeliveryPostalCode, new ElementaryValidationRule(!settings.DeliveryPostalCode.Show, settings.DeliveryPostalCode.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryPostalAddress, existingFields.DeliveryPostalAddress, OrderFields.DeliveryPostalAddress, new ElementaryValidationRule(!settings.DeliveryPostalAddress.Show, settings.DeliveryPostalAddress.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryLocation, existingFields.DeliveryLocation, OrderFields.DeliveryLocation, new ElementaryValidationRule(!settings.DeliveryLocation.Show, settings.DeliveryLocation.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryInfo1, existingFields.DeliveryInfo1, OrderFields.DeliveryInfo1, new ElementaryValidationRule(!settings.DeliveryInfo1.Show, settings.DeliveryInfo1.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryInfo2, existingFields.DeliveryInfo2, OrderFields.DeliveryInfo2, new ElementaryValidationRule(!settings.DeliveryInfo2.Show, settings.DeliveryInfo2.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryInfo3, existingFields.DeliveryInfo3, OrderFields.DeliveryInfo3, new ElementaryValidationRule(!settings.DeliveryInfo3.Show, settings.DeliveryInfo3.Required));
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.DeliveryOuIdId, existingFields.DeliveryOuIdId, OrderFields.DeliveryOuId, new ElementaryValidationRule(!settings.DeliveryOuId.Show, settings.DeliveryOuId.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryName, existingFields.DeliveryName, OrderFields.DeliveryName, new ElementaryValidationRule(!settings.DeliveryName.Show, settings.DeliveryName.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.DeliveryPhone, existingFields.DeliveryPhone, OrderFields.DeliveryPhone, new ElementaryValidationRule(!settings.DeliveryPhone.Show, settings.DeliveryPhone.Required));
        }

        private void ValidateGeneralFields(
            GeneralEditFields updatedFields,
            GeneralEditFields existingFields,
            GeneralEditSettings settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.AdministratorId, existingFields.AdministratorId, OrderFields.GeneralAdministrator, new ElementaryValidationRule(!settings.Administrator.Show, settings.Administrator.Required));
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.DomainId, existingFields.DomainId, OrderFields.GeneralDomain, new ElementaryValidationRule(!settings.Domain.Show, settings.Domain.Required));
            this.elementaryRulesValidator.ValidateDateTimeField(updatedFields.OrderDate, existingFields.OrderDate, OrderFields.GeneralOrderDate, new ElementaryValidationRule(!settings.OrderDate.Show, settings.OrderDate.Required));
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.StatusId, existingFields.StatusId, OrderFields.GeneralStatus, new ElementaryValidationRule(!settings.Status.Show, settings.Status.Required));                        
        }

        private void ValidateLogFields(
            LogEditFields updatedFields,
            LogEditFields existingFields,
            LogEditSettings settings)
        {            
        }

        private void ValidateOrdererFields(
            OrdererEditFields updatedFields,
            OrdererEditFields existingFields,
            OrdererEditSettings settings)
        {
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrdererId, existingFields.OrdererId, OrderFields.OrdererId, new ElementaryValidationRule(!settings.OrdererId.Show, settings.OrdererId.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrdererName, existingFields.OrdererName, OrderFields.OrdererName, new ElementaryValidationRule(!settings.OrdererName.Show, settings.OrdererName.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrdererLocation, existingFields.OrdererLocation, OrderFields.OrdererLocation, new ElementaryValidationRule(!settings.OrdererLocation.Show, settings.OrdererLocation.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrdererEmail, existingFields.OrdererEmail, OrderFields.OrdererEmail, new ElementaryValidationRule(!settings.OrdererEmail.Show, settings.OrdererEmail.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrdererPhone, existingFields.OrdererPhone, OrderFields.OrdererPhone, new ElementaryValidationRule(!settings.OrdererPhone.Show, settings.OrdererPhone.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrdererCode, existingFields.OrdererCode, OrderFields.OrdererCode, new ElementaryValidationRule(!settings.OrdererCode.Show, settings.OrdererCode.Required));            
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.DepartmentId, existingFields.DepartmentId, OrderFields.OrdererDepartment, new ElementaryValidationRule(!settings.Department.Show, settings.Department.Required));            
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.UnitId, existingFields.UnitId, OrderFields.OrdererUnit, new ElementaryValidationRule(!settings.Unit.Show, settings.Unit.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrdererAddress, existingFields.OrdererAddress, OrderFields.OrdererAddress, new ElementaryValidationRule(!settings.OrdererAddress.Show, settings.OrdererAddress.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrdererInvoiceAddress, existingFields.OrdererInvoiceAddress, OrderFields.OrdererInvoiceAddress, new ElementaryValidationRule(!settings.OrdererInvoiceAddress.Show, settings.OrdererInvoiceAddress.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrdererReferenceNumber, existingFields.OrdererReferenceNumber, OrderFields.OrdererReferenceNumber, new ElementaryValidationRule(!settings.OrdererReferenceNumber.Show, settings.OrdererReferenceNumber.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.AccountingDimension1, existingFields.AccountingDimension1, OrderFields.OrdererAccountingDimension1, new ElementaryValidationRule(!settings.AccountingDimension1.Show, settings.AccountingDimension1.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.AccountingDimension2, existingFields.AccountingDimension2, OrderFields.OrdererAccountingDimension2, new ElementaryValidationRule(!settings.AccountingDimension2.Show, settings.AccountingDimension2.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.AccountingDimension3, existingFields.AccountingDimension3, OrderFields.OrdererAccountingDimension3, new ElementaryValidationRule(!settings.AccountingDimension3.Show, settings.AccountingDimension3.Required));            
            this.elementaryRulesValidator.ValidateStringField(updatedFields.AccountingDimension4, existingFields.AccountingDimension4, OrderFields.OrdererAccountingDimension4, new ElementaryValidationRule(!settings.AccountingDimension4.Show, settings.AccountingDimension4.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.AccountingDimension5, existingFields.AccountingDimension5, OrderFields.OrdererAccountingDimension5, new ElementaryValidationRule(!settings.AccountingDimension5.Show, settings.AccountingDimension5.Required));            
        }

        private void ValidateOrderFields(
            OrderEditFields updatedFields,
            OrderEditFields existingFields,
            OrderEditSettings settings)
        {
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.PropertyId, existingFields.PropertyId, OrderFields.OrderProperty, new ElementaryValidationRule(!settings.Property.Show, settings.Property.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrderRow1, existingFields.OrderRow1, OrderFields.OrderRow1, new ElementaryValidationRule(!settings.OrderRow1.Show, settings.OrderRow1.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrderRow2, existingFields.OrderRow2, OrderFields.OrderRow2, new ElementaryValidationRule(!settings.OrderRow2.Show, settings.OrderRow2.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrderRow3, existingFields.OrderRow3, OrderFields.OrderRow3, new ElementaryValidationRule(!settings.OrderRow3.Show, settings.OrderRow3.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrderRow4, existingFields.OrderRow4, OrderFields.OrderRow4, new ElementaryValidationRule(!settings.OrderRow4.Show, settings.OrderRow4.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrderRow5, existingFields.OrderRow5, OrderFields.OrderRow5, new ElementaryValidationRule(!settings.OrderRow5.Show, settings.OrderRow5.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrderRow6, existingFields.OrderRow6, OrderFields.OrderRow6, new ElementaryValidationRule(!settings.OrderRow6.Show, settings.OrderRow6.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrderRow7, existingFields.OrderRow7, OrderFields.OrderRow7, new ElementaryValidationRule(!settings.OrderRow7.Show, settings.OrderRow7.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrderRow8, existingFields.OrderRow8, OrderFields.OrderRow8, new ElementaryValidationRule(!settings.OrderRow8.Show, settings.OrderRow8.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.Configuration, existingFields.Configuration, OrderFields.OrderConfiguration, new ElementaryValidationRule(!settings.Configuration.Show, settings.Configuration.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.OrderInfo, existingFields.OrderInfo, OrderFields.OrderInfo, new ElementaryValidationRule(!settings.OrderInfo.Show, settings.OrderInfo.Required));
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.OrderInfo2, existingFields.OrderInfo2, OrderFields.OrderInfo2, new ElementaryValidationRule(!settings.OrderInfo2.Show, settings.OrderInfo2.Required));                        
        }

        private void ValidateOtherFields(
            OtherEditFields updatedFields,
            OtherEditFields existingFields,
            OtherEditSettings settings)
        {
            this.elementaryRulesValidator.ValidateStringField(updatedFields.FileName, existingFields.FileName, OrderFields.OtherFileName, new ElementaryValidationRule(!settings.FileName.Show, settings.FileName.Required));                        
            //this.elementaryRulesValidator.ValidateDecimalField(updatedFields.CaseNumber, existingFields.CaseNumber, OtherFields.CaseNumber, new ElementaryValidationRule(!settings.CaseNumber.Show, settings.CaseNumber.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.Info, existingFields.Info, OrderFields.OtherInfo, new ElementaryValidationRule(!settings.Info.Show, settings.Info.Required));
        }

        private void ValidateProgramFields(
            ProgramEditFields updatedFields,
            ProgramEditFields existingFields,
            ProgramEditSettings settings)
        {
            this.elementaryRulesValidator.ValidateStringField(updatedFields.InfoProduct, existingFields.InfoProduct, OrderFields.ProgramInfoProduct, new ElementaryValidationRule(!settings.InfoProduct.Show, settings.InfoProduct.Required));
        }

        private void ValidateReceiverFields(
            ReceiverEditFields updatedFields,
            ReceiverEditFields existingFields,
            ReceiverEditSettings settings)
        {
            this.elementaryRulesValidator.ValidateStringField(updatedFields.ReceiverId, existingFields.ReceiverId, OrderFields.ReceiverId, new ElementaryValidationRule(!settings.ReceiverId.Show, settings.ReceiverId.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.ReceiverName, existingFields.ReceiverName, OrderFields.ReceiverName, new ElementaryValidationRule(!settings.ReceiverName.Show, settings.ReceiverName.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.ReceiverEmail, existingFields.ReceiverEmail, OrderFields.ReceiverEmail, new ElementaryValidationRule(!settings.ReceiverEmail.Show, settings.ReceiverEmail.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.ReceiverPhone, existingFields.ReceiverPhone, OrderFields.ReceiverPhone, new ElementaryValidationRule(!settings.ReceiverPhone.Show, settings.ReceiverPhone.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.ReceiverLocation, existingFields.ReceiverLocation, OrderFields.ReceiverLocation, new ElementaryValidationRule(!settings.ReceiverLocation.Show, settings.ReceiverLocation.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.MarkOfGoods, existingFields.MarkOfGoods, OrderFields.ReceiverMarkOfGoods, new ElementaryValidationRule(!settings.MarkOfGoods.Show, settings.MarkOfGoods.Required));                        
        }

        private void ValidateSupplierFields(
            SupplierEditFields updatedFields,
            SupplierEditFields existingFields,
            SupplierEditSettings settings)
        {
            this.elementaryRulesValidator.ValidateStringField(updatedFields.SupplierOrderNumber, existingFields.SupplierOrderNumber, OrderFields.SupplierOrderNumber, new ElementaryValidationRule(!settings.SupplierOrderNumber.Show, settings.SupplierOrderNumber.Required));                        
            this.elementaryRulesValidator.ValidateDateTimeField(updatedFields.SupplierOrderDate, existingFields.SupplierOrderDate, OrderFields.SupplierOrderDate, new ElementaryValidationRule(!settings.SupplierOrderDate.Show, settings.SupplierOrderDate.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.SupplierOrderInfo, existingFields.SupplierOrderInfo, OrderFields.SupplierOrderInfo, new ElementaryValidationRule(!settings.SupplierOrderInfo.Show, settings.SupplierOrderInfo.Required));                        
        }

        private void ValidateUserFields(
            UserEditFields updatedFields,
            UserEditFields existingFields,
            UserEditSettings settings)
        {
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserId, existingFields.UserId, OrderFields.UserId, new ElementaryValidationRule(!settings.UserId.Show, settings.UserId.Required));                        
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserFirstName, existingFields.UserFirstName, OrderFields.UserFirstName, new ElementaryValidationRule(!settings.UserFirstName.Show, settings.UserFirstName.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserLastName, existingFields.UserLastName, OrderFields.UserLastName, new ElementaryValidationRule(!settings.UserLastName.Show, settings.UserLastName.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserPhone, existingFields.UserPhone, OrderFields.UserPhone, new ElementaryValidationRule(!settings.UserPhone.Show, settings.UserPhone.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserEMail, existingFields.UserEMail, OrderFields.UserEMail, new ElementaryValidationRule(!settings.UserEMail.Show, settings.UserEMail.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserExtension, existingFields.UserExtension, OrderFields.UserExtension, new ElementaryValidationRule(!settings.Extension.Show, settings.Extension.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserInitials, existingFields.UserInitials, OrderFields.UserInitials, new ElementaryValidationRule(!settings.Initials.Show, settings.Initials.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserLocation, existingFields.UserLocation, OrderFields.UserLocation, new ElementaryValidationRule(!settings.Location.Show, settings.Location.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserPersonalIdentityNumber, existingFields.UserPersonalIdentityNumber, OrderFields.UserPersonalIdentityNumber, new ElementaryValidationRule(!settings.PersonalIdentityNumber.Show, settings.PersonalIdentityNumber.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserPostalAddress, existingFields.UserPostalAddress, OrderFields.UserPostalAddress, new ElementaryValidationRule(!settings.PostalAddress.Show, settings.PostalAddress.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserRoomNumber, existingFields.UserRoomNumber, OrderFields.UserRoomNumber, new ElementaryValidationRule(!settings.RoomNumber.Show, settings.RoomNumber.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.UserTitle, existingFields.UserTitle, OrderFields.UserTitle, new ElementaryValidationRule(!settings.Title.Show, settings.Title.Required));
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.UserDepartment_Id1, existingFields.UserDepartment_Id1, OrderFields.UserDepartment_Id1, new ElementaryValidationRule(!settings.DepartmentId1.Show, settings.DepartmentId1.Required));
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.UserDepartment_Id2, existingFields.UserDepartment_Id2, OrderFields.UserDepartment_Id2, new ElementaryValidationRule(!settings.DepartmentId2.Show, settings.DepartmentId2.Required));
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.UserOU_Id, existingFields.UserOU_Id, OrderFields.UserOU_Id, new ElementaryValidationRule(!settings.UnitId.Show, settings.UnitId.Required));
            this.elementaryRulesValidator.ValidateStringField(updatedFields.InfoUser, existingFields.InfoUser, OrderFields.UserInfo, new ElementaryValidationRule(!settings.Info.Show, settings.Info.Required));
            this.elementaryRulesValidator.ValidateIntegerField(updatedFields.EmploymentType_Id, existingFields.EmploymentType_Id, OrderFields.UserEmploymentType, new ElementaryValidationRule(!settings.EmploymentType.Show, settings.EmploymentType.Required));
        }

        private void ValidateAccountInfoFields(
            AccountInfoEditFields updatedFields,
            AccountInfoEditFields existingFields,
            AccountInfoEditSettings settings)
        {
            elementaryRulesValidator.ValidateDateTimeField(updatedFields.StartedDate, existingFields.StartedDate, OrderFields.AccountInfoStartedDate, new ElementaryValidationRule(!settings.StartedDate.Show, settings.StartedDate.Required));
            elementaryRulesValidator.ValidateDateTimeField(updatedFields.FinishDate, existingFields.FinishDate, OrderFields.AccountInfoFinishDate, new ElementaryValidationRule(!settings.FinishDate.Show, settings.FinishDate.Required));
            elementaryRulesValidator.ValidateIntegerField((int?)updatedFields.EMailTypeId, (int?)existingFields.EMailTypeId, OrderFields.AccountInfoEMailTypeId, new ElementaryValidationRule(!settings.EMailTypeId.Show, settings.EMailTypeId.Required));
            elementaryRulesValidator.ValidateBooleanField(updatedFields.HomeDirectory, existingFields.HomeDirectory, OrderFields.AccountInfoHomeDirectory, new ElementaryValidationRule(!settings.HomeDirectory.Show, settings.HomeDirectory.Required));
            elementaryRulesValidator.ValidateBooleanField(updatedFields.Profile, existingFields.Profile, OrderFields.AccountInfoProfile, new ElementaryValidationRule(!settings.Profile.Show, settings.Profile.Required));
            elementaryRulesValidator.ValidateStringField(updatedFields.InventoryNumber, existingFields.InventoryNumber, OrderFields.AccountInfoInventoryNumber, new ElementaryValidationRule(!settings.InventoryNumber.Show, settings.InventoryNumber.Required));
            elementaryRulesValidator.ValidateIntegerField(updatedFields.AccountTypeId, existingFields.AccountTypeId, OrderFields.AccountInfoAccountType, new ElementaryValidationRule(!settings.AccountTypeId.Show, settings.AccountTypeId.Required));
            //elementaryRulesValidator.ValidateStringField(updatedFields.AccountTypeId2, existingFields.AccountTypeId2, AccountInfoFields.AccountType2, new ElementaryValidationRule(!settings.AccountTypeId2.Show, settings.AccountTypeId2.Required));
            elementaryRulesValidator.ValidateIntegerField(updatedFields.AccountTypeId3, existingFields.AccountTypeId3, OrderFields.AccountInfoAccountType3, new ElementaryValidationRule(!settings.AccountTypeId3.Show, settings.AccountTypeId3.Required));
            elementaryRulesValidator.ValidateIntegerField(updatedFields.AccountTypeId4, existingFields.AccountTypeId4, OrderFields.AccountInfoAccountType4, new ElementaryValidationRule(!settings.AccountTypeId4.Show, settings.AccountTypeId4.Required));
            elementaryRulesValidator.ValidateIntegerField(updatedFields.AccountTypeId5, existingFields.AccountTypeId5, OrderFields.AccountInfoAccountType5, new ElementaryValidationRule(!settings.AccountTypeId5.Show, settings.AccountTypeId5.Required));
            elementaryRulesValidator.ValidateStringField(updatedFields.Info, existingFields.Info, OrderFields.AccountInfo, new ElementaryValidationRule(!settings.Info.Show, settings.Info.Required));
        }
    }
}
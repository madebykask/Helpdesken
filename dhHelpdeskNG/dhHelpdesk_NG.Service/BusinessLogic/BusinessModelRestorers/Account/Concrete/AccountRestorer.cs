namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Account.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Accounts;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;

    public class AccountRestorer : Restorer<FieldSetting>, IAccountRestorer
    {
        public void Restore(
            AccountForUpdate dto,
            AccountForEdit existingDto,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreOrder(dto.Orderer, existingDto, settings);
            this.RestoreUser(dto.User, existingDto, settings);
            this.RestoreAccountInformation(dto.AccountInformation, existingDto, settings);
            this.RestoreDelivery(dto.DeliveryInformation, existingDto, settings);
            this.RestoreProgram(dto.Program, existingDto, settings);
            this.RestoreOther(dto.Other, existingDto, settings);
            this.RestoreContact(dto.Contact, existingDto, settings);
        }

        protected override bool CreateValidationRule(FieldSetting setting)
        {
            return setting.IsShowInDetails;
        }

        private void RestoreOrder(
            Orderer updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.Id, existing.Orderer.Id, settings.Orderer.Id);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.FirstName,
                existing.Orderer.FirstName,
                settings.Orderer.FirstName);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.LastName,
                existing.Orderer.LastName,
                settings.Orderer.LastName);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Phone,
                existing.Orderer.Phone,
                settings.Orderer.Phone);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Email,
                existing.Orderer.Email,
                settings.Orderer.Email);
        }

        private void RestoreUser(
            User updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.Ids, existing.User.Ids, settings.User.Ids);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.PersonalIdentityNumber,
                existing.User.PersonalIdentityNumber,
                settings.User.PersonalIdentityNumber);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.FirstName,
                existing.User.FirstName,
                settings.User.FirstName);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Initials,
                existing.User.Initials,
                settings.User.Initials);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.LastName,
                existing.User.LastName,
                settings.User.LastName);
            this.RestoreFieldIfNeeded(updated, () => updated.Phone, existing.User.Phone, settings.User.Phone);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Extension,
                existing.User.Extension,
                settings.User.Extension);
            this.RestoreFieldIfNeeded(updated, () => updated.EMail, existing.User.EMail, settings.User.EMail);
            this.RestoreFieldIfNeeded(updated, () => updated.Title, existing.User.Title, settings.User.Title);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Location,
                existing.User.Location,
                settings.User.Location);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.RoomNumber,
                existing.User.RoomNumber,
                settings.User.RoomNumber);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.PostalAddress,
                existing.User.PostalAddress,
                settings.User.PostalAddress);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DepartmentId,
                existing.User.DepartmentId,
                settings.User.DepartmentId);
            this.RestoreFieldIfNeeded(updated, () => updated.UnitId, existing.User.UnitId, settings.User.UnitId);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DepartmentId2,
                existing.User.DepartmentId2,
                settings.User.DepartmentId2);
            this.RestoreFieldIfNeeded(updated, () => updated.Info, existing.User.Info, settings.User.Info);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Responsibility,
                existing.User.Responsibility,
                settings.User.Responsibility);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Activity,
                existing.User.Activity,
                settings.User.Activity);
            this.RestoreFieldIfNeeded(updated, () => updated.Manager, existing.User.Manager, settings.User.Manager);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ReferenceNumber,
                existing.User.ReferenceNumber,
                settings.User.ReferenceNumber);
        }

        private void RestoreAccountInformation(
            AccountInformation updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.StartedDate,
                existing.AccountInformation.StartedDate,
                settings.AccountInformation.StartedDate);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.FinishDate,
                existing.AccountInformation.FinishDate,
                settings.AccountInformation.FinishDate);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.EMailTypeId,
                existing.AccountInformation.EMailTypeId,
                settings.AccountInformation.EMailTypeId);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.HomeDirectory,
                existing.AccountInformation.HomeDirectory,
                settings.AccountInformation.HomeDirectory);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Profile,
                existing.AccountInformation.Profile,
                settings.AccountInformation.Profile);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.InventoryNumber,
                existing.AccountInformation.InventoryNumber,
                settings.AccountInformation.InventoryNumber);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountTypeId,
                existing.AccountInformation.AccountTypeId,
                settings.AccountInformation.AccountTypeId);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountType2,
                existing.AccountInformation.AccountType2,
                settings.AccountInformation.AccountType2);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountType3,
                existing.AccountInformation.AccountType3,
                settings.AccountInformation.AccountType3);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountType4,
                existing.AccountInformation.AccountType4,
                settings.AccountInformation.AccountType4);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountType5,
                existing.AccountInformation.AccountType5,
                settings.AccountInformation.AccountType5);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Info,
                existing.AccountInformation.Info,
                settings.AccountInformation.Info);
        }

        private void RestoreDelivery(
            DeliveryInformation updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Name,
                existing.DeliveryInformation.Name,
                settings.DeliveryInformation.Name);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Phone,
                existing.DeliveryInformation.Phone,
                settings.DeliveryInformation.Phone);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Address,
                existing.DeliveryInformation.Address,
                settings.DeliveryInformation.Address);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.PostalAddress,
                existing.DeliveryInformation.PostalAddress,
                settings.DeliveryInformation.PostalAddress);
        }

        private void RestoreContact(
           Contact updated,
           AccountForEdit existing,
           AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Ids,
                existing.Contact.Ids,
                settings.Contact.Ids);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Name,
                existing.Contact.Name,
                settings.Contact.Name);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Phone,
                existing.Contact.Phone,
                settings.Contact.Phone);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Email,
                existing.Contact.Email,
                settings.Contact.Email);
        }

        private void RestoreProgram(
            Program updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.ProgramIds,
                existing.Program.ProgramIds,
                settings.Program.Programs);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.InfoProduct,
                existing.Program.InfoProduct,
                settings.Program.InfoProduct);
        }

        private void RestoreOther(
            Other updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.Info, existing.Other.Info, settings.Other.Info);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.CaseNumber,
                existing.Other.CaseNumber,
                settings.Other.CaseNumber);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.FileName,
                existing.Other.FileName,
                settings.Other.FileName);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Content,
                existing.Other.Content,
                settings.Other.FileName);
        }
    }
}
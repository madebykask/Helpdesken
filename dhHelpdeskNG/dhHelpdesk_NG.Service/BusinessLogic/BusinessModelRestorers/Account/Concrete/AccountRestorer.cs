namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Account.Concrete
{
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
            this.RestoreOrder(dto, existingDto, settings);
            this.RestoreUser(dto, existingDto, settings);
            this.RestoreAccountInformation(dto, existingDto, settings);
            this.RestoreDelivery(dto, existingDto, settings);
            this.RestoreProgram(dto, existingDto, settings);
            this.RestoreOther(dto, existingDto, settings);
        }

        protected override bool CreateValidationRule(FieldSetting setting)
        {
            return setting.IsShowInDetails;
        }

        private void RestoreOrder(
            AccountForUpdate updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.Orderer.Id, existing.Orderer.Id, settings.Orderer.Id);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Orderer.FirstName,
                existing.Orderer.FirstName,
                settings.Orderer.FirstName);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Orderer.LastName,
                existing.Orderer.LastName,
                settings.Orderer.LastName);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Orderer.Phone,
                existing.Orderer.Phone,
                settings.Orderer.Phone);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Orderer.Email,
                existing.Orderer.Email,
                settings.Orderer.Email);
        }

        private void RestoreUser(
            AccountForUpdate updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.User.Ids, existing.User.Ids, settings.User.Ids);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.PersonalIdentityNumber,
                existing.User.PersonalIdentityNumber,
                settings.User.PersonalIdentityNumber);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.FirstName,
                existing.User.FirstName,
                settings.User.FirstName);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.Initials,
                existing.User.Initials,
                settings.User.Initials);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.LastName,
                existing.User.LastName,
                settings.User.LastName);
            this.RestoreFieldIfNeeded(updated, () => updated.User.Phone, existing.User.Phone, settings.User.Phone);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.Extension,
                existing.User.Extension,
                settings.User.Extension);
            this.RestoreFieldIfNeeded(updated, () => updated.User.EMail, existing.User.EMail, settings.User.EMail);
            this.RestoreFieldIfNeeded(updated, () => updated.User.Title, existing.User.Title, settings.User.Title);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.Location,
                existing.User.Location,
                settings.User.Location);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.RoomNumber,
                existing.User.RoomNumber,
                settings.User.RoomNumber);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.PostalAddress,
                existing.User.PostalAddress,
                settings.User.PostalAddress);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.DepartmentId,
                existing.User.DepartmentId,
                settings.User.DepartmentId);
            this.RestoreFieldIfNeeded(updated, () => updated.User.UnitId, existing.User.UnitId, settings.User.UnitId);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.DepartmentId2,
                existing.User.DepartmentId2,
                settings.User.DepartmentId2);
            this.RestoreFieldIfNeeded(updated, () => updated.User.Info, existing.User.Info, settings.User.Info);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.Responsibility,
                existing.User.Responsibility,
                settings.User.Responsibility);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.Activity,
                existing.User.Activity,
                settings.User.Activity);
            this.RestoreFieldIfNeeded(updated, () => updated.User.Manager, existing.User.Manager, settings.User.Manager);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.User.ReferenceNumber,
                existing.User.ReferenceNumber,
                settings.User.ReferenceNumber);
        }

        private void RestoreAccountInformation(
            AccountForUpdate updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.StartedDate,
                existing.AccountInformation.StartedDate,
                settings.AccountInformation.StartedDate);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.FinishDate,
                existing.AccountInformation.FinishDate,
                settings.AccountInformation.FinishDate);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.EMailTypeId,
                existing.AccountInformation.EMailTypeId,
                settings.AccountInformation.EMailTypeId);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.HomeDirectory,
                existing.AccountInformation.HomeDirectory,
                settings.AccountInformation.HomeDirectory);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.Profile,
                existing.AccountInformation.Profile,
                settings.AccountInformation.Profile);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.InventoryNumber,
                existing.AccountInformation.InventoryNumber,
                settings.AccountInformation.InventoryNumber);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.AccountTypeId,
                existing.AccountInformation.AccountTypeId,
                settings.AccountInformation.AccountTypeId);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.AccountType2,
                existing.AccountInformation.AccountType2,
                settings.AccountInformation.AccountType2);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.AccountType3,
                existing.AccountInformation.AccountType3,
                settings.AccountInformation.AccountType3);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.AccountType4,
                existing.AccountInformation.AccountType4,
                settings.AccountInformation.AccountType4);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.AccountType5,
                existing.AccountInformation.AccountType5,
                settings.AccountInformation.AccountType5);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.Info,
                existing.AccountInformation.Info,
                settings.AccountInformation.Info);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.AccountInformation.StartedDate,
                existing.AccountInformation.StartedDate,
                settings.AccountInformation.StartedDate);
        }

        private void RestoreDelivery(
            AccountForUpdate updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DeliveryInformation.Name,
                existing.DeliveryInformation.Name,
                settings.DeliveryInformation.Name);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DeliveryInformation.Phone,
                existing.DeliveryInformation.Phone,
                settings.DeliveryInformation.Phone);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DeliveryInformation.Address,
                existing.DeliveryInformation.Address,
                settings.DeliveryInformation.Address);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.DeliveryInformation.PostalAddress,
                existing.DeliveryInformation.PostalAddress,
                settings.DeliveryInformation.PostalAddress);
        }

        private void RestoreProgram(
            AccountForUpdate updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Program.ProgramIds,
                existing.Program.Programs,
                settings.Program.Programs);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Program.InfoProduct,
                existing.Program.InfoProduct,
                settings.Program.InfoProduct);
        }

        private void RestoreOther(
            AccountForUpdate updated,
            AccountForEdit existing,
            AccountFieldsSettingsForProcessing settings)
        {
            this.RestoreFieldIfNeeded(updated, () => updated.Other.Info, existing.Other.Info, settings.Other.Info);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Other.CaseNumber,
                existing.Other.CaseNumber,
                settings.Other.CaseNumber);

            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Other.FileName,
                existing.Other.FileName,
                settings.Other.FileName);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Other.FileName,
                existing.Other.FileName,
                settings.Other.FileName);
            this.RestoreFieldIfNeeded(
                updated,
                () => updated.Other.FileName,
                existing.Other.FileName,
                settings.Other.FileName);
        }
    }
}
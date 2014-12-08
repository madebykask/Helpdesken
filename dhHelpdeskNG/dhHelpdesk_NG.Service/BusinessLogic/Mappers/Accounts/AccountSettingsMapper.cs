namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.Enums.Accounts.Fields;
    using DH.Helpdesk.Domain.Accounts;

    public class AccountSettingsMapper
    {
        private static void MapOrdererFieldSettings(
            OrdererFieldSettings updatedSettings,
            NamedObjectCollection<AccountFieldSettings> existingSettings,
            DateTime changedDate)
        {
            MapFieldSettings(updatedSettings.Id, existingSettings.FindByName(OrdererFields.Id), changedDate);
            MapFieldSettings(
                updatedSettings.FirstName,
                existingSettings.FindByName(OrdererFields.FirstName),
                changedDate);
            MapFieldSettings(updatedSettings.LastName, existingSettings.FindByName(OrdererFields.LastName), changedDate);
            MapFieldSettings(updatedSettings.Phone, existingSettings.FindByName(OrdererFields.Phone), changedDate);
            MapFieldSettings(updatedSettings.Email, existingSettings.FindByName(OrdererFields.Email), changedDate);
        }

        private static void MapUserFieldSettings(
            UserFieldSettings updatedSettings,
            NamedObjectCollection<AccountFieldSettings> existingSettings,
            DateTime changedDate)
        {
            MapFieldSettingsMultipleChoices(
                updatedSettings.Ids,
                existingSettings.FindByName(UserFields.Ids),
                changedDate);
            MapFieldSettings(updatedSettings.FirstName, existingSettings.FindByName(UserFields.FirstName), changedDate);
            MapFieldSettings(updatedSettings.Initials, existingSettings.FindByName(UserFields.Initials), changedDate);
            MapFieldSettings(updatedSettings.LastName, existingSettings.FindByName(UserFields.LastName), changedDate);
            MapFieldSettingsMultipleChoices(
                updatedSettings.PersonalIdentityNumber,
                existingSettings.FindByName(UserFields.PersonalIdentityNumber),
                changedDate);
            MapFieldSettings(updatedSettings.Phone, existingSettings.FindByName(UserFields.Phone), changedDate);
            MapFieldSettings(updatedSettings.Extension, existingSettings.FindByName(UserFields.Extension), changedDate);
            MapFieldSettings(updatedSettings.EMail, existingSettings.FindByName(UserFields.EMail), changedDate);
            MapFieldSettings(updatedSettings.Title, existingSettings.FindByName(UserFields.Title), changedDate);
            MapFieldSettings(updatedSettings.Location, existingSettings.FindByName(UserFields.Location), changedDate);
            MapFieldSettings(
                updatedSettings.RoomNumber,
                existingSettings.FindByName(UserFields.RoomNumber),
                changedDate);
            MapFieldSettings(
                updatedSettings.PostalAddress,
                existingSettings.FindByName(UserFields.PostalAddress),
                changedDate);
            MapFieldSettings(
                updatedSettings.EmploymentType,
                existingSettings.FindByName(UserFields.EmploymentType),
                changedDate);
            MapFieldSettings(
                updatedSettings.DepartmentId,
                existingSettings.FindByName(UserFields.DepartmentId),
                changedDate);
            MapFieldSettings(updatedSettings.UnitId, existingSettings.FindByName(UserFields.UnitId), changedDate);
            MapFieldSettings(
                updatedSettings.DepartmentId2,
                existingSettings.FindByName(UserFields.DepartmentId2),
                changedDate);
            MapFieldSettings(updatedSettings.Info, existingSettings.FindByName(UserFields.Info), changedDate);
            MapFieldSettings(
                updatedSettings.Responsibility,
                existingSettings.FindByName(UserFields.Responsibility),
                changedDate);
            MapFieldSettings(updatedSettings.Activity, existingSettings.FindByName(UserFields.Activity), changedDate);
            MapFieldSettings(updatedSettings.Manager, existingSettings.FindByName(UserFields.Manager), changedDate);
            MapFieldSettings(
                updatedSettings.ReferenceNumber,
                existingSettings.FindByName(UserFields.ReferenceNumber),
                changedDate);
        }

        private static void MapFieldSettings(
                FieldSetting updatedSettings,
                AccountFieldSettings fieldSettings,
                DateTime changedDate)
        {
            fieldSettings.Required = updatedSettings.IsRequired.ToInt();
            fieldSettings.Show = updatedSettings.IsShowInDetails.ToInt();
            fieldSettings.ShowExternal = updatedSettings.IsShowExternal.ToInt();
            fieldSettings.ShowInList = updatedSettings.IsShowInList.ToInt();
            fieldSettings.Label = updatedSettings.Caption;
            fieldSettings.FieldHelp = updatedSettings.Help;
            fieldSettings.ChangedDate = changedDate;
        }

        private static void MapFieldSettingsMultipleChoices(
                FieldSettingMultipleChoices updatedSettings,
                AccountFieldSettings fieldSettings,
                DateTime changedDate)
        {
            MapFieldSettings(updatedSettings, fieldSettings, changedDate);
            fieldSettings.MultiValue = updatedSettings.IsMultiple.ToInt();
        }
    }
}

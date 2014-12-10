namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Services.DisplayValues.Account;
    using DH.Helpdesk.Web.Models.Shared;

    public class GridModel
    {
        public GridModel(
            List<GridColumnHeaderModel> headers,
            List<RowModel> orders,
            int currentActivity,
            SortFieldModel sortField)
        {
            this.Headers = headers;
            this.Orders = orders;
            this.CurrentActivity = currentActivity;
            this.SortField = sortField;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; set; }

        [NotNull]
        public List<RowModel> Orders { get; set; }

        public int CurrentActivity { get; set; }

        public string CurrentModeName { get; set; }

        public SortFieldModel SortField { get; set; }

        public static List<GridModel> BuildGrid(
            List<AccountOverview> accountOverviews,
            List<AccountFieldsSettingsOverviewWithActivity> settings)
        {
            List<IGrouping<int, AccountOverview>> groupedModels = accountOverviews.GroupBy(x => x.ActivityId).ToList();

            throw new NotImplementedException();
        }

        private static RowModel Orderer(AccountOverview overview, AccountFieldsSettingsOverview settings)
        {
            var values = new List<NewGridRowCellValueModel>();

            CreateValueIfNeeded(
                settings.Orderer.Id,
                BusinessData.Enums.Accounts.Fields.OrdererFields.Id,
                (StringDisplayValue)overview.Orderer.Id,
                values);
            CreateValueIfNeeded(
                settings.Orderer.FirstName,
                BusinessData.Enums.Accounts.Fields.OrdererFields.FirstName,
                (StringDisplayValue)overview.Orderer.FirstName,
                values);
            CreateValueIfNeeded(
                settings.Orderer.LastName,
                BusinessData.Enums.Accounts.Fields.OrdererFields.LastName,
                (StringDisplayValue)overview.Orderer.LastName,
                values);
            CreateValueIfNeeded(
                settings.Orderer.Phone,
                BusinessData.Enums.Accounts.Fields.OrdererFields.Phone,
                (StringDisplayValue)overview.Orderer.Phone,
                values);
            CreateValueIfNeeded(
                settings.Orderer.Email,
                BusinessData.Enums.Accounts.Fields.OrdererFields.Email,
                (StringDisplayValue)overview.Orderer.Email,
                values);

            string userIds = GetStringFromList(overview.User.Ids);
            string numberIds = GetStringFromList(overview.User.PersonalIdentityNumber);

            CreateValueIfNeeded(
                settings.User.Ids,
                BusinessData.Enums.Accounts.Fields.UserFields.Ids,
                (StringDisplayValue)userIds,
                values);
            CreateValueIfNeeded(
                settings.User.PersonalIdentityNumber,
                BusinessData.Enums.Accounts.Fields.UserFields.PersonalIdentityNumber,
                (StringDisplayValue)numberIds,
                values);
            CreateValueIfNeeded(
                settings.User.FirstName,
                BusinessData.Enums.Accounts.Fields.UserFields.FirstName,
                (StringDisplayValue)overview.User.FirstName,
                values);
            CreateValueIfNeeded(
                settings.User.Initials,
                BusinessData.Enums.Accounts.Fields.UserFields.Initials,
                (StringDisplayValue)overview.User.Initials,
                values);
            CreateValueIfNeeded(
                settings.User.LastName,
                BusinessData.Enums.Accounts.Fields.UserFields.LastName,
                (StringDisplayValue)overview.User.LastName,
                values);
            CreateValueIfNeeded(
                settings.User.Phone,
                BusinessData.Enums.Accounts.Fields.UserFields.Phone,
                (StringDisplayValue)overview.User.Phone,
                values);
            CreateValueIfNeeded(
                settings.User.Extension,
                BusinessData.Enums.Accounts.Fields.UserFields.Extension,
                (StringDisplayValue)overview.User.Extension,
                values);
            CreateValueIfNeeded(
                settings.User.EMail,
                BusinessData.Enums.Accounts.Fields.UserFields.EMail,
                (StringDisplayValue)overview.User.EMail,
                values);
            CreateValueIfNeeded(
                settings.User.Title,
                BusinessData.Enums.Accounts.Fields.UserFields.Title,
                (StringDisplayValue)overview.User.Title,
                values);
            CreateValueIfNeeded(
                settings.User.Location,
                BusinessData.Enums.Accounts.Fields.UserFields.Location,
                (StringDisplayValue)overview.User.Location,
                values);
            CreateValueIfNeeded(
                settings.User.PostalAddress,
                BusinessData.Enums.Accounts.Fields.UserFields.PostalAddress,
                (StringDisplayValue)overview.User.PostalAddress,
                values);
            CreateValueIfNeeded(
                settings.User.RoomNumber,
                BusinessData.Enums.Accounts.Fields.UserFields.RoomNumber,
                (StringDisplayValue)overview.User.RoomNumber,
                values);
            CreateValueIfNeeded(
                settings.User.FirstName,
                BusinessData.Enums.Accounts.Fields.UserFields.FirstName,
                (StringDisplayValue)overview.User.FirstName,
                values);
            CreateValueIfNeeded(
                settings.User.FirstName,
                BusinessData.Enums.Accounts.Fields.UserFields.FirstName,
                (StringDisplayValue)overview.User.FirstName,
                values);
            CreateValueIfNeeded(
                settings.User.EmploymentType,
                BusinessData.Enums.Accounts.Fields.UserFields.EmploymentType,
                (StringDisplayValue)overview.User.EmploymentType,
                values);
            CreateValueIfNeeded(
                settings.User.DepartmentId,
                BusinessData.Enums.Accounts.Fields.UserFields.DepartmentId,
                (StringDisplayValue)overview.User.DepartmentId,
                values);
            CreateValueIfNeeded(
                settings.User.UnitId,
                BusinessData.Enums.Accounts.Fields.UserFields.UnitId,
                (StringDisplayValue)overview.User.UnitId,
                values);
            CreateValueIfNeeded(
                settings.User.DepartmentId2,
                BusinessData.Enums.Accounts.Fields.UserFields.DepartmentId2,
                (StringDisplayValue)overview.User.DepartmentId2,
                values);
            CreateValueIfNeeded(
                settings.User.DepartmentId2,
                BusinessData.Enums.Accounts.Fields.UserFields.DepartmentId2,
                (StringDisplayValue)overview.User.Info,
                values);
            CreateValueIfNeeded(
                settings.User.Responsibility,
                BusinessData.Enums.Accounts.Fields.UserFields.Responsibility,
                (StringDisplayValue)overview.User.Responsibility,
                values);
            CreateValueIfNeeded(
                settings.User.Activity,
                BusinessData.Enums.Accounts.Fields.UserFields.Activity,
                (StringDisplayValue)overview.User.Activity,
                values);
            CreateValueIfNeeded(
                settings.User.Manager,
                BusinessData.Enums.Accounts.Fields.UserFields.Manager,
                (StringDisplayValue)overview.User.Manager,
                values);
            CreateValueIfNeeded(
                settings.User.ReferenceNumber,
                BusinessData.Enums.Accounts.Fields.UserFields.ReferenceNumber,
                (StringDisplayValue)overview.User.ReferenceNumber,
                values);

            string account2Ids = GetStringFromList(overview.AccountInformation.AccountType2);

            CreateValueIfNeeded(
                settings.AccountInformation.StartedDate,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.StartedDate,
                (DateTimeDisplayValue)overview.AccountInformation.StartedDate,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.FinishDate,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.FinishDate,
                (DateTimeDisplayValue)overview.AccountInformation.FinishDate,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.EMailTypeId,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.EMailTypeId,
                (EmailTypesDisplayValue)overview.AccountInformation.EMailTypeId,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.HomeDirectory,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.HomeDirectory,
                (BooleanDisplayValue)overview.AccountInformation.HomeDirectory,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.Profile,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.Profile,
                (BooleanDisplayValue)overview.AccountInformation.Profile,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.InventoryNumber,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.InventoryNumber,
                (StringDisplayValue)overview.AccountInformation.InventoryNumber,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.AccountTypeId,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountTypeId,
                (StringDisplayValue)overview.AccountInformation.AccountTypeId,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.AccountType3,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountType3,
                (StringDisplayValue)overview.AccountInformation.AccountType3,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.AccountType2,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountType2,
                (StringDisplayValue)account2Ids,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.AccountType3,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountType3,
                (StringDisplayValue)overview.AccountInformation.AccountType3,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.AccountType4,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountType4,
                (StringDisplayValue)overview.AccountInformation.AccountType4,
                values);
            CreateValueIfNeeded(
                settings.AccountInformation.AccountType5,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountType5,
                (StringDisplayValue)overview.AccountInformation.AccountType5,
                values);

            var contact2Ids = GetStringFromList(overview.Contact.Ids);

            CreateValueIfNeeded(
                settings.Contact.Ids,
                BusinessData.Enums.Accounts.Fields.ContactFields.Id,
                (StringDisplayValue)contact2Ids,
                values);
            CreateValueIfNeeded(
                settings.Contact.Name,
                BusinessData.Enums.Accounts.Fields.ContactFields.Name,
                (StringDisplayValue)overview.Contact.Name,
                values);
            CreateValueIfNeeded(
                settings.Contact.Phone,
                BusinessData.Enums.Accounts.Fields.ContactFields.Phone,
                (StringDisplayValue)overview.Contact.Phone,
                values);
            CreateValueIfNeeded(
                settings.Contact.Email,
                BusinessData.Enums.Accounts.Fields.ContactFields.Email,
                (StringDisplayValue)overview.Contact.Email,
                values);

            CreateValueIfNeeded(
                settings.DeliveryInformation.Name,
                BusinessData.Enums.Accounts.Fields.DeliveryInformationFields.Name,
                (StringDisplayValue)overview.DeliveryInformation.Name,
                values);
            CreateValueIfNeeded(
                settings.DeliveryInformation.Phone,
                BusinessData.Enums.Accounts.Fields.DeliveryInformationFields.Phone,
                (StringDisplayValue)overview.DeliveryInformation.Phone,
                values);
            CreateValueIfNeeded(
                settings.DeliveryInformation.Address,
                BusinessData.Enums.Accounts.Fields.DeliveryInformationFields.Address,
                (StringDisplayValue)overview.DeliveryInformation.Address,
                values);
            CreateValueIfNeeded(
                settings.DeliveryInformation.PostalAddress,
                BusinessData.Enums.Accounts.Fields.DeliveryInformationFields.PostalAddress,
                (StringDisplayValue)overview.DeliveryInformation.PostalAddress,
                values);

            var programIds = GetStringFromList(overview.Program.Programs);

            CreateValueIfNeeded(
               settings.Program.Programs,
               BusinessData.Enums.Accounts.Fields.ProgramFields.Programs,
               (StringDisplayValue)programIds,
               values);
            CreateValueIfNeeded(
               settings.Program.InfoProduct,
               BusinessData.Enums.Accounts.Fields.ProgramFields.InfoProduct,
               (StringDisplayValue)overview.Program.InfoProduct,
               values);

            CreateValueIfNeeded(
               settings.Other.CaseNumber,
               BusinessData.Enums.Accounts.Fields.OtherFields.CaseNumber,
               (DecimalDisplayValue)overview.Other.CaseNumber,
               values);
            CreateValueIfNeeded(
               settings.Other.FileName,
               BusinessData.Enums.Accounts.Fields.OtherFields.FileName,
               (StringDisplayValue)overview.Other.FileName,
               values);
            CreateValueIfNeeded(
               settings.Other.Info,
               BusinessData.Enums.Accounts.Fields.OtherFields.Info,
               (StringDisplayValue)overview.Other.Info,
               values);

            return new RowModel(overview.Id, values);
        }

        private static string GetStringFromList<T>(IEnumerable<T> values)
        {
            string contact2Ids = values != null ? string.Join(",", values) : null;
            return contact2Ids;
        }

        private static void CreateHeaderIfNeeded(
            FieldSetting setting,
            string fieldName,
            List<GridColumnHeaderModel> headers)
        {
            if (!setting.IsShowInList)
            {
                return;
            }

            var header = new GridColumnHeaderModel(fieldName, setting.Caption);
            headers.Add(header);
        }

        private static void CreateValueIfNeeded(
            FieldSetting setting,
            string fieldName,
            DisplayValue value,
            List<NewGridRowCellValueModel> values)
        {
            if (!setting.IsShowInList)
            {
                return;
            }

            var fieldValue = new NewGridRowCellValueModel(fieldName, value);
            values.Add(fieldValue);
        }
    }
}
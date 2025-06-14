﻿namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order
{
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

        public static List<GridModel> BuildGrid(List<AccountOverview> accountOverviews, List<AccountFieldsSettingsOverviewWithActivity> settings, SortFieldModel sortField)
        {
            var grids = new List<GridModel>();
            var groupedModels =
                accountOverviews.GroupBy(x => new { x.ActivityId, x.ActivityName })
                    .OrderBy(x => x.Key.ActivityId)
                    .ToList();

            foreach (var model in groupedModels)
            {
                AccountFieldsSettingsOverviewWithActivity setting =
                    settings.Single(x => x.ActivityId == model.Key.ActivityId);

                List<RowModel> overviews =
                    model.Select(m => CreateOverview(m, setting.AccountFieldsSettingsOverview)).ToList();
                List<GridColumnHeaderModel> headers = CreateHeaders(setting.AccountFieldsSettingsOverview);

                var gridModel = new GridModel(headers, overviews, model.Key.ActivityId, sortField)
                                    {
                                        CurrentModeName
                                            =
                                            model
                                            .Key
                                            .ActivityName
                                    };

                grids.Add(gridModel);
            }

            return grids;
        }

        private static RowModel CreateOverview(AccountOverview overview, AccountFieldsSettingsOverview settings)
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
                settings.User.Info,
                BusinessData.Enums.Accounts.Fields.UserFields.Info,
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

        private static List<GridColumnHeaderModel> CreateHeaders(AccountFieldsSettingsOverview settings)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateHeaderIfNeeded(settings.Orderer.Id, BusinessData.Enums.Accounts.Fields.OrdererFields.Id, headers);
            CreateHeaderIfNeeded(
                settings.Orderer.FirstName,
                BusinessData.Enums.Accounts.Fields.OrdererFields.FirstName,
                headers);
            CreateHeaderIfNeeded(
                settings.Orderer.LastName,
                BusinessData.Enums.Accounts.Fields.OrdererFields.LastName,
                headers);
            CreateHeaderIfNeeded(
                settings.Orderer.Phone,
                BusinessData.Enums.Accounts.Fields.OrdererFields.Phone,
                headers);
            CreateHeaderIfNeeded(
                settings.Orderer.Email,
                BusinessData.Enums.Accounts.Fields.OrdererFields.Email,
                headers);

            CreateHeaderIfNeeded(settings.User.Ids, BusinessData.Enums.Accounts.Fields.UserFields.Ids, headers);
            CreateHeaderIfNeeded(
                settings.User.PersonalIdentityNumber,
                BusinessData.Enums.Accounts.Fields.UserFields.PersonalIdentityNumber,
                headers);
            CreateHeaderIfNeeded(
                settings.User.FirstName,
                BusinessData.Enums.Accounts.Fields.UserFields.FirstName,
                headers);
            CreateHeaderIfNeeded(
                settings.User.Initials,
                BusinessData.Enums.Accounts.Fields.UserFields.Initials,
                headers);
            CreateHeaderIfNeeded(
                settings.User.LastName,
                BusinessData.Enums.Accounts.Fields.UserFields.LastName,
                headers);
            CreateHeaderIfNeeded(settings.User.Phone, BusinessData.Enums.Accounts.Fields.UserFields.Phone, headers);
            CreateHeaderIfNeeded(
                settings.User.Extension,
                BusinessData.Enums.Accounts.Fields.UserFields.Extension,
                headers);
            CreateHeaderIfNeeded(settings.User.EMail, BusinessData.Enums.Accounts.Fields.UserFields.EMail, headers);
            CreateHeaderIfNeeded(settings.User.Title, BusinessData.Enums.Accounts.Fields.UserFields.Title, headers);
            CreateHeaderIfNeeded(
                settings.User.Location,
                BusinessData.Enums.Accounts.Fields.UserFields.Location,
                headers);
            CreateHeaderIfNeeded(
                settings.User.PostalAddress,
                BusinessData.Enums.Accounts.Fields.UserFields.PostalAddress,
                headers);
            CreateHeaderIfNeeded(
                settings.User.RoomNumber,
                BusinessData.Enums.Accounts.Fields.UserFields.RoomNumber,
                headers);
            CreateHeaderIfNeeded(
                settings.User.EmploymentType,
                BusinessData.Enums.Accounts.Fields.UserFields.EmploymentType,
                headers);
            CreateHeaderIfNeeded(
                settings.User.DepartmentId,
                BusinessData.Enums.Accounts.Fields.UserFields.DepartmentId,
                headers);
            CreateHeaderIfNeeded(settings.User.UnitId, BusinessData.Enums.Accounts.Fields.UserFields.UnitId, headers);
            CreateHeaderIfNeeded(
                settings.User.DepartmentId2,
                BusinessData.Enums.Accounts.Fields.UserFields.DepartmentId2,
                headers);
            CreateHeaderIfNeeded(
                settings.User.Info,
                BusinessData.Enums.Accounts.Fields.UserFields.Info,
                headers);
            CreateHeaderIfNeeded(
                settings.User.Responsibility,
                BusinessData.Enums.Accounts.Fields.UserFields.Responsibility,
                headers);
            CreateHeaderIfNeeded(
                settings.User.Activity,
                BusinessData.Enums.Accounts.Fields.UserFields.Activity,
                headers);
            CreateHeaderIfNeeded(settings.User.Manager, BusinessData.Enums.Accounts.Fields.UserFields.Manager, headers);
            CreateHeaderIfNeeded(
                settings.User.ReferenceNumber,
                BusinessData.Enums.Accounts.Fields.UserFields.ReferenceNumber,
                headers);

            CreateHeaderIfNeeded(
                settings.AccountInformation.StartedDate,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.StartedDate,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.FinishDate,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.FinishDate,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.EMailTypeId,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.EMailTypeId,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.HomeDirectory,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.HomeDirectory,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.Profile,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.Profile,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.InventoryNumber,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.InventoryNumber,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.AccountTypeId,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountTypeId,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.AccountType2,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountType2,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.AccountType3,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountType3,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.AccountType4,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountType4,
                headers);
            CreateHeaderIfNeeded(
                settings.AccountInformation.AccountType5,
                BusinessData.Enums.Accounts.Fields.AccountInformationFields.AccountType5,
                headers);

            CreateHeaderIfNeeded(settings.Contact.Ids, BusinessData.Enums.Accounts.Fields.ContactFields.Id, headers);
            CreateHeaderIfNeeded(settings.Contact.Name, BusinessData.Enums.Accounts.Fields.ContactFields.Name, headers);
            CreateHeaderIfNeeded(
                settings.Contact.Phone,
                BusinessData.Enums.Accounts.Fields.ContactFields.Phone,
                headers);
            CreateHeaderIfNeeded(
                settings.Contact.Email,
                BusinessData.Enums.Accounts.Fields.ContactFields.Email,
                headers);

            CreateHeaderIfNeeded(
                settings.DeliveryInformation.Name,
                BusinessData.Enums.Accounts.Fields.DeliveryInformationFields.Name,
                headers);
            CreateHeaderIfNeeded(
                settings.DeliveryInformation.Phone,
                BusinessData.Enums.Accounts.Fields.DeliveryInformationFields.Phone,
                headers);
            CreateHeaderIfNeeded(
                settings.DeliveryInformation.Address,
                BusinessData.Enums.Accounts.Fields.DeliveryInformationFields.Address,
                headers);
            CreateHeaderIfNeeded(
                settings.DeliveryInformation.PostalAddress,
                BusinessData.Enums.Accounts.Fields.DeliveryInformationFields.PostalAddress,
                headers);

            CreateHeaderIfNeeded(
                settings.Program.Programs,
                BusinessData.Enums.Accounts.Fields.ProgramFields.Programs,
                headers);
            CreateHeaderIfNeeded(
                settings.Program.InfoProduct,
                BusinessData.Enums.Accounts.Fields.ProgramFields.InfoProduct,
                headers);

            CreateHeaderIfNeeded(
                settings.Other.CaseNumber,
                BusinessData.Enums.Accounts.Fields.OtherFields.CaseNumber,
                headers);
            CreateHeaderIfNeeded(
                settings.Other.FileName,
                BusinessData.Enums.Accounts.Fields.OtherFields.FileName,
                headers);
            CreateHeaderIfNeeded(settings.Other.Info, BusinessData.Enums.Accounts.Fields.OtherFields.Info, headers);

            return headers;
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
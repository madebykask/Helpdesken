namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;

    public sealed class AccountInformation
    {
        public AccountInformation()
        {
        }

        public AccountInformation(
            ConfigurableFieldModel<DateTime?> startedDate,
            ConfigurableFieldModel<DateTime?> finishDate,
            ConfigurableFieldModel<EMailTypes> eMailTypeId,
            ConfigurableFieldModel<bool> homeDirectory,
            ConfigurableFieldModel<bool> profile,
            ConfigurableFieldModel<string> inventoryNumber,
            ConfigurableFieldModel<int?> accountTypeId,
            ConfigurableFieldModel<List<int>> accountType2,
            ConfigurableFieldModel<int?> accountType3,
            ConfigurableFieldModel<int?> accountType4,
            ConfigurableFieldModel<int?> accountType5,
            ConfigurableFieldModel<string> info,
            SelectList emailTypes,
            SelectList accountTypes,
            List<ItemOverview> accountTypes2,
            SelectList accountTypes3,
            SelectList accountTypes4,
            SelectList accountTypes5)
        {
            this.StartedDate = startedDate;
            this.FinishDate = finishDate;
            this.EMailTypeId = eMailTypeId;
            this.HomeDirectory = homeDirectory;
            this.Profile = profile;
            this.InventoryNumber = inventoryNumber;
            this.AccountTypeId = accountTypeId;
            this.AccountType2 = accountType2;
            this.AccountType3 = accountType3;
            this.AccountType4 = accountType4;
            this.AccountType5 = accountType5;
            this.Info = info;
            this.EmailTypes = emailTypes;
            this.AccountTypes = accountTypes;
            this.AccountTypes2 = accountTypes2;
            this.AccountTypes3 = accountTypes3;
            this.AccountTypes4 = accountTypes4;
            this.AccountTypes5 = accountTypes5;
        }

        public ConfigurableFieldModel<DateTime?> StartedDate { get; set; }

        public ConfigurableFieldModel<DateTime?> FinishDate { get; set; }

        public ConfigurableFieldModel<EMailTypes> EMailTypeId { get; set; }

        public ConfigurableFieldModel<bool> HomeDirectory { get; set; }

        public ConfigurableFieldModel<bool> Profile { get; set; }

        public ConfigurableFieldModel<string> InventoryNumber { get; set; }

        public ConfigurableFieldModel<int?> AccountTypeId { get; set; }

        public ConfigurableFieldModel<List<int>> AccountType2 { get; set; }

        public ConfigurableFieldModel<int?> AccountType3 { get; set; }

        public ConfigurableFieldModel<int?> AccountType4 { get; set; }

        public ConfigurableFieldModel<int?> AccountType5 { get; set; }

        public ConfigurableFieldModel<string> Info { get; set; }

        [NotNull]
        public SelectList EmailTypes { get; set; }

        [NotNull]
        public SelectList AccountTypes { get; set; }

        [NotNull]
        public List<ItemOverview> AccountTypes2 { get; set; }

        [NotNull]
        public SelectList AccountTypes3 { get; set; }

        [NotNull]
        public SelectList AccountTypes4 { get; set; }

        [NotNull]
        public SelectList AccountTypes5 { get; set; }
    }
}
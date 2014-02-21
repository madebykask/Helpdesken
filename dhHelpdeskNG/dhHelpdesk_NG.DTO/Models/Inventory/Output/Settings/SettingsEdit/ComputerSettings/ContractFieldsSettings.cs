﻿namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContractFieldsSettings
    {
        public ContractFieldsSettings(FieldSetting contractStatusFieldSetting, FieldSetting contractNumberFieldSetting, FieldSetting contractStartDateFieldSetting, FieldSetting contractEndDateFieldSetting, FieldSetting purchasePriceFieldSetting, FieldSetting purchaseDateFieldSetting, FieldSetting accountingDimension1FieldSetting, FieldSetting accountingDimension2FieldSetting, FieldSetting accountingDimension3FieldSetting, FieldSetting accountingDimension4FieldSetting, FieldSetting accountingDimension5FieldSetting)
        {
            this.ContractStatusFieldSetting = contractStatusFieldSetting;
            this.ContractNumberFieldSetting = contractNumberFieldSetting;
            this.ContractStartDateFieldSetting = contractStartDateFieldSetting;
            this.ContractEndDateFieldSetting = contractEndDateFieldSetting;
            this.PurchasePriceFieldSetting = purchasePriceFieldSetting;
            this.PurchaseDateFieldSetting = purchaseDateFieldSetting;
            this.AccountingDimension1FieldSetting = accountingDimension1FieldSetting;
            this.AccountingDimension2FieldSetting = accountingDimension2FieldSetting;
            this.AccountingDimension3FieldSetting = accountingDimension3FieldSetting;
            this.AccountingDimension4FieldSetting = accountingDimension4FieldSetting;
            this.AccountingDimension5FieldSetting = accountingDimension5FieldSetting;
        }

        [NotNull]
        public FieldSetting ContractStatusFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ContractNumberFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ContractStartDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ContractEndDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting PurchasePriceFieldSetting { get; set; }

        [NotNull]
        public FieldSetting PurchaseDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting AccountingDimension1FieldSetting { get; set; }

        [NotNull]
        public FieldSetting AccountingDimension2FieldSetting { get; set; }

        [NotNull]
        public FieldSetting AccountingDimension3FieldSetting { get; set; }

        [NotNull]
        public FieldSetting AccountingDimension4FieldSetting { get; set; }

        [NotNull]
        public FieldSetting AccountingDimension5FieldSetting { get; set; }
    }
}
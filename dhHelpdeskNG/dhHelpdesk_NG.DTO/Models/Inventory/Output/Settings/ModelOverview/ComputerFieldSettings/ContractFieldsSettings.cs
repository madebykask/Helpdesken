namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContractFieldsSettings
    {
        public ContractFieldsSettings(FieldSettingOverview contractStatusFieldSetting, FieldSettingOverview contractNumberFieldSetting, FieldSettingOverview contractStartDateFieldSetting, FieldSettingOverview contractEndDateFieldSetting, FieldSettingOverview purchasePriceFieldSetting, FieldSettingOverview purchaseDateFieldSetting, FieldSettingOverview accountingDimension1FieldSetting, FieldSettingOverview accountingDimension2FieldSetting, FieldSettingOverview accountingDimension3FieldSetting, FieldSettingOverview accountingDimension4FieldSetting, FieldSettingOverview accountingDimension5FieldSetting, FieldSettingOverview warrantyEndDateFieldSetting)
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
            this.WarrantyEndDateFieldSetting = warrantyEndDateFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview ContractStatusFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ContractNumberFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ContractStartDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ContractEndDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview PurchasePriceFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview PurchaseDateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview AccountingDimension1FieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview AccountingDimension2FieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview AccountingDimension3FieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview AccountingDimension4FieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview AccountingDimension5FieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview WarrantyEndDateFieldSetting { get; set; }
    }
}
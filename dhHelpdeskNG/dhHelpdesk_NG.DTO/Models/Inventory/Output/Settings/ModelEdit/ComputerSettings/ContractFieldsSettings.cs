namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContractFieldsSettings
    {
        public ContractFieldsSettings(
            ModelEditFieldSetting contractStatusFieldSetting,
            ModelEditFieldSetting contractNumberFieldSetting,
            ModelEditFieldSetting contractStartDateFieldSetting,
            ModelEditFieldSetting contractEndDateFieldSetting,
            ModelEditFieldSetting purchasePriceFieldSetting,
            ModelEditFieldSetting purchaseDateFieldSetting,
            ModelEditFieldSetting accountingDimension1FieldSetting,
            ModelEditFieldSetting accountingDimension2FieldSetting,
            ModelEditFieldSetting accountingDimension3FieldSetting,
            ModelEditFieldSetting accountingDimension4FieldSetting,
            ModelEditFieldSetting accountingDimension5FieldSetting,
            ModelEditFieldSetting doumentFieldSetting,
            ModelEditFieldSetting warrantyEndDateFieldSettings)
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
            this.DocumentFieldSetting = doumentFieldSetting;
            this.WarrantyEndDateFieldSettings = warrantyEndDateFieldSettings;
        }

        [NotNull]
        public ModelEditFieldSetting ContractStatusFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ContractNumberFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ContractStartDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ContractEndDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting PurchasePriceFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting PurchaseDateFieldSetting { get; set; }

        public ModelEditFieldSetting WarrantyEndDateFieldSettings { get; set; }

        [NotNull]
        public ModelEditFieldSetting AccountingDimension1FieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting AccountingDimension2FieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting AccountingDimension3FieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting AccountingDimension4FieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting AccountingDimension5FieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting DocumentFieldSetting { get; set; }
    }
}
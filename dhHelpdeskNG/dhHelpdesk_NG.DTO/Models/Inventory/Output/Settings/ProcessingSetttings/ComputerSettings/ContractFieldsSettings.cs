namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContractFieldsSettings
    {
        public ContractFieldsSettings(
            ProcessingFieldSetting contractStatusFieldSetting,
            ProcessingFieldSetting contractNumberFieldSetting,
            ProcessingFieldSetting contractStartDateFieldSetting,
            ProcessingFieldSetting contractEndDateFieldSetting,
            ProcessingFieldSetting purchasePriceFieldSetting,
            ProcessingFieldSetting purchaseDateFieldSetting,
            ProcessingFieldSetting accountingDimension1FieldSetting,
            ProcessingFieldSetting accountingDimension2FieldSetting,
            ProcessingFieldSetting accountingDimension3FieldSetting,
            ProcessingFieldSetting accountingDimension4FieldSetting,
            ProcessingFieldSetting accountingDimension5FieldSetting,
             ProcessingFieldSetting warrantyEndDateFieldSetting)
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
        public ProcessingFieldSetting ContractStatusFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ContractNumberFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ContractStartDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ContractEndDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting PurchasePriceFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting PurchaseDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting AccountingDimension1FieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting AccountingDimension2FieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting AccountingDimension3FieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting AccountingDimension4FieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting AccountingDimension5FieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting WarrantyEndDateFieldSetting { get; set; }
    }
}
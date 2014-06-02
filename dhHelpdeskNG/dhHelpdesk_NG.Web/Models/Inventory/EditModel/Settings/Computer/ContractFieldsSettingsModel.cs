namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContractFieldsSettingsModel
    {
        public ContractFieldsSettingsModel(
            FieldSettingModel contractStatusFieldSettingModel,
            FieldSettingModel contractNumberFieldSettingModel,
            FieldSettingModel contractStartDateFieldSettingModel,
            FieldSettingModel contractEndDateFieldSettingModel,
            FieldSettingModel purchasePriceFieldSettingModel,
            FieldSettingModel accountingDimension1FieldSettingModel,
            FieldSettingModel accountingDimension2FieldSettingModel,
            FieldSettingModel accountingDimension3FieldSettingModel,
            FieldSettingModel accountingDimension4FieldSettingModel,
            FieldSettingModel accountingDimension5FieldSettingModel,
            FieldSettingModel documentsFieldSettingModel)
        {
            this.ContractStatusFieldSettingModel = contractStatusFieldSettingModel;
            this.ContractNumberFieldSettingModel = contractNumberFieldSettingModel;
            this.ContractStartDateFieldSettingModel = contractStartDateFieldSettingModel;
            this.ContractEndDateFieldSettingModel = contractEndDateFieldSettingModel;
            this.PurchasePriceFieldSettingModel = purchasePriceFieldSettingModel;
            this.AccountingDimension1FieldSettingModel = accountingDimension1FieldSettingModel;
            this.AccountingDimension2FieldSettingModel = accountingDimension2FieldSettingModel;
            this.AccountingDimension3FieldSettingModel = accountingDimension3FieldSettingModel;
            this.AccountingDimension4FieldSettingModel = accountingDimension4FieldSettingModel;
            this.AccountingDimension5FieldSettingModel = accountingDimension5FieldSettingModel;
            this.DocumentsFieldSettingModel = documentsFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel ContractStatusFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ContractNumberFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ContractStartDateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ContractEndDateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel PurchasePriceFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel AccountingDimension1FieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel AccountingDimension2FieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel AccountingDimension3FieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel AccountingDimension4FieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel AccountingDimension5FieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel DocumentsFieldSettingModel { get; set; }
    }
}
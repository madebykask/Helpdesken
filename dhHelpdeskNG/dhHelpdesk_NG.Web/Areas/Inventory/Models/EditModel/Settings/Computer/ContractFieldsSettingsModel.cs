namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ContractFieldsSettingsModel
    {
        public ContractFieldsSettingsModel()
        {
        }

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
            documentsFieldSettingModel.IsCaptionDisabled = true;
            documentsFieldSettingModel.IsRequiredDisabled = true;
            documentsFieldSettingModel.IsShowInListDisabled = true;
            documentsFieldSettingModel.IsReadOnlyDisabled = true;
            this.DocumentsFieldSettingModel = documentsFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Avtalsstatus")]
        public FieldSettingModel ContractStatusFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Avtalsnummer")]
        public FieldSettingModel ContractNumberFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Avtalsstart")]
        public FieldSettingModel ContractStartDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Avtalsslut")]
        public FieldSettingModel ContractEndDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Inköpspris")]
        public FieldSettingModel PurchasePriceFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension 1")]
        public FieldSettingModel AccountingDimension1FieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension 2")]
        public FieldSettingModel AccountingDimension2FieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension 3")]
        public FieldSettingModel AccountingDimension3FieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension 4")]
        public FieldSettingModel AccountingDimension4FieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension 5")]
        public FieldSettingModel AccountingDimension5FieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Dokument")]
        public FieldSettingModel DocumentsFieldSettingModel { get; set; }
    }
}
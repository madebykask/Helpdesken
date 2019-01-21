namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContractFieldsModel
    {
        public ContractFieldsModel()
        {
        }

        public ContractFieldsModel(
            ConfigurableFieldModel<int?> contractStatusId,
            ConfigurableFieldModel<string> contractNumber,
            ConfigurableFieldModel<DateTime?> contractStartDate,
            ConfigurableFieldModel<DateTime?> contractEndDate,
            ConfigurableFieldModel<int> purchasePrice,
            ConfigurableFieldModel<string> accountingDimension1,
            ConfigurableFieldModel<string> accountingDimension2,
            ConfigurableFieldModel<string> accountingDimension3,
            ConfigurableFieldModel<string> accountingDimension4,
            ConfigurableFieldModel<string> accountingDimension5,
            ConfigurableFieldModel<string> document)
        {
            this.ContractStatusId = contractStatusId;
            this.ContractNumber = contractNumber;
            this.ContractStartDate = contractStartDate;
            this.ContractEndDate = contractEndDate;
            this.PurchasePrice = purchasePrice;
            this.AccountingDimension1 = accountingDimension1;
            this.AccountingDimension2 = accountingDimension2;
            this.AccountingDimension3 = accountingDimension3;
            this.AccountingDimension4 = accountingDimension4;
            this.AccountingDimension5 = accountingDimension5;
            this.Document = document;
        }

        [NotNull]
        public ConfigurableFieldModel<int?> ContractStatusId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> ContractNumber { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> ContractStartDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> ContractEndDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int> PurchasePrice { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension1 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension2 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension3 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension4 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension5 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Document { get; set; }
    }
}
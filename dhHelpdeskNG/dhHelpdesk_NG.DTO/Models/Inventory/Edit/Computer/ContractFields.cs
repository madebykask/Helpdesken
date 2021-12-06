namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContractFields
    {
        public ContractFields(
            int? contractStatusId,
            string contractNumber,
            DateTime? contractStartDate,
            DateTime? contractEndDate,
            int purchasePrice,
            string accountingDimension1,
            string accountingDimension2,
            string accountingDimension3,
            string accountingDimension4,
            string accountingDimension5,
            string document,
            DateTime? warrantyEndDate
            )
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
            this.WarrantyEndDate = warrantyEndDate;
        }

        [IsId]
        public int? ContractStatusId { get; set; }

        public string ContractNumber { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? WarrantyEndDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        [MinValue(0)]
        public int PurchasePrice { get; set; }

        public string AccountingDimension1 { get; set; }

        public string AccountingDimension2 { get; set; }

        public string AccountingDimension3 { get; set; }

        public string AccountingDimension4 { get; set; }

        public string AccountingDimension5 { get; set; }

        public string Document { get; set; }

        public static ContractFields CreateDefault()
        {
            return new ContractFields(null, null, null, null, 0, null, null, null, null, null, null, null);
        }
    }
}
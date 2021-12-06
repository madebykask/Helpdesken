namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    using System;

    public class ContractFields
    {
        public ContractFields(
            string contractStatusName,
            string contractNumber,
            DateTime? contractStartDate,
            DateTime? contractEndDate,
            int purchasePrice,
            string accountingDimension1,
            string accountingDimension2,
            string accountingDimension3,
            string accountingDimension4,
            string accountingDimension5,
            DateTime? warrantyEndDate)
        {
            this.ContractStatusName = contractStatusName;
            this.ContractNumber = contractNumber;
            this.ContractStartDate = contractStartDate;
            this.ContractEndDate = contractEndDate;
            this.PurchasePrice = purchasePrice;
            this.AccountingDimension1 = accountingDimension1;
            this.AccountingDimension2 = accountingDimension2;
            this.AccountingDimension3 = accountingDimension3;
            this.AccountingDimension4 = accountingDimension4;
            this.AccountingDimension5 = accountingDimension5;
            this.WarrantyEndDate = warrantyEndDate;
        }

        public string ContractStatusName { get; set; }

        public string ContractNumber { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public int PurchasePrice { get; set; }

        public string AccountingDimension1 { get; set; }

        public string AccountingDimension2 { get; set; }

        public string AccountingDimension3 { get; set; }

        public string AccountingDimension4 { get; set; }

        public string AccountingDimension5 { get; set; }

        public DateTime? WarrantyEndDate { get; set; }
    }
}
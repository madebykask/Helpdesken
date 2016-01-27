namespace DH.Helpdesk.Domain.Invoice
{
    using global::System;
    using global::System.Collections.Generic;

    public class CaseInvoiceOrderEntity : Entity
    {
        public int InvoiceId { get; set; }        

        public short Number { get; set; }
      
        public DateTime? InvoiceDate { get; set; }

        public int? InvoicedByUserId { get; set; }

        public DateTime Date { get; set; }

        public string ReportedBy { get; set; }

        public string Persons_Name { get; set; }

        public string Persons_Email { get; set; }

        public string Persons_Phone { get; set; }

        public string Persons_Cellphone { get; set; }

        public int? Region_Id { get; set; }

        public int? Department_Id { get; set; }

        public int? OU_Id { get; set; }

        public string Place { get; set; }

        public string UserCode { get; set; }

        public string CostCentre { get; set; }

        public int? CreditForOrder_Id { get; set; }

        public virtual CaseInvoiceEntity Invoice { get; set; }

        public virtual ICollection<CaseInvoiceArticleEntity> Articles { get; set; } 

        public virtual ICollection<CaseInvoiceOrderFileEntity> Files { get; set; } 
    }
}
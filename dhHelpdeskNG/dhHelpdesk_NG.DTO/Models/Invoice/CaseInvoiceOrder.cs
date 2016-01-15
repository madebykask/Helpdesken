namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("SalesHeader")]
    public sealed class CaseInvoiceOrder : IXmlSerializable
    {
        public CaseInvoiceOrder(
                int id, 
                int invoiceId,
                CaseInvoice invoice, 
                short number, 
                DateTime? invoiceDate,
                int? invoicedByUserId,
                DateTime date,
                string reportedBy,
                string persons_Name,
                string persons_Email,
                string persons_Phone,
                string persons_Cellphone,
                int? region_Id,
                int? department_Id,
                int? ou_Id,
                string place,
                string userCode,
                string costCentre,
                CaseInvoiceArticle[] articles,
                CaseInvoiceOrderFile[] files)
        {
            this.Articles = articles != null ? articles : new List<CaseInvoiceArticle>().ToArray();
            this.InvoiceId = invoiceId;
            this.InvoiceDate = invoiceDate;
            this.InvoicedByUserId = invoicedByUserId;
            this.Number = number;
            this.Invoice = invoice;
            this.Id = id;
            this.Date = date;
            this.ReportedBy = reportedBy;
            this.Persons_Name = persons_Name;
            this.Persons_Email = persons_Email;
            this.Persons_Phone = persons_Phone;
            this.Persons_Cellphone = persons_Cellphone;
            this.Region_Id = region_Id;
            this.Department_Id = department_Id;
            this.OU_Id = ou_Id;
            this.Place = place;
            this.UserCode = userCode;
            this.CostCentre = costCentre;
            this.Files = files != null ? files : new List<CaseInvoiceOrderFile>().ToArray();
        }

        public CaseInvoiceOrder(
                int id, 
                int invoiceId,
                short number, 
                DateTime? invoiceDate,
                int? invoicedByUserId,
                DateTime date,
                string reportedBy,
                string persons_Name,
                string persons_Email,
                string persons_Phone,
                string persons_Cellphone,
                int? region_Id,
                int? department_Id,
                int? ou_Id,
                string place,
                string userCode,
                string costCentre,
                CaseInvoiceArticle[] articles,
                CaseInvoiceOrderFile[] files) :
                this(id, invoiceId, null, number, invoiceDate, invoicedByUserId, date, reportedBy, persons_Name, persons_Email, persons_Phone, persons_Cellphone, region_Id, department_Id, ou_Id, place, userCode, costCentre, articles, files)
        {
        }

        public CaseInvoiceOrder()
        {            
        }

        public int Id { get; private set; }

        public int InvoiceId { get; private set; }

        public CaseInvoice Invoice { get; private set; }

        public short Number { get; private set; }

        public DateTime? InvoiceDate { get; set; }

        public int? InvoicedByUserId { get; private set; }

        public string InvoicedByUser { get; set; }

        public DateTime Date { get; private set; }

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

        public CaseInvoiceArticle[] Articles { get; private set; }

        public CaseInvoiceOrderFile[] Files { get; private set; }

        public decimal? CaseNumber { get; set; }

        public void DoInvoice(int userId)
        {
            InvoicedByUserId = userId;
            foreach (var article in this.Articles)
            {
                this.InvoiceDate = DateTime.UtcNow;
                article.DoInvoice();
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("OrderDate", this.Date.ToShortDateString());
            if (this.CaseNumber.HasValue)
            {
                writer.WriteElementString("CaseNo", this.CaseNumber.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (this.Articles != null)
            {
                var serializer = new XmlSerializer(typeof(CaseInvoiceArticle));
                foreach (var article in this.Articles)
                {
                    serializer.Serialize(writer, article);                    
                }
            }

            if (this.Files != null)
            {
                var serializer = new XmlSerializer(typeof(CaseInvoiceOrderFile));
                foreach (var file in this.Files)
                {
                    serializer.Serialize(writer, file);
                }
            }
        }
    }
}
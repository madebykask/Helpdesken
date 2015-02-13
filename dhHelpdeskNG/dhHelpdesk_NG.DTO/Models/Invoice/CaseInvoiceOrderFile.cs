namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [Serializable]
    public sealed class CaseInvoiceOrderFile : IXmlSerializable
    {
        public CaseInvoiceOrderFile(
                int id, 
                int orderId, 
                string fileName, 
                DateTime createdDate)
        {
            this.CreatedDate = createdDate;
            this.FileName = fileName;
            this.OrderId = orderId;
            this.Id = id;
        }

        public CaseInvoiceOrderFile(
                string fileName)
        {
            this.FileName = fileName;
        }

        public CaseInvoiceOrderFile()
        {            
        }

        public int Id { get; private set; }

        public int OrderId { get; private set; }

        public string FileName { get; private set; }

        public DateTime CreatedDate { get; private set; }

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
        }
    }
}
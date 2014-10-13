namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System;
    using System.Web.Script.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using DH.Helpdesk.BusinessData.Models.Case.Output;

    [Serializable]
    [XmlRoot("SalesDoc")]
    public sealed class CaseInvoice : IXmlSerializable 
    {
        public CaseInvoice(
                int id, 
                int caseId, 
                CaseOverview @case, 
                CaseInvoiceOrder[] orders)
        {
            this.Orders = orders;
            this.Case = @case;
            this.CaseId = caseId;
            this.Id = id;
        }

        public CaseInvoice(
                int id,
                int caseId,
                CaseInvoiceOrder[] orders) : 
                this(id, caseId, null, orders)
        {
        }

        public CaseInvoice()
        {            
        }

        public int Id { get; private set; }

        public int CaseId { get; set; }

        [ScriptIgnore]
        public CaseOverview Case { get; private set; }

        public CaseInvoiceOrder[] Orders { get; private set; }

        public bool IsNew()
        {
            return this.Id <= 0;
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
            if (this.Orders != null)
            {
                var serializer = new XmlSerializer(typeof(CaseInvoiceOrder));
                foreach (var order in this.Orders)
                {
                    serializer.Serialize(writer, order);                    
                }
            }
        }
    }
}
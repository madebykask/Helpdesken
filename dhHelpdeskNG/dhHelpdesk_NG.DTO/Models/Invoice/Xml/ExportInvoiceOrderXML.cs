using DH.Helpdesk.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.BusinessData.Models.Invoice.Xml
{
    public class InvoiceXMLDocType
    {
        public static string Order = "Order";
        public static string Credit = "ReturnOrder";
    }

    public class InvoiceXMLLineType
    {
        public static string Article = "Item";
        public static string Description = "Text";
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class SalesDoc
    {

        private SalesDocSalesHeader salesHeaderField;

        /// <remarks/>
        public SalesDocSalesHeader SalesHeader
        {
            get
            {
                return this.salesHeaderField;
            }
            set
            {
                this.salesHeaderField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SalesDocSalesHeader
    {

        private string companyNoField;

        private string docTemplateField;

        private string docTypeField;

        private string sellToCustomerNoField;

        private string dateField;

        private string dueDateField;

        private string ourReferenceField;

        private string yourReference2Field;

        private string orderNoField;

        private string currencyCodeField;

        private SalesDocSalesHeaderSalesLine[] salesLineField;

        private SalesDocSalesHeaderAttachment[] attachmentField;

        /// <remarks/>
        public string CompanyNo
        {
            get
            {
                return this.companyNoField;
            }
            set
            {
                this.companyNoField = value;
            }
        }

        /// <remarks/>
        public string DocTemplate
        {
            get
            {
                return this.docTemplateField;
            }
            set
            {
                this.docTemplateField = value;
            }
        }

        /// <remarks/>
        public string DocType
        {
            get
            {
                return this.docTypeField;
            }
            set
            {
                this.docTypeField = value;
            }
        }

        /// <remarks/>
        public string SellToCustomerNo
        {
            get
            {
                return this.sellToCustomerNoField;
            }
            set
            {
                this.sellToCustomerNoField = value;
            }
        }

        /// <remarks/>
        public string Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public string DueDate
        {
            get
            {
                return this.dueDateField;
            }
            set
            {
                this.dueDateField = value;
            }
        }

        /// <remarks/>
        public string OurReference
        {
            get
            {
                return this.ourReferenceField;
            }
            set
            {
                this.ourReferenceField = value;
            }
        }

        /// <remarks/>
        public string YourReference2
        {
            get
            {
                return this.yourReference2Field;
            }
            set
            {
                this.yourReference2Field = value;
            }
        }

        /// <remarks/>
        public string OrderNo
        {
            get
            {
                return this.orderNoField;
            }
            set
            {
                this.orderNoField = value;
            }
        }

        /// <remarks/>
        public string CurrencyCode
        {
            get
            {
                return this.currencyCodeField;
            }
            set
            {
                this.currencyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SalesLine")]
        public SalesDocSalesHeaderSalesLine[] SalesLine
        {
            get
            {
                return this.salesLineField;
            }
            set
            {
                this.salesLineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Attachment")]
        public SalesDocSalesHeaderAttachment[] Attachment
        {
            get
            {
                return this.attachmentField;
            }
            set
            {
                this.attachmentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SalesDocSalesHeaderSalesLine
    {

        private string lineNoField;

        private string lineTypeField;

        private string numberField;

        private string descriptionField;

        private string quantityField;

        private string unitOfMeasureCodeField;

        private string unitPriceField;

        /// <remarks/>
        public string LineNo
        {
            get
            {
                return this.lineNoField;
            }
            set
            {
                this.lineNoField = value;
            }
        }

        /// <remarks/>
        public string LineType
        {
            get
            {
                return this.lineTypeField;
            }
            set
            {
                this.lineTypeField = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public string UnitOfMeasureCode
        {
            get
            {
                return this.unitOfMeasureCodeField;
            }
            set
            {
                this.unitOfMeasureCodeField = value;
            }
        }

        /// <remarks/>
        public string UnitPrice
        {
            get
            {
                return this.unitPriceField;
            }
            set
            {
                this.unitPriceField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SalesDocSalesHeaderAttachment
    {

        private byte attachmentEntryNoField;

        private string filenameField;

        private string extensionField;

        private string attachmentField;

        /// <remarks/>
        public byte AttachmentEntryNo
        {
            get
            {
                return this.attachmentEntryNoField;
            }
            set
            {
                this.attachmentEntryNoField = value;
            }
        }

        /// <remarks/>
        public string Filename
        {
            get
            {
                return this.filenameField;
            }
            set
            {
                this.filenameField = value;
            }
        }

        /// <remarks/>
        public string Extension
        {
            get
            {
                return this.extensionField;
            }
            set
            {
                this.extensionField = value;
            }
        }

        /// <remarks/>
        public string Attachment
        {
            get
            {
                return this.attachmentField;
            }
            set
            {
                this.attachmentField = value;
            }
        }
    }



    public static class Mappers
    {
        public static ProcessResult ConvertToXML(this SalesDoc it)
        {            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SalesDoc));            
            try
            {
                using (StringWriter textWriter = new Utf8StringWriter())
                {                    
                    xmlSerializer.Serialize(textWriter, it);
                    return new ProcessResult(System.Reflection.MethodBase.GetCurrentMethod().Name, textWriter.ToString());
                }
            }
            catch (Exception ex)
            {
                return new ProcessResult(System.Reflection.MethodBase.GetCurrentMethod().Name, ProcessResult.ResultTypeEnum.ERROR, ex.Message);
            }                                 
        }        
    }

}

using DH.Helpdesk.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace DH.Helpdesk.BusinessData.Models.Invoice.Xml
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class SalesDoc
    {

        private SalesDocSalesHeader salesHeaderField;

        private SalesDocSalesLine[] salesLineField;

        private SalesDocAttachment[] attachmentsField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SalesLine")]
        public SalesDocSalesLine[] SalesLine
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public SalesDocAttachment[] Attachments
        {
            get
            {
                return this.attachmentsField;
            }
            set
            {
                this.attachmentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SalesDocSalesHeader
    {

        private string docTypeField;

        private string sellToCustomerNoField;

        private string orderDateField;

        private string ourReferenceNameField;

        private string yourReferenceNameField;

        private string orderNoField;

        private string currencyCodeField;

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
        public string OrderDate
        {
            get
            {
                return this.orderDateField;
            }
            set
            {
                this.orderDateField = value;
            }
        }

        /// <remarks/>
        public string OurReferenceName
        {
            get
            {
                return this.ourReferenceNameField;
            }
            set
            {
                this.ourReferenceNameField = value;
            }
        }

        /// <remarks/>
        public string YourReferenceName
        {
            get
            {
                return this.yourReferenceNameField;
            }
            set
            {
                this.yourReferenceNameField = value;
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SalesDocSalesLine
    {

        private string itemNoField;

        private string descriptionField;

        private string quantityField;

        private string unitOfMeasureCodeField;

        private string unitPriceField;

        /// <remarks/>
        public string ItemNo
        {
            get
            {
                return this.itemNoField;
            }
            set
            {
                this.itemNoField = value;
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
    public partial class SalesDocAttachment
    {

        private string fileNameField;

        private string encodedFileField;

        /// <remarks/>
        public string FileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        /// <remarks/>
        public string EncodedFile
        {
            get
            {
                return this.encodedFileField;
            }
            set
            {
                this.encodedFileField = value;
            }
        }
    }

    

    public static class Mappers
    {
        public static KeyValuePair<bool,string> ConvertToXML(this SalesDoc it)
        {
            var ret = new KeyValuePair<bool, string>(false, "");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SalesDoc));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            try
            {
                using (StringWriter textWriter = new Utf16StringWriter())
                {                    
                    xmlSerializer.Serialize(textWriter, it, namespaces);
                    ret = new KeyValuePair<bool, string>(true, textWriter.ToString());
                }
            }
            catch (Exception ex)
            {
                ret = new KeyValuePair<bool, string>(false, ex.Message);
            }
                     
            return ret;            
        }        
    }

}

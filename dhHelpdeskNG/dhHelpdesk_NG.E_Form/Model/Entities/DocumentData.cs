using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ECT.Model.Entities
{
    public class DocumentData
    {
        //public string BusinessUnit { get; set; }
        //public string Company { get; set; }
        //public string Function { get; set; }
        //public string Department { get; set; }
        public string Id { get; set; }
        public string Process { get; set; }

        public string TypeCode { get; set; }
        public bool BarCodeUse { get; set; }
        public bool HeaderUse { get; set; }
        public bool FooterUse { get; set; }
        public int BarcodeShowOnMinStateSecondaryId { get; set; }
        public HtmlString EmployerSignatureText { get; set; }
        public HtmlString EmployeeSignatureText { get; set; }

        public List<DocumentField> DocumentFields { get; set; }
        public List<DocumentField> DocumentFieldsUse { get; set; }
   
 

    }
}
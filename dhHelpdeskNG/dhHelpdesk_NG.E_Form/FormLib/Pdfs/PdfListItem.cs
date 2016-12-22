using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.EForm.FormLib.Pdfs
{
    public class PdfListItem
    {
        public string Text { get; set; }
        public string Number { get; set; }
        public List<PdfListItem> SubList { get; set; }
        public List<PdfListItem> SubList2 { get; set; }
    }
}
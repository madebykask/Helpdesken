using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECT.Web.Helpers.Pdf
{
    public class PdfList
    {
        public string Text { get; set; }
        public List<PdfList> SubList { get; set; }
    }
}
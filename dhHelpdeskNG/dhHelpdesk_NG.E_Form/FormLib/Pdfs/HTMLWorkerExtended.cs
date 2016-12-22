using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

namespace DH.Helpdesk.EForm.FormLib.Pdfs
{
    public class HTMLWorkerExtended : HTMLWorker
    {
        public HTMLWorkerExtended(IDocListener document)
            : base(document)
        {

        }
        public override void StartElement(string tag, IDictionary<string, string> str)
        {
            if(tag.Equals("newpage"))
                document.Add(Chunk.NEXTPAGE);
            else
                base.StartElement(tag, str);

            if(tag.Equals("endpage"))
                document.SetMarginMirroring(true);
        }
    }
}
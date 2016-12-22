using System;
using System.Web;
using System.Web.Mvc;

namespace ECT.Web.Helpers
{
    public class PdfContentResult : FileContentResult
    {
        public PdfContentResult(byte[] data) : base(data, "application/pdf") { }

        public PdfContentResult(byte[] data, string fileName)
            : this(data)
        {
            if(fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this.FileDownloadName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ClearHeaders();

            base.ExecuteResult(context);

            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Private);
        }
    }
}
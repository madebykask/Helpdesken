namespace DH.Helpdesk.Mobile.Infrastructure.Mvc
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Mobile.Infrastructure.Tools;

    public sealed class UnicodeFileContentResult : ActionResult
    {
        public UnicodeFileContentResult(byte[] file, string fileName)
        {
            if (file == null)
            {
                throw new ArgumentNullException();
            }

            this.File = file;
            this.FileName = fileName;
        }

        public byte[] File { get; private set; }

        public string FileName { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var encoding = Encoding.UTF8;
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            response.Clear();
            response.AddHeader(
                "Content-Disposition", 
                string.Format("attachment; filename={0}", request.Browser.Browser.ToUpper() == "IE" ? HttpUtility.UrlEncode(this.FileName, encoding) : this.FileName));
            response.ContentType = MimeHelper.GetMimeType(this.FileName);
            response.Charset = encoding.WebName;
            response.HeaderEncoding = encoding;
            response.ContentEncoding = encoding;
            response.BinaryWrite(this.File);
            response.End();
        }
    }
}
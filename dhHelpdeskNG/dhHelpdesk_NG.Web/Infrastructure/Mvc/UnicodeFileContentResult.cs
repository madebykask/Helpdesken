namespace DH.Helpdesk.Web.Infrastructure.Mvc
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class UnicodeFileContentResult : ActionResult
    {
        public UnicodeFileContentResult(byte[] file, string fileName, bool isInline = false)
        {
            if (file == null)
            {
                throw new ArgumentNullException();
            }

            this.File = file;
            this.FileName = fileName;
            this.IsInline = isInline;
        }

        public byte[] File { get; private set; }

        public string FileName { get; private set; }

        public bool IsInline { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var encoding = Encoding.UTF8;
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            
            response.Clear();
            
            if (!this.IsInline)
            {
                response.Headers.Remove("Content-Disposition");

                if (request.Browser.Browser.ToUpper() == "IE" ||
                    request.UserAgent.IndexOf("Edge", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var fileName = HttpUtility.UrlEncode(this.FileName, encoding);
                    response.AddHeader("Content-Disposition", string.Format("attachment;filename=\"{0}\";filename*=UTF-8''{0}", fileName.Replace("+", "%20")));
                }
                else
                {
                    response.AddHeader("Content-Disposition", string.Format("attachment;filename=\"{0}\"", FileName));
                }
            }

            response.ContentType = MimeHelper.GetMimeType(this.FileName);
            response.Charset = encoding.WebName;
            response.HeaderEncoding = encoding;
            response.ContentEncoding = encoding;
            response.BinaryWrite(this.File);
            response.End();
        }
    }
}
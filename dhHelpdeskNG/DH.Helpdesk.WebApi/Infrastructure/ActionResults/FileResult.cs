using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DH.Helpdesk.WebApi.Infrastructure.ActionResults
{
    public class FileResult : IHttpActionResult
    {
        private const string DefaultContentType = "application/octet-stream";
        private readonly string _fileName;
        private readonly byte[] _data;
        private readonly HttpRequestMessage _request;
        private readonly string _contentType;
        private readonly bool _isInlineAttachment;

        #region ctor()

        public FileResult(string fileName, byte[] data, HttpRequestMessage request, bool inline = false, string contentType = null)
        {
            _isInlineAttachment = inline;
            _fileName = fileName;
            _data = data;
            _request = request;
            _contentType = string.IsNullOrEmpty(contentType) ? GetMimeType(fileName) : contentType;
        }

        #endregion

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var httpResponseMessage = _request.CreateResponse(HttpStatusCode.OK);

            // The StreamContent will dispose its stream when it is disposed. 
            var ms = new MemoryStream(_data) { Position = 0 };
            httpResponseMessage.Content = new StreamContent(ms);
            
            var contentDisposition = $"attachment; filename=\"{_fileName}\"";

            httpResponseMessage.Content.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse(contentDisposition);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);

            if (_isInlineAttachment)
                httpResponseMessage.Content.Headers.ContentDisposition.DispositionType = System.Net.Mime.DispositionTypeNames.Inline;

            return Task.FromResult(httpResponseMessage);
        }

        #region Private Methods

        private string GetMimeType(string fileName)
        {
            var mimeType = MimeMapping.GetMimeMapping(Path.GetExtension(fileName));
            return string.IsNullOrEmpty(mimeType) ? DefaultContentType :  mimeType;
        }

        #endregion
    }
}
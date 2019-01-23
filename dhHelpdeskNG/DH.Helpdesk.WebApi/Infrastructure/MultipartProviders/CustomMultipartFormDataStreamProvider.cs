using System.Net.Http;
using System.Net.Http.Headers;

namespace DH.Helpdesk.WebApi.Infrastructure.MultipartProviders
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path)
        {
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            //Chrome submits files in quotation marks which get treated as part of the filename and get escaped
            return headers.ContentDisposition.FileName.Trim('\"');
        }
    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using DH.Helpdesk.Common.Logger;
using Microsoft.Owin;

namespace DH.Helpdesk.WebApi.Infrastructure.Owin
{
    public class LogRequestMiddleware : OwinMiddleware
    {
        private readonly ILoggerService _loggerService;

        public LogRequestMiddleware(OwinMiddleware next, ILoggerService loggerService) 
            : base(next)
        {
            _loggerService = loggerService;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var requestBody = new StreamReader(request.Body).ReadToEndAsync().Result;
            var requestUrl = $"{request.Scheme} {request.Host}{request.Path}{request.QueryString};";
            var requestMethod = request.Method;

            context.Request.Body.Position = 0;

            var stream = context.Response.Body;
            var responseBuffer = new MemoryStream();
            context.Response.Body = responseBuffer;
            
            await Next.Invoke(context);

            responseBuffer.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBuffer).ReadToEnd();

            var responseLog = $"Response: {responseBody}. Status: {context.Response.StatusCode}";

            responseBuffer.Seek(0, SeekOrigin.Begin);
            await responseBuffer.CopyToAsync(stream);

            //log request and response side by side 
            if (requestUrl.IndexOf("/Login", StringComparison.OrdinalIgnoreCase) != -1)
                requestBody = string.Empty;

            _loggerService.Debug($"Request/Response. {Environment.NewLine}Request: {requestMethod} {requestUrl} {requestBody}{Environment.NewLine}{responseLog}");
        }
    }
}
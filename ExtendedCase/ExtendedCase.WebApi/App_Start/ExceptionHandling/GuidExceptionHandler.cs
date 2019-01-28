using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Common.Logging;

namespace ExtendedCase.WebApi.ExceptionHandling
{
    public class GuidExceptionHandler : ExceptionHandler
    {
        private readonly ILogger _logger;

        public GuidExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public override void Handle(ExceptionHandlerContext context)
        {
            var errorId = _logger.Error($"Unhandled exception (with response) requesting {context.Request.RequestUri.AbsoluteUri}. Error : {context.Exception}");

            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, $"Error occured. ErrorId: {errorId}. ErrorMessage: {context.Exception.Message}");

            context.Result = new ResponseMessageResult(response);
        }
    }
}
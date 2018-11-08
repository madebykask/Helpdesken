using System;
using System.Text;
using System.Web;
using DH.Helpdesk.Dal.Infrastructure.Context;

namespace DH.Helpdesk.WebApi.Infrastructure.Logger
{
    public sealed class ErrorContext
    {
        private const string LineSeparator = "----------------------------------------------------------------------------------------";

        public ErrorContext(
                Guid errorId,
                Exception exception,
                string controller,
                string action,
                string url,
                string userId = null,
                string customerId = null,
                string requestId = null)
        {
            //HttpContext = httpContext;
            Action = action;
            Controller = controller;
            Exception = exception;
            ErrorId = errorId;
            Url = url;
            UserId =  !string.IsNullOrEmpty(userId) ? userId : "unknown";
            CustomerId = customerId;
            RequestId = requestId;
        }
        public Guid ErrorId { get; private set; }
        public Exception Exception { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }
        public string Url { get; private set; }
        public string RequestId { get; private set; }
        public string UserId { get; private set; }
        public string CustomerId { get; private set; }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.AppendLine();
            res.AppendLine(ErrorId.ToString());
            if(!string.IsNullOrEmpty(RequestId)) res.AppendLine($"RequestId: {RequestId}");
            res.AppendLine();
            if (!string.IsNullOrEmpty(Url)) res.AppendLine($"Url: {Url}");
	        res.AppendLine(Exception.ToString());
            res.AppendLine(LineSeparator);
            res.AppendLine($"Controller: {Controller}");
            res.AppendLine($"Action: {Action}");
            if (!string.IsNullOrEmpty(UserId)) res.AppendLine($"UserId: {UserId}");
            if (!string.IsNullOrEmpty(CustomerId)) res.AppendLine($"CustomerId: {CustomerId}");
            res.AppendLine(LineSeparator);
            return res.ToString();
        }
    }
}
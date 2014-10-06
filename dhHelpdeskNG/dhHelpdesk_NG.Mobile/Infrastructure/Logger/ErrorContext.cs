namespace DH.Helpdesk.Mobile.Infrastructure.Logger
{
    using System;
    using System.Text;
    using System.Web;

    using DH.Helpdesk.Dal.Infrastructure.Context;

    public sealed class ErrorContext
    {
        private const string LineSeparator = "----------------------------------------------------------------------------------------";

        public ErrorContext(
                Exception exception, 
                string controller, 
                string action,
                HttpContext httpContext, 
                IWorkContext workContext)
        {
            this.WorkContext = workContext;
            this.HttpContext = httpContext;
            this.Action = action;
            this.Controller = controller;
            this.Exception = exception;
        }

        public Exception Exception { get; private set; }

        public string Controller { get; private set; }

        public string Action { get; private set; }

        public HttpContext HttpContext { get; private set; }

        public IWorkContext WorkContext { get; private set; }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.AppendLine();
            res.AppendLine(this.HttpContext.Request.Url.AbsoluteUri);
            res.AppendLine();
            res.AppendLine(this.Exception.ToString());
            res.AppendLine(LineSeparator);
            res.AppendLine(string.Format("Controller: {0}", this.Controller));
            res.AppendLine(string.Format("Action: {0}", this.Action));
            if (this.WorkContext != null)
            {
                if (this.WorkContext.User != null)
                {
                    res.AppendLine(string.Format("UserId: {0}", this.WorkContext.User.UserId));
                    res.AppendLine(string.Format("User: {0}", this.WorkContext.User.UserName));                    
                }

                if (this.WorkContext.Customer != null)
                {
                    res.AppendLine(string.Format("CustomerId: {0}", this.WorkContext.Customer.CustomerId));
                    res.AppendLine(string.Format("Customer: {0}", this.WorkContext.Customer.CustomerName));                                    
                }
            }

            res.AppendLine(LineSeparator);
            return res.ToString();
        }
    }
}
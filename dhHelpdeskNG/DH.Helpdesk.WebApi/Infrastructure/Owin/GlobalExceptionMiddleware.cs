using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using DH.Helpdesk.Common.Logger;
using Microsoft.Owin;

namespace DH.Helpdesk.WebApi.Infrastructure.Owin
{
    public class GlobalExceptionMiddleware : OwinMiddleware
    {
        public GlobalExceptionMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var requestScope = context.GetAutofacLifetimeScope();
            var errorLoggerService = requestScope.ResolveKeyed<ILoggerService>(Log4NetLoggerService.LogType.ERROR);
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                errorLoggerService.Error(ex);
                throw;
            }
        }
    }
}
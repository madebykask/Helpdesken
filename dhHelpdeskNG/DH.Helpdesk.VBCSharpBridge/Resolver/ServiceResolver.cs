using DH.Helpdesk.Services.Infrastructure.Email;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.VBCSharpBridge.DI.Modules;
using Ninject;
using System;

namespace DH.Helpdesk.VBCSharpBridge.Resolver
{
    public static class ServiceResolver
    {
        private static IKernel _kernel;

        static ServiceResolver()
        {
            _kernel = new StandardKernel(new ServiceModule(), new InfrastructureModule(), new DatabaseModule());

            // Add other bindings as necessary
        }

        public static ICaseService GetCaseService()
        {
            return _kernel.Get<ICaseService>();
        }
        public static ICaseMailer GetCaseMailerService()
        {
            return _kernel.Get<ICaseMailer>();
        }

        public static ILogService GetCaseLogService()
        {
            return _kernel.Get<ILogService>();
        }
        public static IEmailService GetEmailService()
        {
            return _kernel.Get<IEmailService>();
        }

        public static IMailTemplateService GetTemplateService()
        {
            return _kernel.Get<IMailTemplateService>();
        }
    }
}

using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Mail2TicketCSharpBridge.DI.Modules;
using DH.Helpdesk.Services.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Mail2TicketCSharpBridge.Resolver
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
    }
}

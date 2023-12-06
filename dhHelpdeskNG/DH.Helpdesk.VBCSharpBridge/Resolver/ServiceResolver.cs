using DH.Helpdesk.Services.Services;
using DH.Helpdesk.VBCSharpBridge.DI.Modules;
using Ninject;

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
    }
}

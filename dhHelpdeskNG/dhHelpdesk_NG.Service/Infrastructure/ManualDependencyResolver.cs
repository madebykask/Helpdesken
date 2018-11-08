using Ninject.Parameters;

namespace DH.Helpdesk.Services.Infrastructure
{
    using Ninject;

    public static class ManualDependencyResolver
    {
        private static StandardKernel _kernel;

        public static void SetKernel(StandardKernel kernel)
        {
            _kernel = kernel;
        }

        public static TService Get<TService>()
        {
            return _kernel.Get<TService>();
        }

        public static TService Get<TService>(string name)
        {
            return _kernel.Get<TService>(name);
        }
    }
}

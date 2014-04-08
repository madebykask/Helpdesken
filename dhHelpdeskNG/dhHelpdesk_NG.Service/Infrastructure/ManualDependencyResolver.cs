namespace DH.Helpdesk.Services.Infrastructure
{
    using Ninject;

    public static class ManualDependencyResolver
    {
        private static StandardKernel kernel;

        public static void SetKernel(StandardKernel kernel)
        {
            ManualDependencyResolver.kernel = kernel;
        }

        public static TService Get<TService>()
        {
            return kernel.Get<TService>();
        }
    }
}

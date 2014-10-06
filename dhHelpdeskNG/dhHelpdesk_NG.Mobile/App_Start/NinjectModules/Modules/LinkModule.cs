using DH.Helpdesk.Web.Infrastructure.ModelFactories.Link;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Link.Concrete;
using Ninject.Modules;

namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    public sealed class LinkModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILinkModelFactory>().To<LinkModelFactory>().InSingletonScope();
        }
    }
}
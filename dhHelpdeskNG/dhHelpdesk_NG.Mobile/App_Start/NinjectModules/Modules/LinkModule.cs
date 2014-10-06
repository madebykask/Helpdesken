using DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Link;
using DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Link.Concrete;
using Ninject.Modules;

namespace DH.Helpdesk.Mobile.NinjectModules.Modules
{
    public sealed class LinkModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILinkModelFactory>().To<LinkModelFactory>().InSingletonScope();
        }
    }
}
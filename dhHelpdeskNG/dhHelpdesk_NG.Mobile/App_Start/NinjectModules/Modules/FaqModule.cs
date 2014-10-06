namespace DH.Helpdesk.Mobile.NinjectModules.Modules
{
    using DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Faq;
    using DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Faq.Concrete;

    using Ninject.Modules;

    public sealed class FaqModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IIndexModelFactory>().To<IndexModelFactory>().InSingletonScope();
            this.Bind<INewFaqModelFactory>().To<NewFaqModelFactory>().InSingletonScope();
            this.Bind<IEditFaqModelFactory>().To<EditFaqModelFactory>().InSingletonScope();
        }
    }
}
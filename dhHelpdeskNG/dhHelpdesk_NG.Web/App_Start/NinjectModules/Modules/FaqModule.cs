namespace dhHelpdesk_NG.Web.NinjectModules.Modules
{
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq.Concrete;

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
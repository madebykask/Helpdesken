namespace dhHelpdesk_NG.Web.App_Start.NinjectModules.Faq
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq.Concrete;

    public sealed class ModelFactoriesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IIndexModelFactory>().To<IndexModelFactory>().InSingletonScope();
            this.Bind<INewFaqModelFactory>().To<NewFaqModelFactory>().InSingletonScope();
            this.Bind<IEditFaqModelFactory>().To<EditFaqModelFactory>().InSingletonScope();
        }
    }
}
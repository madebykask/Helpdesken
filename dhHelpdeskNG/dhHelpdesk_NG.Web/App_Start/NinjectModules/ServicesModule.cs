namespace dhHelpdesk_NG.Web.App_Start.NinjectModules
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Service.Concrete;

    public sealed class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IFaqService>().To<FaqService>();
            this.Bind<IFaqCategoryService>().To<FaqCategoryService>();
            this.Bind<INotifierService>().To<NotifierService>();
        }
    }
}
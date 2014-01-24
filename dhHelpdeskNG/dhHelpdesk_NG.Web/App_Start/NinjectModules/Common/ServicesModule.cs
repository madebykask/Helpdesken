namespace dhHelpdesk_NG.Web.NinjectModules.Common
{
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Service.Concrete;

    using Ninject.Modules;

    public sealed class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IFaqService>().To<FaqService>();
            this.Bind<IFaqCategoryService>().To<FaqCategoryService>();
            this.Bind<INotifierService>().To<NotifierService>();
            this.Bind<IProjectService>().To<ProjectService>();
        }
    }
}
namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;

    using Ninject.Modules;

    public sealed class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IFaqService>().To<FaqService>();
            this.Bind<IFaqCategoryService>().To<FaqCategoryService>();
            this.Bind<INotifierService>().To<NotifierService>();
            this.Bind<IProjectService>().To<ProjectService>();
            this.Bind<IChangeService>().To<ChangeService>();
            this.Bind<IQestionnaireService>().To<QuestionnaireService>();
        }
    }
}
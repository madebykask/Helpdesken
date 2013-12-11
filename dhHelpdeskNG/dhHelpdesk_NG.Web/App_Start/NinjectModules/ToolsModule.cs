namespace dhHelpdesk_NG.Web.App_Start.NinjectModules
{
    using Ninject.Modules;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.Tools;
    using dhHelpdesk_NG.Web.Infrastructure.Tools.Concrete;

    public sealed class ToolsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IWebTemporaryStorage>().To<WebTemporaryStorage>().InSingletonScope();
            this.Bind<IFilesStorage>().To<FilesStorage>().InSingletonScope();
        }
    }
}
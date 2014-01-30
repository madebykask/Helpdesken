namespace dhHelpdesk_NG.Web.NinjectModules.Common
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.Tools;
    using dhHelpdesk_NG.Web.Infrastructure.Tools.Concrete;

    using Ninject.Modules;

    public sealed class ToolsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IWebTemporaryStorage>().To<WebTemporaryStorage>().InSingletonScope();
            this.Bind<IFilesStorage>().To<FilesStorage>().InSingletonScope();
            this.Bind<IUserEditorValuesStorageFactory>().To<UserEditorValuesStorageFactory>().InSingletonScope();
        }
    }
}
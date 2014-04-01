namespace DH.Helpdesk.SelfService.NinjectModules.Common
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Concrete;
    using DH.Helpdesk.SelfService.Infrastructure;    
    using DH.Helpdesk.SelfService.Infrastructure.Tools;
    using DH.Helpdesk.SelfService.Infrastructure.Tools.Concrete;
    using DH.Helpdesk.Services.Infrastructure.SettingProviders;
    using DH.Helpdesk.Services.Infrastructure.SettingProviders.Concrete;

    using Ninject.Modules;

    public sealed class ToolsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IFilesStorage>().To<FilesStorage>().InSingletonScope();
            this.Bind<IUserEditorValuesStorageFactory>().To<UserEditorValuesStorageFactory>().InSingletonScope();
            

            //this.Bind<IUserTemporaryFilesStorage>().To<UserTemporaryFilesStorage>().InSingletonScope();            
        }
    }
}
namespace DH.Helpdesk.NewSelfService
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Concrete;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
    using DH.Helpdesk.NewSelfService.Infrastructure.Common;
    using DH.Helpdesk.NewSelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.NewSelfService.Infrastructure.Tools;
    using DH.Helpdesk.NewSelfService.Infrastructure.Tools.Concrete;
    using Ninject.Modules;
    using Ninject.Web.Common;

    public sealed class ToolsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IUserEditorValuesStorageFactory>().To<UserEditorValuesStorageFactory>().InSingletonScope();

            this.Bind<ICommonFunctions>().To<CommonFunctions>().InRequestScope();
            //this.Bind<IUserTemporaryFilesStorage>().To<UserTemporaryFilesStorage>().InSingletonScope();       

            this.Bind<ISessionFactory>().To<HelpdeskSessionFactory>().InRequestScope();
            this.Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InRequestScope();
        }
    }
}
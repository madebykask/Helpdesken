namespace DH.Helpdesk.SelfService
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Concrete;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
    using DH.Helpdesk.SelfService.Infrastructure.Common;
    using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.SelfService.Infrastructure.Tools;
    using DH.Helpdesk.SelfService.Infrastructure.Tools.Concrete;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Services.BusinessLogic.MailTools.TemplateFormatters;

    public sealed class ToolsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IUserEditorValuesStorageFactory>().To<UserEditorValuesStorageFactory>().InSingletonScope();

            this.Bind<ICommonFunctions>().To<CommonFunctions>().InRequestScope();
            //this.Bind<IUserTemporaryFilesStorage>().To<UserTemporaryFilesStorage>().InSingletonScope();       

            this.Bind<ISessionFactory>().To<HelpdeskSessionFactory>().InRequestScope();
            this.Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InRequestScope();
            this.Bind<IMailTemplateFormatterNew>().To<MailTemplateFormatterNew>().InRequestScope();
        }
    }
}
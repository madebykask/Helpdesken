using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Concrete;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.BusinessLogic;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.MailTools;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.BusinessLogic.OtherTools.Concrete;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Infrastructure.Concrete;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common.Concrete;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;
    using Infrastructure.ModelFactories.Case.Concrete;
    using Ninject.Modules;
    using Ninject.Web.Common;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;
    using UnitOfWork = DH.Helpdesk.Dal.Infrastructure.UnitOfWork;

    public sealed class ToolsModule : NinjectModule
    {
        #region Public Methods and Operators

        public override void Load()
        {
            this.Bind<ICacheProvider>().To<CacheProvider>();
            this.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();
            this.Bind<IElementaryRulesValidator>().To<ElementaryRulesValidator>().InSingletonScope();
            this.Bind<IEmailSendingSettingsProvider>().To<EmailSendingSettingsProvider>().InSingletonScope();
            this.Bind<IExcelFileComposer>().To<ExcelFileComposer>().InSingletonScope();
            this.Bind<IExportFileNameFormatter>().To<ExportFileNameFormatter>().InSingletonScope();
            this.Bind<IFilesStorage>().To<FilesStorage>().InSingletonScope();
            this.Bind<IMailUniqueIdentifierProvider>().To<MailUniqueIdentifierProvider>().InSingletonScope();
            this.Bind<TemporaryIdProvider>().To<TemporaryIdProvider>().InSingletonScope();
            this.Bind<ISendToDialogModelFactory>().To<SendToDialogModelFactory>().InSingletonScope();
#pragma warning disable 0618
            this.Bind<IUnitOfWork>().To<UnitOfWork>();
#pragma warning restore 0618
            this.Bind<IEditorStateCacheFactory>().To<EditorStateCacheFactory>().InSingletonScope();
            this.Bind<ITemporaryFilesCacheFactory>().To<TemporaryFilesCacheFactory>().InSingletonScope();

            this.Bind<ISessionFactory>().To<HelpdeskSessionFactory>().InRequestScope();
            this.Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InRequestScope();

            this.Bind<IMailTemplateFormatterNew>().To<MailTemplateFormatterNew>().InRequestScope();
            this.Bind<IRouteResolver>().To<RouteResolver>().InSingletonScope();
            this.Bind<ICaseRuleFactory>().To<CaseRuleFactory>().InSingletonScope();
        }

        #endregion
    }
}
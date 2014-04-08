namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Concrete;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.BusinessLogic.MailTools;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Infrastructure.Concrete;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common.Concrete;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;

    using Ninject.Modules;

    public sealed class ToolsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IFilesStorage>().To<FilesStorage>().InSingletonScope();
            this.Bind<IUserEditorValuesStorageFactory>().To<UserEditorValuesStorageFactory>().InSingletonScope();
            this.Bind<IUserTemporaryFilesStorageFactory>().To<UserTemporaryFilesStorageFactory>().InSingletonScope();
            this.Bind<ISendToDialogModelFactory>().To<SendToDialogModelFactory>().InSingletonScope();
            this.Bind<IEmailSendingSettingsProvider>().To<EmailSendingSettingsProvider>().InSingletonScope();
            this.Bind<IMailUniqueIdentifierProvider>().To<MailUniqueIdentifierProvider>().InSingletonScope();
            this.Bind<IExcelFileComposer>().To<ExcelFileComposer>().InSingletonScope();
            this.Bind<IExportFileNameFormatter>().To<ExportFileNameFormatter>().InSingletonScope();
        }
    }
}
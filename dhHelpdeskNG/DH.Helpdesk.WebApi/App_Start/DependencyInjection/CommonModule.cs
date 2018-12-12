using Autofac;
using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Concrete;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email.Concrete;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers.Concrete;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users.Concrete;
using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.Services.Infrastructure.Concrete;
using DH.Helpdesk.Services.Infrastructure.Email;
using DH.Helpdesk.Services.Infrastructure.Email.Concrete;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.WebApi.Infrastructure.Cache;
using DH.Helpdesk.WebApi.Infrastructure.Config;
using DH.Helpdesk.WebApi.Logic.CaseFieldSettings;

namespace DH.Helpdesk.WebApi.DependencyInjection
{
    /// <summary>
    /// The common module.
    /// </summary>
    public class CommonModule : Module
    {
        /// <summary>
        /// The load.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //NOTE: When registering a service as a singleton mind its dependicies - they could be of per-request lifetime scope!
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            builder.RegisterType<WebCacheService>()
                .As<ICacheService>().SingleInstance();

            //this. builder.RegisterType<IHelpdeskCache>()
            //    .As<HelpdeskCache>();

            //this. builder.RegisterType<IModulesInfoFactory>().As<ModulesInfoFactory>().SingleInstance();

            //this. builder.RegisterType<IDbQueryExecutorFactory>()
            //    .As<SqlDbQueryExecutorFactory>()
            //    .SingleInstance();

            //this. builder.RegisterType<IJsonSerializeService>()
            //    .As<JsonSerializeService>()
            //    .SingleInstance();

            builder.RegisterType<UserPermissionsChecker>().As<IUserPermissionsChecker>();

            builder.RegisterType<ApplicationConfiguration>().As<IApplicationConfiguration>().SingleInstance();
            

            builder.RegisterType<FilesStorage>()
                .As<IFilesStorage>()
                .SingleInstance();

            builder.RegisterType<TemporaryFilesCacheFactory>()
                .As<ITemporaryFilesCacheFactory>()
                .SingleInstance();

            builder.RegisterType<CaseMailer>()
                .As<ICaseMailer>()
                .SingleInstance();

            builder.RegisterType<EmailFactory>()
                .As<IEmailFactory>()
                .SingleInstance();

            builder.RegisterType<EmailSendingSettingsProvider>()
                .As<IEmailSendingSettingsProvider>()
                .SingleInstance();

            builder.RegisterType<CacheProvider>().
                As<ICacheProvider>();

            builder.RegisterType<MailTemplateFormatterNew>().
                As<IMailTemplateFormatterNew>()
                .SingleInstance();

            builder.RegisterType<NotifierFieldSettingsFactory>()
                .As<INotifierFieldSettingsFactory>()
                .SingleInstance();

            builder.RegisterType<ExcelFileComposer>()
                .As<IExcelFileComposer>()
                .SingleInstance();

            builder.RegisterType<JsonSerializeService>()
                .As<IJsonSerializeService>()
                .SingleInstance();

            builder.RegisterType<CaseFieldSettingsHelper>()
                .As<ICaseFieldSettingsHelper>()
                .SingleInstance();
        }
    }
}
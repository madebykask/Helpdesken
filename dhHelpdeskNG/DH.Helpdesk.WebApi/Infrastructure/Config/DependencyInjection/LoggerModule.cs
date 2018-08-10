using Autofac;
using Autofac.Integration.WebApi;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.WebApi.Infrastructure.Config.Filters;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<Log4NetLoggerService>()
                .Named<ILoggerService>(Log4NetLoggerService.LogType.ERROR)
                //.As<IErrorLoggerService>()
                .WithParameter(new TypedParameter(typeof(string), Log4NetLoggerService.LogType.ERROR))
                .SingleInstance();

            builder.RegisterType<Log4NetLoggerService>()
                .Named<ILoggerService>(Log4NetLoggerService.LogType.Session)
                //.As<ISessionLoggerService>()
                .WithParameter(new TypedParameter(typeof(string), Log4NetLoggerService.LogType.Session))
                .SingleInstance();

            builder.Register(c => new ApiExceptionFilter(c.ResolveKeyed<ILoggerService>(Log4NetLoggerService.LogType.ERROR)))
                .AsWebApiExceptionFilterFor<BaseApiController>()
                .InstancePerRequest();
        }
    }
}
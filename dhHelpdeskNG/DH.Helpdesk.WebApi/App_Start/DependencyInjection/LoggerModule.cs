﻿using Autofac;
using Autofac.Integration.WebApi;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.ClientLogger;
using DH.Helpdesk.WebApi.Infrastructure.Filters;

namespace DH.Helpdesk.WebApi.DependencyInjection
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterLoggerServices(builder);

            RegisterClientLoggerServices(builder);

            builder.Register(c => new ApiExceptionFilter(c.ResolveKeyed<ILoggerService>(Log4NetLoggerService.LogType.ERROR)))
                .AsWebApiExceptionFilterFor<BaseApiController>()
                .InstancePerRequest();
        }

        private static void RegisterLoggerServices(ContainerBuilder builder)
        {
            builder.RegisterType<Log4NetLoggerService>()
                .Named<ILoggerService>(Log4NetLoggerService.LogType.ERROR)
                .WithParameter(new TypedParameter(typeof(string), Log4NetLoggerService.LogType.ERROR))
                .SingleInstance();

            builder.RegisterType<Log4NetLoggerService>()
                .Named<ILoggerService>(Log4NetLoggerService.LogType.Session)
                .WithParameter(new TypedParameter(typeof(string), Log4NetLoggerService.LogType.Session))
                .SingleInstance();

            builder.RegisterType<Log4NetLoggerService>()
                .Named<ILoggerService>(Log4NetLoggerService.LogType.Client)
                .WithParameter(new TypedParameter(typeof(string), Log4NetLoggerService.LogType.Client))
                .SingleInstance();

            //default logger registration
            builder.RegisterType<Log4NetLoggerService>()                
                .As<ILoggerService>()
                .WithParameter(new TypedParameter(typeof(string), "debug"))
                .SingleInstance();
        }

        private static void RegisterClientLoggerServices(ContainerBuilder builder)
        {
            builder.RegisterType<ClientLogMessageFormatter>().As<IClientLogMessageFormatter>().SingleInstance();

            builder.Register(c =>
                    new Infrastructure.ClientLogger.ClientLogger(
                        c.Resolve<IClientLogMessageFormatter>(),
                        c.ResolveKeyed<ILoggerService>(Log4NetLoggerService.LogType.Client)))
                .As<IClientLogger>()
                .SingleInstance();
        }
    }
}
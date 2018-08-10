﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Filters;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using DH.Helpdesk.WebApi.Infrastructure.Config;
using DH.Helpdesk.WebApi.Infrastructure.Config.Filters;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace DH.Helpdesk.WebApi
{
    public static partial class WebApiConfig
    {
        public const string UrlPrefix = "api";
        public const string UrlPrefixRelative = "~/api";

        public static void Register(HttpConfiguration config)
        {
            //config.SuppressDefaultHostAuthentication();

            FiltersConfig(config.Filters);
            RoutesConfig(config);
            JsonFormatConfig(config);

            // make all web-api requests to be sent over https
            //config.MessageHandlers.Add(new EnforceHttpsHandler());
        }

        private static void FiltersConfig(HttpFilterCollection filters)
        {
            //filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //filters.Add(new ApiExceptionFilter()); // Using Autofac implementation of ApiExceptionFilter
            filters.Add(new ModelValidationApiActionFilter());
            filters.Add(new AuthorizeApiAttribute());
            //filters.Add(new SessionApiRequiredAttribute());
            //filters.Add(new ValidateApiAntiForgeryTokenAttribute());

        }

        private static void RoutesConfig(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: ConfigApi.Constants.DefaultRouteName,
                routeTemplate: UrlPrefix + "/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
        }

        private static void JsonFormatConfig(HttpConfiguration config)
        {
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //By default it is ISO 8601 local time 2012-07-27T11:51:45.53403-07:00

            // Convert all dates to UTC 2012-07-27T18:51:45.53403Z
            //jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            //json.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            //json.SerializerSettings.DateFormatString = "MM/dd/yyyy-MM-dd";
            // Convert to Microsoft JSON date format ("\/Date(ticks)\/") 
            // json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}

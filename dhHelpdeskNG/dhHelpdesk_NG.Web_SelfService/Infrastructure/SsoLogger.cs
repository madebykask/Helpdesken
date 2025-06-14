﻿using System;
using System.Web;
using DH.Helpdesk.Common.Logger;
using log4net;

namespace DH.Helpdesk.SelfService.Infrastructure
{
    public static class SsoLogger
    {
        private static ILoggerService _logger = null;
        
        public static void SetLoggerInstance(ILoggerService logger)
        {
            _logger = logger;
        }

        public static void Debug(string msg)
        {
            _logger?.Debug(msg);
        }

        public static void Debug(string msg, HttpContext ctx)
        {
            if (_logger == null)
                return;

            var request = ctx.Request;
            var identity = ctx.User?.Identity;
            var isAuthenticated = identity?.IsAuthenticated ?? false;
            var contextInfo = $"Authenticated: {isAuthenticated}, User: {identity?.Name}, Request: {request.Url}";
            _logger.Debug($"{msg} {contextInfo}");
        }
    }
}
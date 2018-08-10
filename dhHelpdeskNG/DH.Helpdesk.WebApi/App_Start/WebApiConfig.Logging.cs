using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.WebApi
{
    public static partial class WebApiConfig
    {
        public static void InitLogging()
        {
            log4net.Config.XmlConfigurator.Configure(
                new System.IO.FileInfo(
                    HttpContext.Current.Server.MapPath("~/log4net.config")));
        }
    }
}
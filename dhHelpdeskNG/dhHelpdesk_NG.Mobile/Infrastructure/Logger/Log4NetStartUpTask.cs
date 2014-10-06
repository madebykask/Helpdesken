namespace DH.Helpdesk.Mobile.Infrastructure.Logger
{
    using System.Web;

    using DH.Helpdesk.Common.Logger;

    public class Log4NetStartUpTask : IStartUpTask
    {
        public bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        public void Configure()
        {
            log4net.Config.XmlConfigurator.Configure(
                new System.IO.FileInfo(
                    HttpContext.Current.Server.MapPath("~/log4net.config")));

            // TODO: handle wrong or non-existent configuration file
        }
    }
}
using DH.Helpdesk.EmailEngine.Library;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.EmailEngine.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            XmlConfigurator.Configure();
            var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            var emailProcessor = new EmailProcessor(logger);

            emailProcessor.ProcessEmails();
        }
    }
}

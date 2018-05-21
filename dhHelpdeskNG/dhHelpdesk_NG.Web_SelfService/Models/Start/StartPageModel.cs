using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.SelfService.Models.Start
{
    public class StartPageModel
    {
        public StartPageModel(string welcomeHtml)
        {
            this.WelcomeHtml = welcomeHtml;
        }

        public List<BulletinBoard> BulletinBoard { get; set; }
        public IList<OperationLog> OperationLog { get; set; }

        public string WelcomeHtml { get; set; }     

    }
}
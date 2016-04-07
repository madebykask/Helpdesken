using System;

namespace DH.Helpdesk.SelfService.Models.Help
{
    public class AboutModel
    {
        public AboutModel(string htmlPageData)
        {
            this.HtmlPageData = htmlPageData;
        }

        public string HtmlPageData { get; private set; }     
    }
}
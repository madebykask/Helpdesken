using System;

namespace DH.Helpdesk.SelfService.Models.Help
{
    public class HelpModel
    {
        public HelpModel(string htmlPageData)
        {
            this.HtmlPageData = htmlPageData;
        }

        public string HtmlPageData { get; private set; }     
    }
}
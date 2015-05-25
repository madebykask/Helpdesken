using System;

namespace DH.Helpdesk.NewSelfService.Models.Help
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
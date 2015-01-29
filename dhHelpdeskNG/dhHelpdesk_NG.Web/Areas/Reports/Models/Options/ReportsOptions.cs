namespace DH.Helpdesk.Web.Areas.Reports.Models.Options
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public sealed class ReportsOptions
    {
        public ReportsOptions(
            Dictionary<string, string> translations, 
            List<ItemOverview> reports)
        {
            this.Reports = reports;
            this.Translations = translations;
        }

        public Dictionary<string, string> Translations { get; private set; } 

        public List<ItemOverview> Reports { get; private set; }
    }
}
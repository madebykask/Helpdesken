namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Overview.Computer
{
    using System;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(string state, bool stolen, string replaced, bool sendBack, DateTime scrapDate)
        {
            this.State = state;
            this.Stolen = stolen;
            this.Replaced = replaced;
            this.SendBack = sendBack;
            this.ScrapDate = scrapDate;
        }

        public string State { get; set; }

        public bool Stolen { get; set; }

        public string Replaced { get; set; }

        public bool SendBack { get; set; }

        public DateTime ScrapDate { get; set; }
    }
}
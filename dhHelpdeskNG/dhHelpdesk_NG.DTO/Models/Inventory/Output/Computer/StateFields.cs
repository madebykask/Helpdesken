namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    using System;

    public class StateFields
    {
        public StateFields(string stateName, bool stolen, string replaced, bool sendBack, DateTime? scrapDate)
        {
            this.StateName = stateName;
            this.Stolen = stolen;
            this.Replaced = replaced;
            this.SendBack = sendBack;
            this.ScrapDate = scrapDate;
        }

        public string StateName { get; set; }

        public bool Stolen { get; set; }

        public string Replaced { get; set; }

        public bool SendBack { get; set; }

        public DateTime? ScrapDate { get; set; }
    }
}
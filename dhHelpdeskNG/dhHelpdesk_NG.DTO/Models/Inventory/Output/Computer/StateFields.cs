namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    using System;

    public class StateFields
    {
        public StateFields(int state, bool stolen, string replaced, bool sendBack, DateTime scrapDate)
        {
            this.State = state;
            this.Stolen = stolen;
            this.Replaced = replaced;
            this.SendBack = sendBack;
            this.ScrapDate = scrapDate;
        }

        public int State { get; set; }

        public bool Stolen { get; set; }

        public string Replaced { get; set; }

        public bool SendBack { get; set; }

        public DateTime ScrapDate { get; set; }
    }
}
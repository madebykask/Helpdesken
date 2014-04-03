namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFields
    {
        public StateFields(int? state, bool isStolen, string replaced, bool isSendBack, DateTime scrapDate)
        {
            this.State = state;
            this.IsStolen = isStolen;
            this.Replaced = replaced;
            this.IsSendBack = isSendBack;
            this.ScrapDate = scrapDate;
        }

        [IsId]
        public int? State { get; set; }

        public bool IsStolen { get; set; }

        public string Replaced { get; set; }

        public bool IsSendBack { get; set; }

        public DateTime ScrapDate { get; set; }
    }
}
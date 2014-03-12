namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class FaqOverview
    {
        [IsId]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Text { get; set; }
    }
}

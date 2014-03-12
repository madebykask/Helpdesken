namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FaqFileOverview
    {
        [IsId]
        public int FaqId { get; set; }

        public string Name { get; set; }
    }
}

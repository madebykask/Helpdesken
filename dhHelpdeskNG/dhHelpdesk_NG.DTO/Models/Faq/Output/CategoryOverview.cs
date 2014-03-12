namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CategoryOverview
    {
        [IsId]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

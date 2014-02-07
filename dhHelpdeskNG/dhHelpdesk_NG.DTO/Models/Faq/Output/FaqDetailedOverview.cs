namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    public sealed class FaqDetailedOverview : FaqOverview
    {
        public string Answer { get; set; }

        public string InternalAnswer { get; set; }

        public string UrlOne { get; set; }

        public string UrlTwo { get; set; }
    }
}

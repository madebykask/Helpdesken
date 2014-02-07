namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    using System;

    public sealed class Faq
    {
        public int Id { get; set; }

        public string UrlOne { get; set; }

        public string UrlTwo { get; set; }

        public int CustomerId { get; set; }

        public int FaqCategoryId { get; set; }

        public int? WorkingGroupId { get; set; }

        public string Answer { get; set; }

        public string InternalAnswer { get; set; }

        public string Question { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public bool ShowOnStartPage { get; set; }

        public bool InformationIsAvailableForNotifiers { get; set; }
    }
}

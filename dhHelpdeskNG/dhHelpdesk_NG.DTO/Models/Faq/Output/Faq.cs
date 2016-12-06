namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class Faq
    {
        [IsId]
        public int Id { get; set; }

        public string UrlOne { get; set; }

        public string UrlTwo { get; set; }

        [IsId]
        public int CustomerId { get; set; }

        [IsId]
        public int FaqCategoryId { get; set; }

        [IsId]
        public int? WorkingGroupId { get; set; }

        public string Answer { get; set; }

        public string InternalAnswer { get; set; }

        public string Question { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public bool ShowOnStartPage { get; set; }

        public bool InformationIsAvailableForNotifiers { get; set; }

        public FaqFile[] Files { get; set; }

    }

    public sealed class FaqFile
    {
        public int Id { get; set; }

        public int Faq_Id { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}

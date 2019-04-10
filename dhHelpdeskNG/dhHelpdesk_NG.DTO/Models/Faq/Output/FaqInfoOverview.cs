using System;
using DH.Helpdesk.Domain.Faq;

namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    public sealed class FaqInfoOverview : FaqOverview
    {
        public string Answer { get; set; }
        public DateTime ChangeDate { get; set; }
        public FaqCategoryEntity Category { get; set; }
    }
}
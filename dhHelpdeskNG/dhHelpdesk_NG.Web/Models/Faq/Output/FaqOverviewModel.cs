using System;
using DH.Helpdesk.Common.Extensions.DateTime;

namespace DH.Helpdesk.Web.Models.Faq.Output
{
    public class FaqOverviewModel
    {
        public FaqOverviewModel(int id, DateTime createdDate, DateTime changedDate, string text)
        {
            Id = id;
            CreatedDate = createdDate;
            ChangedDate = changedDate;
            Text = text;
        }

        public int Id { get; }

        public string Text { get; }

        public DateTime CreatedDate { get; }

        public DateTime ChangedDate { get; }

        public string CreatedDateText
        {
            get
            {
                return CreatedDate.ToFormattedDate();
            }
        }

        public string ChangedDateText
        {
            get
            {
                return ChangedDate.ToFormattedDate();
            }
        }
    }
}
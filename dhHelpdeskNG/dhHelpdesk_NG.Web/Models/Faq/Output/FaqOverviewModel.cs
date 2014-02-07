namespace DH.Helpdesk.Web.Models.Faq.Output
{
    using System;

    public class FaqOverviewModel
    {
        public FaqOverviewModel(int id, string createdDate, string text)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException("id", "Must be more than zero.");
            }

            if (string.IsNullOrEmpty(createdDate))
            {
                throw new ArgumentNullException("createdDate", "Value cannot be null.");
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text", "Value cannot be null or empty.");
            }

            this.Id = id;
            this.CreatedDate = createdDate;
            this.Text = text;
        }

        public int Id { get; private set; }

        public string Text { get; private set; }

        public string CreatedDate { get; private set; }
    }
}
namespace dhHelpdesk_NG.DTO.DTOs.Faq.Input
{
    using System;

    public sealed class NewFaqFileDto : IBusinessModelWithId
    {
        public NewFaqFileDto(byte[] content, string name, DateTime createdDate, int faqId)
        {
            if (content == null || content.Length == 0)
            {
                throw new ArgumentNullException("content", "Value cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Value cannot be null or empty.");
            }

            if (faqId <= 0)
            {
                throw new ArgumentOutOfRangeException("faqId", "Must be more than zero.");
            }

            this.Content = content;
            this.Name = name;
            this.CreatedDate = createdDate;
            this.FaqId = faqId;
        }

        public byte[] Content { get; private set; }

        public string Name { get; private set; }

        public int FaqId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int Id { get; set; }
    }
}

namespace DH.Helpdesk.BusinessData.Models.Faq.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewFaqFile : INewBusinessModel
    {
        public NewFaqFile(byte[] content, string name, DateTime createdDate, int faqId)
        {
            this.Content = content;
            this.Name = name;
            this.CreatedDate = createdDate;
            this.FaqId = faqId;
        }

        [NotNullAndEmptyArray]
        public byte[] Content { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [IsId]
        public int FaqId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int Id { get; set; }
    }
}

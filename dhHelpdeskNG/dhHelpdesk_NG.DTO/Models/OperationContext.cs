namespace DH.Helpdesk.BusinessData.Models
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OperationContext
    {
        public DateTime DateAndTime { get; set; }

        [IsId]
        public int CustomerId { get; set; }

        [IsId]
        public int LanguageId { get; set; }

        [IsId]
        public int UserId { get; set; }
    }
}

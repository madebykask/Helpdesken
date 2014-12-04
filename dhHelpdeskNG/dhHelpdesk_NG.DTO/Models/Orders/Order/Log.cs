namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using System;

    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class Log
    {
        public Log(int id, DateTime dateAndTime, UserName registeredBy, string text)
        {
            this.Id = id;
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Text = text;
        }

        [IsId]
        public int Id { get; private set; }

        public DateTime DateAndTime { get; private set; }

        [NotNull]
        public UserName RegisteredBy { get; private set; }

        public string Text { get; private set; }
    }
}
namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels
{
    using System;

    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class LogModel
    {
        public LogModel(
                int id, 
                DateTime dateAndTime, 
                UserName registeredBy, 
                string text)
        {
            this.Id = id;
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Text = text;
        }

        [IsId]
        public int Id { get; set; }

        [LocalizedDisplay("Datum")]
        public DateTime DateAndTime { get; set; }

        [NotNull]
        [LocalizedDisplay("Anmälare")]
        public UserName RegisteredBy { get; set; }

        [NotNullAndEmpty]
        [LocalizedDisplay("Text")]
        public string Text { get; set; }
    }
}
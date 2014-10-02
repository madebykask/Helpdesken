namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Write
{
    using System;

    public sealed class CircularForUpdate : Circular
    {
        public CircularForUpdate(int id, string circularName, DateTime changedDate)
            : base(circularName)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
        }

        public DateTime ChangedDate { get; private set; }
    }
}
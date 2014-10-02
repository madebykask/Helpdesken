namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    using System;

    public sealed class CircularForEdit : Circular
    {
        public CircularForEdit(int id, string circularName, int status, DateTime createdDate, DateTime changedDate)
            : base(circularName)
        {
            this.Id = id;
            this.Status = status;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
        }

        public int Status { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }
    }
}

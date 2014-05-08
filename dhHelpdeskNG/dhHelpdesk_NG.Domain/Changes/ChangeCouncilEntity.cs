namespace DH.Helpdesk.Domain.Changes
{
    using global::System;

    public class ChangeCouncilEntity : Entity
    {
        public Guid ChangeCouncilGUID { get; set; }

        public int Change_Id { get; set; }

        public virtual ChangeEntity Change { get; set; }

        public string ChangeCouncilName { get; set; }

        public string ChangeCouncilEmail { get; set; }

        public int ChangeCouncilStatus { get; set; }

        public string ChangeCouncilNote { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }
    }
}
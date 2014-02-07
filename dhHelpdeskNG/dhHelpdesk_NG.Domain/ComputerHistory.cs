namespace DH.Helpdesk.Domain
{
    using global::System;

    public class ComputerHistory : Entity
    {
        public int Computer_Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Computer Computer { get; set; }
    }
}

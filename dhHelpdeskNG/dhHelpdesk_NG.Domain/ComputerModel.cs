namespace DH.Helpdesk.Domain
{
    using global::System;

    public class ComputerModel : Entity
    {
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

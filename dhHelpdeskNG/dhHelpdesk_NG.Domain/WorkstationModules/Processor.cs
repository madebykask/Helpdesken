namespace DH.Helpdesk.Domain.WorkstationModules
{
    using global::System;

    public class Processor : Entity
    {
        public string Name { get; set; }
        public int? Customer_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Customer Customer { get; set; }
    }
}

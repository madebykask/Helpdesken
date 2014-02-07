namespace DH.Helpdesk.Domain
{
    using global::System;

    public class ComputerType : Entity
    {
        public int Customer_Id { get; set; }
        public string ComputerTypeDescription { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}

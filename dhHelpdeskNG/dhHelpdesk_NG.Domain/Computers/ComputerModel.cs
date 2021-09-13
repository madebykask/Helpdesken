namespace DH.Helpdesk.Domain.Computers
{
    using global::System;

    public class ComputerModel : Entity
    {
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? Customer_Id { get; set; }
        public virtual Customer Customer { get; set; }
    }
}

namespace DH.Helpdesk.Domain.Computers
{
    using global::System;

    public class ComputerUserLog : Entity
    {
        public int ComputerUser_Id { get; set; }
        public int CreatedByUser_Id { get; set; }
        public string Logtext { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ComputerUserCustomerUserGroup ComputerUser { get; set; }
    }
}

namespace DH.Helpdesk.Domain.Computers
{
    using global::System;

    public class ComputerUsersBlackList : Entity
    {
        public string User_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

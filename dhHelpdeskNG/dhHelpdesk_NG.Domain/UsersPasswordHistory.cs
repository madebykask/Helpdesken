namespace DH.Helpdesk.Domain
{
    using global::System;

    public class UsersPasswordHistory : Entity
    {
        public int User_Id { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

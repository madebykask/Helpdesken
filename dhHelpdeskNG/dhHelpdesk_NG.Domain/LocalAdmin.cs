namespace DH.Helpdesk.Domain
{
    using global::System;

    public class LocalAdmin : Entity
    {
        public int Case_Id { get; set; }
        public string ComputerName { get; set; }
        public string UserDomain { get; set; }
        public string UserId { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeleteDate { get; set; }
        public DateTime ErrorDate { get; set; }
        public Guid LocalAdminGUID { get; set; }
    }
}

namespace DH.Helpdesk.Domain.Accounts
{
    using global::System;

    public class AccountActivityGroup : Entity
    {
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? AccountActivityGroupGUID { get; set; }
    }
}

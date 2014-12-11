namespace DH.Helpdesk.Domain.Accounts
{
    using global::System;

    public class AccountType : Entity
    {
        public int? AccountActivity_Id { get; set; }

        public int AccountField { get; set; }

        public string Name { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual AccountActivity AccountActivity { get; set; }
    }
}

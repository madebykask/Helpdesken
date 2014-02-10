namespace DH.Helpdesk.Domain.Computers
{
    using global::System;

    public class ComputerUserFieldSettings : Entity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public string ComputerUserField { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public int Customer_Id { get; set; }

        public string LDAPAttribute { get; set; }

        public int MinLength { get; set; }

        public int Required { get; set; }

        public int Show { get; set; }

        public int ShowInList { get; set; }

        #endregion
    }
}
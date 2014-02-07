namespace DH.Helpdesk.Domain
{
    using global::System;

    public class LicenseFile : Entity
    {
        public int License_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual License License { get; set; }
    }
}

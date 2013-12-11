using System;

namespace dhHelpdesk_NG.Domain
{
    public class LicenseFile : Entity
    {
        public int License_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual License License { get; set; }
    }
}

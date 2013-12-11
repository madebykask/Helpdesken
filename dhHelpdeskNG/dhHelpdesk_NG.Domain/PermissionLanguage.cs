using System;

namespace dhHelpdesk_NG.Domain
{
    public class PermissionLanguage : Entity
    {
        public int Language_Id { get; set; }
        public int Permission_Id { get; set; }
        public string PermissionName { get; set; }
    }
}

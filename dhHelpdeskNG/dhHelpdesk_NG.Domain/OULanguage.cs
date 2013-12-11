using System;

namespace dhHelpdesk_NG.Domain
{
    public class OULanguage : Entity
    {
        public int Language_Id { get; set; }
        public int OU_Id { get; set; }
        public string OU { get; set; }
    }
}

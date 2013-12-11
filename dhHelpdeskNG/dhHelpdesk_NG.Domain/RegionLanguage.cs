using System;

namespace dhHelpdesk_NG.Domain
{
    public class RegionLanguage : Entity
    {
        public int Language_Id { get; set; }
        public int Region_Id { get; set; }
        public string Region { get; set; }
    }
}

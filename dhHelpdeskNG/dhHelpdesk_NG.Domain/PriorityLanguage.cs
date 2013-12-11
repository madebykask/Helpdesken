using System;

namespace dhHelpdesk_NG.Domain
{
    public class PriorityLanguage : Entity
    {
        public int Language_Id { get; set; }
        public int Priority_Id { get; set; }
        public string InformUserText { get; set; }
    }
}

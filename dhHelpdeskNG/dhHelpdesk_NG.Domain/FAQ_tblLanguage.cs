using System;

namespace dhHelpdesk_NG.Domain
{
    class FAQ_tblLanguage : Entity
    {
        public int FAQ_Id { get; set; }
        public int Language_Id { get; set; }
        public string FAQQuery { get; set; }
        public string Answer { get; set; }
        public string Answer_Internal { get; set; }
    }
}

using System;

namespace dhHelpdesk_NG.Domain
{
    public class QuestionnaireResult : Entity
    {
        public int Anonymous { get; set; }
        public int QuestionnaireCircularPartic_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual QuestionnaireCircularPart QuestionnaireCircularPartic { get; set; }
    }
}

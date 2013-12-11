using System;

namespace dhHelpdesk_NG.Domain
{
    public class QuestionnaireCircular : Entity
    {
        public int Questionnaire_Id { get; set; }
        public int Status { get; set; }
        public string CircularName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }
    }
}

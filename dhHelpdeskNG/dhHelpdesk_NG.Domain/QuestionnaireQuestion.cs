using System;

namespace dhHelpdesk_NG.Domain
{
    public class QuestionnaireQuestion : Entity
    {
        public int Questionnaire_Id { get; set; }
        public int ShowNote { get; set; }
        public string Name { get; set; }
        public string NoteText { get; set; }
        public string QuestionnaireQuestionNumber { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }
    }
}

namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireQuesLangEntity: Entity
    {
        public int QuestionnaireQuestionId { get; set; }
        public int LanguageId { get; set; }
        public string QuestionnaireQuestion { get; set; }
        public string NoteText { get; set; }        
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual QuestionnaireQuestionEntity QuestionnaireQuestions { get; set; }
        public virtual Language Language { get; set; }

    }
}

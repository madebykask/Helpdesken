namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireQuesOpLangEntity : Entity
    {
        public int QuestionnaireQuestionOptionId { get; set; }
        public int LanguageId { get; set; }        
        public string QuestionnaireQuestionOption { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual QuestionnaireQuestionOptionEntity QuestionnaireQuestionOptions { get; set; }
        public virtual Language Language { get; set; }
    }
}

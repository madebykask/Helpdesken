namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireLanguageEntity
    {
        public int QuestionnaireId { get; set; }
        public int LanguageId { get; set; }
        public string QuestionnaireName { get; set; }
        public string QuestionnaireDescription { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual QuestionnaireEntity Questionnaire { get; set; }
        public virtual Language Language{ get; set; }
        
    }
}

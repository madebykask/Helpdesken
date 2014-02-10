namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireQuestionOptionEntity : Entity
    {
        public int QuestionnaireQuestionId { get; set; }
        public int QuestionnaireQuestionOptionPos { get; set; }
        public string QuestionnaireQuestionOption { get; set; }
        public int OptionValue { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }       
        
        public virtual QuestionnaireQuestionEntity QuestionnaireQuestion { get; set; }
    }
}

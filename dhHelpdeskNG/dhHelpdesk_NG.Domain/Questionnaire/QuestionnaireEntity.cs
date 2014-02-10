namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;    
    public class QuestionnaireEntity : Entity
    {
        public int CustomerId { get; set; }
        public string QuestionnaireDescription { get; set; }
        public string QuestionnaireName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}

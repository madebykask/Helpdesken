namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireCircularEntity : Entity
    {
        public int QuestionnaireId { get; set; }        
        public string CircularName { get; set; }        
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }

        public virtual QuestionnaireEntity Questionnaire { get; set; }
    }
}

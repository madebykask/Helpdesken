namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireResultEntity : Entity
    {
        public int QuestionnaireCircularParticId { get; set; }
        public int Anonymous { get; set; }        
        public DateTime CreatedDate { get; set; }

        public virtual QuestionnaireCircularPartEntity QuestionnaireCircularPart { get; set; }
    }
}

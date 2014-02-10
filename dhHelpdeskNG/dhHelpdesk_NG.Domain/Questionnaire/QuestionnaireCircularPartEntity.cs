namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireCircularPartEntity : Entity
    {
        public Guid GUID { get; set; }
        public int QuestionnaireCircularId { get; set; }
        public int CaseId { get; set; }        
        public DateTime CreatedDate { get; set; }
        public DateTime? InputDate { get; set; }
        public DateTime? SendDate { get; set; }
        
        public virtual Case Case { get; set; }
        public virtual QuestionnaireCircularEntity QuestionnaireCircular { get; set; }
    }
}

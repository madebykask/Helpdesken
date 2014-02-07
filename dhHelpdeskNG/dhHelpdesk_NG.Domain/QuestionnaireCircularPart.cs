namespace DH.Helpdesk.Domain
{
    using global::System;

    public class QuestionnaireCircularPart : Entity
    {
        public int Case_Id { get; set; }
        public int QuestionnaireCircular_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime InputDate { get; set; }
        public Guid GUID { get; set; }

        public virtual Case Case { get; set; }
        public virtual QuestionnaireCircular QuestionnaireCircular { get; set; }
    }
}

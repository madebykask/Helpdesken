namespace DH.Helpdesk.Domain.Questionnaire
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;

    public class QuestionnaireCircularPartEntity : Entity, IGuidEntity
    {
        #region Public Properties

        public virtual Case Case { get; set; }

        public int Case_Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid Guid { get; set; }

        public DateTime? InputDate { get; set; }

        public virtual QuestionnaireCircularEntity QuestionnaireCircular { get; set; }

        public int QuestionnaireCircular_Id { get; set; }

        public DateTime? SendDate { get; set; }

        #endregion
    }
}
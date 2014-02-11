namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireResultEntity : Entity
    {
        #region Public Properties

        public int Anonymous { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual QuestionnaireCircularPartEntity QuestionnaireCircularPart { get; set; }

        public int QuestionnaireCircularPartic_Id { get; set; }

        #endregion
    }
}
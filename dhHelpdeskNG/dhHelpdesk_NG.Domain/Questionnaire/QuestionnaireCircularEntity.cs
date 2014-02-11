namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireCircularEntity : Entity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public string CircularName { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual QuestionnaireEntity Questionnaire { get; set; }

        public int Questionnaire_Id { get; set; }

        public int Status { get; set; }

        #endregion
    }
}
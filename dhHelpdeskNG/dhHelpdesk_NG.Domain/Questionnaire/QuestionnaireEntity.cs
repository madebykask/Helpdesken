namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireEntity : Entity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public int Customer_Id { get; set; }

        public string QuestionnaireDescription { get; set; }

        public string QuestionnaireName { get; set; }

        #endregion
    }
}
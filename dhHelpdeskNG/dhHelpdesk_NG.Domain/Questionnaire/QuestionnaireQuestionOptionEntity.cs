namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireQuestionOptionEntity : Entity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int OptionValue { get; set; }

        public virtual QuestionnaireQuestionEntity QuestionnaireQuestion { get; set; }

        public int QuestionnaireQuestion_Id { get; set; }

        public string QuestionnaireQuestionOption { get; set; }

        public int QuestionnaireQuestionOptionPos { get; set; }

        #endregion
    }
}
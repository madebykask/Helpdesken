namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireQuestionEntity : Entity
    {
        #region Public Properties

        public int Questionnaire_Id { get; set; }

        public virtual QuestionnaireEntity Questionnaire { get; set; }

        public string QuestionnaireQuestionNumber { get; set; }

        public string QuestionnaireQuestion { get; set; }

        public int ShowNote { get; set; }

        public string NoteText { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        #endregion
    }
}
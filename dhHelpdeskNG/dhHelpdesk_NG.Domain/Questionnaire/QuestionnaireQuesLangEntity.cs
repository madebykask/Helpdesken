namespace DH.Helpdesk.Domain.Questionnaire
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;

    public class QuestionnaireQuesLangEntity : ILanguageEntity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Language Language { get; set; }

        public int Language_Id { get; set; }

        public string NoteText { get; set; }

        public string QuestionnaireQuestion { get; set; }

        public int QuestionnaireQuestion_Id { get; set; }

        public virtual QuestionnaireQuestionEntity QuestionnaireQuestions { get; set; }

        #endregion
    }
}
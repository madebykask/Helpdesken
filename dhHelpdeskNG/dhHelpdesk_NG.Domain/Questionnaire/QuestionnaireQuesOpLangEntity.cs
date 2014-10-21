namespace DH.Helpdesk.Domain.Questionnaire
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;

    public class QuestionnaireQuesOpLangEntity : ILanguageEntity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Language Language { get; set; }

        public int Language_Id { get; set; }

        public string QuestionnaireQuestionOption { get; set; }

        public int QuestionnaireQuestionOption_Id { get; set; }

        public virtual QuestionnaireQuestionOptionEntity QuestionnaireQuestionOptions { get; set; }

        #endregion
    }
}
namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireQuesOpLangEntity 
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
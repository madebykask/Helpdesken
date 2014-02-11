namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    public class QuestionnaireLanguageEntity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Language Language { get; set; }

        public int Language_Id { get; set; }

        public virtual QuestionnaireEntity Questionnaire { get; set; }

        public string QuestionnaireDescription { get; set; }

        public string QuestionnaireName { get; set; }

        public int Questionnaire_Id { get; set; }

        #endregion
    }
}
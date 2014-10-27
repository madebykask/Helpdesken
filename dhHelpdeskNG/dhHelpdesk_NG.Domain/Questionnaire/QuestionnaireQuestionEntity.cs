namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;
    using global::System.Collections.Generic;

    public class QuestionnaireQuestionEntity : Entity
    {
        //public QuestionnaireQuestionEntity()
        //{
        //    this.QuestionnaireQuestionOptionEntities = new List<QuestionnaireQuestionOptionEntity>();
        //    this.QuestionnaireQuesLangEntities = new List<QuestionnaireQuesLangEntity>();
        //}

        #region Public Properties

        public int Questionnaire_Id { get; set; }

        public virtual QuestionnaireEntity Questionnaire { get; set; }

        public string QuestionnaireQuestionNumber { get; set; }

        public string QuestionnaireQuestion { get; set; }

        public int ShowNote { get; set; }

        public string NoteText { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public ICollection<QuestionnaireQuestionOptionEntity> QuestionnaireQuestionOptionEntities { get; set; }

        public ICollection<QuestionnaireQuesLangEntity> QuestionnaireQuesLangEntities { get; set; }

        #endregion
    }
}
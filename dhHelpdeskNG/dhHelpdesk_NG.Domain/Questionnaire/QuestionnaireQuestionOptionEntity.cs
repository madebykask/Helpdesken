namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;
    using global::System.Collections.Generic;

    public class QuestionnaireQuestionOptionEntity : Entity
    {
        //public QuestionnaireQuestionOptionEntity()
        //{
        //    this.QuestionnaireQuestionResultEntities = new List<QuestionnaireQuestionResultEntity>();
        //    this.QuestionnaireQuesOpLangEntities = new List<QuestionnaireQuesOpLangEntity>();
        //}

        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int OptionValue { get; set; }

		public string IconId { get; set; }

		public virtual QuestionnaireQuestionEntity QuestionnaireQuestion { get; set; }

        public int QuestionnaireQuestion_Id { get; set; }

        public string QuestionnaireQuestionOption { get; set; }

        public int QuestionnaireQuestionOptionPos { get; set; }

        public byte[] IconSrc { get; set; }

        public virtual ICollection<QuestionnaireQuestionResultEntity> QuestionnaireQuestionResultEntities { get; set; }
 
        public virtual ICollection<QuestionnaireQuesOpLangEntity> QuestionnaireQuesOpLangEntities { get; set; } 

        #endregion
    }
}
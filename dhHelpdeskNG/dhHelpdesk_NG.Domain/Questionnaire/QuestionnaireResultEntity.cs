namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;
    using global::System.Collections.Generic;

    public class QuestionnaireResultEntity : Entity
    {
        public QuestionnaireResultEntity()
        {
            this.QuestionnaireQuestionResultEntities = new List<QuestionnaireQuestionResultEntity>();
        }

        #region Public Properties

        public int Anonymous { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual QuestionnaireCircularPartEntity QuestionnaireCircularPart { get; set; }

        public int QuestionnaireCircularPartic_Id { get; set; }

        public virtual ICollection<QuestionnaireQuestionResultEntity> QuestionnaireQuestionResultEntities { get; set; } 

        #endregion
    }
}
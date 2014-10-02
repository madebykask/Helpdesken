namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;
    using global::System.Collections.Generic;

    public class QuestionnaireCircularEntity : Entity
    {
        public QuestionnaireCircularEntity()
        {
            this.QuestionnaireCircularPartEntities = new List<QuestionnaireCircularPartEntity>();
        }

        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public string CircularName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Questionnaire_Id { get; set; }

        public int Status { get; set; }

        public virtual QuestionnaireEntity Questionnaire { get; set; }

        public virtual ICollection<QuestionnaireCircularPartEntity> QuestionnaireCircularPartEntities { get; set; } 

        #endregion
    }
}
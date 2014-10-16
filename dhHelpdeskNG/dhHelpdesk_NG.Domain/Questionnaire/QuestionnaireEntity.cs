using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.Domain.Questionnaire
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;

    public class QuestionnaireEntity : Entity, ICustomerEntity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public int Customer_Id { get; set; }
        
        public string QuestionnaireDescription { get; set; }
        
        public string QuestionnaireName { get; set; }

        #endregion
    }
}
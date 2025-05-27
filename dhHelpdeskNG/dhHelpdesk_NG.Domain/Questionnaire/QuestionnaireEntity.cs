namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;

    using global::System.Collections.Generic;

    using global::System.Collections.ObjectModel;

    using DH.Helpdesk.Domain.Interfaces;

	public class QuestionnaireEntity : Entity, ICustomerEntity
	{
		//public QuestionnaireEntity()
		//{
		//    this.QuestionnaireQuestionEntities = new List<QuestionnaireQuestionEntity>();
		//    this.QuestionnaireLanguageEntities = new List<QuestionnaireLanguageEntity>();
		//}

		#region Public Properties

		public DateTime ChangedDate { get; set; }

		public DateTime CreatedDate { get; set; }

		public virtual Customer Customer { get; set; }

		public int Customer_Id { get; set; }

		public string QuestionnaireDescription { get; set; }

		public string QuestionnaireName { get; set; }

		public string Identifier { get; set; }

		public bool ExcludeAdministrators { get; set; }

        public bool UseBase64Images { get; set; }

        public QuestionnaireType Type { get; set; }

	public virtual ICollection<QuestionnaireQuestionEntity> QuestionnaireQuestionEntities { get; set; }

        public virtual ICollection<QuestionnaireLanguageEntity> QuestionnaireLanguageEntities { get; set; } 

        #endregion
    }
}
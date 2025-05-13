using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewQuestionnaire : INewBusinessModel
    {
        #region Constructors and Destructors

        public NewQuestionnaire(string name, string description, int customerId, DateTime createDate)
        {
            this.Name = name;
            this.Description = description;
            this.CustomerId = customerId;
            this.CreatedDate = createDate;
			Type = QuestionnaireType.Questionnaire;
        }

	    public NewQuestionnaire(DateTime createdDate, int customerId, string description, QuestionnaireType type, string name)
	    {
		    CreatedDate = createdDate;
		    CustomerId = customerId;
		    Description = description;
		    Type = type;
		    Name = name;
	    }

	    #endregion

        #region Public Properties

        public DateTime CreatedDate { get; private set; }

        public int CustomerId { get; private set; }

        public string Description { get; private set; }

		public string Identifier { get; set; }

		public bool ExcludeAdministrators { get; set; }

        public bool UseBase64Images { get; set; }

        public QuestionnaireType Type { get; set; }

		[IsId]
        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        #endregion
    }
}
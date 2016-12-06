namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class QuestionnaireOverview
    {
        #region Constructors and Destructors

        public QuestionnaireOverview(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

	    public QuestionnaireOverview()
	    {
	    }

	    #endregion

        #region Properties

        [IsId]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        #endregion
    }
}
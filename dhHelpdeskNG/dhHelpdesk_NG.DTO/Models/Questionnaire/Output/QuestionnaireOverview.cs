namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class QuestionnaireOverview
    {
        #region Constructors and Destructors

        public QuestionnaireOverview(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        #endregion

        #region Properties

        [IsId]
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        #endregion
    }
}
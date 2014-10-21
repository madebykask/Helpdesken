namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class QuestionnaireOverview
    {
        public QuestionnaireOverview(
            int id,
            string name,
            string description,
            List<QuestionnaireQuestionOverview> questions)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Questions = questions;
        }

        private QuestionnaireOverview() 
        {
            this.Questions = new List<QuestionnaireQuestionOverview>();
        }

        [IsId]
        public int Id { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public string Description { get; private set; }

        public List<QuestionnaireQuestionOverview> Questions { get; private set; }

        public static QuestionnaireOverview GetDefault()
        {
            return new QuestionnaireOverview();
        }
    }
}

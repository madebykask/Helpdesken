﻿namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class QuestionnaireModel
    {
        public QuestionnaireModel(
            int id,
            string name,
            string description,
            int caseId,
            List<QuestionnaireQuestionModel> questions)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Questions = questions;
            CaseId = caseId;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public string Description { get; private set; }

        [NotNull]
        public List<QuestionnaireQuestionModel> Questions { get; private set; }

        public int CaseId { get; private set; }
    }
}
